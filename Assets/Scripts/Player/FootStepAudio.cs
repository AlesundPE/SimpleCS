using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Footstep Audio Data")]
public class FootStepAudio : ScriptableObject
{
    public List<FootStepAudioData> FootStepAudioDatas = new List<FootStepAudioData>();
}

[System.Serializable]
public class FootStepAudioData{
    public string Tag;
    public List<AudioClip> AudioClips = new List<AudioClip>();
    public float delay;
}