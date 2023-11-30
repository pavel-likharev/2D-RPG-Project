using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour, ISavePoint
{
    public static PlayerManager Instance { get; private set; }

    [SerializeField] private int currency;

    public Player Player { get; private set; }

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
        }

        Player = FindObjectOfType<Player>();
        Debug.Log("Player in PM " + Player != null);
    }

    public bool HaveEnoughCurrency(int price)
    {
        if (price > currency)
        {
            Debug.Log("Not enough money");
            return false;
        }

        currency -= price;

        return true;
    }

    public int GetCurrency() => currency;

    public void AddCurrency(int value)
    {
        currency += value;
    }

    public void SetZeroCurrency()
    {
        currency = 0;
    }

    public void LoadData(GameData data)
    {
        currency = data.currency;
    }

    public void SaveData(ref GameData data)
    {
        data.currency = currency;
    }

}
