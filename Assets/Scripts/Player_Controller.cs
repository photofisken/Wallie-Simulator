using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_Controller : MonoBehaviour {

    [SerializeField]
    private float speed = 30;
    [SerializeField]
    private List<Sprite> wallieWalking;
    [SerializeField]
    private float maxDistance = 5;

    private SpriteRenderer sr;

    private float horizontal;
    private float vertical;

    public InteractObject[] interactables;
    

    Vector3 targetPos;
    Vector3 dir;

    InteractObject io;
    Rigidbody2D rigidBody;

	void Start ()
    {
        sr = GetComponent<SpriteRenderer>();
        rigidBody = GetComponent<Rigidbody2D>();
        interactables = FindObjectsOfType<InteractObject>();
        horizontal = transform.position.x;
        vertical = transform.position.y;
        targetPos = transform.position;
	}
	
	void Update ()
    {
        GetDir();

        if (Input.GetMouseButtonDown(0))
            MouseClicked();

        Move();
    }

    void GetDir()
    {
        // Direction to mouse pos
        dir = targetPos - transform.position;

        if (dir.magnitude > 1f)
            dir.Normalize();
    }

    void MouseClicked()
    {
        Ray();

        Vector2 clickPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        horizontal = clickPosition.x;
        vertical = clickPosition.y;

        targetPos = new Vector3(horizontal, vertical, 0f);

        // Direction to mouse pos
        dir = targetPos - transform.position;

        if (dir.magnitude > 1f)
            dir.Normalize();

        ChangeDirSprite();
    }

    void Ray()
    {
        Vector2 rayPosition = new Vector2(GameManager.instance.mainCamera.ScreenToWorldPoint(Input.mousePosition).x, GameManager.instance.mainCamera.ScreenToWorldPoint(Input.mousePosition).y);
        RaycastHit2D hit = Physics2D.Raycast(rayPosition, Vector2.zero, 0f);
        if (hit)
        {
            io = hit.transform.GetComponent<InteractObject>();
            Debug.Log("Clicked on " + hit.transform.name);
        }
        else
        {
            io = null;
        }
    }

    void ChangeDirSprite()
    {
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

    private void Move()
    {
        // Move Towards Target
        transform.position += dir * speed * Time.deltaTime;

        if (IsInRadius(targetPos, transform.position, maxDistance))
        {
            Debug.Log(rigidBody.velocity.magnitude);
            if (io != null)
            {
                io.Interact();
                io = null;
                dir = Vector2.zero;
                targetPos = transform.position;
            }
            else if (rigidBody.velocity.magnitude <= 0.2)
            {
                dir = Vector2.zero;
                targetPos = transform.position;
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

    bool IsInRadius(Vector3 to, Vector3 from, float maxDistance)
    {
        //float distance2 = distance * distance;
        //float horizontal = from.x - to.x;
        //float vertical = from.y - to.y;
        //horizontal = horizontal * horizontal;
        //vertical = vertical * vertical;
        float distance = Vector3.Distance(to, from);

        return (maxDistance >= distance);
    }
}
