using System;
using ImageDuplicateSearcher.Application.Interfaces;
using ImageDuplicateSearcher.Application.Models;
using Microsoft.Maui.Storage;

namespace ImageDuplicationSearcher.Desktop;

public partial class MainPage : ContentPage
{
    private readonly IResultsLoader _resultsLoader;
    private DuplicateSearchResult[]? _loadedResults;

    public MainPage(IResultsLoader resultsLoader)
    {
        InitializeComponent();
        _resultsLoader = resultsLoader;
    }

    /// <summary>
    /// Opens a file picker to select a JSON results file and loads it using the ResultsLoader service.
    /// </summary>
    private async void OnOpenJsonResults(object? sender, EventArgs e)
    {
        try
        {
            var result = await FilePicker.PickAsync(new PickOptions
            {
                PickerTitle = "Select JSON Results File"
            });

            if (result is null)
            {
                return;
            }

            var filePath = result.FullPath;

            // Show transient loading state and clear previous error
            StatusLabel.Text = "Loading...";
            ErrorLabel.IsVisible = false;

            try
            {
                var results = await _resultsLoader.LoadResultsAsync(filePath);
                _loadedResults = results;
                ResultsFileEntry.Text = filePath;
                StatusLabel.Text = $"Loaded: {_loadedResults.Length} groups";
                await DisplayAlertAsync("Results Loaded", $"{_loadedResults.Length} duplicate groups found.", "OK");
            }
            catch (FileNotFoundException ex)
            {
                StatusLabel.Text = "Load failed";
                ErrorLabel.Text = $"File not found: {ex.Message}";
                ErrorLabel.IsVisible = true;
            }
            catch (InvalidOperationException ex)
            {
                StatusLabel.Text = "Load failed";
                ErrorLabel.Text = $"Invalid JSON: {ex.Message}";
                ErrorLabel.IsVisible = true;
            }
            catch (System.Text.Json.JsonException ex)
            {
                StatusLabel.Text = "Load failed";
                ErrorLabel.Text = $"JSON parsing error: {ex.Message}";
                ErrorLabel.IsVisible = true;
            }
        }
        catch (Exception ex)
        {
            await DisplayAlertAsync("Error", ex.Message, "OK");
        }
    }
}
