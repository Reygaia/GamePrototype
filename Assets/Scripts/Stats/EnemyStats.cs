using UnityEngine;

public class EnemyStats : CharacterStas
{
    private Enemy enemy;
    private ItemDrop myDrop;

    [Header("Level Details")]
    [SerializeField] private int level = 1;

    [Range(0f, 1f)]
    [SerializeField] private float percentageModifer = .2f;

    protected override void Start()
    {
        ApplyLevelModifiers();
        base.Start();

        enemy = GetComponent<Enemy>();
        myDrop = GetComponent<ItemDrop>();
    }

    private void ApplyLevelModifiers()
    {
        //Modify(strength);
        //Modify(agility);
        //Modify(intelligent);
        //Modify(vitality);
        
        Modify(damage);
        //Modify(critChance);
        //Modify(critPower);
        
        Modify(maxHealth);
        Modify(armour);
        Modify(evasion);
        Modify(magicResistance);
        
        Modify(fireDamage);
        Modify(iceDamage);
        Modify(lightningDamage);
    }

    private void Modify(Stat _stat)
    {
        for (int i = 1; i < level; i++)
        {
            float modifier = _stat.GetValue() * percentageModifer;

            _stat.AddModifier(Mathf.RoundToInt(modifier));
        }
    }

    public override void TakeDamage(int _damage)
    {
        base.TakeDamage(_damage);
    }
    protected override void Die()
    {
        base.Die();
        enemy.Die();

        myDrop.GenerateDrop();
    }
}
