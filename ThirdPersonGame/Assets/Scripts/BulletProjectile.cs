using System;
using StarterAssets;
using UnityEngine;

public class BulletProjectile : MonoBehaviour
{
    private Rigidbody _bulletRigidbody;
    [SerializeField] private float speed = 10f;
    [SerializeField] private Transform vfxHitRed;
    [SerializeField] private Transform vfxHitGreen;
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
        if (other.GetComponent<BulletTarget>() != null)
        {
            
        }
        else
        {
            
        }
        Destroy(gameObject);
    }
}
