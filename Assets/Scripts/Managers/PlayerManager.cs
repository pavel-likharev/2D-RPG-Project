using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    public static PlayerManager Instance { get; private set; }

    private int currency = 1000000;

    [field: SerializeField] public Player Player { get; private set; }

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(Instance.gameObject);
        }
        else
        {
            Instance = this;
        }
    }

    public bool HaveEnoughCurrency(int price)
    {
        if (price > currency)
        {
            Debug.Log("Not enough money");
            return false;
        }

        currency -= price;

        UI.Instance.InGame.SetCurrencyText(currency);

        return true;
    }

    public int GetCurrency() => currency;

    public void AddCurrency(int value)
    {
        currency += value;

        UI.Instance.InGame.SetCurrencyText(currency);
    }

}
