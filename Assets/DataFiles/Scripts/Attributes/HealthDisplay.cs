using RPG.Combat;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace RPG.Attributes
{
    public class HealthDisplay : MonoBehaviour
    {
        Health health;

        private void Awake()
        {
            health = GameObject.FindWithTag("Player").GetComponent<Health>();
        }


        // Update is called once per frame
        void Update()
        {
            //GetComponent<TextMeshProUGUI>().text = String.Format("{0:0}%", health.GetPercentage().ToString());
            GetComponent<TextMeshProUGUI>().text = String.Format("{0:0}/{1:0}", health.GetHealthPoints().ToString(), health.GetMaxHealthPoints().ToString());
        }
    }
}