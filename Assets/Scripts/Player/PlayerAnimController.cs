using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class PlayerAnimController : MonoBehaviourPunCallbacks
{
    private Animator anim;

    void Start(){
        if (photonView.IsMine){anim = GetComponent<Animator>();}
    }

    // Update is called once per frame
    void Update()
    {
        if (!photonView.IsMine) {return;}
        if (Input.GetButton("Crouch")){
            anim.SetBool("isCrouching", true);
        } else{
            anim.SetBool("isCrouching", false);
        }

        if (Input.GetButton("Shift")){
            anim.SetBool("isSilent", true);
        } else{
            anim.SetBool("isSilent", false);
        }

        if ((Input.GetButton("Vertical")) || (Input.GetButton("Horizontal"))){
            anim.SetBool("isRunning", true);
        } else{
            anim.SetBool("isRunning", false);
        }
    }
}
