using UnityEngine;

public class SoundManager : MonoBehaviour
{
    private static SoundManager instance;
    
    [Space] [Header("Config")]
    [Range(0f, 1f)] [SerializeField] private float volume = 1f;
    [Range(0f, 1f)] [SerializeField] private float blend = 1f;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public static void PlaySound(AudioSource audioSource, AudioClip clip, float volumeFactor = 1f)
    {
        audioSource.Stop();
        audioSource.volume = instance.volume * volumeFactor;
        audioSource.spatialBlend = instance.blend;

        audioSource.PlayOneShot(clip);
    }

    public static void PlayLoopSound(AudioSource audioSource, AudioClip clip, float volumeFactor = 1f)
    {
        audioSource.Stop();
        audioSource.volume = instance.volume * volumeFactor;
        audioSource.spatialBlend = instance.blend;
        audioSource.clip = clip;
        audioSource.Play();
    }

    public static void StopLoopSound(AudioSource audioSource)
    {
        audioSource.Stop();
        audioSource.clip = null;
    }
}