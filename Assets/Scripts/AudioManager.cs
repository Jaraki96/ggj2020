using UnityEngine;

[System.Serializable]
public class Sound
{
    public string name;
    public AudioClip clip;

    private AudioSource source;

    [Range(0.0f, 1.0f)]
    public float volume = 0.7f;
    [Range(0.5f, 1.5f)]
    public float pitch = 1.0f;

    [Range(0.0f, 0.5f)]
    public float randomVolume;
    [Range(0.0f, 0.5f)]
    public float randomPitch;

    public void SetAudioSource(AudioSource newSource)
    {
        source = newSource;
        source.clip = clip;
    }

    public void Play()
    {
        source.volume = volume * (1 + Random.Range(-randomVolume / 2, randomVolume / 2));
        source.pitch = pitch * (1 + Random.Range(-randomPitch / 2, randomPitch / 2));
        source.Play();
    }
}

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;

    [SerializeField]
    public Sound[] sounds;

    void Awake()
    {
        if(instance != null)
        {
            Debug.LogError("More then 1 Audio Manager in Scene.");
        } else
        {
            instance = this;
        }
    }

    void Start()
    {
        for(int i = 0; i < sounds.Length; ++i)
        {
            GameObject soundObject = new GameObject("Sound_" + i + "_" + sounds[i].name);
            soundObject.transform.SetParent(this.transform);
            sounds[i].SetAudioSource(soundObject.AddComponent<AudioSource>());
        }
    }

    //*** USE THIS FUNCTION TO PLAY SOUNDS IN OTHER SCRIPTS ***
    public void PlaySound(string soundName)
    {
        for(int i = 0; i < sounds.Length; ++i)
        {
            if(soundName == sounds[i].name)
            {
                sounds[i].Play();
                return;
            }
        }

        Debug.LogWarning("Audio Manager: No sound with name " + soundName + " in sound bank.");
    }
}
