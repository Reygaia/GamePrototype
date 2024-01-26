using UnityEngine;

public class PlayerBlackHoleState : PlayerState
{
    private float flyTime = .4f;
    private bool skillUsed;

    private float defaultGravity;

    public PlayerBlackHoleState(Player _player, PlayerStateMachine _stateMachine, string _animboolname) : base(_player, _stateMachine, _animboolname)
    {
    }

    public override void AnimationFinishTrigger()
    {
        base.AnimationFinishTrigger();
    }

    public override void Enter()
    {
        base.Enter();

        defaultGravity = player.rb.gravityScale;

        skillUsed = false;
        stateTimer = flyTime;
        rb.gravityScale = 0;
    }

    public override void Exit()
    {
        base.Exit();

        player.rb.gravityScale = defaultGravity;
        player.fx.MakeTransparent(false);
    }

    public override void Update()
    {
        base.Update();

        if (stateTimer > 0)
            rb.velocity = new Vector2(0, 15);
        if (stateTimer < 0)
        {
            rb.velocity = new Vector2(0, -.1f);
            if (!skillUsed)
            {
                if(player.skill.blackHole.CanUseSkill())
                    skillUsed = true;
            }
        }

        if (player.skill.blackHole.SkillCompleted())
            stateMachine.ChangeState(player.airState);
    }
}
