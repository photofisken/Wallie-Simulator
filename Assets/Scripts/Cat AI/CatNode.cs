using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum NodeType
{
    standard, food, sleep, pee
}

[ExecuteInEditMode]
public class CatNode : MonoBehaviour
{
    public NodeType type = NodeType.standard;
    public CatNode[] neighbors;
    public float gCost;
    public float hCost;
    public CatNode parent;

    public float fCost
    {
        get
        {
            return gCost + hCost;
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, 5f);
    }
}
