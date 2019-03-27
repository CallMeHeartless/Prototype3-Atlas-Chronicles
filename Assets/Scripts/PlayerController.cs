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
    private Animator m_Animator;

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
    [Tooltip("The time that the player can float for")][SerializeField]
    private float m_fFloatTime = 2.0f;
    private float m_fFloatTimer = 0.0f;
    [Tooltip("The fraction of that gravity affects the player while they are floating")][SerializeField]
    private float m_fFloatGravityReduction = 0.8f;
    private bool m_bIsFloating = false;

    // Combat variables
    [Header("Combat Variables")]
    [SerializeField]
    private int m_iMaxHealth = 4;
    private int m_iCurrentHealth;

    // Ability variables
    [Header("Ability Variables")]
    [Tooltip("The game object that will be used as the teleport marker")][SerializeField]
    private GameObject m_TeleportMarkerPrefab;
    private Vector3 m_vecTeleportLocation;
    private bool m_bTeleportMarkerDown = false;
    private GameObject m_TeleportMarker; // Object to be instantiated and moved accordingly
    private GameObject m_SwitchTarget;
#endregion

    // Start is called before the first frame update
    void Start(){
        // Create component references
        m_CharacterController = GetComponent<CharacterController>();
        m_Animator = GetComponentInChildren<Animator>();

        // Initialise variables
        m_MovementDirection = Vector3.zero;
        m_iCurrentHealth = m_iMaxHealth;

        if (m_TeleportMarkerPrefab) {
            m_TeleportMarker = Instantiate(m_TeleportMarkerPrefab);
            m_TeleportMarker.SetActive(false);
        }
    }

    // Update is called once per frame
    void Update(){
        if (!GameState.DoesPlayerHaveControl()) {
            return;
        }

        // Calculate movement for the frame
        m_MovementDirection = Vector3.zero;
        HandlePlayerMovement();
        HandlePlayerAbilities();

    }

    // Handles all of the functions that determine the vector to move the player, then move them
    private void HandlePlayerMovement() {
        CalculatePlayerMovement();
        CalculatePlayerRotation();
        ApplyGravity();

        ProcessFloat();
        Jump();
        m_MovementDirection.y += m_fVerticalVelocity * Time.deltaTime;
        //print(m_fVerticalVelocity);
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
            if (Input.GetButtonDown("XButton")) { // Change this here
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
            if (m_bIsFloating) {
                m_bIsFloating = false;
            }
        } else {
            m_fCoyoteTimer += Time.deltaTime;
        }
    }

    // Updates the player's vertical velocity to consider gravity
    private void ApplyGravity() {
        // Check if floating
        if (m_bIsFloating) {
            m_fGravityMulitplier = m_fFloatGravityReduction;
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
            // The player can start floating after a double jump
            if(Input.GetButtonDown("XButton") && m_fFloatTimer == 0.0f) { // Change comparison to < m_fFloatTimer for multiple floats per jump
                ToggleFloatState(true);
            }
            else if (Input.GetButtonUp("XButton")) {
                ToggleFloatState(false);
            }
        }
        // Increment float timer while the player is floating
        if (m_bIsFloating) {
            m_fFloatTimer += Time.deltaTime;
            if(m_fFloatTimer > m_fFloatTime) {
                ToggleFloatState(false);
            }
        }
        // Allow the character to float again only once they have touched the ground
        if (m_CharacterController.isGrounded) {
            m_fFloatTimer = 0.0f;
        }
    }

    // Toggles the internal variables when the player starts/stops floating
    private void ToggleFloatState(bool _bState) {
        if(m_bIsFloating == _bState) {
            return;
        }
        m_bIsFloating = _bState;

        if (m_bIsFloating) {
            // Level out the player's upward velocity to begin gliding
            m_fVerticalVelocity = 0.0f;
        }
    }

    // Deals damage to the player, and checks for death
    public void DamagePlayer(int _iDamage) {
        m_iCurrentHealth -= _iDamage;
        if(m_iCurrentHealth <= 0) {
            // Death
            print("Player is dead");
        }
    }

    // Handles all of the functions that control player abilities
    private void HandlePlayerAbilities() {
        // Handle placing a teleport marker
        if (Input.GetButtonDown("SquareButton")) {
            PlaceTeleportMarker();
        }
        else if (Input.GetButtonDown("CircleButton")) {
            TeleportToTeleportMarker();
        }
        else if (Input.GetButtonDown("TriangleButton")) {
            if (m_SwitchTarget) {
                SwitchWithTarget();
            } else {
                PlaceSwitchMarker();
            }
        }
    }

    private void TeleportToLocation(Vector3 _vecTargetLocation) {
        // Play VFX

        // Update position
        transform.position = _vecTargetLocation;
    }

    private void PlaceTeleportMarker() {
        if (!m_TeleportMarker) {
            return;
        }

        m_TeleportMarker.transform.position = transform.position; // Need to use an offset, perhaps with animation
        // Enable teleport marker
        if (!m_TeleportMarker.activeSelf) {
            m_TeleportMarker.SetActive(true); // Replace this with teleport scroll animations, etc
            m_bTeleportMarkerDown = true;
        }
    }

    private void TeleportToTeleportMarker() {
        if (!m_bTeleportMarkerDown || !m_TeleportMarker) {
            return; // Error animation / noise
        }
        TeleportToLocation(m_TeleportMarker.transform.position);
        // Disable teleport marker
        m_TeleportMarker.SetActive(false);
    }

    private void PlaceSwitchMarker() {
        // Animation

        // DEBUG
        m_SwitchTarget = GameObject.Find("SwitchTest");
    }

    private void SwitchWithTarget() {
        if (!m_SwitchTarget) {
            return;
        }

        // Switch positions
        Vector3 vecPlayerPosition = transform.position;
        transform.position = m_SwitchTarget.transform.position;
        m_SwitchTarget.transform.position = vecPlayerPosition;

        // Remove reference
        m_SwitchTarget = null;
    }
}
