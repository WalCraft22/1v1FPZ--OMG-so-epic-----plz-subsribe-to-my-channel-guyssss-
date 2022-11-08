using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class Respawn : NetworkBehaviour
{
    [Command]
    public void CmdRespawn()
    {
        RpcRespawn();
        GetComponent<WeaponManager>().CurrentWeapon = null;
        GetComponent<HealthManager>().Health = 100;
        GetComponent<KillPlayer>().HasDied = false;
    }
    [ClientRpc]
    private void RpcRespawn()
    {
        this.gameObject.SetActive(true);
        foreach(Animator anim in GetComponentsInChildren<Animator>())
        {
            if(anim.transform.name == "Body")
            {
                anim.SetBool("IsIdle", true);
            }
        }
        GetComponent<KillPlayer>().HasDied = false;
    }
}
