using System.Collections.Generic;
using UnityEngine;
using System;

namespace UElements.Helpers
{
    public class CreateElementByRequest : MonoBehaviour
    {
        [Serializable]
        public enum ModelType
        {
            None = 0,
            Params = 1,
            Query = 2
        }

        [SerializeField] internal ElementRequest _request;
        [SerializeField] internal ModelType _modelType;
        [SerializeField] internal Param[] _params;
        [SerializeField] internal string _query;
        [SerializeField] private bool _createOnStart;
        private IElements Elements => ElementsGlobal.Elements;

        private void Start()
        {
            if (_createOnStart)
                Create();
        }

        public async void Create()
        {
            Dictionary<string, string> model = _modelType switch
            {
                ModelType.None => new Dictionary<string, string>(),
                ModelType.Params => _params.FromParams(),
                ModelType.Query => _query.FromQuery(),
                _ => new Dictionary<string, string>()
            };

            await Elements.CreateElementWithQueryParams(_request, model);
        }
    }
}