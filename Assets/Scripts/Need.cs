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

    private CatBehaviour cb;

    private void Start()
    {
        cb = GetComponent<CatBehaviour>();
    }

    private void Update()
    {
        if (cb.type == NodeType.standard)
        {
            value -= Time.deltaTime / time;
            value = Mathf.Clamp01(value);

            if (value <= 0.01f)
            {
                GetComponent<CatBehaviour>().NeedType(type);
            }
        }
    }
}
