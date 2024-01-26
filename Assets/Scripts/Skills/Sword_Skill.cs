using System;
using System.Net.NetworkInformation;
using UnityEngine;
using UnityEngine.UI;

public enum SwordType
{
    Regular,
    Bounce,
    Pierce,
    Spin
}

public class Sword_Skill : Skill
{
    public SwordType swordType = SwordType.Regular;

    [Header("Bounce info")]
    [SerializeField] private UI_SkillTreeSlot bounceUnlockBtn;
    [SerializeField] private int bounceAmount;
    [SerializeField] private float bounceGravity;
    [SerializeField] private float bounceSpeed;

    [Header("Pierce Info")]
    [SerializeField] private UI_SkillTreeSlot pierceUnlockBtn;
    [SerializeField] private int pierceAmount;
    [SerializeField] private float pierceGravity;

    [Header("Spin info")]
    [SerializeField] private UI_SkillTreeSlot spinUnlockBtn;
    [SerializeField] private float hitCooldown = .3f;
    [SerializeField] private float maxTravelDistance = 7;
    [SerializeField] private float spinDuration = 2;
    [SerializeField] private float spinGravity;

    [Header("Skill info")]
    [SerializeField] private UI_SkillTreeSlot swordUnlockBtn;
    public bool swordUnlock { get; private set; }
    [SerializeField] private GameObject swordPrefab;
    [SerializeField] private Vector2 launchForce;
    [SerializeField] private float swordGravity;
    [SerializeField] private float freezeTime;
    [SerializeField] private float returnSpeed;

    [Header("Passive Skills")]
    [SerializeField] private UI_SkillTreeSlot timeStopUnlockBtn;
    public bool timeStopUnlock {  get; private set; }
    [SerializeField] private UI_SkillTreeSlot vulnerableUnlockBtn;
    public bool vulnerableUnlock { get; private set; }


    private Vector2 finalDir;

    [Header("Aim dots")]
    [SerializeField] private int numberOfDots;
    [SerializeField] private float spaceBetweenDots;
    [SerializeField] private GameObject dotPrefab;
    [SerializeField] private Transform dotsParent;

    private GameObject[] dots;

    protected override void Start()
    {
        base.Start();

        GenerateDots();
        SetupGravity();

        swordUnlockBtn.GetComponent<Button>().onClick.AddListener(UnlockSword);
        pierceUnlockBtn.GetComponent<Button>().onClick.AddListener(UnlockPierceSword);
        bounceUnlockBtn.GetComponent<Button>().onClick.AddListener(UnlockBounceSword);
        spinUnlockBtn.GetComponent<Button>().onClick.AddListener(UnlockSpinSword);
        timeStopUnlockBtn.GetComponent<Button>().onClick.AddListener(UnlockTimeStop);
        vulnerableUnlockBtn.GetComponent<Button>().onClick.AddListener(UnlockVulnerableSword);
    }

    private void SetupGravity()
    {
        if (swordType == SwordType.Pierce)
            swordGravity = pierceGravity;
        else if(swordType == SwordType.Bounce)
            swordGravity = bounceGravity;
        else if(swordType == SwordType.Spin)
            swordGravity = spinGravity;
    }

    protected override void Update()
    {
        if (Input.GetKeyUp(KeyCode.Mouse1))
            finalDir = new Vector2(AimDirection().normalized.x * launchForce.y, AimDirection().normalized.y * launchForce.y);

        if (Input.GetKey(KeyCode.Mouse1))
        {
            for (int i = 0; i < dots.Length; i++)
            {
                dots[i].transform.position = DotsPosition(i * spaceBetweenDots);
            }
        }
    }

    #region unlock region

    private void UnlockTimeStop()
    {
        if (timeStopUnlockBtn.unlocked)
            timeStopUnlock = true;
    }

    private void UnlockVulnerableSword()
    {
        if (vulnerableUnlockBtn.unlocked)
            vulnerableUnlock = true;
    }

    private void UnlockSword()
    {
        if(swordUnlockBtn.unlocked)
        {
            swordType = SwordType.Regular;
            swordUnlock = true;
        }
    }

    private void UnlockBounceSword()
    {
        if (bounceUnlockBtn.unlocked)
            swordType = SwordType.Bounce;
    }
    private void UnlockPierceSword()
    {
        if (bounceUnlockBtn.unlocked)
            swordType = SwordType.Pierce;
    }
    private void UnlockSpinSword()
    {
        if (bounceUnlockBtn.unlocked)
            swordType = SwordType.Spin;
    }



    #endregion


    public void CreateSword()
    {
        GameObject newSword = Instantiate(swordPrefab, player.transform.position, transform.rotation);
        Sword_Skill_Controller newSwordScipt = newSword.GetComponent<Sword_Skill_Controller>();

        if (swordType == SwordType.Bounce)
            newSwordScipt.SetupBounce(true, bounceAmount,bounceSpeed);
        else if (swordType == SwordType.Pierce)
            newSwordScipt.SetupPierce(pierceAmount);
        else if (swordType == SwordType.Spin)
            newSwordScipt.SetupSpin(true, maxTravelDistance, spinDuration, hitCooldown);


        newSwordScipt.SetupSword(finalDir, swordGravity, player, freezeTime,returnSpeed);

        player.AssignNewSword(newSword);

        DotsActive(false);
    }

    #region Aim sword
    public Vector2 AimDirection()
    {
        Vector2 playerPosition = player.transform.position;
        Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector2 direction = mousePosition - playerPosition;

        return direction;
    }

    public void DotsActive(bool _isActive)
    {
        for (int i = 0; i < dots.Length; i++)
        {
            dots[i].SetActive(_isActive);
        }
    }

    private void GenerateDots()
    {
        dots = new GameObject[numberOfDots];
        for (int i = 0; i < numberOfDots; i++)
        {
            dots[i] = Instantiate(dotPrefab, player.transform.position, Quaternion.identity, dotsParent);
            dots[i].SetActive(false);
        }
    }

    private Vector2 DotsPosition(float t)
    {
        Vector2 position = (Vector2)player.transform.position + new Vector2(
            AimDirection().normalized.x * launchForce.x,
            AimDirection().normalized.y * launchForce.y) * t + .5f * (Physics2D.gravity * swordGravity) * (t * t);

        return position;
    }
    #endregion
}
