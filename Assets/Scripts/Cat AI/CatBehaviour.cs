using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public enum CatAction
{
    idle, walking, busy
}

public class CatBehaviour : MonoBehaviour
{
    [Header("State")]
    public CatAction action = CatAction.idle;
    public NodeType type = NodeType.standard;

    private Need[] needs;

    [Header("AI")]
    [SerializeField]
    private CatNode current;
    [SerializeField]
    private CatNode[] nodes;
    [SerializeField]
    private CatNode target;
    [SerializeField]
    private List<CatNode> path;

    [SerializeField]
    private float decideTime = 2f;
    [SerializeField]
    private float busyTimer = 1f;

    [SerializeField]
    private Sprite[] sprites;
    private SpriteRenderer sr;

    private void Start()
    {
        sr = GetComponent<SpriteRenderer>();
        nodes = FindObjectsOfType<CatNode>();
        needs = GetComponents<Need>();
    }

    private void Update ()
    {
        switch(action)
        {
            case CatAction.idle:
                sr.sprite = sprites[1];
                decideTime -= Time.deltaTime;
                if (decideTime <= 0f)
                {
                    decideTime = Random.Range(3f, 6f);
                    Decide();
                }
                break;

            case CatAction.busy:
                sr.sprite = sprites[1];
                float time = 1f;
                switch(type)
                {
                    case NodeType.food:
                        time = 5f;
                        needs[0].value = 1f;
                        break;
                    case NodeType.pee:
                        time = 5f;
                        needs[1].value = 1f;
                        break;
                    case NodeType.sleep:
                        time = 2f;
                        needs[2].value = 1f;
                        break;
                }

                if (current.ready)
                {
                    busyTimer -= Time.deltaTime / time;
                    if (busyTimer <= 0f)
                    {
                        action = CatAction.idle;
                        type = NodeType.standard;
                        current.Use();
                        decideTime = 1f;
                    }
                }
                else
                {
                    Debug.Log("Complain");
                }
                break;

            case CatAction.walking:
                sr.sprite = sprites[0];
                if(path.Count > 0)
                    Walk();
                break;
        }
    }

    public void NeedType(NodeType type)
    {
        this.type = type;
        Decide();
    }

    private void Decide()
    {
        switch(type)
        {
            case NodeType.pee:
                target = FindTarget(NodeType.pee);
                break;
            case NodeType.food:
                target = FindTarget(NodeType.food);
                break;
            case NodeType.sleep:
                target = FindTarget(NodeType.sleep);
                break;

            default:
                target = FindTarget(NodeType.standard);
                break;
        }
        if (current != target)
        {
            FindPath(current, target);
            action = CatAction.walking;
        }
    }

    public CatNode FindTarget(NodeType type)
    {
        List<CatNode> targets = new List<CatNode>();
        foreach(CatNode node in nodes)
        {
            if(node.type == type)
                targets.Add(node);
        }
        return targets[Random.Range(0, targets.Count)];
    }

    public void FindPath(CatNode start, CatNode target)
    {
        List<CatNode> openSet = new List<CatNode>();
        HashSet<CatNode> closedSet = new HashSet<CatNode>();
        openSet.Add(start);

        while (openSet.Count > 0)
        {
            CatNode currentNode = openSet[0];
            for(int i = 1; i < openSet.Count; i++)
            {
                if(openSet[i].fCost < currentNode.fCost || openSet[i].fCost == currentNode.fCost && openSet[i].hCost < currentNode.hCost)
                {
                    currentNode = openSet[i];
                }
            }

            openSet.Remove(currentNode);
            closedSet.Add(currentNode);

            if(currentNode == target)
            {
                RetracePath(start, target);
                return;
            }

            foreach(CatNode neighbor in currentNode.neighbors)
            {
                if(closedSet.Contains(neighbor))
                {
                    continue;
                }

                float newMovementCostToNeighbor = currentNode.gCost + GetDistance(currentNode, neighbor);
                if(newMovementCostToNeighbor < neighbor.gCost || !openSet.Contains(neighbor))
                {
                    neighbor.gCost = newMovementCostToNeighbor;
                    neighbor.hCost = GetDistance(neighbor, target);
                    neighbor.parent = currentNode;

                    if (!openSet.Contains(neighbor))
                        openSet.Add(neighbor);
                }
            }
        }
    }

    private void RetracePath(CatNode start, CatNode end)
    {
        List<CatNode> path = new List<CatNode>();
        CatNode currentNode = end;
        while(currentNode != start)
        {
            path.Add(currentNode);
            currentNode = currentNode.parent;
        }

        path.Reverse();

        this.path = path;
    }

    private void Walk()
    {
        Vector3 dir = path[0].transform.position + path[0].offset - transform.position;
        dir.Normalize();

        GetComponent<SpriteRenderer>().flipX = dir.x < 0f;

        transform.position += dir / 6f;
        if(Vector3.Distance(transform.position, path[0].transform.position + path[0].offset) < 1f)
        {
            if (path[0] == target)
            {
                if (target.type == NodeType.standard)
                    action = CatAction.idle;
                else
                {
                    action = CatAction.busy;
                    busyTimer = 1f;
                }
            }
            current = path[0];
            path.RemoveAt(0);
        }
    }

    private float GetDistance(CatNode a, CatNode b)
    {
        return Vector3.Distance(a.transform.position, a.transform.position);
    }
}
