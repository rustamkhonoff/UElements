#if ODIN_INSPECTOR

// -------------------------------------------------------------------
// Author: Shokhrukhkhon Rustamkhonov
// Date: 18.05.2026
// Description:
// -------------------------------------------------------------------
using System;
using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using Sirenix.OdinInspector.Editor;
using Sirenix.OdinInspector.Editor.ValueResolvers;
using UElements.Profiles.Editor;
using UnityEditor;
using UnityEngine;

public sealed class ConditionalValueDropdownAttributeDrawer<T> : OdinAttributeDrawer<ConditionalValueDropdownAttribute, T>
{
    private ValueResolver<bool> _conditionResolver;
    private ValueResolver<IEnumerable> _valuesResolver;

    protected override void Initialize()
    {
        _conditionResolver = ValueResolver.Get<bool>(
            Property,
            Attribute.Condition
        );

        _valuesResolver = ValueResolver.Get<IEnumerable>(
            Property,
            Attribute.Values
        );
    }

    protected override void DrawPropertyLayout(GUIContent label)
    {
        ValueResolver.DrawErrors(_conditionResolver, _valuesResolver);

        if (_conditionResolver.HasError || _valuesResolver.HasError)
        {
            CallNextDrawer(label);
            return;
        }

        bool shouldUseDropdown = _conditionResolver.GetValue();

        if (!shouldUseDropdown)
        {
            CallNextDrawer(label);
            return;
        }

        IEnumerable rawValues = _valuesResolver.GetValue();

        if (rawValues == null)
        {
            EditorGUILayout.HelpBox(
                $"ConditionalValueDropdown: values source '{Attribute.Values}' returned null.",
                MessageType.Warning
            );

            CallNextDrawer(label);
            return;
        }

        List<DropdownEntry> entries = BuildEntries(rawValues);

        if (entries.Count == 0)
        {
            EditorGUILayout.HelpBox(
                $"ConditionalValueDropdown: values source '{Attribute.Values}' returned no compatible values for type '{typeof(T).Name}'.",
                MessageType.Warning
            );

            CallNextDrawer(label);
            return;
        }

        T currentValue = ValueEntry.SmartValue;

        int selectedIndex = FindCurrentIndex(entries, currentValue);

        if (selectedIndex < 0 && Attribute.IncludeCurrentValueIfMissing)
        {
            entries.Insert(0, new DropdownEntry(
                $"Current: {GetDisplayName(currentValue)}",
                currentValue
            ));

            selectedIndex = 0;
        }

        string[] labels = new string[entries.Count];

        for (int i = 0; i < entries.Count; i++)
            labels[i] = entries[i].Label;

        EditorGUI.BeginChangeCheck();

        int newIndex = EditorGUILayout.Popup(
            label,
            Mathf.Max(0, selectedIndex),
            labels
        );

        if (EditorGUI.EndChangeCheck())
        {
            ValueEntry.SmartValue = entries[newIndex].Value;
        }
    }

    private static List<DropdownEntry> BuildEntries(IEnumerable rawValues)
    {
        var entries = new List<DropdownEntry>();

        foreach (object rawItem in rawValues)
        {
            if (TryCreateEntry(rawItem, out DropdownEntry entry))
                entries.Add(entry);
        }

        return entries;
    }

    private static bool TryCreateEntry(object rawItem, out DropdownEntry entry)
    {
        entry = default;

        if (TryReadValueDropdownItem(rawItem, out string customLabel, out object customValue))
        {
            if (!TryConvert(customValue, out T convertedValue))
                return false;

            entry = new DropdownEntry(customLabel, convertedValue);
            return true;
        }

        if (!TryConvert(rawItem, out T value))
            return false;

        entry = new DropdownEntry(GetDisplayName(rawItem), value);
        return true;
    }

    private static bool TryReadValueDropdownItem(
        object rawItem,
        out string label,
        out object value
    )
    {
        label = null;
        value = null;

        if (rawItem == null)
            return false;

        Type type = rawItem.GetType();

        if (!type.IsGenericType)
            return false;

        if (type.GetGenericTypeDefinition() != typeof(ValueDropdownItem<>))
            return false;

        var textField = type.GetField(nameof(ValueDropdownItem<int>.Text));
        var valueField = type.GetField(nameof(ValueDropdownItem<int>.Value));

        if (textField == null || valueField == null)
            return false;

        label = textField.GetValue(rawItem) as string;
        value = valueField.GetValue(rawItem);

        if (string.IsNullOrEmpty(label))
            label = GetDisplayName(value);

        return true;
    }

    private static bool TryConvert(object rawValue, out T value)
    {
        value = default;

        Type targetType = typeof(T);
        Type nullableType = Nullable.GetUnderlyingType(targetType);
        Type actualTargetType = nullableType ?? targetType;

        if (rawValue == null)
        {
            if (!targetType.IsValueType || nullableType != null)
            {
                value = default;
                return true;
            }

            return false;
        }

        if (rawValue is T typedValue)
        {
            value = typedValue;
            return true;
        }

        try
        {
            if (actualTargetType.IsEnum)
            {
                if (rawValue is string stringValue)
                {
                    value = (T)Enum.Parse(actualTargetType, stringValue);
                    return true;
                }

                value = (T)Enum.ToObject(actualTargetType, rawValue);
                return true;
            }

            object converted = Convert.ChangeType(rawValue, actualTargetType);
            value = (T)converted;
            return true;
        }
        catch
        {
            return false;
        }
    }

    private static int FindCurrentIndex(List<DropdownEntry> entries, T currentValue)
    {
        var comparer = EqualityComparer<T>.Default;

        for (int i = 0; i < entries.Count; i++)
        {
            if (comparer.Equals(entries[i].Value, currentValue))
                return i;
        }

        return -1;
    }

    private static string GetDisplayName(object value)
    {
        return value == null ? "Null" : value.ToString();
    }

    private readonly struct DropdownEntry
    {
        public readonly string Label;
        public readonly T Value;

        public DropdownEntry(string label, T value)
        {
            Label = label;
            Value = value;
        }
    }
}
#endif