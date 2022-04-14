using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;



public class SliderScripts : MonoBehaviour
{
    public Image fill;
    private PlayerMovement playerMovement;
    private float playerBoostAmount;
    private float boostFillAmount;
    private void Start()
    {
        playerMovement = GetComponent<PlayerMovement>();
        FillSlider();
    }

    private void Update()
    {
        playerBoostAmount = playerMovement.GetBoostAmount();
        boostFillAmount = Mathf.InverseLerp(0f, 1f, playerBoostAmount);
        FillSlider();
    }

    public void FillSlider()
    {
        fill.fillAmount = boostFillAmount;
    }
}
