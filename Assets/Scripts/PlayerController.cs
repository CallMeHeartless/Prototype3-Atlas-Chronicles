using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

[RequireComponent(typeof(CharacterController))]
public class PlayerController : MonoBehaviour
{

    // External references
    [SerializeField]
    private Camera m_CameraReference;

    // Component references
    private CharacterController m_CharacterController;

    // Movement variables
    [SerializeField]
    private float m_fMovementSpeed;
    private Vector3 m_MovementDirection;

    // Start is called before the first frame update
    void Start(){
        // Create component references
        m_CharacterController = GetComponent<CharacterController>();

        // Initialise variables
        m_MovementDirection = Vector3.zero;
    }

    // Update is called once per frame
    void Update(){
        m_MovementDirection = Vector3.zero;
        CalculatePlayerMovement();
        CalculatePlayerRotation();

        // Move the player
        m_CharacterController.Move(m_MovementDirection * m_fMovementSpeed * Time.deltaTime);
    }

    // Calculate movement
    private void CalculatePlayerMovement() {
        // Take player input
        m_MovementDirection = (m_CameraReference.transform.right * Input.GetAxis("Horizontal") + m_CameraReference.transform.forward * Input.GetAxis("Vertical")).normalized;
        m_MovementDirection.y = -9.81f;

        // Add gravity
    }

    // Rotates the player to look in the direction they are moving
    private void CalculatePlayerRotation() {
        // Prevent turning when stationary
        if(m_MovementDirection.sqrMagnitude == 0) {
            return;
        }
        Vector3 vecLookDirection = m_MovementDirection;
        vecLookDirection.y = 0.0f;
        transform.rotation = Quaternion.LookRotation(vecLookDirection);
    }
}
