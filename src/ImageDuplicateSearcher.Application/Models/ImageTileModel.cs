using System.ComponentModel;

namespace ImageDuplicateSearcher.Application.Models;

/// <summary>
/// View-model shape for an image tile shown in the Desktop UI.
/// Kept in the Application layer so mapping from source JSON is straightforward.
/// </summary>
public class ImageTileModel : INotifyPropertyChanged
{
    private string _path = string.Empty;
    private double _sizeMB;
    private bool _isDeleted;
    private ImageRenderStatus _renderStatus = ImageRenderStatus.Available;
    private string? _placeholderReason;

    /// <inheritdoc />
    public event PropertyChangedEventHandler? PropertyChanged;

    /// <summary>
    /// File system path to the image.
    /// </summary>
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

    /// <summary>
    /// File size in megabytes as provided by the JSON results.
    /// </summary>
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
    /// Indicates whether the image has been marked deleted in the current session.
    /// </summary>
    public bool IsDeleted
    {
        get => _isDeleted;
        set
        {
            if (value == _isDeleted) return;
            _isDeleted = value;
            OnPropertyChanged(nameof(IsDeleted));
        }
    }

    /// <summary>
    /// Current render status for the tile (available, missing, unreadable).
    /// </summary>
    public ImageRenderStatus RenderStatus
    {
        get => _renderStatus;
        set
        {
            if (value == _renderStatus) return;
            _renderStatus = value;
            OnPropertyChanged(nameof(RenderStatus));
        }
    }

    /// <summary>
    /// Optional short reason shown when a placeholder is used (e.g., "Missing file").
    /// </summary>
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
    /// Create a new instance mapped from a source <see cref="DuplicateSearchResultImage"/>.
    /// </summary>
    public static ImageTileModel From(DuplicateSearchResultImage src)
    {
        return new ImageTileModel
        {
            Path = src.Path,
            SizeMB = src.SizeMB,
            IsDeleted = src.IsDeleted,
            RenderStatus = ImageRenderStatus.Available,
            PlaceholderReason = null
        };
    }

    private void OnPropertyChanged(string propertyName)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
