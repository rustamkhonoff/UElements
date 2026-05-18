// -------------------------------------------------------------------
// Author: Shokhrukhkhon Rustamkhonov
// Date: 13.05.2026
// Description:
// -------------------------------------------------------------------
using UnityEngine;

namespace UElements.Profiles
{
    public class BooleanProfileValueAdapter : MonoBehaviour
    {
        [SerializeField] private ProfileController _controller;
        [SerializeField] private string _key;

        public void Adapt(bool value)
        {
            _controller.SetBool(_key, value);
        }
    }
}