using System.Collections;
using System.Collections.Generic;
using Unity.IO.LowLevel.Unsafe;
using UnityEngine;

public class Enemy : Character
{

    [SerializeField] protected Transform playerCheck;
    [SerializeField] protected float playerCheckDistance = 5f;
    protected LayerMask playerLayer;

    [Header("Move info")]
    [SerializeField] public float moveSpeed = 1.5f;
    [SerializeField] public float idleTime = 1.5f;

    public EnemyStateMachine StateMachine { get; private set; }


    protected override void Awake()
    {
        base.Awake();

        StateMachine = new EnemyStateMachine();

        groundLayer = LayerMask.GetMask("Player");
    }

    protected override void Start()
    {
        base.Start();

    }

    protected override void Update()
    {
        base.Update();

        StateMachine.CurrentState.Update();
    }

    #region Collusion
    public RaycastHit2D IsPlayerDetected() => Physics2D.Raycast(playerCheck.position, Vector2.right * MoveDir, playerCheckDistance, playerLayer);

    protected override void OnDrawGizmos()
    {
        Gizmos.DrawLine(playerCheck.position, new Vector3(playerCheck.position.x + playerCheckDistance, playerCheck.position.y));
    }
    #endregion
}
