using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum NodeType
{
    standard, food, sleep, play, pee
}

[ExecuteInEditMode]
public class CatNode : MonoBehaviour
{
    public NodeType type = NodeType.standard;
    public CatNode[] neighbors;
    public float gCost;
    public float hCost;
    public CatNode parent;
    public bool active;

    public float fCost
    {
        get
        {
            return gCost + hCost;
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        foreach (CatNode node in neighbors)
        {
            if (node.active && active)
            {
                Transform t = node.transform;
                Gizmos.DrawLine(transform.position, t.position);
            }
        }
        if(active)
        {
            Gizmos.DrawWireSphere(transform.position, 5f);
        }
    }
}
