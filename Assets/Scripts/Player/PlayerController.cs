using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using Photon.Pun;

namespace com.yiran.SimpleCS{
    public class PlayerController : MonoBehaviourPunCallbacks
    {
        #region Variables
        private CharacterController cc;
        private float horizontalMove, verticalMove;
        private Vector3 dir;

        //Speed & Gravity
        public float silentSpeed;
        public float crouchSpeed;
        public float walkSpeed;
        public float jumpSpeed;
        public float gravity;
        private Vector3 velocity;

        //Crouch funtion
        private float crouchedHeight = 1f;
        private float originalHeight;
        public bool isCrouching;

        //GroundCheckParameters
        public Transform groundCheck;
        public float checkRadius;
        public LayerMask groundLayer;
        public bool isGround;

        //Camera function
        public GameObject cameraParent;

        //UI Health bar function
        public int max_health;
        private int current_health;
        private Transform UI_Healthbar;

        //UI Ammo text
        private Text ui_ammo;

        //Global manager
        private Manager manager;

        //Access weapon
        private Weapons weapons;


        #endregion

        #region Photon Calls
        /*
        public void OnPhotonSerializeView(PhotonStream p_stream, PhotonMessageInfo p_message){
            if (p_stream.IsWriting){
                p_stream.SendNext((int)(weaponParent))
            }
        }
        */
        #endregion

        #region Monobehavior Callbacks
        private void Start(){
            manager = GameObject.Find("Manager").GetComponent<Manager>();
            weapons = GetComponent<Weapons>();

            current_health = max_health;

            cameraParent.SetActive(photonView.IsMine);

            if(!photonView.IsMine) gameObject.layer = 8;

            if(Camera.main) Camera.main.enabled = false;
            if(photonView.IsMine){
                UI_Healthbar = GameObject.Find("HUD/Health/Bar").transform;
                ui_ammo = GameObject.Find("HUD/Ammo/Text").GetComponent<Text>();
                RefreshHealthBar();
            }
            
            cc = GetComponent<CharacterController>();
            originalHeight = cc.height; //Obtain the original height of collide box for crouching
        }
        private void Update(){
            if(!photonView.IsMine) return;

            //Declare local move speed
            float moveSpeed = walkSpeed;

            #region Control Pause
            bool pause = Input.GetKeyDown(KeyCode.Escape);

            if(pause){GameObject.Find("Pause").GetComponent<Pause>().TogglePause();}
            if(Pause.paused){
                isGround = false;
                isCrouching = false;
                pause = false;

            }
            #endregion


            //Update UI
            RefreshHealthBar();
            weapons.RefreshAmmo(ui_ammo);

            isGround = Physics.CheckSphere(groundCheck.position,checkRadius,groundLayer);

            //Reset the grounded velocity
            if (isGround && velocity.y < 0){
                velocity.y = -2f;
            }

            //Get move inputs
            horizontalMove = Input.GetAxis ("Horizontal") ;
            verticalMove = Input.GetAxis ("Vertical") ;

            //Shift for silent walk, or sprinting
            if (Input.GetButton("Shift")){
                moveSpeed = silentSpeed;
            }
            else if (Input.GetButton("Crouch")){
                moveSpeed = crouchSpeed;
            }
            else{
                moveSpeed = walkSpeed;
            }

            //Crouch script
            if (Input.GetButtonDown("Crouch")){
                Crouch();
            }
            else if (Input.GetButtonUp("Crouch")){
                afterCrouch();
            }

            //Testing for suicide
            if (Input.GetKeyDown(KeyCode.K)) TakeDamage(20);

            //Move the controller 
            Vector3 dir = transform.forward * verticalMove + transform.right * horizontalMove;
            cc.Move(dir * Time.deltaTime * moveSpeed);

            //Jump script, along with grounded
            if (Input.GetButtonDown("Jump") && isGround){
                
                velocity.y = Mathf.Sqrt(jumpSpeed * 2f * gravity);
            }

            //Gravity script for free fall
            velocity.y -= gravity * Time.deltaTime;
            cc.Move(velocity*Time.deltaTime);

        }
        #endregion


        void RefreshHealthBar(){
            float t_health_ratio = (float)current_health/(float)max_health;
            UI_Healthbar.localScale = Vector3.Lerp(UI_Healthbar.localScale, new Vector3(t_health_ratio,1,1), Time.deltaTime * 8f);
        }

        //Do crouch, reducing height
        void Crouch(){
            cc.height = crouchedHeight;
            isCrouching = true;
        }
        //Reset height after crouch
        void afterCrouch(){
            cc.height = originalHeight;
            isCrouching = false;
        }

        public void TakeDamage(int p_damage){
            if (photonView.IsMine){
                current_health -= p_damage;
                Debug.Log(current_health);
                RefreshHealthBar();
            }
            if(current_health <= 0){
                Debug.Log("YOU DIED");
                manager.Spawn();
                PhotonNetwork.Destroy(gameObject);
            }
        }
        
    }
}