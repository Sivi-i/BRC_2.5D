using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SFX_Player : MonoBehaviour
{
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioSource grindAudioSource;
    [SerializeField] private AudioClip jump5;
    [SerializeField] private AudioClip jump4;
    [SerializeField] private AudioClip jump3;
    [SerializeField] private AudioClip jump2;
    [SerializeField] private AudioClip jump1;
    [SerializeField] private AudioClip boost;
    [SerializeField] private AudioClip grindLanding;
    [SerializeField] private AudioClip grindLeaving;
    private int rand;
    private Rigidbody rb;
    private bool canPlayJumpSFX;
    private bool canPlayBoostSFX;
    private bool canPlayGrindLandingSFX;
    private bool canPlayGrindLeavingSFX;
    private PlayerMovement playerMovement;


    // Start is called before the first frame update
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        playerMovement = GetComponent<PlayerMovement>();
        rb = GetComponent<Rigidbody>();
        canPlayJumpSFX = true;
        canPlayBoostSFX = true;
        canPlayGrindLandingSFX = true;
        canPlayGrindLeavingSFX = true;
    }

    // Update is called once per frame
    void Update()
    {

        PlayJumpSFX();
        PlayBoostSFX();
    }

    public void PlayJumpSFX()
    {
        if (canPlayJumpSFX)
        {
            if (rb.velocity.y >= 5f)
            {
                rand = Random.Range(0, 4);
                switch (rand)
                {
                    case 0:
                        audioSource.PlayOneShot(jump5, 0.2f);
                        ResetJumpSFX();
                        break;
                    case 1:
                        audioSource.PlayOneShot(jump4, 0.2f);
                        ResetJumpSFX();
                        break;
                    case 2:
                        audioSource.PlayOneShot(jump3, 0.2f);
                        ResetJumpSFX();
                        break;
                    case 3:
                        audioSource.PlayOneShot(jump2, 0.2f);
                        ResetJumpSFX();
                        break;
                    case 4:
                        audioSource.PlayOneShot(jump1, 0.2f);
                        ResetJumpSFX();
                        break;
                }
            }
        }
    }

    void ResetJumpSFX()
    {
        canPlayJumpSFX = false;
        StartCoroutine(PlayJumpSFXCooldown());
    }

    IEnumerator PlayJumpSFXCooldown()
    {
        yield return new WaitForSeconds(0.3f);
        canPlayJumpSFX = true;
    }

    void PlayBoostSFX()
    {
        if (canPlayBoostSFX)
        {
            if (playerMovement.GetBoost())
            {
                Debug.Log(playerMovement.GetBoost());
                audioSource.PlayOneShot(boost, 0.35f);
                ResetBoostSFX();
            }
        }

    }

    void ResetBoostSFX()
    {
        canPlayBoostSFX = false;
        StartCoroutine(BoostSFXCooldown());
    }

    IEnumerator BoostSFXCooldown()
    {
        yield return new WaitForSeconds(4f);
        canPlayBoostSFX = true;
    }

    public void PlayGrindLandingSFX()
    {
        if (canPlayGrindLandingSFX)
        {
            grindAudioSource.PlayOneShot(grindLanding, 0.9f);
            ResetGrindLandingSFX();
        }
    }

    void ResetGrindLandingSFX()
    {
        canPlayGrindLandingSFX = false;
        StartCoroutine(GrindLandingSFXCooldown());
    }

    IEnumerator GrindLandingSFXCooldown()
    {
        yield return new WaitForSeconds(2f);
        canPlayGrindLandingSFX = true;
    }

    public void PlayGrindLeavingSFX()
    {
        if (canPlayGrindLeavingSFX)
        {
            grindAudioSource.Stop();
            grindAudioSource.PlayOneShot(grindLeaving, 0.9f);
            ResetGrindLeavingSFX();
        }
    }

    void ResetGrindLeavingSFX()
    {
        canPlayGrindLeavingSFX = false;
        StartCoroutine(GrindLeavingSFXCooldown());
    }

    IEnumerator GrindLeavingSFXCooldown()
    {
        yield return new WaitForSeconds(0.5f);
        canPlayGrindLeavingSFX = true;
    }
}