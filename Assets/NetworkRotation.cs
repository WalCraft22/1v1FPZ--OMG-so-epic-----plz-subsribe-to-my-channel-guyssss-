using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class NetworkRotation : NetworkBehaviour
{
    public GameObject HeadRotationPivot;
    public GameObject WeaponRotationPivot;
    [SyncVar]
    public float HeadRotationX = 0;
    [SyncVar]
    public float WeaponRotationX = 0;
    void Start()
    {
        
    }

    void Update()
    {

        if(isLocalPlayer)
        {
            HeadRotationX = GetComponentInChildren<Camera>().gameObject.transform.localEulerAngles.x;
            CmdRotateHead(HeadRotationX);
        }
        HeadRotationPivot.transform.localEulerAngles = new Vector3(HeadRotationX, 0,0);
        WeaponRotationPivot.transform.localEulerAngles = new Vector3(HeadRotationX, 0,0);
    }
    [Command]
    private void CmdRotateHead(float NewRotX)
    {
        HeadRotationX = NewRotX;
        WeaponRotationX = NewRotX;
    }
}
