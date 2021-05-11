using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace com.yiran.SimpleCS{
    [CreateAssetMenu(fileName="New Gun", menuName="Gun")]

    public class GunGeneration : ScriptableObject
    {
        public string name;
        public int damage;
        public int ammo;
        public int clipSize;
        public float firerate;
        public float bloom;
        public float recoil;
        public float kickback;
        public float reload;
        public bool isAutomatic;
        public GameObject prefab;
        public AudioClip[] gunshotSound;
        public float pitchRandomization;

        private int stash;
        private int clip;

        public void Initialize(){
            stash = ammo;
            clip = clipSize;
        }
        public bool FireBullet(){
            if (clip > 0){
                clip -= 1;
                return true;
            }
            else return false;

        }

        public void Reload(){
            stash += clip;
            clip = Mathf.Min(clipSize, stash);
            stash -= clip;
        }

        public int GetStash(){return stash;}
        public int GetClip(){return clip;}
    }
}