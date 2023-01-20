public class Stat
{
    private int _value;
    private string _key;

    public int Value
    {
        get => _value;
        set
        {
            _value = value;
            Prefs.SaveVariable(_value, nameof(_key));
        }
    }

    public Stat(string key, int defaultValue)
    {
        _key = key;
        LoadValue(defaultValue);
    }

    public void LoadValue(int defaultValue)
    {
        _value = Prefs.LoadVariable(_key, defaultValue);
    }
}