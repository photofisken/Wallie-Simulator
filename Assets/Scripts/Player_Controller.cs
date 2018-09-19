using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_Controller : MonoBehaviour {

    [SerializeField]
    private float speed;
    [SerializeField]
    private List<Sprite> wallieWalking;

    private SpriteRenderer sr;

	void Start () {
        sr = GetComponent<SpriteRenderer>();
	}
	
	void Update ()
    {
        float horizontal = Input.GetAxisRaw("Horizontal");
        float vertical = Input.GetAxisRaw("Vertical");

        Vector3 movement = new Vector3(horizontal, vertical, 0f);
        movement.Normalize();

        transform.position += movement * speed * Time.deltaTime;

        if (vertical >= 0.9)
        {
            sr.sprite = wallieWalking[2];
        }
        else if(vertical <= -0.9f)
        {
            sr.sprite = wallieWalking[0];
        }

        if (horizontal >= 0.9)
        {
            sr.sprite = wallieWalking[1];
            sr.flipX = false;
        }
        else if(horizontal <= -0.9f)
        {           
            sr.sprite = wallieWalking[1];
            sr.flipX = true;
        }

    }
}
