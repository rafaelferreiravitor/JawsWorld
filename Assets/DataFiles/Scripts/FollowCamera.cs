using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowCamera : MonoBehaviour
{

    [SerializeField] GameObject target;
    int xOffset =0;
    int yOffset = 5;
    int zOffset = -5;
    void Start()
    {
        transform.Rotate(new Vector3(1, 0, 0), 45);
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = new Vector3(target.transform.position.x + xOffset,
                                        target.transform.position.y + yOffset,
                                        target.transform.position.z + zOffset);
        

    }
}
