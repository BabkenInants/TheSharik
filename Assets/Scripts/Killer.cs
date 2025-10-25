using UnityEngine;

public class Killer : MonoBehaviour
{
    public float lifeTime;

    private void Start() => Destroy(gameObject, lifeTime);
}