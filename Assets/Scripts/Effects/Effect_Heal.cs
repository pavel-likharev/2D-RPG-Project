using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Effect_Heal : Effect
{
    [Range(0f, 1f)][SerializeField] private float healPercientage;

    public override void ExecuteEffect(Transform target)
    {
        PlayerStats playerStats = PlayerManager.Instance.Player.GetComponent<PlayerStats>();

        int healValue = Mathf.RoundToInt(playerStats.GetMaxHealthValue() * healPercientage);

        playerStats.IncreaseHealth(healValue);
    }
}
