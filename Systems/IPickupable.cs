using UnityEngine;

namespace ML.Systems
{
    public interface IPickupable
    {
        public float PickupDistance { get; }
        public Vector3 PickupPosition { get; }
        public void PickUp(Transform picker);      


    }
}