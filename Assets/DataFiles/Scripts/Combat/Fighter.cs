﻿using RPG.Core;
using RPG.Movement;
using RPG.Resources;
using RPG.Saving;
using RPG.Stats;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace RPG.Combat
{
    public class Fighter : MonoBehaviour, IAction, ISaveable
    {
        [SerializeField] float timeBetweenEffects = 5f;
        Health _target;
        [SerializeField] Transform rightHandTransform = null;
        [SerializeField] Transform leftHandTransform = null;
        [SerializeField] Weapon defaultWeapon = null;
        [SerializeField] Weapon currentWeapon = null;
        float timeSinceLastAttack = Mathf.Infinity;

        public Health GetTarget()
        {
            return _target;
        }


        private void Start()
        {
            if (currentWeapon == null)
            {
                EquipWeapon(defaultWeapon);
            }
        }
        private void Update()
        {
            timeSinceLastAttack += Time.deltaTime;
            CheckIfAlive();
            if (_target != null)
            {
                if (IsInRange() == false)
                {
                    //GetComponent<Mover>().StartAction(_target.transform.position,1f,weaponDamage);
                    StartAction(_target);
                }
                else 
                {
                    //GetComponent<Mover>().Cancel();
                    AttackBehaviour();
                }



                //GetComponent<ActionScheduler>().StartAction(this);
            }
        }

        public void StartAction(Health target)
        {
            //GetComponent<ActionScheduler>().StartAction(this);
            Attack(target);
            //GetComponent<Mover>().StartAction(_target.transform.position, weaponRange);

        }

        private void AttackBehaviour()
        {
            transform.LookAt(_target.transform);

            if (timeSinceLastAttack > timeBetweenEffects)
            {
                timeSinceLastAttack = 0;
                TriggerAttack();
            }
            //GetComponent<Mover>().MoveTo(_target.transform.position, weaponRange);
        }

        private void TriggerAttack()
        {
            GetComponent<Animator>().ResetTrigger("stopAttack");
            GetComponent<Animator>().SetTrigger("attack");
        }

        bool IsInRange()
        {
            if (_target != null)
            {
                if (Vector3.Distance(transform.position, _target.transform.position) <= currentWeapon.GetWeaponRange())
                    return true;
            }
            return false;
        }

        public void Attack(Health target)
        {
            if (target != null)
                _target = target;
            //else
            //Cancel();
        }

        public bool CanAttack(GameObject target)
        {
            return target != null && target.GetComponent<Health>().GetIsAlive();
        }


        public void Cancel()
        {
            StopAttack();
            GetComponent<Mover>().Cancel();
            _target = null;
        }

        private void StopAttack()
        {
            GetComponent<Animator>().ResetTrigger("attack");
            GetComponent<Animator>().SetTrigger("stopAttack");
        }

        void CheckIfAlive()
        {

            if (_target?.GetComponent<Health>().GetIsAlive() == false)
            {
                Cancel();
            }
        }

        //Animation event
        void Hit()
        {
            if (_target == null)
                return;
            
            float damage = GetComponent<BaseStats>().GetStat(Stat.Damage);
            if (currentWeapon.HasProjectile())
            {
                currentWeapon.LunchProjectile(rightHandTransform, leftHandTransform, _target,gameObject,damage);
            }
            else
            {
                _target.GetComponent<Health>().TakeDamage(gameObject,damage);
            }
        }

        void Shoot()
        {
            Hit();
        }

        public void EquipWeapon(Weapon weapon)
        {
            currentWeapon = weapon;
            weapon.Spawn(rightHandTransform,leftHandTransform, GetComponent<Animator>());
        }

        public object CaptureState()
        {
            return currentWeapon.name;
        }

        public void RestoreState(object state)
        {
            string weaponName = (string)state;
            Weapon weapon = UnityEngine.Resources.Load<Weapon>(weaponName);
            EquipWeapon(weapon);
        }
    }
}