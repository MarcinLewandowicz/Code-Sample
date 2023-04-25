using ML.Combat;
using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;


namespace ML.Combat
{

    public class FieldOfView : MonoBehaviour
    {

        [SerializeField] private float viewAngle = 30f;
        [SerializeField] private float viewDistance = 5f;

        [SerializeField] private Vector3 directionToTarget = Vector3.zero;
        [SerializeField] private Transform targetTransform;

        [SerializeField] private LayerMask targetMask;
        [SerializeField] private LayerMask obstacleMask;
        public bool inVision;

        private void Start()
        {
            if (GetComponent<Enemy>() != null)
            {
                targetTransform = GetComponent<Enemy>().PlayerHealthComponent.transform;
            }
        }

        public bool IsTargetVisible()
        {
            if (!IsTargetInRange()) { return false; }
            directionToTarget = (targetTransform.position - transform.position).normalized;
            if (Vector3.Angle(transform.forward, directionToTarget) < viewAngle / 2)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        private bool IsTargetInRange()
        {
            float distanceToTarget = Vector3.Distance(transform.position, targetTransform.position);
            if (distanceToTarget > viewDistance)
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        private void Update()
        {
            inVision = IsTargetVisible();
        }
    }

}