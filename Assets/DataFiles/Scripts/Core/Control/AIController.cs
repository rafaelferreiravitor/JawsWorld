using RPG.Combat;
using RPG.Movement;
using UnityEngine;
using UnityEngine.AI;
using RPG.Core;
using System;
using RPG.Attributes;
using GameDevTV.Utils;

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
        float patrolFractionSpeed = 0.2f;
        float attackFractionSpeed = 0.7f;
        LazyValue<Vector3> guardposition;

        private void Awake()
        {
            timeSinceLastSawPlayer = Time.time;
            timeSinceLastDweling = Time.time;
            mover = GetComponent<Mover>();
            playerTarget = GameObject.FindWithTag("Player");
            fighter = GetComponent<Fighter>();
            guardposition = new LazyValue<Vector3>(GetGuardPosition);
        }

        private Vector3 GetGuardPosition()
        {
            return patrolPath.GetWaypoint(currentWaypointIndex);
        }

        private void Start()
        {
            //startPosition = patrolPath.GetChildPosition(0);
            guardposition.ForceInit();
            transform.position = guardposition.value;

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

            mover.StartAction(nextPosition,patrolFractionSpeed);
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
            mover.StartAction(playerTarget.GetComponent<Health>().transform.position, attackFractionSpeed);

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