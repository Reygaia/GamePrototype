using UnityEngine;

[CreateAssetMenu(fileName = "Freeze Enemies Effect", menuName = "Data/Item effect/Freeze enemies Effect")]
public class FreezeEnemiesEffect : ItemEffect
{
    [SerializeField] private float freezeDuration;

    public override void ExecuteEffect(Transform _transform)
    {
        PlayerStats playerStats = PlayerManager.instance.player.GetComponent<PlayerStats>();

        if (playerStats.currentHealth > playerStats.GetMaxhealth() * .3f)
            return;            

        if (!Inventory.instance.CanUseArmour())
            return;

        Collider2D[] colliders = Physics2D.OverlapCircleAll(_transform.position, 2);

        foreach (var hit in colliders)
        {
            if (hit.GetComponent<Enemy>() != null)
            {
                hit.GetComponent<Enemy>().FreezeTimeFor(freezeDuration);
            }
        }
    }
}
