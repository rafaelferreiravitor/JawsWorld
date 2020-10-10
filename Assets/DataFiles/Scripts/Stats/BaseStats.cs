using RPG.Resources;
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

        int currentLevel = 0;

        public event Action onLevelUp;

        private void Start()
        {
            currentLevel = CalculateLevel();
            Experience experience = GetComponent<Experience>();
            if (experience != null)
            {
                experience.onExperienceGained += UpdateLevel;

            }
        }

        private void UpdateLevel()
        {
            int newLevel = CalculateLevel();
            if (newLevel > currentLevel)
            {
                currentLevel = newLevel;
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
            return progression.GetStat(stat,characterClass, currentLevel-1);
        }

        public int GetLevel()
        {
            if(currentLevel < 1)
            {
                currentLevel = CalculateLevel();
            }
            return currentLevel;
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
