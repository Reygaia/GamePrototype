using System.Collections.Generic;
using Unity.Burst.CompilerServices;
using Unity.VisualScripting;
using UnityEngine;

public class Sword_Skill_Controller : MonoBehaviour
{
    private Animator anim;
    private Rigidbody2D rb;
    private CircleCollider2D cd;
    private Player player;

    private float freezeTime;
    private float returnSpeed;



    private bool canRotate = true;
    private bool isReturning;
    private bool isStuck;
    private float autoReturnTime = 3;
    private float autoReturnTimer;
    

    [Header("Pierce Info")]
    private float pierceAmount;

    [Header("Bounce info")]
    private float bounceSpeed;
    private bool isBouncing;
    private int bounceAmount;
    private List<Transform> enemyTarget;
    private int TargetIndex;

    [Header("Spin info")]
    private float maxTravelDistance;
    private float spinDuration;
    private float spinTimer;
    private bool wasStopped;
    private bool isSpinning;

    private float hitTimer;
    private float hitCooldown;

    private float spinDirection;

    private void Awake()
    {
        anim = GetComponentInChildren<Animator>();
        rb = GetComponent<Rigidbody2D>();
        cd = GetComponent<CircleCollider2D>();
    }
    private void DestroyMe()
    {
        Destroy(gameObject);
    }
    private void Update()
    {

        if (canRotate)
            transform.right = rb.velocity;

        if (isReturning || StuckReturn())
        {
            CallingSword();
        }
        BounceLogic();
        SpinLogic();
    }

    private bool StuckReturn()
    {
        if (isStuck)
        {
            autoReturnTimer -= Time.deltaTime;
            if (autoReturnTimer < 0)
                return true;
        }

        return false;
    }

    private void CallingSword()
    {
        transform.position = Vector2.MoveTowards(transform.position, player.transform.position, returnSpeed * Time.deltaTime);
        if (Vector2.Distance(transform.position, player.transform.position) < 1)
        {
            player.CatchTheSword();
        }
    }

    public void SetupSword(Vector2 _dir, float _gravityScale, Player _player, float _freezetime, float _returnSpeed)
    {

        player = _player;
        freezeTime = _freezetime;
        rb.gravityScale = _gravityScale;
        rb.velocity = _dir;
        returnSpeed = _returnSpeed;

        if (pierceAmount <= 0)
            anim.SetBool("Rotation", true);

        spinDirection = Mathf.Clamp(rb.velocity.x, -1, 1);

        Invoke("DestroyMe", 7);
    }
    #region Setup sword
    public void SetupBounce(bool _isBouncing, int _amountOfBounce, float _bounceSpeed)
    {
        isBouncing = _isBouncing;
        bounceAmount = _amountOfBounce;
        bounceSpeed = _bounceSpeed;
        enemyTarget = new List<Transform>();
    }
    public void SetupPierce(int _pierceAmount)
    {
        pierceAmount = _pierceAmount;
    }
    public void SetupSpin(bool _isSpinning, float _maxTravelDistance, float _spinDuration, float _hitCooldown)
    {
        isSpinning = _isSpinning;
        maxTravelDistance = _maxTravelDistance;
        spinDuration = _spinDuration;
        hitCooldown = _hitCooldown;
    }
    #endregion

    #region Skill Logic
    private void SpinLogic()
    {
        if (isSpinning)
        {
            if (Vector2.Distance(player.transform.position, transform.position) > maxTravelDistance && !wasStopped)
            {
                StopWhenSpin();
            }
            if (wasStopped)
            {
                spinTimer -= Time.deltaTime;

                transform.position = Vector2.MoveTowards(transform.position, new Vector2(transform.position.x + spinDirection, transform.position.y), 1.5f * Time.deltaTime);
                if (spinTimer < 0)
                {
                    isReturning = true;
                    isSpinning = false;
                }

                hitTimer -= Time.deltaTime;
                if (hitTimer < 0)
                {
                    hitTimer = hitCooldown;

                    Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, 1);

                    foreach (var hit in colliders)
                    {
                        if (hit.GetComponent<Enemy>() != null)
                            DamageEnemySkillSword(hit.GetComponent<Enemy>());
                    }
                }
            }
        }
    }
    private void BounceLogic()
    {
        if (isBouncing && enemyTarget.Count > 0)
        {
            transform.position = Vector2.MoveTowards(transform.position, enemyTarget[TargetIndex].position, bounceSpeed * Time.deltaTime);
            if (Vector2.Distance(transform.position, enemyTarget[TargetIndex].position) < .1f)
            {
                DamageEnemySkillSword(enemyTarget[TargetIndex].GetComponent<Enemy>());
                TargetIndex++;
                bounceAmount--;

                if (bounceAmount <= 0)
                {
                    isBouncing = false;
                    isReturning = true;
                }

                if (TargetIndex >= enemyTarget.Count)
                    TargetIndex = 0;
            }
        }
    }
    #endregion

    #region Collision
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (isReturning)
            return;

        if (collision.GetComponent<Enemy>() != null)
        {
            Enemy enemy = collision.GetComponent<Enemy>();
            DamageEnemySkillSword(enemy);
        }

        SetupTargetForBounce(collision);

        StuckInto(collision);
    }

    private void DamageEnemySkillSword(Enemy enemy)
    {
        var enemyStats = enemy.GetComponent<EnemyStats>();

        player.stats.DoDamage(enemyStats);

        if (player.skill.sword.timeStopUnlock)
            enemy.FreezeTimeFor(freezeTime);

        if (player.skill.sword.vulnerableUnlock)
            enemyStats.MakeVulnerableFor(freezeTime);

        ItemData_Equipment equipedAmulet = Inventory.instance.GetEquipment(EquipmentType.Amulet);

        if (equipedAmulet != null)
            equipedAmulet.Effect(enemy.transform);
    }

    //pass enemy component to the parameter
    //private void DamageAndFreeze(Enemy enemy)
    //{
    //    enemy.Damage();
    //    enemy.StartCoroutine("FreezeTimerFor", freezeTime);
    //}

    private void SetupTargetForBounce(Collider2D collision)
    {
        if (collision.GetComponent<Enemy>() != null)
        {
            if (isBouncing && enemyTarget.Count <= 0)
            {
                Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, 10);

                foreach (var hit in colliders)
                {
                    if (hit.GetComponent<Enemy>() != null)
                        enemyTarget.Add(hit.transform);
                }
            }
        }
    }
    private void StuckInto(Collider2D collision)
    {
        if (pierceAmount > 0 && collision.GetComponent<Enemy>() != null)
        {
            isStuck = true;
            autoReturnTimer = autoReturnTime;
            pierceAmount--;
            return;
        }
        if (isSpinning)
        {
            StopWhenSpin();
            return;
        }

        canRotate = false;
        cd.enabled = false;

        rb.isKinematic = true;
        rb.constraints = RigidbodyConstraints2D.FreezeAll;

        if (isBouncing && enemyTarget.Count > 0)
            return;

        anim.SetBool("Rotation", false);
        transform.parent = collision.transform;
    }
    public void ReturnSword()
    {
        rb.constraints = RigidbodyConstraints2D.FreezeAll;
        //rb.isKinematic = false;
        isStuck = false;
        transform.parent = null;
        isReturning = true;
    }
    private void StopWhenSpin()
    {
        wasStopped = true;
        rb.constraints = RigidbodyConstraints2D.FreezePosition;
        spinTimer = spinDuration;
    }
    #endregion
}
