using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static SoundManager Instance;

    [Header("Clips")]
    public AudioClip jump;
    public AudioClip playerHit;
    public AudioClip enemyHit;
    public AudioClip shoot;
    public AudioClip gold;

    private AudioSource source;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            source = gameObject.AddComponent<AudioSource>();
        }
        else
            Destroy(gameObject);
    }

    public void Play(AudioClip clip)
    {
        if (clip != null)
            source.PlayOneShot(clip);
    }
}