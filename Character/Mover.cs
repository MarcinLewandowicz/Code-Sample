using UnityEngine.AI;
using UnityEngine;
using ML.Systems;
using ML.Combat;
using System.Collections;

namespace ML.Character
{
    public class Mover : MonoBehaviour, IActionable
    {

        private NavMeshAgent navMeshAgent;
        private Animator animator;


        private void Awake()
        {
            navMeshAgent = GetComponent<NavMeshAgent>();
            animator = GetComponent<Animator>();
        }

        private void Update()
        {
            UpdateAnimator();
        }

        private void OnEnable()
        {
            Health health = GetComponent<Health>();
            health.OnDeath += StopAction;
            StartCoroutine(UpdateRotation());
        }

        private void OnDisable()
        {
            Health health = GetComponent<Health>();
            health.OnDeath -= StopAction;
        }

        public void StartMovement(Vector3 target)
        {
            GetComponent<ActionScheduler>().SetAction(this);
            navMeshAgent.isStopped = false;
            MoveToTarget(target);
        }
        public void MoveToTarget(Vector3 target)
        {
            navMeshAgent.isStopped = false;
            navMeshAgent.SetDestination(target);
        }

        public void SetMovementSpeed(float speed)
        {
            navMeshAgent.speed = speed;
        }

        private void UpdateAnimator()
        {
            Vector3 velocity = navMeshAgent.velocity;
            float localVelocity = transform.InverseTransformDirection(velocity).z;
            animator.SetFloat("movementSpeed", localVelocity);
        }

        public void StopMovement()
        {
            if (navMeshAgent.isActiveAndEnabled)
            {
                navMeshAgent.isStopped = true;
            }
        }

        public void StopAction()
        {
            StopMovement();
        }

        private IEnumerator UpdateRotation()
        {
            while (true)
            {
                float rotationY = Quaternion.LookRotation(navMeshAgent.velocity).eulerAngles.y;
                if (rotationY != 0)
                {
                    transform.eulerAngles = new Vector3(0, rotationY, 0);
                }                
                yield return new WaitForSeconds(0.1f); 
            }
        }
    }

}