﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RPG.Saving;
using RPG.Core;
using RPG.Stats;
using System;
using GameDevTV.Utils;

namespace RPG.Resources
{
    public class Health : MonoBehaviour,ISaveable
    {
        [SerializeField] float PercentageRegenerate = 70f;
        bool isAlive = true;

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
            print(gameObject.name + "took damage: " + damage);
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
            healthPoints.value = (float) state;
            CheckIfAlive();
        }

        public float GetPercentage()
        {
            return 100 * healthPoints.value / GetComponent<BaseStats>().GetStat(Stat.Health);
        }
    }

}