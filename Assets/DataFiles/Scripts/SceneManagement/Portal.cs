using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

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
            
            yield return fader.FadeOut(fadeOutTime);
            SavingWraper savingWraper = FindObjectOfType<SavingWraper>();
            savingWraper.Save();

            yield return SceneManager.LoadSceneAsync(sceneName);
            savingWraper.Load();

            
            
            Portal otherPortal = GetOtherPortal();
            UpdatePlayer(otherPortal);
            savingWraper.Save();
            yield return new WaitForSeconds(fadeWaitTime);



            yield return fader.FadeIn(fadeOutTime);
            Destroy(gameObject);
        }

        private Portal GetOtherPortal()
        {
            foreach (Portal portal in FindObjectsOfType<Portal>())
            {
                if (portal == this) continue;
                if (portal.id != id) continue;
                return portal;
            }
            return null;
        }

        private void UpdatePlayer(Portal otherPortal)
        {
            GameObject player = GameObject.FindWithTag("Player");
            player.GetComponent<NavMeshAgent>().enabled = false;
            player.transform.position = otherPortal.PlayerSpawnPosition.transform.position;
            player.transform.rotation = otherPortal.PlayerSpawnPosition.transform.rotation;
            player.GetComponent<NavMeshAgent>().enabled = true;
        }
    }


}