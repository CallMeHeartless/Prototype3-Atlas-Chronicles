using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New HoldableItem", menuName ="Holdable Item", order = 51)]
public class HoldableItem : ScriptableObject
{
    [SerializeField]
    private Mesh m_Mesh;
    [SerializeField]
    private MeshRenderer m_Renderer;
}
