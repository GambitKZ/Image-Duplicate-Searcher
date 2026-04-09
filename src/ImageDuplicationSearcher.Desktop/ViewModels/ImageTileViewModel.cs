using System.ComponentModel;
using System.IO;
using System.Threading.Tasks;
using Microsoft.Maui.Controls;
using Microsoft.Maui.ApplicationModel;
using ImageDuplicateSearcher.Application.Interfaces;

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

    public event PropertyChangedEventHandler? PropertyChanged;

    public ImageTileViewModel(string path, double sizeMB)
    {
        Path = path;
        SizeMB = sizeMB;
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
}
