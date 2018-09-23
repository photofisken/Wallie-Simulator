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

    [Header("Needs")]
    private Need[] needs;

    private void Start()
    {
        nodes = FindObjectsOfType<CatNode>();
        needs = GetComponents<Need>();
    }

    private void Update ()
    {
        foreach (Need need in needs)
        {
            need.value -= Time.deltaTime / need.time;
            need.value = Mathf.Clamp01(need.value);

            if (need.value <= 0.01f && type == NodeType.standard)
            {
                decideTime = Random.Range(3f, 6f);
                type = need.type;
                Decide();
                return;
            }
        }

        switch(action)
        {
            case CatAction.idle:
                decideTime -= Time.deltaTime;
                if (decideTime <= 0f)
                {
                    decideTime = Random.Range(3f, 6f);
                    Decide();
                }
                break;

            case CatAction.busy:
                busyTimer -= Time.deltaTime;
                if(busyTimer <= 0f)
                {
                    action = CatAction.idle;

                }
                break;

            case CatAction.walking:
                if(path.Count > 0)
                    Walk();
                break;
        }
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
        Vector3 dir = path[0].transform.position - transform.position;
        dir.Normalize();

        GetComponent<SpriteRenderer>().flipX = dir.x < 0f;

        transform.position += dir / 6f;
        if(Vector3.Distance(transform.position, path[0].transform.position) < 1f)
        {
            if (path[0] == target)
            {
                if (target.type == NodeType.standard)
                    action = CatAction.idle;
                else
                {
                    action = CatAction.busy;
                    busyTimer = 10f;
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
