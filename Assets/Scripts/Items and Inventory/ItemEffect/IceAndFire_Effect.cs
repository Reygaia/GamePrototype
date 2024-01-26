using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Thunder Strike Effect", menuName = "Data/Item effect/Ice And Fire")]
public class IceAndFire_Effect : ItemEffect
{

    [SerializeField] private GameObject iceAndFirePrefab;
    [SerializeField] private float XVelocity;

    public override void ExecuteEffect(Transform _respawnPosition)
    {
        Player player = PlayerManager.instance.player;

        bool thirdAttack = player.primaryAttackState.comboCounter == 2;

        if(thirdAttack)
        {
            GameObject newIceAndFire = Instantiate(iceAndFirePrefab, _respawnPosition.position, player.transform.rotation);
            newIceAndFire.GetComponent<Rigidbody2D>().velocity = new Vector2(XVelocity * player.facingDir,0);

            Destroy(newIceAndFire, 3f);
        }

       
    }
}
