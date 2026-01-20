using UnityEngine;
using UnityEngine.SceneManagement;

public class SpawnManager : MonoBehaviour
{
    void Start()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        GameObject spawn = GameObject.Find("PlayerSpawn");

        if (player != null && spawn != null)
        {
            player.transform.position = spawn.transform.position;
        }
    }
}
