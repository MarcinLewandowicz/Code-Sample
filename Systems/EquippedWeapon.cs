using ML.Combat;
using System;
using UnityEngine;

namespace ML.Systems
{
    [Serializable]
    public struct EquippedWeapon
    {
        public Weapon weaponScriptableObject;
        public GameObject weaponInstance;


        public EquippedWeapon(Weapon weaponScriptableObject, GameObject weaponInstance)
        {
            this.weaponScriptableObject = weaponScriptableObject;
            this.weaponInstance = weaponInstance;
        }
    }


}