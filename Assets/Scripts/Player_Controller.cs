using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_Controller : MonoBehaviour {

    [SerializeField]
    private float speed;
    [SerializeField]
    private List<Sprite> wallieWalking;

    private SpriteRenderer sr;

    private float horizontal;
    private float vertical;

    public InteractObject[] interactables;

    Vector3 mousePos;
    Vector3 dir;

	void Start ()
    {
        sr = GetComponent<SpriteRenderer>();
        interactables = FindObjectsOfType<InteractObject>();
	}
	
	void Update ()
    {
        // Direction to mouse pos
        dir = mousePos - transform.position;

        if (dir.magnitude > 1f)
            dir.Normalize();

        if (Input.GetMouseButtonDown(0))
        {
            Vector2 clickPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);

            horizontal = clickPosition.x; //Input.GetAxisRaw("Horizontal");
            vertical = clickPosition.y; //Input.GetAxisRaw("Vertical");

            mousePos = new Vector3(horizontal, vertical, 0f);

            // Direction to mouse pos
            dir = mousePos - transform.position;

            if (dir.magnitude > 1f)
                dir.Normalize();

            //Debug.Log("Clicked this position: " + clickPosition + ", direction: " + dir + ", returned " + DirectionSprite(dir));
            switch (DirectionSprite(dir))
            {
                case 0: // Right
                    sr.sprite = wallieWalking[1];
                    sr.flipX = false;
                    break;
                case 1: // Left
                    sr.sprite = wallieWalking[1];
                    sr.flipX = true;
                    break;
                case 2: // Up
                    sr.sprite = wallieWalking[2];
                    break;
                case 3: // Down
                    sr.sprite = wallieWalking[0];
                    break;
            }
        }

        transform.position += dir * speed * Time.deltaTime;

        if(Input.GetKeyDown(KeyCode.Space))
        {
            foreach(InteractObject i in interactables)
            {
                i.Interact();
            }
        }

    }

    private int DirectionSprite(Vector3 direction)
    {
        if (direction.magnitude > 0.01f)
        {
            if (Mathf.Abs(direction.x) > Mathf.Abs(direction.y))
            {
                // Right
                if (direction.x > 0)
                {
                    return 0;                 
                }
                // Left
                else
                {
                    return 1;
                }

            }
            else
            {
                // Up
                if (direction.y > 0)
                {
                    return 2;
                }
                // Down
                else
                {
                    return 3;
                }
            }
        }
        else
            return -1;
    }
}
