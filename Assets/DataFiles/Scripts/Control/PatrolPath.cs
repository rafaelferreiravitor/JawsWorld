using System.Collections;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Runtime.CompilerServices;
using UnityEngine;

namespace RPG.Control
{
    public class PatrolPath : MonoBehaviour
    {
        private void OnDrawGizmos()
        {
            Vector3 lastWaypoint = transform.GetChild(0).position;
            for(int i =0;i< transform.childCount; i++)
            {

                Gizmos.color = Color.white;
                Gizmos.DrawSphere(GetWaypoint(i), 0.2f);
                Gizmos.DrawLine(GetWaypoint(i), GetNextWaypoint(i));
            }
            //Gizmos.DrawLine(GetChildPosition(transform.childCount-1), GetChildPosition(0));
        }

        public Vector3 GetNextWaypoint(int current)
        {
            if(current == transform.childCount-1) 
                return GetWaypoint(0);
            return GetWaypoint(current + 1);
        }

        public int GetNextIndex(int current)
        {
            if (current == transform.childCount - 1) 
                return 0;
            return current+1;
        }


        public Vector3 GetWaypoint(int i)
        {
            return transform.GetChild(i).position;
        }
    }
}
