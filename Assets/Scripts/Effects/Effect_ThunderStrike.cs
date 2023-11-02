using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Effect_ThunderStrike : Effect
{
    public override void ExecuteEffect(Transform target)
    {
        GameObject newThunderStrike = Instantiate(this.gameObject, target.position, Quaternion.identity);

        Destroy(newThunderStrike, 1f);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.GetComponent<Enemy>() != null)
        {
            PlayerStats playerStats = PlayerManager.Instance.Player.GetComponent<PlayerStats>();
            EnemyStats enemy = collision.GetComponent<EnemyStats>();

            playerStats.DoMagicalDamage(enemy, 0);
        }
    }
}
