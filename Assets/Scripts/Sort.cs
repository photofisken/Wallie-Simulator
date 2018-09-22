using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sort : MonoBehaviour
{
    [SerializeField]
    private bool dynamic;
    [SerializeField]
    private float yOffset;
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
        sr.sortingOrder = Mathf.RoundToInt(transform.position.y + yOffset);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.DrawWireSphere(transform.position + new Vector3(0f, yOffset, 0f), 1f);
    }
}
