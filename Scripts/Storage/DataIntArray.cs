using System;

public sealed class DataIntArray
{
    private readonly string _key;

    private int[] _value;

    public event Action<int[]> OnValueChanged;

    public int[] Value
    {
        get => _value;
        set
        {
            _value = value;
            Prefs.SaveArray(_value, _key);
            OnValueChanged?.Invoke(value);
        }
    }

    public DataIntArray(string key)
    {
        _key = key;
        LoadValue();
    }

    private void LoadValue()
    {
        _value = Prefs.LoadArray(_key);
    }
}