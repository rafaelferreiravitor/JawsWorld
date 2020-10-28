using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace RPG.Attributes
{
    public class HealthBar : MonoBehaviour
    {
        [SerializeField] RectTransform foreground = null;
        [SerializeField] Health healthComponent = null;
        void Update()
        {
            foreground.localScale = new Vector3(healthComponent.GetFraction(), 1, 1);
        }
    }
}

