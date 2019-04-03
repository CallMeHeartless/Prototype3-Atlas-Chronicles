using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Switchable : MonoBehaviour
{
    [SerializeField]
    private bool m_bChangeMaterialOnTag = false;
    [SerializeField]
    private Material m_AlternateMaterial;
    private Material m_Material;

    private void Start() {
        m_Material = GetComponent<MeshRenderer>().material;
    }

    public void Tag() {
        if (m_bChangeMaterialOnTag) {
            GetComponent<MeshRenderer>().material = m_AlternateMaterial;
        }
    }

    public void DeTag() {
        if (m_bChangeMaterialOnTag) {
            GetComponent<MeshRenderer>().material = m_Material;
        }
    }

}
