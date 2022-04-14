using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationHandler : MonoBehaviour
{
    [SerializeField] private Animator animator;
    private PlayerMovement playerMovement;
    private bool isManualing;
    private bool isAir;
    // Start is called before the first frame update
    void Start()
    {
        playerMovement = GetComponent<PlayerMovement>();
        animator = GetComponent<Animator>();
        isManualing = false;
    }

    // Update is called once per frame
    void Update()
    {
        PushAnimation();
        ManualAnimation();
        JumpAnimation();
        AirIdleAnimation();
        UpdateActionBooleans();
    }

  
    void PushAnimation()
    {
        if (playerMovement.GetGrounded())
        {
            float moveDirection = Input.GetAxis("Horizontal");
            int movementHash = Animator.StringToHash("X");
            animator.SetFloat(movementHash, moveDirection);
        }
    }
    void JumpAnimation()
    {
        if (Input.GetAxis("Jump") == 1)
        {
            animator.SetFloat("Y", 1);
        }

        if (Input.GetAxis("Jump") <= 0.9)
        {
            animator.SetFloat("Y", 0);
        }
    }

    void UpdateActionBooleans()
    {
        Debug.Log("isSurface = " + animator.GetBool("isSurface"));
    }

    void AirIdleAnimation()
    {
        if (isAir)
        {
            animator.SetBool("isSurface", false);
        }
        else
        {
            animator.SetBool("isSurface", true);
        }
    }

    void ManualAnimation()
    {
        if (Input.GetKeyDown(KeyCode.C))
        {
            if (isManualing)
            {
                animator.SetBool("isManual", false);
                isManualing = false;
            }
            else
            {
                animator.SetBool("isManual", true);
                isManualing = true;
            }
        }
    }
}
