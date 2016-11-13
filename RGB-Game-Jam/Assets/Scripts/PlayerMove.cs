using UnityEngine;
using System.Collections;

public class PlayerMove : MonoBehaviour
{
    public float moveSpeed = 5.0f;
    public enum ControllerSet
    {
        WASD,
        ULDR
    }
    public ControllerSet controlScheme = ControllerSet.WASD;

    private Rigidbody rigidBody;
    private float deathHeight;
    private Vector3 velocity;
    private Vector3 respawnPoint;
    private KeyCode[] inputKeys;
    private bool Grounded;

    void Start()
    {
        rigidBody = GetComponent<Rigidbody>();
        deathHeight = -5.0f;
        respawnPoint = transform.position;

        SetControlScheme();
    }

    void FixedUpdate()
    {
        float moveHorizontal = 0.0f, moveVertical = 0.0f, Jump = 0.0f;

        if (Input.GetKey(inputKeys[0]))
            moveVertical += 1.0f;
        if (Input.GetKey(inputKeys[2]))
            moveVertical -= 1.0f;

        if (Input.GetKey(inputKeys[1]))
            moveHorizontal -= 1.0f;
        if (Input.GetKey(inputKeys[3]))
            moveHorizontal += 1.0f;

        if (Input.GetKeyDown(inputKeys[4]))
        {  
            //if not in the air
            RaycastHit hit;
            if (Physics.Raycast(transform.position, Vector3.down, out hit, 0.26f))
            {
                Jump += 25.0f;
            }               
        }

       Vector3 velocity = new Vector3(moveHorizontal, Jump, moveVertical);
        rigidBody.AddForce(velocity * moveSpeed);
    }

    void Update()
    {
        if (transform.position.y <= deathHeight)
        {
            Respawn();
        }
    }

    private void Respawn()
    {
        rigidBody.velocity = Vector3.zero;
        transform.position = respawnPoint;
    }

    private void SetControlScheme()
    {
        inputKeys = new KeyCode[5];

        if (controlScheme == ControllerSet.WASD)
        {
            inputKeys[0] = KeyCode.W;
            inputKeys[1] = KeyCode.A;
            inputKeys[2] = KeyCode.S;
            inputKeys[3] = KeyCode.D;
            inputKeys[4] = KeyCode.LeftShift;
        }
        else if (controlScheme == ControllerSet.ULDR)
        {
            inputKeys[0] = KeyCode.UpArrow;
            inputKeys[1] = KeyCode.LeftArrow;
            inputKeys[2] = KeyCode.DownArrow;
            inputKeys[3] = KeyCode.RightArrow;
            inputKeys[4] = KeyCode.RightShift;
        }
    }
}


