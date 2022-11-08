using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using Mirror;

public class HealthManager : NetworkBehaviour
{
    [SyncVar]
    public float Health = 100;
    void Update()
    {
        if(Health <= 0)
        {
            Debug.Log(this.gameObject.name + " Has Died!");
        }
    }
}
