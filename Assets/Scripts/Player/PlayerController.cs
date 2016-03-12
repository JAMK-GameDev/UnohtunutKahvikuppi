﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PlayerController : MonoBehaviour {

    public Player player;

    public Vector3 actualDirection, movement;

    private Rigidbody rb;

    // Use this for initialization
    void Start () {
        rb = GetComponent<Rigidbody>();
        player = new Player();
    }
	
	// Update is called once per frame
	void Update () {
        CalculateActualDirection();
        

        // Jump
        if (Input.GetButtonDown("Jump") && player.Grounded)
        {
            rb.velocity += new Vector3(0, player.JumpStrength, 0);
        }

        // Animation
        if (movement.x != 0 || movement.z != 0)
        {
            GetComponent<PlayerAnimations>().CmdSetBool("Moving", true);
        }
        else
        {
            GetComponent<PlayerAnimations>().CmdSetBool("Moving", false);
        }
    }

    //TODO: Can player move while attacking?
    void FixedUpdate()
    {
        movement = actualDirection.normalized * Time.deltaTime * player.Speed;
        rb.velocity = new Vector3(movement.x, rb.velocity.y, movement.z);
    }

    private void CalculateActualDirection()
    {
        float verticalInput = Input.GetAxis("Vertical");
        float horizontalInput = Input.GetAxis("Horizontal");

        Vector3 direction = new Vector3(horizontalInput, 0, verticalInput);

        actualDirection = transform.TransformDirection(direction);

        actualDirection.Set(actualDirection.x, 0, actualDirection.z);

    }
}
