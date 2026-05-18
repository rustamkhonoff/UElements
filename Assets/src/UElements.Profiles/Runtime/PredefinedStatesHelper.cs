// -------------------------------------------------------------------
// Author: Shokhrukhkhon Rustamkhonov
// Date: 18.05.2026
// Description:
// -------------------------------------------------------------------
using System;

namespace UElements.Profiles
{
    public static class PredefinedStatesHelper
    {
        public static string[] Get => Enum.GetNames(typeof(PredefinedStates));
    }
}