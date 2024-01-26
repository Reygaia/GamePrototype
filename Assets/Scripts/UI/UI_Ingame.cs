using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UI_Ingame : MonoBehaviour
{
    [SerializeField] private PlayerStats playerStats;
    [SerializeField] private Slider slider;
    // Start is called before the first frame update

    [SerializeField] private Image cloneOnDashImage;
    [SerializeField] private Image parryImage;
    [SerializeField] private Image crystalImage;
    [SerializeField] private Image swordImage;
    [SerializeField] private Image blackholeImage;
    [SerializeField] private Image flaskImage;

    [SerializeField] private TextMeshProUGUI currentSouls;

    private SkillManager skills;

    void Start()
    {
        if (playerStats != null)
            playerStats.onHealthChanged += UpdateHealthUI;

        skills = SkillManager.instance;
        //cloneOnArrivalCooldown = SkillManager.instance.dash.cloneOnArrivalCooldown;
    }

    // Update is called once per frame
    void Update()
    {
        currentSouls.text = PlayerManager.instance.GetCurrency().ToString();

        UpdateHealthUI();

        if (Input.GetKeyDown(KeyCode.LeftShift) && skills.dash.cloneOnDashUnlocked)
            SetCooldownOf(cloneOnDashImage);

        if (Input.GetKeyDown(KeyCode.F) && skills.parry.parryUnlock)
            SetCooldownOf(parryImage);

        if(Input.GetKeyDown(KeyCode.C) && skills.crystal.crystalUnlocked)
            SetCooldownOf(crystalImage);

        if (Input.GetKeyDown(KeyCode.Mouse1) && skills.sword.swordUnlock)
            SetCooldownOf(swordImage);

        if (Input.GetKeyDown(KeyCode.R) && skills.blackHole.blackholeUnlocked)
            SetCooldownOf(blackholeImage);

        if(Input.GetKeyDown(KeyCode.Alpha1) && Inventory.instance.GetEquipment(EquipmentType.Flask) != null)
            SetCooldownOf(flaskImage);

        CheckCooldownOf(cloneOnDashImage, skills.dash.cloneOnArrivalCooldown);
        CheckCooldownOf(parryImage, skills.parry.cooldown);
        CheckCooldownOf(crystalImage, skills.crystal.cooldown);
        CheckCooldownOf(swordImage, skills.sword.cooldown);
        CheckCooldownOf(blackholeImage,skills.blackHole.cooldown);
        CheckCooldownOf(flaskImage, Inventory.instance.flaskCooldown);
    }

    private void UpdateHealthUI()
    {
        slider.maxValue = playerStats.GetMaxhealth();
        slider.value = playerStats.currentHealth;
    }

    private void SetCooldownOf(Image _image)
    {
        if (_image.fillAmount <= 0)
            _image.fillAmount = 1;

    }

    private void CheckCooldownOf(Image _image, float _cooldown)
    {
        if (_image.fillAmount > 0)
            _image.fillAmount -= 1 / _cooldown * Time.deltaTime;
    }
}
