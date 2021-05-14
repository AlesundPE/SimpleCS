using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponRecoil : MonoBehaviour
{
    [HideInInspector] public Cinemachine.CinemachineFreeLook playerCamera;

    public float verticalRecoil;
    
    
    public void GenerateRecoil(){
        playerCamera.m_YAxis.Value -= verticalRecoil;
    }

    void Update()
    {
        
    }
}
