using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimationController : MonoBehaviour
{
    private GameObject m_AttackCollider;
    private GameObject m_HandCollider;
    private PlayerController m_PlayerController;

    // Start is called before the first frame update
    void Start()
    {
        m_PlayerController = transform.root.GetComponent<PlayerController>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }


}
