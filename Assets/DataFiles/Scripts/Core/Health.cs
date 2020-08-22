using System.Collections;
using System.Collections.Generic;
using UnityEngine;



namespace RPG.Core
{
    public class Health : MonoBehaviour
    {
        [SerializeField] float healthPoints = 100f;
        bool isAlive = true;
        public void TakeDamage(float damage)
        {
            if (isAlive)
            {
                healthPoints = Mathf.Max(healthPoints - damage, 0);
                if (healthPoints == 0)
                    Die();
            }

        }

        public void Die()
        {
            GetComponent<Animator>().SetTrigger("die");
            isAlive = false;
            GetComponent<ActionScheduler>().CancelCurrentAction();
        }

        public bool GetIsAlive()
        {
            return isAlive;
        }
    }

}