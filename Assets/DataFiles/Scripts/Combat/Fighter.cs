using RPG.Core;
using RPG.Movement;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace RPG.Combat
{
    public class Fighter : MonoBehaviour, IAction
    {
        [SerializeField] float weaponRange = 2f;
        [SerializeField] float weaponDamage = 20;
        [SerializeField] float timeBetweenEffects =1f;
        Health _target;

        float timeSinceLastAttack = 0;

        private void Update()
        {
            timeSinceLastAttack += Time.deltaTime;
            CheckIfAlive();
            if (_target != null)
            {
                if (!IsInRange())
                {
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

        private void StartAction(Health target)
        {
            GetComponent<ActionScheduler>().StartAction(this);
            Attack(target);
            GetComponent<Mover>().MoveTo(_target.transform.position, weaponRange);
            
        }

        private void AttackBehaviour()
        {
            if (timeSinceLastAttack > timeBetweenEffects) {
                timeSinceLastAttack = 0;
                GetComponent<Animator>().SetTrigger("attack");
            }
        }

        bool IsInRange()
        {
            if (_target != null)
            {
                if (Vector3.Distance(transform.position, _target.transform.position) <= weaponRange)
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

        

        public void Cancel()
        {
            GetComponent<Animator>().SetTrigger("stopAttack");
            _target = null;
        }

        void CheckIfAlive()
        {
            if(_target?.GetComponent<Health>().GetIsAlive() == false)
                Cancel();
        }

        //Animation event
        void Hit()
        {
            _target.GetComponent<Health>().TakeDamage(weaponDamage);
        }
    }
}