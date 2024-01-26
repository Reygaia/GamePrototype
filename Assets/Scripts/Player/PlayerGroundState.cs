using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerGroundState : PlayerState
{
    public PlayerGroundState(Player _player, PlayerStateMachine _stateMachine, string _animboolname) : base(_player, _stateMachine, _animboolname)
    {
    }

    public override void Enter()
    {
        base.Enter();
        rb.gravityScale = 3;
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void Update()
    {
        base.Update();

        if (Input.GetKeyDown(KeyCode.R) && player.skill.blackHole.blackholeUnlocked)
            stateMachine.ChangeState(player.blackHoleState);

        if (Input.GetKeyDown(KeyCode.Mouse1) && HasNoSword() && player.skill.sword.swordUnlock)
            stateMachine.ChangeState(player.aimSwordState);

        //calculate cooldown
        player.counterAttackCooldownTimer -= Time.deltaTime;

        if (Input.GetKeyDown(KeyCode.F) && player.counterAttackCooldownTimer < 0 && player.skill.parry.parryUnlock)
        {
            player.counterAttackCooldownTimer = player.counterAttackCooldown;
            stateMachine.ChangeState(player.counterAttackState);
        }


        //attack input
        if (Input.GetKeyDown(KeyCode.Mouse0))
            stateMachine.ChangeState(player.primaryAttackState);


        //detect ground
        if (!player.IsGroundDetected())
            stateMachine.ChangeState(player.airState);


        //detect ground to jump
        if (Input.GetKeyDown(KeyCode.Space) && player.IsGroundDetected())
            stateMachine.ChangeState(player.jumpState);
    }

    private bool HasNoSword()
    {
        if (!player.sword)
        {
            Debug.Log("Has no sword");
            return true;
        }
        Debug.Log("has sword");
        player.sword.GetComponent<Sword_Skill_Controller>().ReturnSword();
        return false;
    }
}
