using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LostCurrencyController : MonoBehaviour
{
    public int Currency { get; private set; }

    public void AddCurrency(int currency) => Currency = currency;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.GetComponent<Player>() != null)
        {
            PlayerManager.Instance.AddCurrency(Currency);
            Destroy(gameObject);
        }
    }
}
