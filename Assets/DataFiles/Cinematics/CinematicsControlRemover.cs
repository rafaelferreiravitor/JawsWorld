using RPG.Control;
using RPG.Core;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

namespace RPG.Cinematics
{

    

    public class CinematicsControlRemover : MonoBehaviour
    {
        GameObject player;
        // Start is called before the first frame update
        void Start()
        {
            GetComponent<PlayableDirector>().played += DisableControl;
            GetComponent<PlayableDirector>().stopped += EnableControl;
            GetComponent<FakeAction>().TargetPos += ListenEvent;

        }

        public void ListenEvent(Vector3 target)
        {
            print(target);
        }

        public void DisableControl(PlayableDirector playableDirector)
        {
            player = GameObject.FindGameObjectWithTag("Player");
            player.GetComponent<ActionScheduler>().CancelCurrentAction();
            player.GetComponent<PlayerController>().enabled = false;
        }

        public void EnableControl(PlayableDirector playableDirector)
        {
            player.GetComponent<PlayerController>().enabled = true;
            print("Enable Control!");
        }

        // Update is called once per frame
        void Update()
        {

        }
    }
}