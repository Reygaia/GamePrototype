using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Parry_Skill : Skill
{
    [Header("Parry")]
    [SerializeField] private UI_SkillTreeSlot parryUnlockBtn;
    public bool parryUnlock { get; private set; }

    [Header("Parry Restore")]
    [SerializeField] private UI_SkillTreeSlot parryRestoreUnlockBtn;
    [Range(0f, 1f)]
    [SerializeField] private float restoreHealthAmount;
    public bool parryRestoreUnlock { get; private set; }

    [Header("Parry with mirage")]
    [SerializeField] private UI_SkillTreeSlot parryMirageUnlockBtn;
    public bool parryMirageUnlock { get; private set; }


    public override void UseSkill()
    {
        base.UseSkill();
        if (parryRestoreUnlock)
        {
            int restoreAmount = Mathf.RoundToInt(player.stats.GetMaxhealth() * restoreHealthAmount);
            player.stats.IncreaseHealthBy(restoreAmount);
        }
    }

    protected override void Start()
    {
        base.Start();

        parryUnlockBtn.GetComponent<Button>().onClick.AddListener(UnlockParry);
        parryRestoreUnlockBtn.GetComponent<Button>().onClick.AddListener(UnlockParryRestore);
        parryMirageUnlockBtn.GetComponent<Button>().onClick.AddListener(UnlockParryMirage);
    }

    private void UnlockParry()
    {
        if (parryUnlockBtn.unlocked)
            parryUnlock = true;
    }

    private void UnlockParryRestore()
    {
        if(parryRestoreUnlockBtn.unlocked)
            parryRestoreUnlock = true;
    }

    private void UnlockParryMirage()
    {
        if(parryMirageUnlockBtn.unlocked)
            parryMirageUnlock = true;
    }

    public void MakeMirageOnParry(Transform _respawnTransform)
    {
        if (parryMirageUnlock)
            SkillManager.instance.clone.CreateCloneWithDelay(_respawnTransform);
    }
}
