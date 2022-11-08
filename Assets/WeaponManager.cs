using System.Collections;
using System.Collections.Generic;
using UnityEngine.Networking;
using UnityEngine;
using Mirror;

public class WeaponManager : NetworkBehaviour
{
    public Animator PlayerAnimator;
    [SyncVar]
    public int CurrentWeaponIndex = -1;
    public GameObject CurrentWeapon;
    public float FireCooldown;
    public bool IsIdle = true;
    public float SFXVolume = 1;
    public GameObject[] Weapons;
    public float[] WeaponDamage;
    public float[] WeaponSpeed;
    public float[] WeaponMaxAmmunition;
    public float[] WeaponAmmunition;
    public float[] WeaponReloadTime;
    public bool[] IsAutomatic;
    public string[] DrawAnimations;
    public string[] FireAnimations;
    public string[] ReloadAnimations;
    public AudioClip[] FireSFX;
    public Vector3[] NonLocalWeaponPositions;
    public Animator[] WeaponAnimators;
    private int ReloadingWeapon;
    void Start()
    {
        //CurrentWeapon = Weapons[CurrentWeaponIndex];
        Animator[] TempAnimArray = new Animator[Weapons.Length];
        for(int i = 0; i < Weapons.Length; i++)
        {
            TempAnimArray[i] = Weapons[i].GetComponent<Animator>();
        }
        WeaponAnimators = TempAnimArray;
        foreach(Transform transform in GetComponentsInChildren<Transform>())
        {
            if(transform.name == "Body")
            {
                transform.GetComponent<Animator>().SetBool("IsIdle", true);
            }
        }
        if(!isLocalPlayer)
        {
            //Allign weapon(s) to non-local character
            for(int i = 0; i < Weapons.Length; i++)
            {
                Debug.Log(Weapons[i]);
                Weapons[i].transform.localPosition = NonLocalWeaponPositions[i];
            }
        }
        CurrentWeaponIndex = 0;
        DrawWeapon(0);
    }
    void Update()
    {
        if(!isLocalPlayer)return;
        if(Input.GetKeyDown(KeyCode.Alpha1))
        {
            DrawWeapon(0);
        }
        if(Input.GetKeyDown(KeyCode.Alpha2))
        {
            DrawWeapon(1);
        }
        if(Input.GetKeyDown(KeyCode.Alpha3))
        {
            DrawWeapon(2);
        }
        if(Input.GetKeyDown(KeyCode.R))
        {
            Reload();
        }
        if(Input.GetMouseButtonDown(0))
        {
            Fire();
        }
    }
    public void DrawWeapon(int WeaponIndex)             //DRAW
    {
        if(isServer)
        {
            RpcDrawWeapon(WeaponIndex);
        }
        if(isClient)
        {
            CmdDrawWeapon(WeaponIndex);
            if(CurrentWeapon != null)
            {
                CurrentWeapon.SetActive(false);
            }
            CurrentWeaponIndex = WeaponIndex;
            CurrentWeapon = Weapons[CurrentWeaponIndex];
            CurrentWeapon.SetActive(true);
            WeaponAnimators[WeaponIndex].Play(DrawAnimations[WeaponIndex]);
            CurrentWeapon = Weapons[WeaponIndex];
        }
    }
    [ClientRpc]
    private void RpcDrawWeapon(int WeaponIndex)
    {
        if(CurrentWeapon != null)
        {
            CurrentWeapon.SetActive(false);
        }
        CurrentWeaponIndex = WeaponIndex;
        CurrentWeapon = Weapons[CurrentWeaponIndex];
        CurrentWeapon.SetActive(true);
        WeaponAnimators[WeaponIndex].Play(DrawAnimations[WeaponIndex]);
        CurrentWeapon = Weapons[WeaponIndex];
    }
    [Command]
    private void CmdDrawWeapon(int WeaponIndex)
    {
        CurrentWeaponIndex = WeaponIndex;
        RpcDrawWeapon(WeaponIndex);
    }
                                                                        //END DRAW


                                                                        //RELOAD
    public void Reload()
    {print("reload");
        if(isServer)
        {
            print("serv");
            RpcReload();
            /*if(CurrentWeapon != null)
            {
                WeaponAnimators[CurrentWeaponIndex].Play(ReloadAnimations[CurrentWeaponIndex]);
                ReloadingWeapon = CurrentWeaponIndex;
                Invoke("SetAmmoMax", WeaponReloadTime[CurrentWeaponIndex]);
            }*/
        }
        if(isClient)
        {
            print("cli");
            CmdReload();
            if(CurrentWeapon != null)
            {
                WeaponAnimators[CurrentWeaponIndex].Play(ReloadAnimations[CurrentWeaponIndex]);
                ReloadingWeapon = CurrentWeaponIndex;
                Invoke("SetAmmoMax", WeaponReloadTime[CurrentWeaponIndex]);
            }
        }
    }
    [ClientRpc]
    private void RpcReload()
    {
        if(CurrentWeapon != null)
        {
            WeaponAnimators[CurrentWeaponIndex].Play(ReloadAnimations[CurrentWeaponIndex]);
            ReloadingWeapon = CurrentWeaponIndex;
            Invoke("SetAmmoMax", WeaponReloadTime[CurrentWeaponIndex]);
        }
    }
    [Command]
    private void CmdReload()
    {
        RpcReload();
        if(CurrentWeapon != null)
        {
            WeaponAnimators[CurrentWeaponIndex].Play(ReloadAnimations[CurrentWeaponIndex]);
            ReloadingWeapon = CurrentWeaponIndex;
            Invoke("SetAmmoMax", WeaponReloadTime[CurrentWeaponIndex]);
        }
    }
    private void SetAmmoMax()
    {
        if(ReloadingWeapon == CurrentWeaponIndex)
        {
            WeaponAmmunition[CurrentWeaponIndex] = WeaponMaxAmmunition[CurrentWeaponIndex];
        }
    }
                                                                        //END RELOAD

                                                                        //START FIRE

    public void Fire()
    {print("Fire");
        if(isServer)
        {
            print("serv");
            RpcFire();
        }
        if(isClient)
        {
            print("cli");
            CmdFire();
            WeaponAnimators[CurrentWeaponIndex].Play(FireAnimations[CurrentWeaponIndex]);
            print(FireAnimations[CurrentWeaponIndex]);
            print(WeaponAnimators[CurrentWeaponIndex]);
            CurrentWeapon.GetComponentInChildren<ParticleSystem>().Play();
            CurrentWeapon.GetComponentInChildren<AudioSource>().PlayOneShot(FireSFX[CurrentWeaponIndex], SFXVolume);
            //print(FireAnimations[CurrentWeaponIndex]);
            DrawRay();
        }
    }
    [ClientRpc]
    private void RpcFire()
    {
        if(!isLocalPlayer)
        {
            WeaponAnimators[CurrentWeaponIndex].Play(FireAnimations[CurrentWeaponIndex]);
            //print(FireAnimations[CurrentWeaponIndex]);
            CurrentWeapon.GetComponentInChildren<ParticleSystem>().Play();
            CurrentWeapon.GetComponentInChildren<AudioSource>().PlayOneShot(FireSFX[CurrentWeaponIndex], SFXVolume);
        }
    }
    [Command]
    private void CmdFire()
    {
        RpcFire();
    }
    private void DrawRay()
    {
        print("ray");
    }
                                                                    //END FIRE
}