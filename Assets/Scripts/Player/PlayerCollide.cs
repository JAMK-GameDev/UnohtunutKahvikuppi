﻿using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public class PlayerCollide : NetworkBehaviour{

    private Rigidbody rig;
    private PlayerController controls;
    private PlayerAnimations anim;

    void Start()
    {
        rig = gameObject.GetComponent<Rigidbody>();
        controls = GetComponent<PlayerController>();
        anim = GetComponent<PlayerAnimations>();
    }

    void OnTriggerEnter(Collider info)
    {
        if (info.gameObject.CompareTag("Death") && isLocalPlayer)
        {
            if(controls.player.Lives > 0)
            {
                StartCoroutine(gameObject.GetComponent<PlayerScore>().StartReSpawn());
                GameManager.instance.TakePlayerLifeToken();
            }
            else{
                gameObject.GetComponent<PlayerScore>().DeclareLoss();
            }
        }
        if (info.gameObject.CompareTag("Weapon"))
        {
            anim.CmdSetTrigger("Knockback");
            Vector3 heading = this.GetComponentInParent<Transform>().position - info.GetComponentInParent<Transform>().position;
            info.gameObject.GetComponent<Attack>().UpdateDirection(info.GetComponentInParent<Transform>().rotation.eulerAngles);
            controls.GetHitByAttack(info.gameObject.GetComponent<Attack>());
            // Update UI
            GameManager.instance.UpdateKnockoutPercent(controls.player.KnockoutPercent);
        }
        if (info.gameObject.CompareTag("CameraTrigger"))
        {
            if (isLocalPlayer)
                controls.Camera.GetComponent<NewCamController>().setFalltoDeathPosition();
        }        
    }

    void OnCollisionStay(Collision info)
    {
        // check if player touches ground
        if (isLocalPlayer && info.gameObject.CompareTag("Ground"))
        {
            controls.player.DoubleJumped = false;
            controls.player.Grounded = true;
        }
    }

    void OnCollisionExit(Collision info)
    {
        if (isLocalPlayer && info.gameObject.CompareTag("Ground"))
            controls.player.Grounded = false;  
    }

    void OnCollisionEnter(Collision info)
    {
        if (isLocalPlayer && info.gameObject.CompareTag("Ground"))
        {
            anim.CmdSetBool("Jumping", false);
        }
    }

}
