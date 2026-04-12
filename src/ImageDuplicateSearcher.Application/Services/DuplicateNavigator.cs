using System.ComponentModel;
using ImageDuplicateSearcher.Application.Interfaces;
using ImageDuplicateSearcher.Application.Models;

namespace ImageDuplicateSearcher.Application.Services;

/// <summary>
/// Application-layer service that manages navigation over duplicate search result groups.
/// Exposes a thin, testable API for the UI to drive Previous/Next and Jump-to-Group behaviors.
/// </summary>
public class DuplicateNavigator : IDuplicateNavigator
{
    private readonly object _sync = new();
    private DuplicateSearchResult[]? _groups;
    private int _currentIndex = -1;

    public event PropertyChangedEventHandler? PropertyChanged;

    /// <summary>
    /// Initialize the navigator with the provided groups. Resets the current index to the first group when available.
    /// </summary>
    public void Initialize(DuplicateSearchResult[] groups)
    {
        if (groups is null)
        {
            throw new ArgumentNullException(nameof(groups));
        }

        lock (_sync)
        {
            _groups = groups;
            _currentIndex = groups.Length > 0 ? 0 : -1;
        }

        OnPropertyChanged(nameof(TotalCount));
        NotifyIndexChanged();
    }

    public int TotalCount
    {
        get
        {
            lock (_sync)
            {
                return _groups?.Length ?? 0;
            }
        }
    }

    public int CurrentIndex
    {
        get
        {
            lock (_sync)
            {
                return _currentIndex;
            }
        }
    }

    public DuplicateSearchResult? CurrentGroup
    {
        get
        {
            lock (_sync)
            {
                if (_groups == null || _currentIndex < 0 || _currentIndex >= _groups.Length)
                {
                    return null;
                }

                return _groups[_currentIndex];
            }
        }
    }

    public bool CanMovePrevious
    {
        get
        {
            lock (_sync)
            {
                return _currentIndex > 0;
            }
        }
    }

    public bool CanMoveNext
    {
        get
        {
            lock (_sync)
            {
                return _groups != null && _currentIndex >= 0 && _currentIndex < _groups.Length - 1;
            }
        }
    }

    public void Previous()
    {
        lock (_sync)
        {
            if (_currentIndex <= 0)
            {
                return;
            }

            _currentIndex--;
        }

        NotifyIndexChanged();
    }

    public void Next()
    {
        lock (_sync)
        {
            if (_groups == null || _currentIndex < 0 || _currentIndex >= _groups.Length - 1)
            {
                return;
            }

            _currentIndex++;
        }

        NotifyIndexChanged();
    }

    public bool TryGoToGroup(int displayIndex)
    {
        if (displayIndex <= 0)
        {
            return false;
        }

        lock (_sync)
        {
            if (_groups == null)
            {
                return false;
            }

            int target = displayIndex - 1;
            if (target < 0 || target >= _groups.Length)
            {
                return false;
            }

            if (target == _currentIndex)
            {
                return true;
            }

            _currentIndex = target;
        }

        NotifyIndexChanged();
        return true;
    }

    private void NotifyIndexChanged()
    {
        OnPropertyChanged(nameof(CurrentIndex));
        OnPropertyChanged(nameof(CurrentGroup));
        OnPropertyChanged(nameof(CanMovePrevious));
        OnPropertyChanged(nameof(CanMoveNext));
    }

    private void OnPropertyChanged(string propertyName)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
