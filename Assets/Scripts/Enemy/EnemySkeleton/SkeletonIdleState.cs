using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkeletonIdleState : SkeletonGroundState
{

    public SkeletonIdleState(Enemy _enemyBase, EnemyStateMachine _statemachine, string _animBoolName, Enemy_Skeleton _enemy) : base(_enemyBase, _statemachine, _animBoolName, _enemy)
    {
    }

    public override void Enter()
    {
        base.Enter();
        stateTimer = enemy.idleTime + Random.Range(0,3);
    }

    public override void Exit()
    {
        base.Exit();


    }

    public override void Update()
    {
        base.Update();

        if (stateTimer < 0)
            stateMachine.ChangeState(enemy.moveState);
    }
}
