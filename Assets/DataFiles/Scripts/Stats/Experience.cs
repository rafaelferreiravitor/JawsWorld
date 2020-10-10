using RPG.Saving;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Stats
{
    public class Experience : MonoBehaviour,ISaveable
    {
        [SerializeField] float experiencePoints;

        //public delegate void ExperienceGainedDelegate();
        public event Action onExperienceGained;
        public void GainExperience(float experience)
        {
            experiencePoints += experience;
            onExperienceGained.Invoke();
        }

        public object CaptureState()
        {
            return experiencePoints;
        }

        public void RestoreState(object state)
        {
            experiencePoints = (float) state;
        }

        public float GetExperience()
        {
            return experiencePoints;
        }
    }
}
