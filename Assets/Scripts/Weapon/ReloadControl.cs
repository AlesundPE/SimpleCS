using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class ReloadControl : MonoBehaviourPunCallbacks
{
    public GameObject gun;

    // Update is called once per frame
    void Update()
    {
        if(!photonView.IsMine) return;
        if (Input.GetButtonDown("Reload")){
            gun.GetComponent<Animator>().Play("Reload");
        }
        
    }
}
