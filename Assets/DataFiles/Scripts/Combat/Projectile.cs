using System.Collections;
using System.Collections.Generic;
using System.Net.Sockets;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    [SerializeField] float speed = 0.01f;
    GameObject player;
    
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        transform.LookAt(GetAimLocation());
    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate(Vector3.forward*speed*Time.deltaTime);
    }

    public Vector3 GetAimLocation()
    {
        CapsuleCollider collider = player.GetComponent<CapsuleCollider>();
        if(collider != null)
        {
            return player.transform.position /*+ Vector3.up * collider.height/2*/;
        }
        return player.transform.position;
    }
}
