using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RPG.Saving;
using RPG.Core;
using RPG.Stats;
using System;

namespace RPG.Resources
{
    public class Health : MonoBehaviour,ISaveable
    {
        [SerializeField] float healthPoints = 100f;
        bool isAlive = true;
        
        private void Start()
        {
            healthPoints = GetComponent<BaseStats>().GetStat(Stat.Health);
        }

        public void TakeDamage(GameObject instigator,float damage)
        {
            if (isAlive)
            {
                healthPoints = Mathf.Max(healthPoints - damage, 0);
                CheckIfAlive();
            }
            if (healthPoints == 0)
            {
                Die();
                AwardExperience(instigator);
            }

        }

        private void AwardExperience(GameObject instigator)
        {
            if (instigator.GetComponent<Experience>() != null)
            {
                instigator.GetComponent<Experience>().GainExperience(GetComponent<BaseStats>().GetStat(Stat.ExperienceReward));
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

        public float GetPercentage()
        {
            return 100 * healthPoints / GetComponent<BaseStats>().GetStat(Stat.Health);
        }
    }

}