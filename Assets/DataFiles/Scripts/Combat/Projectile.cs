﻿using RPG.Core;
using System.Collections;
using System.Collections.Generic;
using System.Net.Sockets;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    [SerializeField] float speed = 0.01f;
    [SerializeField] bool homing = false;
    Health target;
    float damage = 0;
    
    void Start()
    {
        transform.LookAt(GetAimLocation());
    }

    // Update is called once per frame
    void Update()
    {
        if (target == null) return;
        if (homing && target.GetIsAlive())
        {
            transform.LookAt(GetAimLocation());
        }
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
        if (other.GetComponent<Health>() != target) return;
        if (!target.GetIsAlive()) return;
        other.GetComponent<Health>().TakeDamage(damage);
        Destroy(gameObject);
    }
}
