using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.SceneManagement;


namespace RPG.SceneManagement
{
    public class Portal : MonoBehaviour
    {
        [SerializeField] string sceneName;
        [SerializeField] GameObject PlayerSpawnPosition;
        [SerializeField] float fadeInTime = 1f;
        [SerializeField] float fadeOutTime = 1f;
        [SerializeField] float fadeWaitTime = 0.5f;

        public enum PortalWord
        {
            A,B,C
        };
        [SerializeField] PortalWord id;
        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Player"))
            {
                StartCoroutine(SceneLoad());
            }
        }

        IEnumerator SceneLoad()
        {
           DontDestroyOnLoad(gameObject);
            Fader fader = FindObjectOfType<Fader>();
            
            yield return fader.FadeOut(fadeInTime);
            yield return SceneManager.LoadSceneAsync(sceneName);
            
            Portal[] portals = FindObjectsOfType<Portal>();
            foreach (var item in portals)
            {
                if (item.sceneName == sceneName)
                {
                    if (id == item.id)
                    {
                        //print(item.PlayerSpawnPosition.transform.position);
                        var player = GameObject.FindGameObjectWithTag("Player");

                        //STILL NEED TO IMPLEMENT THE TELEPORT TO POSITION IT IS NOT WORKING!!!!!!

                        //player.GetComponent<NavMeshAgent>().enabled = true;
                        //player.GetComponent<NavMeshAgent>().Warp(item.PlayerSpawnPosition.transform.position);
                        //player.transform.position = item.PlayerSpawnPosition.transform.position;
                    }
                    break;
                }
            }

            yield return new WaitForSeconds(fadeWaitTime);
            yield return fader.FadeIn(fadeOutTime);
            Destroy(gameObject);
        }
    }


}