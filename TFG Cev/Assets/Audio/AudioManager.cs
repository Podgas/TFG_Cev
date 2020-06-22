using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{

    [SerializeField]
    public List<AudioLB> library;


    [System.Serializable]

    public class AudioLB{

        public string name;
        public AudioClip clip;
        public AudioSource ac;
    }

    public static AudioManager Instance;
    private void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
    public void PlaySound(string name)
    {
        AudioLB lb = GetSound(name);

        lb.ac.clip = lb.clip; 
        lb.ac.Play();
    }

    public void PlaySound(string name, float pitch)
    {
        AudioLB lb = GetSound(name);

        lb.ac.clip = lb.clip;
        lb.ac.pitch = pitch;
        lb.ac.Play();
    }
    public void PlaySound(string name, float pitch, float volume)
    {
        AudioLB lb = GetSound(name);

        lb.ac.clip = lb.clip;
        lb.ac.pitch = pitch;
        lb.ac.volume = volume;
        lb.ac.Play();
    }

    AudioLB GetSound(string name)
    {

        foreach(AudioLB lb in library)
        {
            if (lb.name == name)
            {
                return lb;
            }

        }
        return null;

    }

}
