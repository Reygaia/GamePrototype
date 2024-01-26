using UnityEngine;
using UnityEngine.UI;

public class UI_HealthBar : MonoBehaviour
{
    private Entity entity;
    private CharacterStas myStats;
    private RectTransform rectTransform;
    private Slider slider;
    private void Start()
    {
        entity = GetComponentInParent<Entity>();
        rectTransform = GetComponent<RectTransform>();
        slider = GetComponentInChildren<Slider>();
        myStats = GetComponentInParent<CharacterStas>();

        entity.onFlipped += FlipUI;
        myStats.onHealthChanged += UpdateHealthUI;

        UpdateHealthUI();
    }
    private void Update()
    {
        UpdateHealthUI();
    }
    private void UpdateHealthUI()
    {
        slider.maxValue = myStats.GetMaxhealth();
        slider.value = myStats.currentHealth;
    }


    private void FlipUI() => rectTransform.Rotate(0, 180, 0);
    private void OnDisable()
    {
        entity.onFlipped -= FlipUI;
        myStats.onHealthChanged -= UpdateHealthUI;
    }
}
