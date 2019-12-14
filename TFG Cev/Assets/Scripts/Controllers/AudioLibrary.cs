using UnityEngine;


[CreateAssetMenu(fileName = "AudioLibrary", menuName = "AudioLibrary")]
public class AudioLibrary : ScriptableObject
{
   
    public enum VfxSounds
    {
        Drink,
        Footstep,
        GunShot,
        Hurt,
        SwordHit
    }

    [System.Serializable]
    public class SoundAudioClip
    {
        public VfxSounds sound;
        public AudioClip audioClip;


    }
    public SoundAudioClip[] sounds;

    public void PlayVFX(VfxSounds _sound)
    {
        GameObject soundGameObject = new GameObject("Sound");

        AudioSource audiosource = soundGameObject.AddComponent<AudioSource>();
        foreach(SoundAudioClip s in sounds)
        {
            if (s.sound == _sound)
            {
                audiosource.PlayOneShot(s.audioClip);
            }
        }
    }
    public void PlayVFX(VfxSounds _sound, float volume)
    {
        GameObject soundGameObject = new GameObject("Sound");

        AudioSource audiosource = soundGameObject.AddComponent<AudioSource>();
        foreach (SoundAudioClip s in sounds)
        {
            if (s.sound == _sound)
            {
                audiosource.PlayOneShot(s.audioClip, volume);
            }
        }
    }

}
