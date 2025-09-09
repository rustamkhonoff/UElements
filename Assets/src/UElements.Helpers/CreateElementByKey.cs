using System;
using System.Collections.Specialized;
using System.Linq;
using System.Web;
using Newtonsoft.Json;
using UnityEngine;

namespace UElements.Helpers
{
    public class CreateElementByRequestOnStart : MonoBehaviour
    {
        [SerializeField] private ElementRequest _request;
        [SerializeField] private string _params;

        private async void Start()
        {
            if (_params != null)
            {
                Type type = await ElementsGlobal.GetElementTypeForRequest(_request);
                if (UElementsExtensions.IsSubclassOfRawGeneric(type, typeof(ModelElement<>)))
                {
                    Type modelType = UElementsExtensions.GetGenericBaseTypeArgument(type, typeof(ModelElement<>));
                    if (modelType != null)
                    {
                        NameValueCollection dict = HttpUtility.ParseQueryString(_params);
                        string json = JsonConvert.SerializeObject(dict.Cast<string>().ToDictionary(k => k, v => dict[v]));
                        object a = JsonConvert.DeserializeObject(json, modelType);
                        ElementsGlobal.Create(_request, a);
                    }
                }
            }
            else
            {
                ElementsGlobal.Create(_request);
            }
        }
    }
}