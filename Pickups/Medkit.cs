using ML.Character;
using ML.Combat;
using ML.Player;
using UnityEngine;

namespace ML.Systems
{    public class Medkit : MonoBehaviour, IPickupable, IClickable
    {
        [SerializeField] private float distanceToPickup;
        [SerializeField] private float healAmount = 20;
        [SerializeField] private GameObject healFX;
        public float PickupDistance { get { return distanceToPickup; } }
        public Vector3 PickupPosition { get { return transform.position; } }

        public CursorType GetCursorType()
        {
            return CursorType.Pickup;
        }

        public bool InteractWithRaycast(PlayerController playerController)
        {
            if (Input.GetMouseButtonDown(0))
            {
                Health playerHealthComponent = playerController.GetComponent<Health>();
                if (playerHealthComponent.HealthPoints == playerHealthComponent.StartHealthPoints) { return false; }
                playerController.GetComponent<Picker>().SetPickupTarget(this);
            }
            return true;
        }

        public void PickUp(Transform transform)
        {
            transform.GetComponent<Health>().Heal(healAmount);
            transform.GetComponent<Picker>().StopAction();
            Instantiate(healFX, transform.position, Quaternion.identity);
            Destroy(gameObject);
        }
    }

}