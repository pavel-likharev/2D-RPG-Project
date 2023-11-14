using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class DashSkill : MonoBehaviour
{
    [Header("Dash info")]
    public float dashForce = 12f;
    public float dashDuration = 0.3f;
    public float DashDir { get; private set; }
    public float defaultDashForce;

    private Player player;

    private void Start()
    {
        player = PlayerManager.Instance.Player;
    }

    public void SetupDash()
    {
        DashDir = Input.GetAxisRaw("Horizontal");

        if (DashDir == 0)
            DashDir = player.MoveDir;
    }
}
