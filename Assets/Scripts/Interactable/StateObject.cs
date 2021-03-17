using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum State
{
    interactable, unavailable
}

public class StateObject : InteractObject
{
    public State state;
    public Sprite sprite;

    public override void Interact()
    {
        base.Interact();
        if(state == State.interactable)
        {
            state = State.unavailable;
            GetComponent<SpriteRenderer>().sprite = sprite;
        }
    }

    public void Use()
    {
        if(state == State.unavailable)
        {
            state = State.interactable;
        }
    }
}
