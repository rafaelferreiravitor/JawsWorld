using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.SceneManagement;

public class Portal : MonoBehaviour
{
    [SerializeField] string sceneName;
    [SerializeField] GameObject PlayerSpawnPosition;

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

        yield return SceneManager.LoadSceneAsync(sceneName);

        Portal[] portals = FindObjectsOfType<Portal>();
        foreach (var item in portals)
        {
            if (item.sceneName == sceneName)
            {
                print(item.PlayerSpawnPosition.transform.position);
                var player = GameObject.FindGameObjectWithTag("Player");
                
                
                //STILL NEED TO IMPLEMENT THE TELEPORT TO POSITION IT IS NOT WORKING!!!!!!


                //player.GetComponent<NavMeshAgent>().enabled = true;
                //player.GetComponent<NavMeshAgent>().Warp(item.PlayerSpawnPosition.transform.position);
                //player.transform.position = item.PlayerSpawnPosition.transform.position;
                break;
            }
        }
        Destroy(gameObject);
    }
}


