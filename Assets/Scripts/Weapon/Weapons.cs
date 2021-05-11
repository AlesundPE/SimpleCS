using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using Photon.Pun;

namespace com.yiran.SimpleCS{
    public class Weapons : MonoBehaviourPunCallbacks
    {
        #region Variables
        public GunGeneration[] loadout;
        [HideInInspector] public GunGeneration currentGunData;

        public Transform weaponParent;
        public GameObject bulletHolePrefab;
        public LayerMask canBeShot;

        private int currentIndex;
        private GameObject currentEquipment;
        private float currentCooldown;

        public AudioSource sfx;
        AudioClip previousClip;

        private bool isReloading;
        private bool isCrouching;

        #endregion

        #region Private methods

        void Start(){
            foreach(GunGeneration a in loadout) a.Initialize();
        }

        void Update()
        {
            if(Pause.paused && photonView.IsMine) return;

            if (photonView.IsMine && Input.GetKeyDown(KeyCode.Alpha1)) {photonView.RPC("Equip", RpcTarget.All, 0);}
            if (photonView.IsMine && Input.GetKeyDown(KeyCode.Alpha2)) {photonView.RPC("Equip", RpcTarget.All, 1);}
            if (currentEquipment != null){
                if (photonView.IsMine){
                    if(loadout[currentIndex].isAutomatic){
                        if(Input.GetMouseButton(0) && currentCooldown <= 0){
                            if (loadout[currentIndex].FireBullet()) photonView.RPC("Shoot",RpcTarget.All);
                            else StartCoroutine(Reload(loadout[currentIndex].reload));
                        }
                    }
                    else {
                        if(Input.GetMouseButtonDown(0) && currentCooldown <= 0){
                            if (loadout[currentIndex].FireBullet()) photonView.RPC("Shoot",RpcTarget.All);
                            else StartCoroutine(Reload(loadout[currentIndex].reload));
                        }
                    }
                    

                    if (Input.GetKeyDown(KeyCode.R)) StartCoroutine(Reload(loadout[currentIndex].reload));

                    //Fire cooldown
                    if(currentCooldown >0) currentCooldown -= Time.deltaTime;
                }

                //Weapon position elasticity 
                currentEquipment.transform.localPosition = Vector3.Lerp(
                currentEquipment.transform.localPosition, Vector3.zero, Time.deltaTime * 4f);
            }

            
        }

        IEnumerator Reload(float p_wait){
            isReloading = true;

            yield return new WaitForSeconds(p_wait);

            loadout[currentIndex].Reload();
            
            isReloading = false;
        }

        [PunRPC]
        void Equip(int p_index){

            if (currentEquipment != null) {
                if (isReloading) StopCoroutine("Reload");
                Destroy(currentEquipment);
            }
            currentIndex = p_index;

            GameObject newEquipment = Instantiate(loadout[p_index].prefab, 
            weaponParent.position, weaponParent.rotation, weaponParent) as GameObject;
            newEquipment.transform.localPosition = Vector3.zero;
            newEquipment.transform.localEulerAngles = Vector3.zero;
            newEquipment.GetComponent<Sway>().isMine = photonView.IsMine;

            currentEquipment = newEquipment;
        }
        
        [PunRPC]
        void Shoot(){
            isCrouching = GetComponent<PlayerController>().isCrouching;

            Transform t_Spawn = transform.Find("Player Camera");

            //Set bloom
            Vector3 t_bloom = t_Spawn.position + t_Spawn.forward* 1000f;


            //Bloom

            if (isCrouching){
                t_bloom += Random.Range(-loadout[currentIndex].bloom*0.5f, loadout[currentIndex].bloom*0.5f)* t_Spawn.up;
                t_bloom += Random.Range(-loadout[currentIndex].bloom*0.5f, loadout[currentIndex].bloom*0.5f)* t_Spawn.right;
                t_bloom -= t_Spawn.position;
                t_bloom.Normalize();
            }
            else {
                t_bloom += Random.Range(-loadout[currentIndex].bloom, loadout[currentIndex].bloom)* t_Spawn.up;
                t_bloom += Random.Range(-loadout[currentIndex].bloom, loadout[currentIndex].bloom)* t_Spawn.right;
                t_bloom -= t_Spawn.position;
                t_bloom.Normalize();
            }
            //RPM
            currentCooldown = loadout[currentIndex].firerate;

            //Raycast
            RaycastHit t_Hit = new RaycastHit();
            if (Physics.Raycast(t_Spawn.position, t_bloom, out t_Hit, 1000f, canBeShot)){
                GameObject t_newHole = Instantiate(bulletHolePrefab, 
                t_Hit.point + t_Hit.normal * 0.001f, Quaternion.identity) as GameObject;
                t_newHole.transform.LookAt(t_Hit.point + t_Hit.normal);
                Destroy(t_newHole, 5f);

                if (photonView.IsMine){
                    //If shooting other player
                    if (t_Hit.collider.gameObject.layer == 8){
                        t_Hit.collider.transform.root.gameObject.GetPhotonView().RPC("TakeDamage", RpcTarget.All, loadout[currentIndex].damage);
                    }
                }
            }

            //Sound
            sfx.pitch = Random.Range (0.9f, 1.1f);
            sfx.volume = Random.Range (0.8f, 1);
            sfx.PlayOneShot(GetClipFromArray(loadout[currentIndex].gunshotSound), 1);

            //Recoil

            if (isCrouching){
                currentEquipment.transform.Rotate(-loadout[currentIndex].recoil*0.5f,0,0);
                currentEquipment.transform.position -= currentEquipment.transform.forward*
                loadout[currentIndex].kickback*0.5f;
            }
            else {
                currentEquipment.transform.Rotate(-loadout[currentIndex].recoil,0,0);
                currentEquipment.transform.position -= currentEquipment.transform.forward*
                loadout[currentIndex].kickback;
            }
            
        }

        [PunRPC]
        private void TakeDamage(int p_damage){
            GetComponent<PlayerController>().TakeDamage(p_damage);
        }

        AudioClip GetClipFromArray(AudioClip[] clipArray){
            AudioClip selectedClip = clipArray[UnityEngine.Random.Range(0, clipArray.Length)];

            while (selectedClip == previousClip){
                selectedClip = clipArray[UnityEngine.Random.Range(0, clipArray.Length)];
            }
            previousClip = selectedClip;
            return selectedClip;
        }

        #endregion

        #region Public methods
        public void RefreshAmmo(Text p_text){
            int t_clip = loadout[currentIndex].GetClip();
            int t_stash = loadout[currentIndex].GetStash();

            p_text.text = t_clip.ToString() + " / " + t_stash.ToString();
        }
        #endregion
    }
}