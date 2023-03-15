using TMPro;
using UnityEngine;

public sealed class StatsMenu : MonoBehaviour
{
    [SerializeField] private GameObject _menu;
    [SerializeField] private TMP_Text _balanceValue;
    [SerializeField] private TMP_Text _recordValue;

    public void ChangeActiveState(bool state)
    {
        _menu.SetActive(state);

        if (state)
        {
            _balanceValue.text = Stats.Money.Value.ToString();
            _recordValue.text = Stats.Record.Value.ToString();
        }
    }

    private void OnEnable()
    {
        Stats.Money.OnValueChanged += UpdateBalance;
    }

    private void OnDisable()
    {
        Stats.Money.OnValueChanged -= UpdateBalance;
    }

    private void UpdateBalance(int newValue)
    {
        if (!_menu.activeSelf) return;

        _balanceValue.text = newValue.ToString();
    }
}