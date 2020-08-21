using RPG.Combat;
using RPG.Movement;
using UnityEngine;
using UnityEngine.AI;

namespace RPG.Control
{
    public class AIController : MonoBehaviour
    {
        [SerializeField] float chaseDistance = 5f;
        GameObject playerTarget;
        Fighter fighter;
        private void Start()
        {
            playerTarget = GameObject.FindWithTag("Player");
            fighter = GetComponent<Fighter>();
        }

        private void Update()
        {
            if (InAttackRangeOfPlayer())
            {
                fighter.StartAction(playerTarget.GetComponent<Health>());
            }
            else
            {
                fighter.Cancel();
            }
        }

        private bool InAttackRangeOfPlayer()
        {
            
            return (Vector3.Distance(transform.position, playerTarget.transform.position) <= chaseDistance && fighter.CanAttack(playerTarget));
        }
    }

}