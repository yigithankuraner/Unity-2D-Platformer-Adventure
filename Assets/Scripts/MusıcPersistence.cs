using UnityEngine;

public class MusicPersistence : MonoBehaviour
{
    private static MusicPersistence instance;

    void Awake()
    {
        if (instance != null)
        {
            Destroy(gameObject);
        }
        else
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
    }
}