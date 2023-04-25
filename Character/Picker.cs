using ML.Combat;
using ML.Systems;
using UnityEngine;

namespace ML.Character
{
    public class Picker : MonoBehaviour, IActionable
    {
        [SerializeField] private IPickupable targetToPickUp;
        private void OnEnable()
        {
            Health health = GetComponent<Health>();
            health.OnDeath += StopAction;
        }

        private void OnDisable()
        {
            Health health = GetComponent<Health>();
            health.OnDeath -= StopAction;
        }

        // Update is called once per frame
        void Update()
        {
            if (targetToPickUp == null) { return; }

            float distanceToTarget = Vector3.Distance(transform.position, targetToPickUp.PickupPosition);
            if (distanceToTarget > targetToPickUp.PickupDistance)
            {
                GetComponent<Mover>().MoveToTarget(targetToPickUp.PickupPosition);
            }
            else
            {
                targetToPickUp.PickUp(gameObject.transform);
            }
        }

        public void SetPickupTarget(IPickupable target)
        {
            GetComponent<ActionScheduler>().SetAction(this);
            targetToPickUp = target;
        }

        public void StopAction()
        {
            targetToPickUp = null;
        }
    }

}