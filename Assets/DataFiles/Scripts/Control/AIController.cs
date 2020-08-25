using RPG.Combat;
using RPG.Movement;
using UnityEngine;
using UnityEngine.AI;
using RPG.Core;
using System;

namespace RPG.Control
{
    public class AIController : MonoBehaviour
    {
        [SerializeField] float chaseDistance = 5f;
        GameObject playerTarget;
        Fighter fighter;
        Mover mover;
        //Vector3 startPosition;
        int currentWaypointIndex = 0;
        float timeSinceLastSawPlayer;
        float SuspiciousTime = 5f;
        [SerializeField] PatrolPath patrolPath;
        float waypointTolerance = 2f;
        float dwelingTime = 5f;
        float timeSinceLastDweling;

        private void Start()
        {
            timeSinceLastSawPlayer = Time.time;
            timeSinceLastDweling = Time.time;
            playerTarget = GameObject.FindWithTag("Player");
            fighter = GetComponent<Fighter>();
            //startPosition = patrolPath.GetChildPosition(0);
            transform.position = patrolPath.GetWaypoint(currentWaypointIndex);
            mover = GetComponent<Mover>();
        }

        private void Update()
        {
            if (InAttackRangeOfPlayer())
            {
                AttackBehaviour();
            }
            else if (Suspicious() && SuspiciousTime != Mathf.Infinity)
            {
                SuspiciousBehaviour();
            }
            else
            {
                PatrolBehaviour();
            }
        }

        private bool Suspicious()
        {
            return Time.time - timeSinceLastSawPlayer <= SuspiciousTime;
        }

        private void PatrolBehaviour()
        {
            Vector3 nextPosition = new Vector3();
            
            if (patrolPath != null)
            {
                if (AtWaypoint())
                {
                    CycleWaypoint();
                }
                else
                    timeSinceLastDweling = Time.time;
                nextPosition = GetWaypoint();
            }

            mover.StartAction(nextPosition);
        }

        private Vector3 GetWaypoint()
        {
            return patrolPath.GetWaypoint(currentWaypointIndex);
        }

        private void CycleWaypoint()
        {
            if(DwelingTime())
                currentWaypointIndex = patrolPath.GetNextIndex(currentWaypointIndex);
        }

        private bool DwelingTime()
        {
            if (Time.time - timeSinceLastDweling >= dwelingTime)
            {
                return true;
            }
            return false;
        }

        private bool AtWaypoint()
        {

            return (Vector3.Distance(transform.position, GetWaypoint()) <= waypointTolerance);
        }

        private void SuspiciousBehaviour()
        {
            fighter.Cancel();
        }

        private void AttackBehaviour()
        {
            fighter.StartAction(playerTarget.GetComponent<Health>());
            //fighter.Attack(playerTarget.GetComponent<Health>());
            timeSinceLastSawPlayer = Time.time;
        }

        private bool InAttackRangeOfPlayer()
        {
            return (Vector3.Distance(transform.position, playerTarget.transform.position) <= chaseDistance && fighter.CanAttack(playerTarget));
        }

        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.blue;
            Gizmos.DrawWireSphere(transform.position, chaseDistance);
        }

    }

}