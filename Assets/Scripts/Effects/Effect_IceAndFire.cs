using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Effect_IceAndFire : Effect
{
    [SerializeField] private float xVelocity;

    public override void ExecuteEffect(Transform target)
    {
        Player player = PlayerManager.Instance.Player;

        bool isThirdAttack = player.PrimaryAttackState.ComboCounter == 2;

        if (isThirdAttack)
        {
            GameObject newIceAndFire = Instantiate(this.gameObject, target.position, Quaternion.identity);
            newIceAndFire.transform.localScale = new Vector3(player.MoveDir, 1);
            newIceAndFire.GetComponent<Rigidbody2D>().velocity = new Vector2(player.MoveDir * xVelocity, 0);

            Destroy(newIceAndFire, 3f);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.GetComponent<Enemy>() != null)
        {
            PlayerStats playerStats = PlayerManager.Instance.Player.GetComponent<PlayerStats>();
            EnemyStats enemy = collision.GetComponent<EnemyStats>();

            playerStats.DoMagicalDamage(enemy, (int)Mathf.Sign(transform.localScale.x));
        }
    }
}
