using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FakeAction : MonoBehaviour
{
    public event Action<Vector3> TargetPos;

    private void Start()
    {
        Invoke("CallEventWithParameter", 3f);
    }

    public void CallEventWithParameter()
    {
        TargetPos.Invoke(new Vector3(150, 150, 150));
        
    }
}
