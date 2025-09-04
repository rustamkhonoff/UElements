using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using R3;
using Sirenix.OdinInspector;
using UElements;
using UnityEngine;

namespace Sandbox.Demos.DisposingElements
{
    public class TestingElementTest : MonoBehaviour
    {
        [SerializeField] private TestingElement _prefab;

        private TestingElement m_element;
        private CancellationTokenSource m_cancellationTokenSource = new();

        [Button] private async void Create()
        {
            m_element = await ElementsGlobal.Create<TestingElement>(_prefab.gameObject);
            m_element.AddTo(m_cancellationTokenSource);
        }

        [Button] private void Dispose() => m_element.SafeDispose();
        [Button] private void Hide() => m_element.Close();
        [Button] private void HideNoDispose() => m_element.Hide();
        [Button] private void Show() => m_element.Show();
        [Button] private void Destroy() => GameObject.Destroy(m_element);

        [Button] private void DisposeToken()
        {
            m_cancellationTokenSource.Cancel();
            m_cancellationTokenSource.Dispose();
            m_cancellationTokenSource = new CancellationTokenSource();
        }
    }
}