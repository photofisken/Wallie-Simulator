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
    public bool ready = true;

    public NodeType type = NodeType.standard;
    public CatNode[] neighbors;
    public float gCost;
    public float hCost;
    public CatNode parent;
    public Vector3 offset;

    public SpriteRenderer target;
    public Sprite[] sprites;

    private void Start()
    {
        
    }

    public void Use()
    {
        if (sprites.Length > 0)
        {
            if (GetComponent<StateObject>())
                GetComponent<StateObject>().Use();
            target.sprite = sprites[1];
        }
    }

    public void Refill()
    {
        ready = true;
        if(sprites.Length > 0)
            target.sprite = sprites[0];
    }

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
        Gizmos.DrawWireSphere(transform.position + offset, 5f);
    }
}
