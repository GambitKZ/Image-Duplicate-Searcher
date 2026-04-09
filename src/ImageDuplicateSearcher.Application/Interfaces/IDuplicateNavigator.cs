using System.ComponentModel;
using ImageDuplicateSearcher.Application.Models;

namespace ImageDuplicateSearcher.Application.Interfaces;

/// <summary>
/// Centralized navigation contract for duplicate result groups.
/// Implementations manage an internal 0-based index while exposing
/// user-facing behaviors (Previous/Next/Go) and change notifications
/// so the UI layer can remain thin.
/// </summary>
public interface IDuplicateNavigator : INotifyPropertyChanged
{
    /// <summary>
    /// Initialize the navigator with the provided duplicate groups.
    /// </summary>
    /// <param name="groups">Array of duplicate groups to navigate.</param>
    void Initialize(DuplicateSearchResult[] groups);

    /// <summary>
    /// Total number of groups loaded.
    /// </summary>
    int TotalCount { get; }

    /// <summary>
    /// Current active group's zero-based index.
    /// </summary>
    int CurrentIndex { get; }

    /// <summary>
    /// The currently active duplicate group, or null if none.
    /// </summary>
    DuplicateSearchResult? CurrentGroup { get; }

    /// <summary>
    /// True if calling <see cref="Previous()"/> will move to a valid group.
    /// </summary>
    bool CanMovePrevious { get; }

    /// <summary>
    /// True if calling <see cref="Next()"/> will move to a valid group.
    /// </summary>
    bool CanMoveNext { get; }

    /// <summary>
    /// Move to the previous group (no-op if at lower boundary).
    /// </summary>
    void Previous();

    /// <summary>
    /// Move to the next group (no-op if at upper boundary).
    /// </summary>
    void Next();

    /// <summary>
    /// Attempts to jump to a group using a 1-based display index.
    /// Returns true if navigation succeeded (index valid).
    /// </summary>
    /// <param name="displayIndex">1-based group index as shown to the user.</param>
    bool TryGoToGroup(int displayIndex);
}
