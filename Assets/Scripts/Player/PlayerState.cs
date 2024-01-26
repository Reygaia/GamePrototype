using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerState
{
    protected PlayerStateMachine stateMachine;
    protected Player player;

    private string animBoolName;
    protected Rigidbody2D rb;

    #region Input
    protected float xInput;
    protected float yInput;
    #endregion

    #region stateTimer
    protected float wallSlideStateTimeDefault = .2f;
    protected float wallSlideStatetimer;
    protected float stateTimer;
    #endregion

    protected bool triggerCalled;

    public PlayerState(Player _player, PlayerStateMachine _stateMachine,string _animboolname)
    {
        this.player = _player;
        this.stateMachine = _stateMachine;
        this.animBoolName = _animboolname;
    }

    public virtual void Enter() 
    {
        player.anim.SetBool(animBoolName, true);
        rb = player.rb;
        triggerCalled = false;
    }
    public virtual void Update() 
    {
        xInput = Input.GetAxisRaw("Horizontal");
        yInput = Input.GetAxisRaw("Vertical");
        stateTimer -= Time.deltaTime;

        player.anim.SetFloat("yVelocity",rb.velocity.y);
    }
    public virtual void Exit() 
    {
        player.anim.SetBool(animBoolName, false);
    }
    public virtual void AnimationFinishTrigger()
    {
        triggerCalled = true;
    }
}
