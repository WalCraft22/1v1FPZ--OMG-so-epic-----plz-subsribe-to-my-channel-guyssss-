using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Mirror;
public class RespawnManager : NetworkBehaviour
{
    public GameObject RespawnButton;
    public GameObject Spawn;
    public GameObject LocalPlayerObj;
    private GameObject MainCamera;
    void Start()
    {
        MainCamera = Camera.main.gameObject;
        GameObject[] Players = GameObject.FindGameObjectsWithTag("Player");
        for(int i = 0; i < Players.Length; i++)
        {
            try
            {
                if(Players[i].GetComponent<NetworkIdentity>().isLocalPlayer)
                {
                    LocalPlayerObj = Players[i];
                }
            }catch{}
        }
    }
    void Update()
    {
        if(!LocalPlayerObj)
        {
            try
            {
                MainCamera = Camera.main.gameObject;
                LocalPlayerObj = NetworkClient.localPlayer.gameObject;
            }catch{}
        }
        if(!LocalPlayerObj){return;}
        if(LocalPlayerObj.GetComponent<HealthManager>().Health <= 0)
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
            RespawnButton.SetActive(true);
            LocalPlayerObj.GetComponent<WeaponManager>().CurrentWeapon = null;
        }
    }
    public void OnButtonPress()
    {
        LocalPlayerObj.GetComponent<Respawn>().CmdRespawn();
        LocalPlayerObj.GetComponent<KillPlayer>().HasDied = false;
        LocalPlayerObj.GetComponent<HealthManager>().Health = 100;
        LocalPlayerObj.GetComponent<PlayerMovement>().enabled = true;
        LocalPlayerObj.GetComponent<WeaponManager>().enabled = true;
        RespawnButton.SetActive(false);
        LocalPlayerObj.transform.position = Spawn.transform.position;
        LocalPlayerObj.transform.rotation = Spawn.transform.rotation;
        Camera.main.transform.SetParent(LocalPlayerObj.transform);
        MainCamera.transform.localPosition = new Vector3(0 ,0.6f, 0); //playerCamera.transform.localPosition = new Vector3(0, 1.9f, 0);
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        WeaponManager WM = LocalPlayerObj.GetComponent<WeaponManager>();
        for(int i = 0; i < WM.WeaponAmmunition.Length; i++)
        {
            WM.WeaponAmmunition[i] = LocalPlayerObj.GetComponent<WeaponManager>().WeaponMaxAmmunition[i];
            WM.Weapons[i].transform.localEulerAngles = Vector3.zero;
        }
    }
    public void Disconnect()
    {
        if(isServer)
        {
            NetworkManager.singleton.StopHost();
        }else
        {
            NetworkManager.singleton.StopClient();
        }
        
    }
}
