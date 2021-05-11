using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float bulletSpeed;
    private Rigidbody bulletRigidbody;
    private Transform bulletTransform;

    private void Start(){
        bulletRigidbody = GetComponent<Rigidbody>();
        bulletTransform = transform;
    }

    private void FixedUpdate(){
        bulletRigidbody.velocity = bulletTransform.forward * bulletSpeed * Time.fixedTime;
    }
}
