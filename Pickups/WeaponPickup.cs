using ML.Character;
using ML.Combat;
using ML.Player;
using UnityEngine;

namespace ML.Systems
{
    public class WeaponPickup : MonoBehaviour, IClickable, IPickupable
    {
        [SerializeField] private float distanceToPickup;
        [SerializeField] private Weapon weapon;

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
                playerController.GetComponent<Picker>().SetPickupTarget(this);
            }            
            return true;
        }

        public void PickUp(Transform transform)
        {
            weapon.PickUp(transform);
            transform.GetComponent<Picker>().StopAction();
            Destroy(gameObject);
        }
    }
}
