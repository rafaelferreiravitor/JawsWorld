using RPG.Combat;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace RPG.Stats
{
    public class LevelDisplay : MonoBehaviour
    {
        BaseStats bs;

        private void Awake()
        {
            bs = GameObject.FindWithTag("Player").GetComponent<BaseStats>();
        }


        // Update is called once per frame
        void Update()
        {
            GetComponent<TextMeshProUGUI>().text = String.Format("{0}", bs.GetLevel().ToString());
        }
    }
}