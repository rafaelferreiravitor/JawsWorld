using RPG.Combat;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace RPG.Stats
{
    public class XPDisplay : MonoBehaviour
    {
        Experience xp;

        private void Awake()
        {
            xp = GameObject.FindWithTag("Player").GetComponent<Experience>();
        }


        // Update is called once per frame
        void Update()
        {
            GetComponent<TextMeshProUGUI>().text = String.Format("{0}", xp.GetExperience().ToString());
        }
    }
}