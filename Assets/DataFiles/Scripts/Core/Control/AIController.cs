using RPG.Combat;
using RPG.Movement;
using UnityEngine;
using UnityEngine.AI;
using RPG.Core;
using System;
using RPG.Attributes;
using GameDevTV.Utils;
using RPG.Stats;

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
        float timeSinceLastAggrevated = Mathf.Infinity;
        float aggroCooldownTime = 5f;
        float patrolFractionSpeed = 0.2f;
        float attackFractionSpeed = 0.7f;
        LazyValue<Vector3> guardposition;
        [SerializeField] float shoutDistance = 5f;

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
            if (IsAggrevated())
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

        public void Aggrevate()
        {
            timeSinceLastAggrevated = 0;
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
            //if(GetComponent<BaseStats>().characterClass == CharacterClass.Archer || GetComponent<BaseStats>().characterClass == CharacterClass.Mage)
            //mover.StartAction(playerTarget.GetComponent<Health>().transform.position, attackFractionSpeed);

            //fighter.Attack(playerTarget.GetComponent<Health>());
            timeSinceLastSawPlayer = Time.time;

            AggrevatedNearbyEnemies();
        }

        private void AggrevatedNearbyEnemies()
        {
            RaycastHit[] hits = Physics.SphereCastAll(transform.position, shoutDistance,Vector3.up,0);
            foreach (RaycastHit hit in hits)
            {
                AIController ai = hit.collider.GetComponent<AIController>();
                if (ai == null) continue;
                ai.Aggrevate();
            }
        }

        private bool IsAggrevated()
        {
            timeSinceLastAggrevated += Time.deltaTime;
            return (Vector3.Distance(transform.position, playerTarget.transform.position) <= chaseDistance && fighter.CanAttack(playerTarget)) ||
                timeSinceLastAggrevated < aggroCooldownTime;
        }

        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.blue;
            Gizmos.DrawWireSphere(transform.position, chaseDistance);
        }

    }

}