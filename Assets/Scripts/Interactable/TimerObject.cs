using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Type
{
    general, other
}

public class TimerObject : InteractObject
{
    public Type type;
    public float timeDelay;
    [SerializeField]
    private List<Sprite> sprites = new List<Sprite>();
    [SerializeField]
    private float amountOfTime = 3;
    private bool activated = false;
    private SpriteRenderer sr;
    private void Start()
    {
        sr = GetComponent<SpriteRenderer>();
    }
    public override void Interact()
    {
        if (type == Type.general)
        {
            StartCoroutine(Timer());
        }
    }
    private void Update()
    {
        if (activated == true && sprites.Count > 0)
        {
            sr.sprite = sprites[1];
        }
        else if (sprites.Count > 0)
            sr.sprite = sprites[0];
    }

    /*public IEnumerator timer()
    {
        yield return new WaitForSeconds(timeDelay);
        // Do something after timeDelay seconds
    }*/

    private IEnumerator Timer()
    {
        activated = true;
        yield return new WaitForSeconds(amountOfTime);
        Debug.Log("ping!");
        activated = false;
    }
}
