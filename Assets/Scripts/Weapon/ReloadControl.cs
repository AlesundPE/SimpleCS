using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReloadControl : MonoBehaviour
{
    public GameObject gun;

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("Reload")){
            gun.GetComponent<Animator>().Play("Reload");
        }
        
    }
}
