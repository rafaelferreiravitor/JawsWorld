using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RPG.Saving;


namespace RPG.Core
{
    public class Health : MonoBehaviour,ISaveable
    {
        [SerializeField] float healthPoints = 100f;
        bool isAlive = true;
        public void TakeDamage(float damage)
        {
            if (isAlive)
            {
                healthPoints = Mathf.Max(healthPoints - damage, 0);
                CheckIfAlive();
            }

        }

        private void CheckIfAlive()
        {
            if (healthPoints == 0)
                Die();
        }

        public void Die()
        {
            //GetComponent<Collider>().enabled = false;
            GetComponent<Animator>().SetTrigger("die");
            isAlive = false;
            GetComponent<ActionScheduler>().CancelCurrentAction();
        }

        public bool GetIsAlive()
        {
            return isAlive;
        }

        public object CaptureState()
        {
            return healthPoints;
        }

        public void RestoreState(object state)
        {
            healthPoints = (float) state;
            CheckIfAlive();
        }
    }

}