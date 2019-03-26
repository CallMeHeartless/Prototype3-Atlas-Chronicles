using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

[RequireComponent(typeof(CharacterController))]
public class PlayerController : MonoBehaviour {

    // External references
    [SerializeField]
    private Camera m_CameraReference;

    // Component references
    private CharacterController m_CharacterController;

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
    private float m_fGravityMulitplier = 4.0f;

    // Start is called before the first frame update
    void Start(){
        // Create component references
        m_CharacterController = GetComponent<CharacterController>();

        // Initialise variables
        m_MovementDirection = Vector3.zero;
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

    private void HandlePlayerMovement() {
        CalculatePlayerMovement();
        CalculatePlayerRotation();
        ApplyGravity();
        Jump();
        m_MovementDirection.y += m_fVerticalVelocity * Time.deltaTime;

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
        // Handle jump input
        if(m_CharacterController.isGrounded || m_bCanDoubleJump || m_fCoyoteTimer < m_fCoyoteTime) {
            print("grounded: " + m_CharacterController.isGrounded + " dj: " + m_bCanDoubleJump + " ctimer: " + m_fCoyoteTimer);
            // Jump code
            if (Input.GetKeyDown(KeyCode.Space)) { // Change this here
                m_fVerticalVelocity = m_fJumpPower;
                m_fGravityMulitplier = 1.0f;
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
        m_fVerticalVelocity += Physics.gravity.y * m_fGravityMulitplier *  Time.deltaTime;
        if (m_CharacterController.isGrounded) {
            m_fGravityMulitplier = 1.0f;
        } else {
            m_fGravityMulitplier *= 1.1f;
            Mathf.Clamp(m_fGravityMulitplier, 1.0f, 20.0f);
        }
        print(m_fGravityMulitplier);
    }
}
