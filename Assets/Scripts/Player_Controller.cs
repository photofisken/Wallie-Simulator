using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_Controller : MonoBehaviour {

    [SerializeField]
    private float speed;

	void Start () {
		
	}
	
	void Update ()
    {
        float horizontal = Input.GetAxisRaw("Horizontal");
        float vertical = Input.GetAxisRaw("Vertical");

        Vector3 movement = new Vector3(horizontal, vertical, 0f);
        movement.Normalize();

        transform.position += movement * speed * Time.deltaTime;
    }
}
