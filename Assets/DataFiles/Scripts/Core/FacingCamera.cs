using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace RPG.Core { 

    public class FacingCamera : MonoBehaviour
    {


        // Update is called once per frame
        void Update()
        {
            transform.forward = Camera.main.transform.forward;
            //transform.LookAt(Camera.main.transform.position);
        }
    }
}