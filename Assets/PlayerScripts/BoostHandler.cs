using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoostHandler : MonoBehaviour
{

    private PlayerMovement playerMovement;
    
    // Start is called before the first frame update
    void Start()
    {
        playerMovement = GetComponent<PlayerMovement>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void AddBoost(float boostAmount)
    {
        playerMovement.SetBoostAmount(boostAmount);
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "BoostPickup")
        {
            playerMovement.SetBoostAmount(.1f);
            Destroy(other.gameObject);
        }
    }
}
