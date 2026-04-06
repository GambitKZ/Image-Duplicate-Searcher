using CommunityToolkit.Maui.Storage;
using ImageDuplicateSearcher.Application.Services;
using ImageDuplicateSearcher.Application.Settings;
using Microsoft.Extensions.Options;

namespace ImageDuplicationSearcher.Desktop;

public partial class MainPage : ContentPage
{
    private readonly IFolderPicker _folderPicker;
    private readonly WorkflowService _workflowService;
    private readonly UiReporter _reporter;
    private readonly ImageDuplicationOptions _runtimeOptions;
    private readonly IReadOnlyList<string> _defaultFormats;
    private CancellationTokenSource? _cts;

    /// <summary>
    /// Initializes the main page and seeds the UI with default runtime settings.
    /// </summary>
    public MainPage(
        IFolderPicker folderPicker,
        WorkflowService workflowService,
        UiReporter reporter,
        IOptions<ImageDuplicationOptions> options)
    {
        InitializeComponent();
        _folderPicker = folderPicker;
        _workflowService = workflowService;
        _reporter = reporter;

        var configured = options.Value;
        _defaultFormats = configured.SupportedFormats.Count > 0
            ? configured.SupportedFormats.ToList()
            : new List<string> { ".jpeg", ".jpg", ".png", ".bmp" };

        _runtimeOptions = new ImageDuplicationOptions
        {
            ImageDirectory = configured.ImageDirectory,
            OutputFilePath = configured.OutputFilePath,
            SupportedFormats = _defaultFormats.ToList()
        };

        _reporter.OnLogMessage += AppendResponse;

        ImageDirectoryEntry.Text = _runtimeOptions.ImageDirectory;
        OutputFileEntry.Text = _runtimeOptions.OutputFilePath;
        FormatsEntry.Text = string.Join(';', _runtimeOptions.SupportedFormats);
    }

    /// <summary>
    /// Opens a folder picker and updates the image directory.
    /// </summary>
    private async void OnSetImageDirectory(object? sender, EventArgs e)
    {
        try
        {
            var result = await _folderPicker.PickAsync();
            if (result.IsSuccessful)
            {
                ImageDirectoryEntry.Text = result.Folder.Path;
                _runtimeOptions.ImageDirectory = result.Folder.Path;
            }
        }
        catch (Exception ex)
        {
            AppendResponse($"Error selecting directory: {ex.Message}");
        }
    }

    /// <summary>
    /// Picks a folder and creates an output file path inside it.
    /// </summary>
    private async void OnSetOutputFile(object? sender, EventArgs e)
    {
        try
        {
            var result = await _folderPicker.PickAsync();
            if (!result.IsSuccessful)
            {
                return;
            }

            var currentName = Path.GetFileName(OutputFileEntry.Text);
            if (string.IsNullOrWhiteSpace(currentName))
            {
                currentName = "duplicates.json";
            }

            var outputPath = Path.Combine(result.Folder.Path, currentName);
            OutputFileEntry.Text = outputPath;
            _runtimeOptions.OutputFilePath = outputPath;
        }
        catch (Exception ex)
        {
            AppendResponse($"Error selecting output path: {ex.Message}");
        }
    }

    /// <summary>
    /// Starts duplicate detection with current runtime options.
    /// </summary>
    private async void OnStartScanClicked(object? sender, EventArgs e)
    {
        if (_cts is not null)
        {
            AppendResponse("A scan is already running.");
            return;
        }

        if (!TryUpdateRuntimeOptions(out var validationError))
        {
            AppendResponse(validationError);
            return;
        }

        _cts = new CancellationTokenSource();
        SetScanButtons(isRunning: true);
        AppendResponse("Starting scan...");

        try
        {
            var progress = new Progress<string>(AppendResponse);

            await Task.Run(() => _workflowService.ExecuteWorkflowAsync(_runtimeOptions, progress, _cts.Token));
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
            _cts.Dispose();
            _cts = null;
            SetScanButtons(isRunning: false);
        }
    }

    /// <summary>
    /// Requests cancellation for the currently running scan.
    /// </summary>
    private void OnCancelScanClicked(object? sender, EventArgs e)
    {
        _cts?.Cancel();
    }

    /// <summary>
    /// Validates and applies values from UI controls to runtime options.
    /// </summary>
    private bool TryUpdateRuntimeOptions(out string validationError)
    {
        validationError = string.Empty;

        var imageDirectory = ImageDirectoryEntry.Text?.Trim() ?? string.Empty;
        if (string.IsNullOrWhiteSpace(imageDirectory))
        {
            validationError = "Image directory is required.";
            return false;
        }

        var outputPath = OutputFileEntry.Text?.Trim() ?? string.Empty;
        if (string.IsNullOrWhiteSpace(outputPath))
        {
            validationError = "Output file path is required.";
            return false;
        }

        _runtimeOptions.ImageDirectory = imageDirectory;
        _runtimeOptions.OutputFilePath = outputPath;

        _runtimeOptions.SetSupportedFormatsFromString(FormatsEntry.Text);
        if (_runtimeOptions.SupportedFormats.Count == 0)
        {
            _runtimeOptions.SupportedFormats = _defaultFormats.ToList();
        }

        return true;
    }

    /// <summary>
    /// Toggles start/cancel button state while scan is running.
    /// </summary>
    private void SetScanButtons(bool isRunning)
    {
        StartScanButton.IsEnabled = !isRunning;
        CancelScanButton.IsEnabled = isRunning;
    }

    /// <summary>
    /// Appends a status line to the response editor from any thread.
    /// </summary>
    private void AppendResponse(string message)
    {
        MainThread.BeginInvokeOnMainThread(() =>
        {
            if (string.IsNullOrWhiteSpace(ResponseEditor.Text))
            {
                ResponseEditor.Text = message;
                return;
            }

            ResponseEditor.Text += Environment.NewLine + message;
        });
    }

    /// <summary>
    /// Unsubscribes from reporter events when the page is removed.
    /// </summary>
    protected override void OnDisappearing()
    {
        _reporter.OnLogMessage -= AppendResponse;
        base.OnDisappearing();
    }
}
