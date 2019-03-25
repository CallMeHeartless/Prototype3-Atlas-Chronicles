using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

[RequireComponent(typeof(CharacterController))]
public class PlayerController : MonoBehaviour
{

    // Component references
    private CharacterController m_CharacterController;

    // Start is called before the first frame update
    void Start(){
        m_CharacterController = GetComponent<CharacterController>();
    }

    // Update is called once per frame
    void Update(){
        
    }

    // Calculate movement

}
