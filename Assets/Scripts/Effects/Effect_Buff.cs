using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Effect_Buff : Effect
{
    private PlayerStats stats;
    [SerializeField] private StatType buffType;
    [SerializeField] private int buffValue;
    [SerializeField] private float buffDuartion;

    public override void ExecuteEffect(Transform target)
    {
        stats = PlayerManager.Instance.Player.GetComponent<PlayerStats>();

        stats.IncreaseStat(stats.GetStatFromType(buffType), buffValue, buffDuartion);
    }


}
