using RPG.Combat;
using RPG.Core;
using UnityEngine;
using UnityEngine.AI;

namespace RPG.Movement
{


    public class Mover : MonoBehaviour, IAction
    {

        [SerializeField] Vector3 targetDestination;
        NavMeshAgent navMeshAgent;
        Health health;

        void Start()
        {
            navMeshAgent = GetComponent<NavMeshAgent>();
            health = transform.GetComponent<Health>();
        }

        // Update is called once per frame
        void Update()
        {
            UpdateAnimator();
            
            //print(navMeshAgent.steeringTarget);
        }

        private void UpdateAnimator()
        {
            //Getting the global direction
            Vector3 velocity = navMeshAgent.velocity;
            //convert global to local direction
            Vector3 localVelocity = transform.InverseTransformDirection(velocity);
            float speed = localVelocity.z;
            GetComponent<Animator>().SetFloat("fowardSpeed", speed);

        }

        private void FaceTarget()
        {
            /*float speed = 100;
            Vector3 direction = (targetDestination - transform.position).normalized;
            Quaternion lookRotation = Quaternion.LookRotation(new Vector3(direction.x, 0, direction.z));
            transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * speed);*/
        }



        public void MoveTo(Vector3 destination, float stopRange = 0)
        {
            if (!CanMove())
                return;
                
            targetDestination = destination;
            navMeshAgent.isStopped = false;
            navMeshAgent.stoppingDistance = stopRange;
            navMeshAgent.SetDestination(destination);
        }

        public bool CanMove()
        {
            navMeshAgent.enabled = health.GetIsAlive();
            return health.GetIsAlive();

        }


        public void StartAction(Vector3 destination, float stopRange = 0)
        {
            GetComponent<ActionScheduler>().StartAction(this);
            MoveTo(destination, stopRange);
        }

        public void Cancel()
        {
            if(navMeshAgent.enabled)
                navMeshAgent.isStopped = true;
        }
    }
}
