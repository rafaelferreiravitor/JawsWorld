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
        [SerializeField] Canvas canvas = null;
        void Update()
        {
            float fraction = healthComponent.GetFraction();
            if (Mathf.Approximately(fraction, 0) || Mathf.Approximately(fraction, 1))
            {
                canvas.enabled = false;
                return;
            }
            canvas.enabled = true;
            foreground.localScale = new Vector3(healthComponent.GetFraction(), 1, 1);
        }
    }
}

