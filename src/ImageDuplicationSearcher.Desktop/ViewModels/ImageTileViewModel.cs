using System.ComponentModel;
using System.IO;
using System.Threading.Tasks;
using System.Windows.Input;
using Microsoft.Maui.Controls;
using Microsoft.Maui.ApplicationModel;
using ImageDuplicateSearcher.Application.Interfaces;
using ImageDuplicateSearcher.Application.Models;

namespace ImageDuplicationSearcher.Desktop.ViewModels;

/// <summary>
/// Desktop view-model backing each image tile in the UI.
/// Holds an `ImageSource` so the UI can bind directly to renderable data.
/// </summary>
public class ImageTileViewModel : INotifyPropertyChanged
{
    private ImageSource? _image;
    private string _path = string.Empty;
    private double _sizeMB;
    private bool _isPlaceholder;
    private string? _placeholderReason;
    private DuplicateSearchResultImage? _sourceImage;
    private readonly IImageRemovalService? _removalService;
    private readonly Func<Task>? _onRemoved;
    private readonly Func<string, bool, Task>? _reportStatus;
    private bool _isDeleted;

    public event PropertyChangedEventHandler? PropertyChanged;

    public ImageTileViewModel(string path, double sizeMB)
    {
        Path = path;
        SizeMB = sizeMB;
    }

    /// <summary>
    /// Construct a tile bound to an application source model and a removal service.
    /// </summary>
    public ImageTileViewModel(DuplicateSearchResultImage source, IImageRemovalService removalService, Func<Task>? onRemoved = null, Func<string, bool, Task>? reportStatus = null)
    {
        _sourceImage = source ?? throw new ArgumentNullException(nameof(source));
        _removalService = removalService ?? throw new ArgumentNullException(nameof(removalService));
        _onRemoved = onRemoved;
        _reportStatus = reportStatus;

        Path = source.Path;
        SizeMB = source.SizeMB;
        IsDeleted = source.IsDeleted;

        RemoveCommand = new Command(async () => await ExecuteRemoveAsync().ConfigureAwait(false));
    }

    public ImageSource? Image
    {
        get => _image;
        set
        {
            if (ReferenceEquals(value, _image)) return;
            _image = value;
            OnPropertyChanged(nameof(Image));
        }
    }

    public string Path
    {
        get => _path;
        set
        {
            if (value == _path) return;
            _path = value;
            OnPropertyChanged(nameof(Path));
        }
    }

    public double SizeMB
    {
        get => _sizeMB;
        set
        {
            if (value.Equals(_sizeMB)) return;
            _sizeMB = value;
            OnPropertyChanged(nameof(SizeMB));
        }
    }

    /// <summary>
    /// Indicates whether the underlying source image has been marked deleted in this session.
    /// </summary>
    public bool IsDeleted
    {
        get => _sourceImage is null ? _isDeleted : _sourceImage.IsDeleted;
        set
        {
            if (value == _isDeleted) return;
            _isDeleted = value;
            if (_sourceImage is not null)
            {
                _sourceImage.IsDeleted = value;
            }
            OnPropertyChanged(nameof(IsDeleted));
        }
    }

    public bool IsPlaceholder
    {
        get => _isPlaceholder;
        set
        {
            if (value == _isPlaceholder) return;
            _isPlaceholder = value;
            OnPropertyChanged(nameof(IsPlaceholder));
        }
    }

    public string? PlaceholderReason
    {
        get => _placeholderReason;
        set
        {
            if (value == _placeholderReason) return;
            _placeholderReason = value;
            OnPropertyChanged(nameof(PlaceholderReason));
        }
    }

    /// <summary>
    /// Command to attempt removal of the underlying file.
    /// </summary>
    public ICommand? RemoveCommand { get; private set; }

    private void OnPropertyChanged(string name)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
    }

    /// <summary>
    /// Asynchronously loads image bytes via the provided display manager and updates the Image property.
    /// </summary>
    public async Task LoadAsync(IImageDisplayManager displayManager)
    {
        if (displayManager is null) return;

        try
        {
            var result = await displayManager.GetImageDisplayAsync(Path).ConfigureAwait(false);
            var imgSrc = ImageSource.FromStream(() => new MemoryStream(result.ImageBytes));

            MainThread.BeginInvokeOnMainThread(() =>
            {
                Image = imgSrc;
                IsPlaceholder = result.IsPlaceholder;
                PlaceholderReason = result.Reason;
            });
        }
        catch
        {
            MainThread.BeginInvokeOnMainThread(() =>
            {
                IsPlaceholder = true;
                PlaceholderReason = "Unreadable image";
            });
        }
    }

    private async Task ExecuteRemoveAsync()
    {
        if (_sourceImage is null || _removalService is null) return;

        var result = await _removalService.RemoveImageAsync(_sourceImage.Path).ConfigureAwait(false);

        switch (result)
        {
            case RemovalResult.Success:
                // Mark in-memory state and refresh UI via callback
                _sourceImage.IsDeleted = true;
                MainThread.BeginInvokeOnMainThread(() =>
                {
                        IsDeleted = true;
                        _ = _reportStatus?.Invoke("Image removed successfully", false);
                        _ = _onRemoved?.Invoke();
                });
                break;

            case RemovalResult.NotFound:
                MainThread.BeginInvokeOnMainThread(() =>
                {
                    IsPlaceholder = true;
                    PlaceholderReason = "File not found";
                    _ = _reportStatus?.Invoke("File not found; it may have already been removed.", true);
                });
                break;

            case RemovalResult.Unauthorized:
                MainThread.BeginInvokeOnMainThread(() =>
                {
                    IsPlaceholder = true;
                    PlaceholderReason = "Permission denied";
                    _ = _reportStatus?.Invoke("Permission denied when deleting the file. Close other apps or run with privileges.", true);
                });
                break;

            case RemovalResult.Locked:
                MainThread.BeginInvokeOnMainThread(() =>
                {
                    IsPlaceholder = true;
                    PlaceholderReason = "File in use by another process";
                    _ = _reportStatus?.Invoke("File is in use by another process. Close the locking app and try again.", true);
                });
                break;

            default:
                MainThread.BeginInvokeOnMainThread(() =>
                {
                    IsPlaceholder = true;
                    PlaceholderReason = "Unable to delete file";
                    _ = _reportStatus?.Invoke("An unexpected error occurred while attempting to delete the file.", true);
                });
                break;
        }
    }
}
