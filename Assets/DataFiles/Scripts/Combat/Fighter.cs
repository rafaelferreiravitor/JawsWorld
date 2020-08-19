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
        CombatTarget combatTarget;

        float timeSinceLastAttack = 0;

        private void Update()
        {
            timeSinceLastAttack += Time.deltaTime;
            if (combatTarget != null)
            {
                
                if (!IsInRange())
                {
                    StartAction(combatTarget);
                }
                else
                {
                    //GetComponent<Mover>().Cancel();
                    AttackBehaviour();
                }
                CheckIfAlive();

                //GetComponent<ActionScheduler>().StartAction(this);
            }
        }

        private void StartAction(CombatTarget target)
        {
            GetComponent<ActionScheduler>().StartAction(this);
            Attack(target);
            GetComponent<Mover>().MoveTo(combatTarget.transform.position, weaponRange);
            
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
            if (combatTarget != null)
            {
                if (Vector3.Distance(transform.position, combatTarget.transform.position) <= weaponRange)
                    return true;
            }
            return false;
        }

        public void Attack(CombatTarget target)
        {
            if (target != null)
                combatTarget = target;
            //else
                //Cancel();
        }

        

        public void Cancel()
        {
            combatTarget = null;
        }

        void CheckIfAlive()
        {
            if(combatTarget.GetComponent<Health>().GetIsAlive() == false)
                Cancel();
        }

        //Animation event
        void Hit()
        {
            combatTarget.GetComponent<Health>().TakeDamage(weaponDamage);
        }
    }
}