using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class BGMManager : MonoBehaviour
{
    public AudioClip bgmClip;

    private static BGMManager instance;

    void Awake()
    {
        if (instance != null)
        {
            Destroy(gameObject); // Only one instance allowed
            return;
        }

        instance = this;
        DontDestroyOnLoad(gameObject); // Persist across scene loads

        AudioSource source = GetComponent<AudioSource>();
        source.clip = bgmClip;
        source.loop = true;
        source.playOnAwake = false;
        source.Play();
    }
}
