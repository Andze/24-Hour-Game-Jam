using UnityEngine;
using System.Collections;

public class Player1_Move : MonoBehaviour {

    public float moveSpeed = 1;

    private Rigidbody myRigidbody;
    private float deathHeight = -5;
    private Vector3 velocity;

    void Start ()
    {
        myRigidbody = GetComponent<Rigidbody>();
    }
	
	void FixedUpdate ()
    {
        float moveHorizontal = Input.GetAxis("Horizontal");
        float moveVertical = Input.GetAxis("Vertical");

        Vector3 velocity = new Vector3(moveHorizontal, 0.0f, moveVertical);

        myRigidbody.AddForce(velocity * moveSpeed);

    }

    void Update ()
    {
        if (transform.position.y <= deathHeight)
        {
            Respawn();
        }
    }

    void Respawn()
    {
        myRigidbody.velocity = Vector3.zero;
        transform.position = new Vector3(0, 1, 0);
    }
}
