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

	void Start ()
    {
        sr = GetComponent<SpriteRenderer>();
        interactables = FindObjectsOfType<InteractObject>();
	}
	
	void Update ()
    {

        Vector3 mousePos = new Vector3(horizontal, vertical, 0f);

        // Direction to mouse pos
        Vector3 dir = mousePos - transform.position;

        if (dir.magnitude > 1f)
            dir.Normalize();

        if (Input.GetMouseButtonDown(0))
        {
            Vector2 clickPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Debug.Log("Clicked this position: " + clickPosition);

            horizontal = clickPosition.x; //Input.GetAxisRaw("Horizontal");
            vertical = clickPosition.y; //Input.GetAxisRaw("Vertical");
            Debug.Log("direction: " + dir);
        }


        if (vertical >= transform.position.y)
        {
            sr.sprite = wallieWalking[2];
        }
        else if(vertical <= transform.position.y)
        {
            sr.sprite = wallieWalking[0];
        }

        if (horizontal >= transform.position.x)
        {
            sr.sprite = wallieWalking[1];
            sr.flipX = false;
        }
        else if(horizontal <= transform.position.x)
        {           
            sr.sprite = wallieWalking[1];
            sr.flipX = true;
        }

        if(Input.GetKeyDown(KeyCode.Space))
        {
            foreach(InteractObject i in interactables)
            {
                i.Interact();
            }
        }


        transform.position += dir * speed * Time.deltaTime;

    }
}
