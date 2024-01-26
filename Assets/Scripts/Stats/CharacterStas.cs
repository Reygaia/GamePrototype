using System.Collections;
using UnityEngine;

public enum StatType
{
    strength,
    agility,
    intelligent,
    vitality,
    damage,
    critChance,
    critPower,
    health,
    armour,
    evasion,
    magicRes,
    fireDamage,
    iceDamage,
    lightningDamage
}

public class CharacterStas : MonoBehaviour
{
    private EntityFX fx;

    [Header("Major Stats")]
    public Stat strength; // increase damage by 1 and crit power 1%
    public Stat agility; // increase evasion 1% crit chance 1% 
    public Stat intelligent; // increase 1 magic damage 3 magic resistance
    public Stat vitality; // increase 5 hp

    [Header("Offensive Stats")]
    public Stat damage;
    public Stat critChance;
    public Stat critPower; //default value 150%

    [Header("Defensive stats")]
    public Stat maxHealth;
    public Stat armour;
    public Stat evasion;
    public Stat magicResistance;

    [Header("Magic Stats")]
    public Stat fireDamage;
    public Stat iceDamage;
    public Stat lightningDamage;

    public bool isIgnited; // does damage overtime
    public bool isChilled; // reduce armor by 20%
    public bool isShocked; //reduce accuracy by 20%

    [SerializeField] private float alimentsDuration = 4;
    private float ignitedTimer;
    private float chilledTimer;
    private float shockedTimer;

    private float ingiteDamageCooldown = .3f;
    private float igniteDmageTimer;
    private int igniteDamage;
    [SerializeField] private GameObject ThunderStrikePrefab;
    private int shockDamage;


    public int currentHealth;

    public System.Action onHealthChanged;
    public bool isDead { get; private set; }
    private bool isVulnerable;

    protected virtual void Start()
    {
        critPower.SetDefaultValue(150);
        critChance.SetDefaultValue(5);
        currentHealth = GetMaxhealth();

        fx = GetComponent<EntityFX>();

        ////example of equip an item
        //damage.AddModifier(5);
    }

    protected virtual void Update()
    {
        AlimentsTimers();

        if (isIgnited)
            ApplyIgniteDamage();

    }

    public virtual void IncreaseStatBy(int _modifer, float _duration, Stat _statToModify)
    {
        StartCoroutine(StatModCoroutine(_modifer, _duration, _statToModify));
    }

    private IEnumerator StatModCoroutine(int _modifer, float _duration, Stat _statToModify)
    {
        _statToModify.AddModifier(_modifer);

        yield return new WaitForSeconds(_duration);

        _statToModify.RemoveModifier(_modifer);
    }


    #region Damage
    public virtual void DoDamage(CharacterStas _targetStats)
    {
        if (TargetCanAvoidAttack(_targetStats))
            return;

        int totalDamage = damage.GetValue() + strength.GetValue();

        if (CanCrit())
        {
            totalDamage = CalculateCritDamage(totalDamage);
        }

        totalDamage = CheckTargetAmour(_targetStats, totalDamage);
        _targetStats.TakeDamage(totalDamage);

        DoMagicalDamage(_targetStats);//apply magic damage from modifiers
    }

    public virtual void DoMagicalDamage(CharacterStas _targetStats)
    {
        int _fireDamage = fireDamage.GetValue();
        int _iceDamage = iceDamage.GetValue();
        int _lightningDamage = lightningDamage.GetValue();

        int totalMagicalDamage = _fireDamage + _iceDamage + _lightningDamage + intelligent.GetValue();
        totalMagicalDamage = CheckTargetResistance(_targetStats, totalMagicalDamage);
        _targetStats.TakeDamage(totalMagicalDamage);


        if (Mathf.Max(_fireDamage, _iceDamage, _lightningDamage) <= 0)
            return;

        ApplyAlimentsEffect(_targetStats, _fireDamage, _iceDamage, _lightningDamage);

    }

    public virtual void TakeDamage(int _damage)
    {
        DecreaseHealthBy(_damage);

        GetComponent<Entity>().DamageImpact();
        fx.StartCoroutine("FlashFX");

        if (currentHealth <= 0 && !isDead)
            Die();
    }

    #region In/De - crease Health

    protected virtual void DecreaseHealthBy(int _damage)
    {
        if (isVulnerable)
            _damage = Mathf.RoundToInt(_damage * 1.2f);

        currentHealth -= _damage;
        if (onHealthChanged != null)
            onHealthChanged();
    }

    public virtual void IncreaseHealthBy(int _amount)
    {
        currentHealth += _amount;
        if (currentHealth > GetMaxhealth())
            currentHealth = GetMaxhealth();
    }

    #endregion

    #endregion

    #region Aliments

    #region Ignite Damage
    private void ApplyIgniteDamage()
    {
        igniteDmageTimer -= Time.deltaTime;

        if (igniteDmageTimer < 0)
        {
            DecreaseHealthBy(igniteDamage);

            if (currentHealth < 0 && !isDead)
                Die();

            igniteDmageTimer = ingiteDamageCooldown;
        }
    }

    public void SetupIgniteDamage(int _damage) => igniteDamage = _damage;
    #endregion

    //ignite damage

    #region ShockStrike Component
    public void SetupShockStrikeDamage(int _damage) => shockDamage = _damage;

    private void HitNearestTargetShockStrike()
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, 4);

        float closestDistance = Mathf.Infinity;

        Transform closestEnemy = null;

        foreach (var hit in colliders)
        {
            if ((hit.GetComponent<Enemy>() != null && Vector2.Distance(transform.position, hit.transform.position) > 1))
            {
                float distanceToEnemy = Vector2.Distance(transform.position, hit.transform.position);

                if (distanceToEnemy < closestDistance)
                {
                    closestDistance = distanceToEnemy;
                    closestEnemy = hit.transform;
                }
            }

            //if (closestEnemy == null)
            //    closestEnemy = transform;
        }

        if (closestEnemy != null)
        {
            GameObject newShockStrike = Instantiate(ThunderStrikePrefab, transform.position, Quaternion.identity);

            newShockStrike.GetComponent<ShockStrike_Controller>().Setup(shockDamage, closestEnemy.GetComponent<CharacterStas>());
        }
    }
    #endregion

    //shock strike

    #region Aliments Effect
    private void AlimentsTimers()
    {
        ignitedTimer -= Time.deltaTime;
        chilledTimer -= Time.deltaTime;
        shockedTimer -= Time.deltaTime;

        if (ignitedTimer < 0)
            isIgnited = false;

        if (chilledTimer < 0)
            isChilled = false;

        if (shockedTimer < 0)
            isShocked = false;
    }

    public void MakeVulnerableFor(float _duration)
    {
        StartCoroutine(VulnerableForCoroutine(_duration));
    }

    private IEnumerator VulnerableForCoroutine(float _duration)
    {
        isVulnerable = true;

        yield return new WaitForSeconds(_duration);

        isVulnerable = false;
    }

    public void ApplyAliments(bool _ignite, bool _chill, bool _shock)
    {
        bool canApplyIgnite = !isIgnited && !isChilled && !isShocked;
        bool canApplyChill = !isIgnited && !isChilled && !isShocked;
        bool canApplyShock = !isIgnited && !isChilled;

        if (_ignite && canApplyIgnite)
        {
            isIgnited = _ignite;
            ignitedTimer = alimentsDuration;

            fx.IgniteFXFor(alimentsDuration);
        }

        if (_chill && canApplyChill)
        {
            isChilled = _chill;
            chilledTimer = alimentsDuration;

            float slowPercentage = .2f;
            GetComponent<Entity>().SlowEntityBy(slowPercentage, alimentsDuration);
            fx.ChillFXFor(alimentsDuration);
        }

        if (_shock && canApplyShock)
        {
            if (!isShocked)
            {
                ApplyShock(_shock);
            }
            else
            {
                if (GetComponent<Player>() != null)
                    return;

                HitNearestTargetShockStrike();

            }


        }
    }

    private void ApplyAlimentsEffect(CharacterStas _targetStats, int _fireDamage, int _iceDamage, int _lightningDamage)
    {
        bool canApplyIgnite = _fireDamage > _iceDamage && _fireDamage > _lightningDamage;
        bool canApplyChill = _iceDamage > _fireDamage && _iceDamage > _lightningDamage;
        bool canApplyShock = _lightningDamage > _fireDamage && _lightningDamage > _iceDamage;

        while (!canApplyIgnite && !canApplyChill && !canApplyShock)
        {
            //coin flip 50/50
            if (Random.value < .33f && _fireDamage > 0)
            {
                canApplyIgnite = true;
                _targetStats.ApplyAliments(canApplyIgnite, canApplyChill, canApplyShock);
                return;
            }
            if (Random.value < .4f && _iceDamage > 0)
            {
                canApplyChill = true;
                _targetStats.ApplyAliments(canApplyIgnite, canApplyChill, canApplyShock);
                return;
            }
            if (Random.value < .45f && _lightningDamage > 0)
            {
                canApplyShock = true;
                _targetStats.ApplyAliments(canApplyIgnite, canApplyChill, canApplyShock);
                return;
            }
        }

        if (canApplyIgnite)
            _targetStats.SetupIgniteDamage(Mathf.RoundToInt(_fireDamage * .2f));

        if (canApplyShock)
            _targetStats.SetupShockStrikeDamage(Mathf.RoundToInt(_lightningDamage * .1f));

        _targetStats.ApplyAliments(canApplyIgnite, canApplyChill, canApplyShock);
    }

    public void ApplyShock(bool _shock)
    {
        if (isShocked)
            return;

        isShocked = _shock;
        shockedTimer = alimentsDuration;

        fx.ShockFXFor(alimentsDuration);
    }

    #endregion

    //aliments 

    #endregion

    #region Cal Damage

    #region Resitances
    protected int CheckTargetAmour(CharacterStas _targetStats, int totalDamage)
    {
        if (_targetStats.isChilled)
            totalDamage -= Mathf.RoundToInt(_targetStats.armour.GetValue() * .8f);
        else
            totalDamage -= _targetStats.armour.GetValue();

        totalDamage = Mathf.Clamp(totalDamage, 0, int.MaxValue);
        return totalDamage;
    }

    private int CheckTargetResistance(CharacterStas _targetStats, int totalMagicalDamage)
    {
        totalMagicalDamage -= _targetStats.magicResistance.GetValue() + (_targetStats.intelligent.GetValue() * 3);
        totalMagicalDamage = Mathf.Clamp(totalMagicalDamage, 0, int.MaxValue);
        return totalMagicalDamage;
    }

    public virtual void OnEvasion()
    {

    }

    protected bool TargetCanAvoidAttack(CharacterStas _targetStats)
    {
        int totalEvasion = _targetStats.evasion.GetValue() + _targetStats.agility.GetValue();

        if (isShocked)
            totalEvasion += 20;

        if (Random.Range(0, 100) < totalEvasion)
        {
            Debug.Log("Evaded");
            _targetStats.OnEvasion();
            return true;
        }
        return false;
    }

    public int GetMaxhealth() => maxHealth.GetValue() + (vitality.GetValue() * 5);
    #endregion

    //armour magic resist evasion hp

    #region Crit
    protected bool CanCrit()
    {
        int totalCritChance = critChance.GetValue() + agility.GetValue();
        if (Random.Range(0, 100) <= totalCritChance)
        {
            return true;
        }
        return false;
    }

    protected int CalculateCritDamage(int _damage)
    {
        float totalCritPower = (critPower.GetValue() + strength.GetValue()) * .01f;
        float critDamage = _damage * totalCritPower;

        return Mathf.RoundToInt(critDamage);
    }
    #endregion

    //crit rate crit damage

    #endregion
    protected virtual void Die()
    {
        isDead = true;
    }

    public Stat GetStatType(StatType _statType)
    {
        if (_statType == StatType.strength) return strength;
        else if (_statType == StatType.agility) return agility;
        else if (_statType == StatType.intelligent) return intelligent;
        else if (_statType == StatType.vitality) return vitality;
        else if (_statType == StatType.damage) return damage;
        else if (_statType == StatType.critChance) return critChance;
        else if (_statType == StatType.critPower) return critPower;
        else if (_statType == StatType.health) return maxHealth;
        else if (_statType == StatType.armour) return armour;
        else if (_statType == StatType.evasion) return evasion;
        else if (_statType == StatType.magicRes) return magicResistance;
        else if (_statType == StatType.fireDamage) return fireDamage;
        else if (_statType == StatType.iceDamage) return iceDamage;
        else if (_statType == StatType.lightningDamage) return lightningDamage;

        return null;
    }
}
