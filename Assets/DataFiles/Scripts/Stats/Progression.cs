using UnityEngine;

namespace RPG.Stats
{
    [CreateAssetMenu(fileName = "Progression", menuName = "Stats/New Progression", order = 0)]
    public class Progression : ScriptableObject
    {
        [SerializeField] ProgressionCharacterClass[] characterClasses;



        [System.Serializable]
        public class ProgressionCharacterClass
        {
            public CharacterClass characterClass;
            public ProgressionStat[] stats;
            
        }

        [System.Serializable]
        public class ProgressionStat
        {
            public Stat stat;
            public float[] levels;
        }

        public float GetHealth(CharacterClass character, int level)
        {
            foreach (ProgressionCharacterClass item in characterClasses)
            {
                if (item.characterClass == character)
                {
                    //return item.health[level - 1];
                }
            }
            return 0f;
        }

    }
}