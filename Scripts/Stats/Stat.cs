using System;

public sealed class Stat
{
    private readonly string _key;

    private int _value;

    public event Action<int> OnValueChanged;

    public int Value
    {
        get => _value;
        set
        {
            _value = value;
            Prefs.SaveVariable(_value, _key);
            OnValueChanged?.Invoke(value);
        }
    }

    public Stat(string key, int defaultValue)
    {
        _key = key;
        LoadValue(defaultValue);
    }

    private void LoadValue(int defaultValue)
    {
        _value = Prefs.LoadVariable(_key, defaultValue);
    }
}