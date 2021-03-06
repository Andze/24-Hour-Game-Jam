﻿using UnityEngine;
using System.Collections;

public class PlayerMove : MonoBehaviour
{
    public float moveSpeed = 5.0f;
    public float jumpForce = 200.0f;
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
    private PlayerPaint pp;
    private ParticlePlayer particles;

    void Start()
    {
        rigidBody = GetComponent<Rigidbody>();
        deathHeight = -5.0f;
        respawnPoint = transform.position;
        pp = GetComponent<PlayerPaint>();
        particles = GetComponent<ParticlePlayer>();
        if (particles && pp)
            particles.color = pp.brushColor;

        SetControlScheme();
    }
    
    void FixedUpdate()
    {
        float moveHorizontal = 0.0f, moveVertical = 0.0f;

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
            RaycastHit hit;
            if (Physics.Raycast(transform.position, Vector3.down, out hit, 0.26f))
            {
                rigidBody.AddForce(new Vector3(0.0f, jumpForce, 0.0f));
                
                inAir = true;
            }
        }

       Vector3 velocity = new Vector3(moveHorizontal * moveSpeed, 0.0f, moveVertical * moveSpeed);
        rigidBody.AddForce(velocity);
    }

    float airTime = 0.0f;
    bool inAir = true;
    void Update()
    {
        if (inAir)
            airTime += Time.deltaTime;

        if (transform.position.y <= deathHeight)
        {
            Respawn();
        }
    }

    private void Respawn()
    {
        rigidBody.velocity = Vector3.zero;
        transform.position = respawnPoint;
        airTime = 0.0f;
        inAir = true;
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

    void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.tag == "Floor")
        {
            if (pp)
            {
                pp.Paint(pp.brushSize * (1.0f + (airTime * 1.5f)));
                inAir = false;
                airTime = 0.0f;
            }

            if (particles)
                particles.Play(pp.brushColor);
        }
    }
}


