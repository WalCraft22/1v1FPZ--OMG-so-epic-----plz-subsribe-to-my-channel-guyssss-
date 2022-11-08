using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Mirror;
public class IGMenuManager : NetworkBehaviour
{
    public GameObject MenuOBJ;
    public GameObject OptionsMenu;
    public GameObject OptionsButton;
    public GameObject BackButton;
    public bool IsPaused;
    public bool IsInOptionsMenu;
    public float SFXVolume;
    public GameObject PingDisplay;
    public GameObject HealthDisplay;
    public GameObject AmmoDisplay;
    public GameObject FPSDisplay;
    public GameObject Disconnect;
    void Start()
    {
        MenuOBJ = GameObject.Find("Menu");
    }
    void Update()
    {
        foreach(Slider SL in MenuOBJ.transform.parent.GetComponentsInChildren<Slider>())
        {
            if(SL.gameObject.name == "SFXVolume")
            {
                SFXVolume = SL.value;
            }
        }

        GameObject[] Players = GameObject.FindGameObjectsWithTag("Player");
        for(int i = 0; i < Players.Length; i++)
        {
            try
            {
                if(Players[i].GetComponent<NetworkIdentity>().isLocalPlayer)
                {
                    Players[i].GetComponent<WeaponManager>().SFXVolume = SFXVolume;
                }
            }catch{}
        }
        if(MenuOBJ.activeSelf)
        {
            if(!IsInOptionsMenu)
            {
                OptionsButton.SetActive(true);
            }
            IsPaused = true;
            PingDisplay.SetActive(false);
            HealthDisplay.SetActive(false);
            AmmoDisplay.SetActive(false);
            FPSDisplay.SetActive(false);
            Disconnect.SetActive(true);
        }else
        {
            IsPaused = false;
            IsInOptionsMenu = false;
            OptionsMenu.SetActive(false);
            OptionsButton.SetActive(false);

            PingDisplay.SetActive(true);
            HealthDisplay.SetActive(true);
            AmmoDisplay.SetActive(true);
            FPSDisplay.SetActive(true);
            Disconnect.SetActive(false);
            BackButton.SetActive(false);
        }
        if(OptionsMenu.activeSelf)
        {
            IsInOptionsMenu = true;
        }else
        {
            IsInOptionsMenu = false;
        }
    }
    public void OnOptionsButton()
    {
        IsInOptionsMenu = true;
        OptionsMenu.SetActive(true);
        OptionsButton.SetActive(false);
        BackButton.SetActive(true);
    }
    public void OnBackButtonPress()
    {
        if(IsInOptionsMenu)
        {
            IsInOptionsMenu = false;
            OptionsMenu.SetActive(false);
            OptionsButton.SetActive(true);
            BackButton.SetActive(false);
        }
    }
}