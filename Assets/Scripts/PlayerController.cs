using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

[RequireComponent(typeof(CharacterController))]
public class PlayerController : MonoBehaviour {

    // External references
    [Header("External References")]
    [SerializeField]
    private Camera m_CameraReference;

    // Component references
    private CharacterController m_CharacterController;

#region INTERNAL_VARIABLES
    // Movement variables
    [Header("Movement Variables")]
    [SerializeField]
    private float m_fMovementSpeed;
    [SerializeField]
    private float m_fJumpPower;
    [Tooltip("Time where the player may still jump after falling")][SerializeField]
    private float m_fCoyoteTime = 0.5f;
    private float m_fCoyoteTimer = 0.0f;
    private Vector3 m_MovementDirection;
    private bool m_bCanDoubleJump = true;
    private float m_fVerticalVelocity = 0.0f;
    private float m_fGravityMulitplier = 1.0f;
    [SerializeField]
    private float m_fFloatTime = 2.0f;
    private float m_fFloatTimer = 0.0f;
    [SerializeField]
    private float m_fFloatGravityReduction = 0.8f;
    private bool m_bIsFloating = false;

    // Combat variables
    [Header("Combat Variables")]
    [SerializeField]
    private int m_iMaxHealth = 4;
    private int m_iCurrentHealth;
#endregion

    // Start is called before the first frame update
    void Start(){
        // Create component references
        m_CharacterController = GetComponent<CharacterController>();

        // Initialise variables
        m_MovementDirection = Vector3.zero;
        m_iCurrentHealth = m_iMaxHealth;
    }

    // Update is called once per frame
    void Update(){
        if (!GameState.DoesPlayerHaveControl()) {
            return;
        }

        // Calculate movement for the frame
        m_MovementDirection = Vector3.zero;
        HandlePlayerMovement();

    }

    // Handles all of the functions that determine the vector to move the player, then move them
    private void HandlePlayerMovement() {
        CalculatePlayerMovement();
        CalculatePlayerRotation();
        ApplyGravity();
        Jump();
        ProcessFloat();
        m_MovementDirection.y += m_fVerticalVelocity * Time.deltaTime;
        print(m_fVerticalVelocity);
        // Move the player
        m_CharacterController.Move(m_MovementDirection * m_fMovementSpeed * Time.deltaTime);
    }

    // Calculate movement
    private void CalculatePlayerMovement() {
        // Take player input
        m_MovementDirection = (m_CameraReference.transform.right * Input.GetAxis("Horizontal") + m_CameraReference.transform.forward * Input.GetAxis("Vertical")).normalized;
        m_MovementDirection.y = 0.0f;
    }

    // Rotates the player to look in the direction they are moving
    private void CalculatePlayerRotation() {
        // Prevent turning when stationary
        if(m_MovementDirection.sqrMagnitude == 0) {
            return;
        }
        Vector3 vecLookDirection = m_MovementDirection;
        vecLookDirection.y = 0.0f; // Remove y component
        transform.rotation = Quaternion.LookRotation(vecLookDirection);
    }

    // Performs a simple jump
    private void Jump() {
        //print("isGrounded: " + m_CharacterController.isGrounded + " doubleJump: " + m_bCanDoubleJump + " cTimer: " + m_fCoyoteTimer);
        // Handle jump input
        if (m_CharacterController.isGrounded || m_bCanDoubleJump || m_fCoyoteTimer < m_fCoyoteTime) {
            // Jump code
            if (Input.GetKeyDown(KeyCode.Space)) { // Change this here
                m_fVerticalVelocity = m_fJumpPower;
                m_fGravityMulitplier = 1.0f;
                // Control use of double jump
                if (!m_CharacterController.isGrounded) {
                    m_bCanDoubleJump = false;
                }
            }

        }

        // Handle related variables
        if (m_CharacterController.isGrounded) {
            m_bCanDoubleJump = true;
            m_fCoyoteTimer = 0.0f;
        } else {
            m_fCoyoteTimer += Time.deltaTime;
        }
    }

    // Updates the player's vertical velocity to consider gravity
    private void ApplyGravity() {
        // Check if floating
        if (m_bIsFloating) {
         //   m_fGravityMulitplier = m_fFloatGravityReduction;
        }
        if (m_CharacterController.isGrounded) {
            return;
        }

        m_fVerticalVelocity += Physics.gravity.y * m_fGravityMulitplier *  Time.deltaTime;
        if (m_CharacterController.isGrounded) {
            m_fGravityMulitplier = 1.0f;
        } else {
            m_fGravityMulitplier *= 1.1f;
            m_fGravityMulitplier = Mathf.Clamp(m_fGravityMulitplier, 1.0f, 20.0f);
        }
        m_fVerticalVelocity = Mathf.Clamp(m_fVerticalVelocity, -100.0f, 100.0f);
    }

    // Handles the player floating slowly downwards
    private void ProcessFloat() {
        if (!m_CharacterController.isGrounded && !m_bCanDoubleJump) {
            if(Input.GetKey(KeyCode.Space) && m_fFloatTimer < m_fFloatTime) {
                //m_fVerticalVelocity = -m_fFloatFallSpeed;
                m_bIsFloating = true;
                m_fFloatTimer += Time.deltaTime;
            }
        } else {
            m_fFloatTimer = 0.0f;
            m_bIsFloating = false;
            //m_fGravityMulitplier = 1.0f;
        }
    }
}
