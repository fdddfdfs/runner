using System;
using System.Collections.Generic;
using System.Linq;

public sealed class DataStringArray
{
    private readonly string _key;

    private List<string> _value;

    public event Action<IReadOnlyList<string>> OnValueChanged;

    public IReadOnlyList<string> Value => _value;

    public void AddElement(string newElement)
    {
        _value.Add(newElement);
        Prefs.SaveArray(_value.ToArray(), _key);
        OnValueChanged?.Invoke(_value);
    }

    public DataStringArray(string key)
    {
        _key = key;
        LoadValue();
    }

    private void LoadValue()
    {
        _value = new List<string>(Prefs.LoadStringArray(_key));
    }
}