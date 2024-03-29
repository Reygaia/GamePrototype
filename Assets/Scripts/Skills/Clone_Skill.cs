using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class Clone_Skill : Skill
{
    [Header("Clone info")]
    [SerializeField] private float attackMultiplier;
    [SerializeField] private float cloneDuration;
    [SerializeField] private GameObject clonePrefab;
    [Space]
    [Header("Clone Attack")]
    [SerializeField] private UI_SkillTreeSlot cloneAttackUnlockBtn;
    [SerializeField] private float cloneAttackMultiplier;
    [SerializeField] private bool canAttack;

    [Header("Agressive clone Unlock")]
    [SerializeField] private UI_SkillTreeSlot agressiveCloneAttackUnlockBtn;
    [SerializeField] private float agressiveCloneAttackMultiplier;
    public bool canApplyOnHitEffect { get; private set; }

    [Header("Duplicate Clone")]
    [SerializeField] private UI_SkillTreeSlot MultiCloneUnlockBtn;
    [SerializeField] private float multiCloneAttackMultiplier;
    [SerializeField] private bool canDuplicateClone;
    [SerializeField] private float chanceToDuplicate;

    [Header("Crystal Instead of clone")]
    [SerializeField] private UI_SkillTreeSlot CrystalCloneUnlockBtn;
    public bool crystalInsteadOfClone;

    protected override void Start()
    {
        base.Start();

        cloneAttackUnlockBtn.GetComponent<Button>().onClick.AddListener(UnlockCloneAttack);
        agressiveCloneAttackUnlockBtn.GetComponent<Button>().onClick.AddListener(UnlockAgressiveClone);
        MultiCloneUnlockBtn.GetComponent<Button>().onClick.AddListener(UnlockMultiClone);
        CrystalCloneUnlockBtn.GetComponent<Button>().onClick.AddListener(UnlockCrystalClone);
    }

    #region unlock region

    private void UnlockCloneAttack()
    {
        if (cloneAttackUnlockBtn.unlocked)
        {
            canAttack = true;
            attackMultiplier = cloneAttackMultiplier;
        }
    }

    private void UnlockAgressiveClone()
    {
        if (agressiveCloneAttackUnlockBtn.unlocked)
        {
            canApplyOnHitEffect = true;
            attackMultiplier = agressiveCloneAttackMultiplier;
        }
    }

    private void UnlockMultiClone()
    {
        if (MultiCloneUnlockBtn.unlocked)
        {
            canDuplicateClone = true;
            attackMultiplier = multiCloneAttackMultiplier;
        }
    }

    private void UnlockCrystalClone()
    {
        if (CrystalCloneUnlockBtn.unlocked)
        {
            crystalInsteadOfClone = true;
        }
    }




    #endregion

    public void CreateClone(Transform _clonePosition, Vector3 _offset)
    {
        if (crystalInsteadOfClone)
        {
            SkillManager.instance.crystal.CreateCrystal();
            return;
        }

        GameObject newClone = Instantiate(clonePrefab);

        newClone.GetComponent<Clone_Skill_Controller>()
                .SetupClone(_clonePosition, cloneDuration, canAttack, _offset, FindClosetEnemy(newClone.transform), canDuplicateClone, chanceToDuplicate, player, attackMultiplier);
    }

    public void CreateCloneWithDelay(Transform _enemyTransform)
    {
        StartCoroutine(CloneDelayCoroutine(_enemyTransform, new Vector2(.5f * player.facingDir, 0)));
    }

    private IEnumerator CloneDelayCoroutine(Transform _transform, Vector2 _offset)
    {
        yield return new WaitForSeconds(.2f);
        CreateClone(_transform, _offset);
    }
}
