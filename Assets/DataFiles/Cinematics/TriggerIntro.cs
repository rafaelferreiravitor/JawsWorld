using System.Collections;
using System.Collections.Generic;
using System.Net;
using UnityEngine;
using UnityEngine.Playables;

namespace RPG.Cinematics
{

    public class TriggerIntro : MonoBehaviour
    {
        bool firstTime = true;
        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Player") && firstTime)
            {
                GetComponent<PlayableDirector>().Play();
                firstTime = false;
            }
        }
    }
}