using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Mirror;

public class KillPlayer : NetworkBehaviour
{
    public GameObject ExplosionParticle;
    private GameObject InstantiatedParticle;
    public HealthManager healthManager;
    public bool HasDied = false;
    void Start()
    {
        healthManager = GetComponent<HealthManager>();
    }

    void Update()
    {
        //if(!isLocalPlayer){return;}
        if(healthManager.Health <= 0 && !HasDied)
        {
            HasDied = true;
            ExplodePlayer();
        }
    }
    public void ExplodePlayer()
    {
        InstantiatedParticle = Instantiate(ExplosionParticle, this.transform.position, this.transform.rotation) as GameObject;
        Invoke("KillParticles", 2f);
        if(HasDied)
        {
            if(isLocalPlayer)
            {
                Camera.main.transform.SetParent(null);
                this.GetComponent<InGameHUD>().HealthDisplay.GetComponent<Text>().text = "0 HP";
            }
            this.gameObject.SetActive(false);
        }else
        {
            GetComponent<PlayerMovement>().enabled = false;
            GetComponent<WeaponManager>().enabled = false;
        }
    }
    private void KillParticles()
    {
        Destroy(InstantiatedParticle);
    }
}
