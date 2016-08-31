using UnityEngine;
using System.Collections;

public class ProjectileClass : MonoBehaviour {

    public float lifetime = 2.0f;

    void Awake()
    {
        Destroy(gameObject, lifetime);
    }
}
