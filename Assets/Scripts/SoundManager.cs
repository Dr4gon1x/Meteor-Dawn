
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

    private float timer = 0f;
    public float timeBetween = 4f;

    void Start() {
        Audiosrc = this.AddComponent<AudioSource>();
        Audiosrc.playOnAwake = false;

    }
    void Update()
    {
        timer += Time.deltaTime;

        if(timer >= timeBetween)
        {
            PlaySound(SoundEffects.Meteor);
            timer = 0f;
        }
        
    }
    public void PlaySound(SoundEffects type){
        foreach(Sounds sound in sounds) {
            if(sound.Type == type) {
                Audiosrc.clip = sound.Audio;
                Audiosrc.Play();
                Debug.Log("AFSPILLET!");
            }
        }
    }
}
