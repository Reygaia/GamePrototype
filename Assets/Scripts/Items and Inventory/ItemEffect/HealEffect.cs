using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Heal Effect", menuName = "Data/Item effect/Heal Effect")]
public class HealEffect : ItemEffect
{
    [Range(0f, 1f)]
    [SerializeField] private float healPercent;
    public override void ExecuteEffect(Transform _enemyPosition)
    {
        PlayerStats playerStats = PlayerManager.instance.player.GetComponent<PlayerStats>();

        int healamount = Mathf.RoundToInt(playerStats.GetMaxhealth() * healPercent);

        playerStats.IncreaseHealthBy(healamount);
    }
}
