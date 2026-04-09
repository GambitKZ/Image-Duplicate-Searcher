using System;
using System.ComponentModel;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using ImageDuplicateSearcher.Application.Interfaces;
using ImageDuplicateSearcher.Application.Models;
using ImageDuplicationSearcher.Desktop.ViewModels;
using Microsoft.Maui.Storage;
using Microsoft.Maui.ApplicationModel;

namespace ImageDuplicationSearcher.Desktop;

public partial class MainPage : ContentPage
{
    private readonly IResultsLoader _resultsLoader;
    private readonly IDuplicateNavigator _navigator;
    private readonly IImageDisplayManager _imageDisplayManager;
    private readonly IImageRemovalService _imageRemovalService;
    private DuplicateSearchResult[]? _loadedResults;
    private readonly ObservableCollection<ImageTileViewModel> _tiles = new();

    public MainPage(IResultsLoader resultsLoader, IDuplicateNavigator navigator, IImageDisplayManager imageDisplayManager, IImageRemovalService imageRemovalService)
    {
        InitializeComponent();
        _resultsLoader = resultsLoader;
        _navigator = navigator;
        _imageDisplayManager = imageDisplayManager;
        _imageRemovalService = imageRemovalService;

        // Wire navigator change notifications and UI handlers
        _navigator.PropertyChanged += Navigator_PropertyChanged;

        PrevButton.Clicked += OnPrevClicked;
        NextButton.Clicked += OnNextClicked;
        GoButton.Clicked += OnGoClicked;

        // Bind collection to UI
        ImageCollectionView.ItemsSource = _tiles;

        // Ensure initial UI state
        UpdateNavigationUI();
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

                // Initialize navigator with loaded results so UI navigation becomes available
                _navigator.Initialize(results);
                UpdateNavigationUI();

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

// UI helpers and event handlers
partial class MainPage
{
    private void Navigator_PropertyChanged(object? sender, PropertyChangedEventArgs e)
    {
        // Keep UI in sync when navigator updates
        UpdateNavigationUI();

        // Refresh image tiles whenever the current group changes
        _ = RefreshTilesAsync();
    }

    private async Task RefreshTilesAsync()
    {
        var group = _navigator.CurrentGroup;

        if (group is null)
        {
            MainThread.BeginInvokeOnMainThread(() =>
            {
                _tiles.Clear();
                GroupImageCountLabel.Text = "Images: 0";
            });

            return;
        }

        var images = group.Images?.Where(i => !i.IsDeleted).ToList() ?? new System.Collections.Generic.List<DuplicateSearchResultImage>();

        MainThread.BeginInvokeOnMainThread(() =>
        {
            GroupImageCountLabel.Text = $"Images: {images.Count}";
            _tiles.Clear();

            foreach (var img in images)
            {
                // Pass the source model and removal service to each tile; provide RefreshTilesAsync as the callback when an item is removed.
                _tiles.Add(new ImageTileViewModel(img, _imageRemovalService, RefreshTilesAsync, ShowStatusAsync));
            }
        });

        // Load images asynchronously and populate ImageSource per tile
        foreach (var tile in _tiles.ToList())
        {
            await tile.LoadAsync(_imageDisplayManager).ConfigureAwait(false);
        }
    }

    private void UpdateNavigationUI()
    {
        // Ensure UI updates run on the main thread.
        MainThread.BeginInvokeOnMainThread(() =>
        {
            int current = _navigator.CurrentIndex >= 0 ? _navigator.CurrentIndex + 1 : 0;
            SummaryLabel.Text = $"{current} / {_navigator.TotalCount}";

            var group = _navigator.CurrentGroup;
            GroupHashLabel.Text = group is null ? "Hash: -" : $"Hash: {group.Hash}";

            PrevButton.IsEnabled = _navigator.CanMovePrevious;
            NextButton.IsEnabled = _navigator.CanMoveNext;

            NavErrorLabel.IsVisible = false;
        });
    }

    /// <summary>
    /// Show a status or error message in the main UI.
    /// </summary>
    private Task ShowStatusAsync(string message, bool isError)
    {
        MainThread.BeginInvokeOnMainThread(() =>
        {
            if (isError)
            {
                ErrorLabel.Text = message;
                ErrorLabel.IsVisible = true;
                StatusLabel.Text = "Last action failed";
            }
            else
            {
                StatusLabel.Text = message;
                ErrorLabel.IsVisible = false;
            }
        });

        return Task.CompletedTask;
    }

    private void OnPrevClicked(object? sender, EventArgs e)
    {
        _navigator.Previous();
    }

    private void OnNextClicked(object? sender, EventArgs e)
    {
        _navigator.Next();
    }

    private void OnGoClicked(object? sender, EventArgs e)
    {
        NavErrorLabel.IsVisible = false;

        if (string.IsNullOrWhiteSpace(GroupEntry.Text) || !int.TryParse(GroupEntry.Text.Trim(), out var displayIndex))
        {
            NavErrorLabel.Text = "Enter a valid group number.";
            NavErrorLabel.IsVisible = true;
            return;
        }

        if (!_navigator.TryGoToGroup(displayIndex))
        {
            NavErrorLabel.Text = $"Group must be between 1 and {_navigator.TotalCount}.";
            NavErrorLabel.IsVisible = true;
            return;
        }

        // success — UI will update via PropertyChanged handler
    }
}
