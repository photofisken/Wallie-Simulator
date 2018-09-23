using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Need : MonoBehaviour
{
    public string needName;
    public NodeType type;
    [Range(0f, 1f)]
    public float value = 1f;
    public float time = 60f;

    public float refreshTime;
    public float refreshTimer;

    public bool refresh;

    private void Update()
    {
        if (refresh)
        {
            refreshTimer += Time.deltaTime / refreshTime;
            if (refreshTimer >= 1f)
            {
                refresh = false;
                GetComponent<CatBehaviour>().action = CatAction.idle;
            }
        }
    }
}
