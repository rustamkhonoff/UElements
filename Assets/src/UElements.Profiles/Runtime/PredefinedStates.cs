// -------------------------------------------------------------------
// Author: Shokhrukhkhon Rustamkhonov
// Date: 18.05.2026
// Description:
// -------------------------------------------------------------------

namespace UElements.Profiles
{
    public enum StateNames
    {
        None = -1,
        IsActive = 0,
        IsEnabled = 1,
        IsDisabled = 2,
        IsInteractable = 3,
        IsSelected = 4,
        IsHovered = 5,
        IsReached = 6,
        IsUnlocked = 7,
        IsCompleted = 8,
        State = 999
    }

    public enum StateValues
    {
        None = -1,
        True = 0,
        False = 1,
        Active = 2,
        Disabled = 3,
        Default = 4,
        Interactable = 5,
        NotInteractable = 6,
        Selected = 7,
        NotSelected = 8,
        Hovered = 9,
        NotHovered = 10,
    }
}