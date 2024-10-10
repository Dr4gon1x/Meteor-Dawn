using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class SoundManager : MonoBehaviour
{

    AudioSource Audiosrc;
    public enum SoundEffects{Meteor, walk};

    [System.Serializable]
    struct Sounds{
        public AudioClip Audio;
        public SoundEffects Type;
    };

    [SerializeField]
    Sounds[] sounds;

    void Start() {
        Audiosrc = this.AddComponent<AudioSource>();
        Audiosrc.playOnAwake = false;
    }

    public void PlaySound(SoundEffects type){
        foreach(Sounds sound in sounds) {
            if(sound.Type == type) {
                Audiosrc.clip = sound.Audio;
                Audiosrc.Play();
            }
        }
    }
}
