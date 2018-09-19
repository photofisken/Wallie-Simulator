using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sort : MonoBehaviour
{
    [SerializeField]
    private bool dynamic;
    private SpriteRenderer sr;

	void Start ()
    {
        sr = GetComponent<SpriteRenderer>();
        SetZ();
    }

    void Update ()
    {
        if(dynamic)
            SetZ();
	}

    private void SetZ()
    {
        transform.position -= new Vector3(0f, 0f, (transform.position.y + sr.bounds.size.y / 2f) / 10f);
    }
}
