using ML.Systems;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ML.Combat
{
    public class GuardAI : Enemy
    {
        [SerializeField]
        private float patrolSpeed;
        public float PatrolSpeed { get { return patrolSpeed; } }
        [SerializeField]
        public float chaseSpeed;
        public float ChaseSpeed { get { return chaseSpeed; } }
        public float MaximumTimeOfIdle { get; set; }
        public GuardBaseState CurrentState { get; private set; }
        public GuardIdleState GuardIdleState { get; } = new GuardIdleState();
        public GuardPatrolState GuardPatrolState { get; } = new GuardPatrolState();
        public GuardChaseState GuardChaseState { get; } = new GuardChaseState();

        public GuardSuspiciousState GuardSuspiciousState { get; } = new GuardSuspiciousState();
        public Transform[] PatrolTransforms { get; private set; }
        public Vector3[] PatrolVectors { get; private set; }
        public int CurrentPointIndex { get; set; } = 0;

        protected override void OnEnable()
        {
            base.OnEnable();
            enemyHealth.OnHit += OnDamage;
            enemyHealth.OnDeath += OnDeath;
        }

        protected override void OnDisable()
        {
            base.OnDisable();
            enemyHealth.OnHit -= OnDamage;
            enemyHealth.OnDeath -= OnDeath;
        }

        protected override void RespawnBehaviour()
        {
            PatrolTransforms = new Transform[0];
            PatrolVectors = new Vector3[0];
            BuildPatrolVectors();
            base.RespawnBehaviour();
        }

        private void Start()
        {
            BuildPatrolVectors();
            CurrentState = GuardIdleState;
            CurrentState.EnterState(this);
        }

        private void BuildPatrolVectors()
        {
            if (PatrolTransforms.Length == 0)
            {
                PatrolVectors = new Vector3[2];
                PatrolVectors[0] = transform.position;
                PatrolVectors[1] = PlayerHealthComponent.transform.position;
            }
            else
            {
                PatrolVectors = new Vector3[PatrolTransforms.Length];
                for (int i = 0; i < PatrolTransforms.Length; i++)
                {
                    PatrolVectors[i] = PatrolTransforms[i].position;
                }
            }
        }

        private void Update()
        {
            if (enemyHealth.IsDead()) { return; }
            CurrentState.UpdateState(this);
        }

        public void SwitchState(GuardBaseState state)
        {
            CurrentState = state;
            CurrentState.EnterState(this);
        }

        private void OnDamage()
        {
            if (CurrentState != GuardChaseState) { SwitchState(GuardChaseState); }            
        }

        private void OnDeath()
        {
            CurrentState.CancelState(this);
        }
    }

    public class GuardIdleState : GuardBaseState
    {
        private float timeSinceIdleStarted = Mathf.NegativeInfinity;

        public override void EnterState(GuardAI guard)
        {
            timeSinceIdleStarted = 0;
        }

        public override void UpdateState(GuardAI guard)
        {
            timeSinceIdleStarted += Time.deltaTime;
            if (guard.enemyFOV.IsTargetVisible()) { guard.SwitchState(guard.GuardChaseState); }
            if (timeSinceIdleStarted > guard.MaximumTimeOfIdle)
            {
                UpdatePatroPointIndex(guard);
                guard.SwitchState(guard.GuardPatrolState);
            }
        }

        private void UpdatePatroPointIndex(GuardAI guard)
        {
            if (guard.CurrentPointIndex + 1 == guard.PatrolVectors.Length)
            {
                guard.CurrentPointIndex = 0;
            }
            else
            {
                guard.CurrentPointIndex++;
            }
        }
    }

    public class GuardPatrolState : GuardBaseState
    {
        public override void EnterState(GuardAI guard)
        {
            guard.Move(guard.PatrolVectors[guard.CurrentPointIndex]);
            guard.SetMovementSpeed(guard.PatrolSpeed);
        }

        public override void UpdateState(GuardAI guard)
        {
            float distanceToDestination = Vector3.Distance(guard.transform.position, guard.PatrolVectors[guard.CurrentPointIndex]);
            if (guard.enemyFOV.IsTargetVisible()) { guard.SwitchState(guard.GuardChaseState); }
            if (distanceToDestination <= 1f) { guard.SwitchState(guard.GuardIdleState); }
        }
    }

    public class GuardChaseState : GuardBaseState
    {
        float timeSinceChaseStarted = 0;
        public override void EnterState(GuardAI guard)
        {
            guard.SetExclamationMarkStatus(true);
            timeSinceChaseStarted = 0;
            guard.SetMovementSpeed(guard.ChaseSpeed);
            guard.Attack();
        }

        public override void UpdateState(GuardAI guard)
        {
            if (guard.enemyFOV.IsTargetVisible()) { timeSinceChaseStarted = 0; }
            else { timeSinceChaseStarted += Time.deltaTime; }

            if(timeSinceChaseStarted > 2f)
            {                
                guard.SwitchState(guard.GuardSuspiciousState);
            }
        }

        public override void CancelState(GuardAI guard)
        {
            guard.SetExclamationMarkStatus(false);
        }
    }

    public class GuardSuspiciousState : GuardBaseState
    {
        float timeSinceSusStarted = 0;
        float suspicionTime = 2f;

        public override void EnterState(GuardAI guard)
        {
            timeSinceSusStarted = 0;
            guard.Move(guard.transform.position);
        }

        public override void UpdateState(GuardAI guard)
        {
            timeSinceSusStarted += Time.deltaTime;
            if (guard.enemyFOV.IsTargetVisible()) { guard.SwitchState(guard.GuardChaseState); }
            if(timeSinceSusStarted >= suspicionTime) 
            {
                guard.SetExclamationMarkStatus(false);
                guard.SwitchState(guard.GuardPatrolState); 
            }

        }
    }
}