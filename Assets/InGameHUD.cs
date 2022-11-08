using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Mirror;
using System;

public class InGameHUD : NetworkBehaviour
{
    private bool IsReady = false;
    public GameObject Menu;
    public GameObject PingDisplay;
    public GameObject FPSDisplay;
    public GameObject AmmoDisplay;
    public GameObject HealthDisplay;
    private WeaponManager WM;
    void Start()
    {
        if(!isLocalPlayer)return;
        Menu = GameObject.Find("Menu");
        Menu.SetActive(false);
        WM = GetComponent<WeaponManager>();
        foreach(Transform transform in Menu.transform.parent.GetComponentsInChildren<Transform>())
        {
            if(transform.name == "Ping"){PingDisplay = transform.gameObject;}
            if(transform.name == "FPS"){FPSDisplay = transform.gameObject;}
            if(transform.name == "Ammo"){AmmoDisplay = transform.gameObject;}
            if(transform.name == "Health"){HealthDisplay = transform.gameObject;}
        }
    }

    // Update is called once per frame
    void Update()
    {
        if(!isLocalPlayer)return;
        if(isClientOnly && !IsReady)return;
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            if(Menu.activeSelf)
            {
                Menu.SetActive(false);
                Cursor.lockState = CursorLockMode.Locked;
                this.GetComponent<PlayerMovement>().enabled = true;
                this.GetComponent<WeaponManager>().enabled = true;
            }else
            {
                Menu.SetActive(true);
                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;
                this.GetComponent<PlayerMovement>().enabled = false;
                this.GetComponent<WeaponManager>().enabled = false;
            }
        }
        PingDisplay.GetComponent<Text>().text = (Mathf.Round((float)NetworkTime.rtt * 1000f)).ToString()+" Ms";
        FPSDisplay.GetComponent<Text>().text = (Mathf.Round(1f / Time.unscaledDeltaTime)).ToString()+" FPS";
        HealthDisplay.GetComponent<Text>().text = GetComponent<HealthManager>().Health.ToString()+" HP";
        if(WM.CurrentWeapon == null)return;
        AmmoDisplay.GetComponent<Text>().text = WM.WeaponAmmunition[System.Array.IndexOf(WM.Weapons, WM.CurrentWeapon)].ToString()+"/"+WM.WeaponMaxAmmunition[System.Array.IndexOf(WM.Weapons, WM.CurrentWeapon)].ToString();
    }
    public virtual void OnClientSceneChanged()
    {
        IsReady = true;
    }

}