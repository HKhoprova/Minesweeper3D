using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.ParticleSystem;

public class SootStain : MonoBehaviour
{
    [SerializeField] private GameObject particles;

    private void Awake()
    {
        Explode();
    }

    private void Explode()
    {
        if (particles != null)
        {
            Vector3 position = transform.position;
            position.y += 0.5f;
            GameObject explosion = Instantiate(particles, position, Quaternion.identity);
            Destroy(explosion, 1f);
        }
    }
}
