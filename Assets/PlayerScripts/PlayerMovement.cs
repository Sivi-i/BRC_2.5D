using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    private CharacterController character;
    private SFX_Player SFX_;
    private Rigidbody rb;
    private Animator animator;
    private float moveDirection;
    private bool jumpPushed;
    private float playerSpeed = 6.5f;
    private float playerJumpHeightMax = 5f;
    private float playerJumpHeightCurrent = 0f;
    private float playerJumpPower = 0.16f;
    private float playerBoostPower = 0.09f;
    private float playerVelocityMax = 8f;
    private float billboardBoostPower = 0.4f;
    private float railBoostPower = 1.4f;

    public static float boostAmount = 0f;
    public static float boostAmountMax = 1f;
    private bool isJumping;
    public static bool isGrounded;
    private bool isBillboard;
    private static bool isRail;
    private bool isAirBoosting;
    private static bool isBoosting;
    private bool canJump;
    private bool canBoost;
    private bool canAirBoost;
    private bool playJumpSFX;
    private int movementHash;
    private int jumpHash;
    // Start is called before the first frame update
    void Start()
    {
        rb = gameObject.GetComponent<Rigidbody>();
        animator = GetComponent<Animator>();
        rb.centerOfMass = Vector3.zero;
        rb.inertiaTensorRotation = Quaternion.identity;
        SFX_ = GetComponent<SFX_Player>();
        canJump = true;

    }

    // Update is called once per frame
    void Update()
    {
        //AnimJump();
        HorizontalMovement();
        Jump();
        Boost();
        BillboardRunning();
        RailGrinding();
        AirBoost();
        BoostAmountHandler();

    }

    void HorizontalMovement() {
        Vector3 playerMovement = new Vector3(Input.GetAxis("Horizontal") * playerSpeed * Time.deltaTime, 0, 0);
        gameObject.transform.position += playerMovement;
        
    }

    void Jump()
    {
        if (canJump)
        {
            if (Input.GetButton("Jump"))
            {
                 if (playerJumpHeightCurrent >= playerJumpHeightMax)
                {
                    canJump = false;
                    return;
                }
                rb.AddForce(0, playerJumpPower, 0, ForceMode.Impulse);
                playerJumpHeightCurrent += 0.1f;
                isJumping = true;
                canAirBoost = true;
                canBoost = false;
            }
        }
    }

    void Boost()
    {
        if (canBoost)
        {
            if (boostAmount > 0)
            {
                if (Input.GetButton("Sprint"))
                {
                    if (rb.velocity.x > playerVelocityMax)
                    {
                        rb.velocity = new Vector3(8f, 0, 0);
                    }
                    Debug.Log("Normal Boost");
                    Vector3 playerSprint = new Vector3(playerBoostPower * Input.GetAxis("Horizontal"), 0, 0);
                    rb.AddForce(playerSprint, ForceMode.VelocityChange);
                    isBoosting = true;
                    boostAmount -= 0.0005f;
                }
            }
        }

        if (Input.GetButtonUp("Sprint"))
        {
            isBoosting = false;
        }
    }

    void AirBoost()
    {
        if (canAirBoost)
        {
            if (Input.GetKeyDown(KeyCode.LeftShift))
            {
                Debug.Log("Air Boost");
                isAirBoosting = true;
                Vector3 airBoostPower = new Vector3(Input.GetAxis("Horizontal") * 10, 0, 0);
                rb.AddForce(airBoostPower, ForceMode.Impulse);
                StartCoroutine(BoostSlowdown());
                canAirBoost = false;
            }
        }
    }

    IEnumerator BoostSlowdown()
    {
        Vector3 resetAirSpeed;
        yield return new WaitForSeconds(0.2f);
        if (Input.GetAxis("Horizontal") > 0)
        {
            resetAirSpeed = new Vector3(-3, 0, 0);
            rb.AddForce(resetAirSpeed, ForceMode.Impulse);
        }

        if (Input.GetAxis("Horizontal") < 0)
        {
            resetAirSpeed = new Vector3(3, 0, 0);
            rb.AddForce(resetAirSpeed, ForceMode.Impulse);
        }
    }

    void BoostAmountHandler()
    {
        if(boostAmount > boostAmountMax)
        {
            boostAmount = boostAmountMax;
        }
    }

    void BillboardRunning()
    {
        if (isBillboard)
        {
            rb.constraints = RigidbodyConstraints.FreezePositionY;
            Vector3 playerBillboardSpeed = new Vector3(playerSpeed * Input.GetAxis("Horizontal") * billboardBoostPower * Time.deltaTime, 0, 0);
            gameObject.transform.position += playerBillboardSpeed;
            isJumping = false;
            isGrounded = false;
            canJump = true;

            if (Input.GetKeyDown(KeyCode.Space))
            {
                isBillboard = false;
                rb.constraints = RigidbodyConstraints.None;
                return;
            }
        }

    }

    void RailGrinding()
    {
        if (isRail)
        {
            Vector3 playerRailSpeed = new Vector3(playerSpeed * Input.GetAxis("Horizontal") * railBoostPower * Time.deltaTime, 0, 0);
            gameObject.transform.position += playerRailSpeed;
        }
    }

    public bool GetBoost() {
        if (isBoosting)
        {
            return true;
        }
        return false;
    }

    public float GetBoostAmount()
    {
        return boostAmount;
    }
    public void SetBoostAmount(float boostToAdd)
    {
        boostAmount += boostToAdd;
    }

    public bool GetRail()
    {
        if (isRail)
        {
            return true;
        }
        return false;
    }

    public bool GetBillboard()
    {
        if (isBillboard)
        {
            return true;
        }
        return false;
    }

    public bool GetGrounded()
    {
        if (isGrounded)
        {
            return true;
        }
        return false;
    }
   

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.tag == "Surface")
        {
            canBoost = true;
            canJump = true;
            isGrounded = true;
            playerJumpHeightCurrent = 0f;
        }

        if(collision.collider.tag == "Rail")
        {
            isRail = true;
            canJump = true;
            isGrounded = false;
            playerJumpHeightCurrent = 0f;
            SFX_.PlayGrindLandingSFX();
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        if(collision.collider.tag == "Rail")
        {
            isRail = false;
            SFX_.PlayGrindLeavingSFX();
        }

        if(collision.collider.tag == "Surface")
        {
            canBoost = false;
            isGrounded = false;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Billboard")
        {
            isBillboard = true;
            canJump = true;
            isGrounded = false;
            playerJumpHeightCurrent = 0f;
            playerJumpHeightMax += rb.position.y/18;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if(other.tag == "Billboard")
        {
            isBillboard = false;
            canJump = false;
            playerJumpHeightMax = 1f;
            rb.constraints = RigidbodyConstraints.None;
            playerJumpHeightMax = 5f;
        }
    }
}
