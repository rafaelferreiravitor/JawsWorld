using GameDevTV.Utils;
using RPG.Attributes;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Stats
{

    public class BaseStats : MonoBehaviour
    {
        [Range(1,99)]
        [SerializeField] int startingLevel = 1;
        [SerializeField] CharacterClass characterClass;
        [SerializeField] Progression progression = null;
        [SerializeField] GameObject levelUpFX;
        [SerializeField] bool shouldUseModifiers = false;


        public event Action onLevelUp;
        Experience experience;

        LazyValue<int> currentLevel;

        private void Awake()
        {
            experience = GetComponent<Experience>();
            currentLevel = new LazyValue<int>(CalculateLevel);
        }

        private void Start()
        {
            currentLevel.ForceInit();
        }

        private void OnEnable()
        {
            if (experience != null)
            {
                experience.onExperienceGained += UpdateLevel;
            }
        }

        private void OnDisable()
        {
            if (experience != null)
            {
                experience.onExperienceGained -= UpdateLevel;
            }
        }

        private void UpdateLevel()
        {
            int newLevel = CalculateLevel();
            if (newLevel > currentLevel.value)
            {
                currentLevel.value = newLevel;
                LevelUpFX();

                onLevelUp.Invoke();
            }
        }

        private void LevelUpFX()
        {
            Instantiate(levelUpFX, transform);
        }

        public float GetStat(Stat stat)
        {
            if(shouldUseModifiers)
                return (GetBaseStats(stat) + GetAdditiveModifier(stat)) * (1 + GetPercentageModifier(stat)/100);
            else
            {
                return GetBaseStats(stat);
            }
        }

        private float GetPercentageModifier(Stat stat)
        {
            float sum = 0;
            foreach (IModifierProvider provider in GetComponents<IModifierProvider>())
            {
                foreach (float modifiers in provider.GetPercentageModifiers(stat))
                {
                    sum += modifiers;
                }
            }
            return sum;
        }

        private float GetBaseStats(Stat stat)
        {
            return progression.GetStat(stat, characterClass, GetLevel());
        }

        private float GetAdditiveModifier(Stat stat)
        {
            float sum = 0;
            foreach (IModifierProvider provider in GetComponents<IModifierProvider>())
            {
                foreach (float modifiers in provider.GetAdditiveModifiers(stat))
                {
                    sum += modifiers;
                }
            }
            return sum;
        }

        public int GetLevel()
        {
            return currentLevel.value+1;
        }

        public int CalculateLevel()
        {
            Experience experience = GetComponent<Experience>();
            if (experience == null) return startingLevel;
            float currentXP = experience.GetExperience();

            float maxLevels = progression.GetLevels(Stat.ExperienceToLevelUp, characterClass);
            int level = 0;
            for (level=0;level<maxLevels;level++) {
                float XPTolevelUp = progression.GetStat(Stat.ExperienceToLevelUp, characterClass, level);
                if (XPTolevelUp > currentXP)
                    return level;
            }
            return level;
        }

    }
}
