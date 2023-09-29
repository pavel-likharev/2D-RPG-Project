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

    [Header("Battle info")]
    public float agressiveDistance = 10f;
    public float visibleDistance = 2f;
    public float attackDistance = 1f;
    public float attackCooldown = 2f;
    public float battleTime = 2f;
    [HideInInspector] public float lastTimeAttack;


    public EnemyStateMachine StateMachine { get; private set; }


    protected override void Awake()
    {
        base.Awake();

        StateMachine = new EnemyStateMachine();

        playerLayer = LayerMask.GetMask("Player");
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

    public void AnimationTrigger() => StateMachine.CurrentState.AnimationFinishTrigger();

    #region Collusion
    public RaycastHit2D IsPlayerDetected() => Physics2D.Raycast(playerCheck.position, Vector2.right * MoveDir, playerCheckDistance, playerLayer);

    protected override void OnDrawGizmos()
    {
        base.OnDrawGizmos();

        Gizmos.DrawLine(playerCheck.position, new Vector3(playerCheck.position.x + playerCheckDistance * MoveDir, playerCheck.position.y));
        Gizmos.color = Color.yellow;
        Gizmos.DrawLine(playerCheck.position, new Vector3(playerCheck.position.x + attackDistance * MoveDir, playerCheck.position.y));
    }
    #endregion
}
