using RPG.Core;
using System.Collections;
using System.Collections.Generic;
using System.Net.Sockets;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    [SerializeField] float speed = 0.01f;
    Health target;
    float damage = 0;
    
    void Start()
    {
        transform.LookAt(GetAimLocation());
    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate(Vector3.forward*speed*Time.deltaTime);
    }

    public void SetTarget(Health target, float damage)
    {
        this.target = target;
        this.damage = damage;
    }

    public Vector3 GetAimLocation()
    {
        CapsuleCollider collider = target.GetComponent<CapsuleCollider>();
        if(collider != null)
        {
            return target.transform.position + Vector3.up * collider.height/2;
        }
        return target.transform.position;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player") && other.GetComponent<Health>())
        {
            other.GetComponent<Health>().TakeDamage(damage);
        }
        Destroy(gameObject);
    }
}
