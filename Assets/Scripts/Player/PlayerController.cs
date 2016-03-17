﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PlayerController : MonoBehaviour {

    public Player player;

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
            GetComponent<PlayerAnimations>().CmdSetBool("Jumping", true);
        }

        // Double Jump
        if (Input.GetButtonDown("Jump") && !player.Grounded && !player.DoubleJumped)
        {
            player.DoubleJumped = true;
            rb.velocity = new Vector3(rb.velocity.x, 0, rb.velocity.z);
            rb.velocity += new Vector3(0, player.JumpStrength, 0);
            GetComponent<PlayerAnimations>().CmdSetBool("Jumping", true);
        }

        // Animation
        if (player.Movement.x != 0 || player.Movement.z != 0)
        {
            GetComponent<PlayerAnimations>().CmdSetBool("Moving", true);
        }
        else
        {
            GetComponent<PlayerAnimations>().CmdSetBool("Moving", false);
        }

        //TODO: Fix block animation premature looping
        if (Input.GetMouseButtonDown(1))
        {
            GetComponent<PlayerAnimations>().CmdSetBool("Blocking", true);
        }

        if (Input.GetMouseButtonUp(1))
        {
            GetComponent<PlayerAnimations>().CmdSetBool("Blocking", false);
        }

        if (Input.GetMouseButtonDown(0))
        {
            GetComponent<PlayerAnimations>().CmdSetTrigger("RightPunch");
        }

        if (Input.GetKeyDown(KeyCode.H))
        {
            GetComponent<PlayerAnimations>().CmdSetTrigger("Hadouken");
        }

        if (Input.GetKeyDown(KeyCode.G))
        {
            GetComponent<PlayerAnimations>().CmdSetTrigger("RightLongPunch");
        }

        if(Input.GetKeyDown(KeyCode.Escape))
        {
            GameManager.instance.ActivatePauseMenu();
        }
    }

    //TODO: Can player move while attacking?
    void FixedUpdate()
    {
        player.Movement = player.ActualDirection.normalized * Time.deltaTime * player.Speed;
        rb.velocity = new Vector3(player.Movement.x, rb.velocity.y, player.Movement.z);
    }

    public void GetHitByAttack(Attack attack)
    {
        //player.TakeDamage(attack.damage);
        rb.AddForce(10, 0, 0);
    }

    private void CalculateActualDirection()
    {
        float verticalInput = Input.GetAxis("Vertical");
        float horizontalInput = Input.GetAxis("Horizontal");

        Vector3 direction = new Vector3(horizontalInput, 0, verticalInput);

        player.ActualDirection = transform.TransformDirection(direction);

        player.ActualDirection.Set(player.ActualDirection.x, 0, player.ActualDirection.z);
    }
}
