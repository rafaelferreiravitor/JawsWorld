using RPG.Core;
using RPG.Attributes;
using System.Collections;
using System.Collections.Generic;
using System.Net.Sockets;
using UnityEngine;

namespace RPG.Combat
{
    public class Projectile : MonoBehaviour
    {
        [SerializeField] float speed = 0.01f;
        [SerializeField] bool homing = false;
        [SerializeField] GameObject hitFX = null;
        [SerializeField] float maxLifetime = 5f;
        [SerializeField] GameObject[] destroyOrder;
        [SerializeField] float timeToDestroy = 0.1f;
        Health target;
        GameObject instigator;
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
            transform.Translate(Vector3.forward * speed * Time.deltaTime);
        }

        public void SetTarget(GameObject instigator,Health target, float damage)
        {
            this.instigator = instigator;
            this.target = target;
            this.damage = damage;

            Destroy(gameObject, maxLifetime);
        }

        public Vector3 GetAimLocation()
        {
            CapsuleCollider collider = target.GetComponent<CapsuleCollider>();
            if (collider != null)
            {
                return target.transform.position + Vector3.up * collider.height / 2;
            }
            return target.transform.position;
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.GetComponent<Health>() != target) return;
            if (!target.GetIsAlive()) return;
            other.GetComponent<Health>().TakeDamage(instigator,damage);
            if (hitFX != null)
            {
                Instantiate(hitFX, GetAimLocation(), transform.rotation);
            }
            speed = 0;


            foreach (var item in destroyOrder)
            {
                Destroy(item);
            }
            Destroy(gameObject, timeToDestroy);
        }

    }

}