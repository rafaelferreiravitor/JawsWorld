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
        [SerializeField] float PercentageRegenerate = 70f;
        float healthPoints = -1f;
        bool isAlive = true;

        private void Start()
        {
            GetComponent<BaseStats>().onLevelUp += RegenerateLevelUp;

            if (healthPoints<0)
                healthPoints = GetComponent<BaseStats>().GetStat(Stat.Health);
        }

        public void RegenerateLevelUp()
        {
            float regenHealthpoints = GetComponent<BaseStats>().GetStat(Stat.Health) * PercentageRegenerate / 100;
            healthPoints = Mathf.Max(regenHealthpoints,healthPoints);
        }

        public void TakeDamage(GameObject instigator,float damage)
        {
            print(gameObject.name + "took damage: " + damage);
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

        public float GetHealthPoints()
        {
            return healthPoints;
        }

        public float GetMaxHealthPoints()
        {
            return GetComponent<BaseStats>().GetStat(Stat.Health);
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