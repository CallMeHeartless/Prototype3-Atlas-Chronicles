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
    }

    // Calculate movement
    private void CalculatePlayerMovement() {
        // Get free look camera rotation
        Vector3 vecCameraRotation = m_CameraReference.transform.eulerAngles;
        print(vecCameraRotation);
        // Take player input



        // Add gravity
    }
}
