using System.Collections.Generic;

namespace UElements
{
    public interface IElementsConfiguration
    {
        ElementsRoot ElementsRootPrefab { get; }
        List<ElementsModule> Modules { get; }
    }
}