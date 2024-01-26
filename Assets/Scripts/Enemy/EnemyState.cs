using UnityEngine;

public class EnemyState
{
    protected EnemyStateMachine stateMachine;
    protected Enemy enemyBase;
    protected Rigidbody2D rb;

    private string animBoolName;

    protected bool triggerCalled;
    protected float stateTimer;

    public EnemyState(Enemy _enemyBase, EnemyStateMachine _statemachine, string _animBoolName)
    {
        this.enemyBase = _enemyBase;
        this.stateMachine = _statemachine;
        this.animBoolName = _animBoolName;
    }

    public virtual void Enter()
    {
        triggerCalled = false;
        enemyBase.anim.SetBool(animBoolName, true);
        rb = enemyBase.rb;
    }

    public virtual void Update()
    {
        stateTimer -= Time.deltaTime;
    }

    public virtual void Exit()
    {
        enemyBase.anim.SetBool(animBoolName, false);
        enemyBase.AssignLastAnimName(animBoolName);
    }
    public virtual void AnimationFinishTrigger()
    {
        triggerCalled = true;
    }
}
