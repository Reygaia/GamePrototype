using UnityEngine;

public class SkeletonMoveState : SkeletonGroundState
{
    //private float randomDir;

    public SkeletonMoveState(Enemy _enemyBase, EnemyStateMachine _statemachine, string _animBoolName, Enemy_Skeleton _enemy) : base(_enemyBase, _statemachine, _animBoolName, _enemy)
    {
    }

    public override void Enter()
    {
        base.Enter();

        //randomDir = GetRandomValue();
    }

    public override void Exit()
    {
        base.Exit();

    }

    //private float GetRandomValue()
    //{
    //    float randomValue;

    //    do
    //    {
    //        randomValue = Random.Range(-1, 1);
    //    }while(Mathf.Approximately(randomValue, 0f));

    //    return randomValue;
    //}

    public override void Update()
    {
        base.Update();

        enemy.SetVelocity(enemy.moveSpeed * enemy.facingDir, rb.velocity.y);


        if (enemy.IsWallDetected() || !enemy.IsGroundDetected())
        {
            enemy.Flip();
            stateMachine.ChangeState(enemy.idleState);
        }

    }


}
