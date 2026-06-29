// -------------------------------------------------------------------
// Author: Shokhrukhkhon Rustamkhonov
// Date: 18.05.2026
// Description:
// -------------------------------------------------------------------
using System;

namespace UElements.Profiles
{
    public static class PredefinedKeysHelper
    {
        public static string[] Values => Enum.GetNames(typeof(StateValues));
        public static string[] Names => Enum.GetNames(typeof(StateNames));
    }
}