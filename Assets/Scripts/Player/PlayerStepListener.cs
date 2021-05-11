using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStepListener : MonoBehaviour
{
    /*
    public FootStepAudio FootStepAudio;
    public AudioSource FootStepAudioSource;

    private CharacterController cc;
    private Transform footstepTransform;
    */

    public Transform groundCheck;
    public float checkRadius;
    public LayerMask groundLayer;
    public bool isGround;

    public AudioSource audioSource;
    
    public AudioClip[] stepClips;
    public AudioClip[] dirtClips;
    AudioClip previousClip;

    CharacterController cc;
    float currentSpeed;
    bool walking;

    float airTime;

    private void Start(){
        cc = GetComponent<CharacterController>();
        
    }
    
    private void Update(){

        #region PreviousStepScript
        /*
        if (isGround){ //if is grounded
            if(cc.velocity.normalized.magnitude > 1f){ // 
                bool tmp_IsHit = Physics.Linecast(footstepTransform.position, 
                Vector3.down * cc.height/2, out RaycastHit tmp_HitInfo);

                if (tmp_IsHit){
                    foreach (var tmp_AudioElement in FootStepAudio.FootStepAudioDatas){
                        if (tmp_HitInfo.collider.CompareTag(tmp_AudioElement.Tag)){
                            int tmp_AudioCount = tmp_AudioElement.AudioClips.Count;
                            int tmp_AudioIndex = UnityEngine.Random.Range(0, tmp_AudioCount);
                            AudioClip tmpFootStep = tmp_AudioElement.AudioClips[tmp_AudioIndex];
                            FootStepAudioSource.clip = tmpFootStep;
                            FootStepAudioSource.Play();
                        }
                    }
                }
            }
        }
        */
        #endregion

        isGround = Physics.CheckSphere(groundCheck.position,checkRadius,groundLayer);

        currentSpeed = cc.velocity.magnitude;

        walking = CheckIfWalking();
        PlaySoundIfFalling();

        if(walking){
            if (!(Input.GetButton("Shift"))&&!(Input.GetButton("Crouch"))){
                TriggerNextClip();
            }
        }

    }

    bool CheckIfWalking(){
        if (currentSpeed >0 && isGround){
            return true;
        } else {
            return false;
        }
    }

    AudioClip GetClipFromArray(AudioClip[] clipArray){
        AudioClip selectedClip = clipArray[UnityEngine.Random.Range(0, clipArray.Length)];

        while (selectedClip == previousClip){
            selectedClip = clipArray[UnityEngine.Random.Range(0, clipArray.Length)];
        }
        previousClip = selectedClip;
        return selectedClip;
    }

    void TriggerNextClip(){
        audioSource.pitch = Random.Range (0.9f, 1.1f);
        audioSource.volume = Random.Range (0.8f, 1);

        if (isGround && !audioSource.isPlaying){
            audioSource.PlayOneShot(GetClipFromArray(stepClips), 1);
        }


    }

    void PlaySoundIfFalling(){
        if (!isGround){
            airTime += Time.deltaTime;
        } else{
            if (airTime > 0.25f){
                TriggerNextClip();
                airTime = 0;
            }
        }
    }
    
}
