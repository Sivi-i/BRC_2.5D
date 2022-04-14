using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ComboHandler : MonoBehaviour
{
    private PlayerMovement playerMovement;
    private BoostHandler boostHandler;
    private float currentComboPoints = 0f;
    private float boostToAdd = 0f;
    // Start is called before the first frame update
    void Start()
    {
        playerMovement = GetComponent<PlayerMovement>();
        boostHandler = GetComponent<BoostHandler>();
    }

    // Update is called once per frame
    void Update()
    {
        GrindingPoints();
        BillboardPoints();
        ComboFinished();
    }

    void GrindingPoints()
    {
        if (playerMovement.GetRail())
        {
            currentComboPoints += 0.005f;
        }
    }

    void BillboardPoints()
    {
        if (playerMovement.GetBillboard())
        {
            currentComboPoints += 0.005f;
        }
    }


    void ComboFinished()
    {
        if (playerMovement.GetGrounded())
        {
            SendPointsToBoost();
        }
    }

    void SendPointsToBoost()
    {
        boostToAdd = currentComboPoints / 80;
        boostHandler.AddBoost(boostToAdd);
        currentComboPoints = 0;
    }
}
