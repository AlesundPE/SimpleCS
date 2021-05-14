using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class MuzzleFlash : MonoBehaviourPunCallbacks
{   
    public GameObject muzzleFlash;    
    void Update()
    {
        if(!photonView.IsMine) return;
        if(Input.GetMouseButtonDown(0) || Input.GetMouseButton(0)){
            muzzleFlash.SetActive(true);
        } else{
            muzzleFlash.SetActive(false);
        }
        
    }
}
