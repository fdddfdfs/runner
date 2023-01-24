using System;
using TMPro;
using UnityEngine;

public class StatsMenu : MonoBehaviour
{
    [SerializeField] private GameObject _menu;
    [SerializeField] private TMP_Text _balance;
    [SerializeField] private TMP_Text _record;

    public void ChangeActiveState(bool state)
    {
        _menu.SetActive(state);

        if (state)
        {
            _balance.text = Stats.Money.Value.ToString();
            _record.text = Stats.Record.Value.ToString();
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

        _balance.text = newValue.ToString();
    }
}