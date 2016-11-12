using UnityEngine;
using System.Collections;

public class Player1_Move : MonoBehaviour {

    public float moveSpeed = 1;

    private Rigidbody myRigidbody;

    void Start ()
    {
        myRigidbody = GetComponent<Rigidbody>();
    }
	
	void FixedUpdate ()
    {
        float moveHorizontal = Input.GetAxis("Horizontal");
        float moveVertical = Input.GetAxis("Vertical");

        Vector3 movement = new Vector3(moveHorizontal, 0.0f, moveVertical);

        myRigidbody.AddForce(movement * moveSpeed);

    }
}
