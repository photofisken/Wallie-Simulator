using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractObject : MonoBehaviour
{
    public virtual void Interact()
    {
        Debug.Log("Interacted with " + transform.name);
    }
}
