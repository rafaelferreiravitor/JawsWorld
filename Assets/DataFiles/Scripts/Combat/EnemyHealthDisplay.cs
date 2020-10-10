using RPG.Resources;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace RPG.Combat
{
    public class EnemyHealthDisplay : MonoBehaviour
    {
        Fighter fighter;
        private void Awake()
        {
            fighter = GameObject.FindWithTag("Player").GetComponent<Fighter>();
        }


        // Update is called once per frame
        void Update()
        {
            Health health = fighter.GetTarget();
            if (health == null)
            {
                GetComponent<TextMeshProUGUI>().text = "N/A";
                return;
            }
            GetComponent<TextMeshProUGUI>().text = String.Format("{0:0}/{1:0}", health.GetHealthPoints().ToString(), health.GetMaxHealthPoints().ToString());
        }
    }
}