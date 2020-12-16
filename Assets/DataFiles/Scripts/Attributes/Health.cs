using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameDevTV.Saving;
using RPG.Core;
using RPG.Stats;
using System;
using GameDevTV.Utils;
using UnityEngine.Events;

namespace RPG.Attributes
{
    public class Health : MonoBehaviour,ISaveable
    {
        [SerializeField] float PercentageRegenerate = 70f;
        bool isAlive = true;

        [SerializeField] UnityEvent<float> takeDamage;
        [SerializeField] UnityEvent onDie;

        LazyValue<float> healthPoints;
        private void Awake()
        {
            healthPoints = new LazyValue<float>(GetInitialHealth);

        }

        private void Start()
        {
            healthPoints.ForceInit();   
        }

        private float GetInitialHealth()
        {
            return GetComponent<BaseStats>().GetStat(Stat.Health);
        }

        private void OnEnable()
        {
            GetComponent<BaseStats>().onLevelUp += RegenerateLevelUp;
        }

        private void OnDisable()
        {
            GetComponent<BaseStats>().onLevelUp -= RegenerateLevelUp;
        }

        public void RegenerateLevelUp()
        {
            float regenHealthpoints = GetComponent<BaseStats>().GetStat(Stat.Health) * PercentageRegenerate / 100;
            healthPoints.value = Mathf.Max(regenHealthpoints,healthPoints.value);
        }

        public void TakeDamage(GameObject instigator,float damage)
        {
            //print(gameObject.name + "took damage: " + damage);
            
            if (isAlive)
            {
                healthPoints.value = Mathf.Max(healthPoints.value - damage, 0);
                CheckIfAlive();
            }
            if (healthPoints.value == 0)
            {
                
                Die();
                AwardExperience(instigator);
            }
            else
            {
                takeDamage.Invoke(damage);
            }
        }

        public float GetHealthPoints()
        {
            return healthPoints.value;
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
            if (healthPoints.value == 0)
                Die();
            
        }

        public void Die()
        {
            onDie.Invoke();
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
            return healthPoints.value;
        }

        public void RestoreState(object state)
        {
            healthPoints.value = (float) state;
            CheckIfAlive();
        }

        public float GetPercentage()
        {
            return 100 * GetFraction();
        }

        public float GetFraction()
        {
            return healthPoints.value / GetComponent<BaseStats>().GetStat(Stat.Health);
        }

        public void Heal(float healPointsToAdd)
        {
            healthPoints.value = Mathf.Min(healthPoints.value + healPointsToAdd, GetMaxHealthPoints());
        }
    }



}