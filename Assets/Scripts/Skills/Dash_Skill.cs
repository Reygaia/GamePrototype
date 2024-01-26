using UnityEngine;
using UnityEngine.UI;

public class Dash_Skill : Skill
{
    [Header("Dash")]
    [SerializeField] private UI_SkillTreeSlot dashUnlockbtn;
    public bool dashUnlocked { get; private set; }

    [Header("Clone on dash")]
    [SerializeField] private UI_SkillTreeSlot cloneOnDashUnlockedbtn;
    public float cloneOnDashCooldown;
    private float cloneOnDashTimer;

    public bool cloneOnDashUnlocked { get; private set; }

    [Header("Clone on arrival")]
    [SerializeField] private UI_SkillTreeSlot cloneOnArrivalUnlockedbtn;
    public float cloneOnArrivalCooldown;
    private float cloneOnArrivalTimer;
    public bool cloneOnArrivalUnlocked { get; private set; }

    public override void UseSkill()
    {
        base.UseSkill();
    }

    protected override void Update()
    {
        base.Update();
        cloneOnDashTimer -= Time.deltaTime;
        cloneOnArrivalTimer -= Time.deltaTime;

    }

    protected override void Start()
    {
        base.Start();

        dashUnlockbtn.GetComponent<Button>().onClick.AddListener(UnlockDash);
        cloneOnDashUnlockedbtn.GetComponent<Button>().onClick.AddListener(UnlockCloneOnDash);
        cloneOnArrivalUnlockedbtn.GetComponent<Button>().onClick.AddListener(UnlockCloneOnArrival);
    }

    private void UnlockDash()
    {
        if (dashUnlockbtn.unlocked)
            dashUnlocked = true;
    }

    private void UnlockCloneOnDash()
    {
        if (cloneOnDashUnlockedbtn.unlocked)
            cloneOnDashUnlocked = true;
    }

    private void UnlockCloneOnArrival()
    {
        if (cloneOnArrivalUnlockedbtn.unlocked)
            cloneOnArrivalUnlocked = true;
    }
    public void CloneOnDash()
    {
        if (cloneOnDashUnlocked && cloneOnDashTimer < 0)
        {
            SkillManager.instance.clone.CreateClone(player.transform, new Vector2(1 * player.facingDir, 0));
            cloneOnDashTimer = cloneOnDashCooldown;
        }
    }
    public void CloneOnArrival()
    {
        if (cloneOnArrivalUnlocked && cloneOnArrivalTimer < 0)
        {
            SkillManager.instance.clone.CreateClone(player.transform, new Vector2(1 * player.facingDir, 0));
            cloneOnArrivalTimer = cloneOnArrivalCooldown;
        }
    }

}
