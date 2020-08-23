using RPG.Combat;
using RPG.Movement;
using UnityEngine;
using UnityEngine.AI;
using RPG.Core;

namespace RPG.Control
{
    public class AIController : MonoBehaviour
    {
        [SerializeField] float chaseDistance = 5f;
        GameObject playerTarget;
        Fighter fighter;
        Mover mover;
        Vector3 guardPosition;
        float timeSinceLastSawPlayer = Mathf.Infinity;
        [SerializeField] float SuspiciousTime = 5f;

        private void Start()
        {
            playerTarget = GameObject.FindWithTag("Player");
            fighter = GetComponent<Fighter>();
            guardPosition = transform.position;
            mover = GetComponent<Mover>();
        }

        private void Update()
        {
            if (InAttackRangeOfPlayer())
            {
                AttackBehaviour();
            }
            else if (Suspicious())
            {
                SuspiciousBehaviour();
            }
            else
            {
                GuardBehaviour();
            }
        }

        private bool Suspicious()
        {
            return Time.time - timeSinceLastSawPlayer <= SuspiciousTime;
        }

        private void GuardBehaviour()
        {
            mover.StartAction(guardPosition);
        }

        private void SuspiciousBehaviour()
        {
            fighter.Cancel();
        }

        private void AttackBehaviour()
        {
            fighter.StartAction(playerTarget.GetComponent<Health>());
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