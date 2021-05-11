using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

namespace com.yiran.SimpleCS{
    public class CameraController : MonoBehaviourPunCallbacks
    {
        #region Variables
        public Transform player;
        private float mouseX, mouseY;
        private float mouseSensitivity = 100f;
        float xRotation = 0f;

        public Transform weapon;
        #endregion

        private void Start(){

            Cursor.lockState = CursorLockMode.Locked;
        }

        private void Update(){
            if(!photonView.IsMine) return;
            if(Pause.paused && photonView.IsMine) return;

            mouseX = Input.GetAxis ("Mouse X") * mouseSensitivity * Time.deltaTime;
            mouseY = Input.GetAxis ("Mouse Y") * mouseSensitivity * Time.deltaTime;

            xRotation -= mouseY;
            xRotation = Mathf.Clamp(xRotation, -90f, 90f);

            player.Rotate(Vector3.up * mouseX);
            weapon.localRotation = Quaternion.Euler (xRotation, 0, 0);
            transform.localRotation = Quaternion.Euler (xRotation, 0, 0);

            
        }
    }
}