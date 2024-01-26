using UnityEngine;
using UnityEngine.UI;

public class Dodge_Skill : Skill
{
    [Header("Dodge")]
    [SerializeField] private int evasionAmount;
    [SerializeField] private UI_SkillTreeSlot unlockDodgeBtn;
    public bool dodgeUnlock;

    [Header("Mirage Dodge")]
    [SerializeField] private UI_SkillTreeSlot unlockMirageDodgeBtn;
    public bool dodgeMirageUnlock;

    protected override void Start()
    {
        base.Start();

        unlockDodgeBtn.GetComponent<Button>().onClick.AddListener(UnlockDodge);
        unlockMirageDodgeBtn.GetComponent<Button>().onClick.AddListener(UnlockDodgeMirage);
    }

    private void UnlockDodge()
    {
        if (unlockDodgeBtn.unlocked && !dodgeUnlock)
        {
            player.stats.evasion.AddModifier(evasionAmount);
            Inventory.instance.UpdateStatsUI();
            dodgeUnlock = true;
        }
    }

    private void UnlockDodgeMirage()
    {
        if (unlockMirageDodgeBtn.unlocked)
            dodgeMirageUnlock = true;
    }

    public void CreateMirageOnDodge()
    {
        if (dodgeMirageUnlock)
            SkillManager.instance.clone.CreateClone(player.transform, new Vector3(2 * player.facingDir,0));
    }
}
