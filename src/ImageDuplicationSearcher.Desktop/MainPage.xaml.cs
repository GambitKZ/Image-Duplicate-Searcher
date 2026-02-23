using Microsoft.Maui.Storage;
using Microsoft.Extensions.DependencyInjection;
using ImageDuplicateSearcher.Application.Services;
using System.Threading.Tasks;
using System;
using System.IO;

namespace ImageDuplicationSearcher.Desktop;

public partial class MainPage : ContentPage
{
    private readonly WorkflowService? _workflowService;
    private readonly ImageDuplicateSearcher.Application.Interfaces.IReporter? _reporter;
    private CancellationTokenSource? _cts;

    public MainPage()
    {
        InitializeComponent();

        try
        {
            var mauiServices = Microsoft.Maui.Controls.Application.Current?.Handler?.MauiContext?.Services;
            _workflowService = mauiServices?.GetService<WorkflowService>();
            _reporter = mauiServices?.GetService<ImageDuplicateSearcher.Application.Interfaces.IReporter>();

            if (_reporter is ImageDuplicateSearcher.Application.Services.UiReporter uiReporter)
            {
                uiReporter.OnLogMessage += (msg) => AppendResponse(msg);
            }
        }
        catch
        {
            _workflowService = null;
        }
    }

    private async void OnSetImageDirectory(object? sender, EventArgs e)
    {
        try
        {
            var result = await FilePicker.Default.PickAsync();
            if (result != null)
            {
                var dir = Path.GetDirectoryName(result.FullPath);
                ImageDirectoryEntry.Text = dir;
            }
        }
        catch (Exception ex)
        {
            AppendResponse($"Error selecting directory: {ex.Message}");
        }
    }

    private async void OnSetOutputFile(object? sender, EventArgs e)
    {
        try
        {
            var result = await FilePicker.Default.PickAsync();
            if (result != null)
            {
                OutputFileEntry.Text = result.FullPath;
            }
        }
        catch (Exception ex)
        {
            AppendResponse($"Error selecting output file: {ex.Message}");
        }
    }

    private void AppendResponse(string message)
    {
        Microsoft.Maui.ApplicationModel.MainThread.BeginInvokeOnMainThread(() =>
        {
            ResponseEditor.Text += message + "\n";
        });
    }

    private async void OnStartScanClicked(object? sender, EventArgs e)
    {
        StartScanButton.IsEnabled = false;
        CancelScanButton.IsEnabled = true;
        AppendResponse("Starting scan...");

        // Build runtime options from UI
        var runtimeOptions = new ImageDuplicateSearcher.Application.Settings.ImageDuplicationOptions();
        runtimeOptions.ImageDirectory = ImageDirectoryEntry.Text ?? string.Empty;
        runtimeOptions.OutputFilePath = OutputFileEntry.Text ?? string.Empty;
        var formatsText = FormatsEntry.Text ?? string.Empty;
        runtimeOptions.SupportedFormats = formatsText.Split(new[] { ',', ';' }, StringSplitOptions.RemoveEmptyEntries)
            .Select(s => s.Trim().ToLowerInvariant())
            .Select(s => s.StartsWith('.') ? s : "." + s)
            .ToList();

        _cts = new CancellationTokenSource();
        var progress = new Progress<string>(s => AppendResponse(s));

        try
        {
            if (_workflowService == null)
            {
                AppendResponse("WorkflowService not available via DI.");
                return;
            }

            // If reporter supports runtime option updates, ensure it uses the same options
            if (_reporter is ImageDuplicateSearcher.Application.Services.UiReporter ui)
            {
                ui.UpdateOptions(runtimeOptions);
            }

            await _workflowService.ExecuteWorkflowAsync(runtimeOptions, progress, _cts.Token);
            AppendResponse("Scan completed.");
        }
        catch (OperationCanceledException)
        {
            AppendResponse("Scan cancelled.");
        }
        catch (Exception ex)
        {
            AppendResponse($"Scan failed: {ex.Message}");
        }
        finally
        {
            StartScanButton.IsEnabled = true;
            CancelScanButton.IsEnabled = false;
            _cts?.Dispose();
            _cts = null;
        }
    }

    private void OnCancelScanClicked(object? sender, EventArgs e)
    {
        if (_cts != null && !_cts.IsCancellationRequested)
        {
            _cts.Cancel();
            AppendResponse("Cancellation requested...");
        }
    }
}
