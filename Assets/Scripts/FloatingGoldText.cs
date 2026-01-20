using UnityEngine;

public class FloatingGoldText : MonoBehaviour
{
    public float moveUpSpeed = 1f;
    public float lifeTime = 1f;

    void Start()
    {
        Destroy(gameObject, lifeTime);
    }

    void Update()
    {
        transform.position += Vector3.up * moveUpSpeed * Time.deltaTime;
    }
}
