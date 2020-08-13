using RPG.Movement;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace RPG.Combat
{
    public class Fighter : MonoBehaviour
    {
        [SerializeField] float weaponRange = 2f;
        Transform target;

        private void Update()
        {
            if (target != null)
            {
                GetComponent<Mover>().MoveTo(target.position, weaponRange);
            }
        }

        public void Attack(CombatTarget combatTarget)
        {
            target = combatTarget?.transform;
        }

        public void CancelAttack()
        {
            target = null;
        }

    }
}