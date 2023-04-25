using ML.Character;
using ML.Systems;
using System;
using UnityEngine;
using UnityEngine.UI;

namespace ML.Combat
{
    [CreateAssetMenu(fileName = "Weapon",menuName = "Weapons/Create New",order =0)]
    public class Weapon : ScriptableObject
    {
        [SerializeField] private GameObject weaponPrefab;
        [SerializeField] private float weaponRange;
        [SerializeField] private float weaponDamage;
        [SerializeField] private float weaponRangeThreshold;
        [SerializeField] private float attackInterval;
        [SerializeField] private AnimatorOverrideController animatorOverrideController;
        [SerializeField] private Sprite weaponIcon;

        


        public void PickUp(Transform transform)
        {
            Inventory inventory = transform.GetComponent<Inventory>();
            inventory.SetCurrentWeapon(this);
            SetAnimatorOverrideController(transform);
            if (weaponPrefab == null) { return; }
            Fighter fighter = transform.GetComponent<Fighter>();
            SpawnWeapon(inventory);            
        }

        public float WeaponRange { get { return weaponRange; } }

        public float WeaponRangeThreshold { get { return weaponRangeThreshold; } }

        public float WeaponDamage { get { return weaponDamage; } }


        public Sprite WeaponIcon { get { return weaponIcon; } }

        public float AttackInterval { get { return attackInterval; } }

        public AnimatorOverrideController AnimatorOverrideController { get { return animatorOverrideController; } }

        private void SetAnimatorOverrideController(Transform targetTransform)
        {
            if (animatorOverrideController == null) { return; }
            Animator animator = targetTransform.GetComponent<Animator>();
            animator.runtimeAnimatorController = animatorOverrideController;
            
        }

        private void SpawnWeapon(Inventory inventory)
        {
            Transform handTransform = inventory.GetHandTransform();
            GameObject pickedUpWeapon = Instantiate(weaponPrefab, handTransform);
            inventory.AddWeaponToInventory(this, pickedUpWeapon);
        }
    }



}