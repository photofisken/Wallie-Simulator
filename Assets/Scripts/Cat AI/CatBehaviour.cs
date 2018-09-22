using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[System.Serializable]
public class Need
{
    public string name;
    [Range(0f, 1f)]
    public float value = 1f;
    public float time = 60f;
    public UnityEvent action;
}

public enum CatAction
{
    sleeping, idle, walking, eating, jumping, peeing, complaining
}

public enum CatMood
{
    happy, hungry, tired, pee
}

public class CatBehaviour : MonoBehaviour
{
    [Header("State")]
    public CatAction action = CatAction.idle;
    public CatMood mood = CatMood.happy;

    [Header("AI")]
    [SerializeField]
    private CatNode current;
    [SerializeField]
    private CatNode[] nodes;
    [SerializeField]
    private CatNode target;
    [SerializeField]
    private List<CatNode> path;

    [Header("Needs")]
    public Need[] needs;

    private void Start()
    {
        nodes = FindObjectsOfType<CatNode>();
    }

    private void Update ()
    {
        foreach(Need need in needs)
        {
            need.value -= Time.deltaTime / need.time;
            need.value = Mathf.Clamp01(need.value);
        }

        if(Input.GetKeyDown(KeyCode.Space))
        {
            FindPath(current, target);
            target = nodes[Random.Range(0, nodes.Length)];
        }
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
        foreach (CatNode node in nodes)
            node.active = false;

        List<CatNode> path = new List<CatNode>();
        CatNode currentNode = end;
        while(currentNode != start)
        {
            path.Add(currentNode);
            currentNode = currentNode.parent;
        }

        path.Reverse();
        end.active = true;
        current.active = true;
        foreach(CatNode node in path)
        {
            node.active = true;
        }
        this.path = path;
    }

    private float GetDistance(CatNode a, CatNode b)
    {
        return Vector3.Distance(a.transform.position, a.transform.position);
    }
}
