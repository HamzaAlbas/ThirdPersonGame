using System;
using StarterAssets;
using UnityEngine;

public class BulletProjectile : MonoBehaviour
{
    private Rigidbody _bulletRigidbody;
    [SerializeField] private float speed = 10f;
    private void Awake()
    {
        _bulletRigidbody = GetComponent<Rigidbody>();
    }

    private void Start()
    {
        _bulletRigidbody.velocity = transform.forward * speed;
    }

    private void OnTriggerEnter(Collider other)
    {
        Destroy(gameObject);
    }
}
