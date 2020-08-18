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
        CombatTarget combatTarget;


        private void Update()
        {
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
            GetComponent<Animator>().SetTrigger("attack");
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



        //Animation event
        void Hit()
        {
            
        }
    }
}