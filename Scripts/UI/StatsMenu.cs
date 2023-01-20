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
            _balance.text = Stats.Money.ToString();
            _record.text = Stats.Record.ToString();
        }
    }
}