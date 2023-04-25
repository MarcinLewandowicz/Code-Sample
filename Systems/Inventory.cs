using ML.Combat;
using ML.UI;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ML.Systems
{
    public class Inventory : MonoBehaviour
    {
        [SerializeField] private List<EquippedWeapon> weaponInventory = new List<EquippedWeapon>();
        [SerializeField] private Weapon[] startingWeapon;
        [SerializeField] private Transform handTransform;
        [SerializeField] private WeaponsUI weaponsUI;
        private Weapon currentWeapon;
        private int currentWeaponIndex = 1;
        private Animator animator;
        public event Action OnWeaponChanged;

        private void Start()
        {
            animator = GetComponent<Animator>();
            if (startingWeapon == null) { return; }
            startingWeapon[UnityEngine.Random.Range(0,startingWeapon.Length)].PickUp(transform);
        }

        private void ActivateWeapon(Weapon weaponToActivate)
        {
            foreach (EquippedWeapon weapon in weaponInventory)
            {
                if (weapon.weaponScriptableObject == currentWeapon)
                {
                    weapon.weaponInstance.SetActive(true);
                    animator.runtimeAnimatorController = weapon.weaponScriptableObject.AnimatorOverrideController;
                }
                else
                {
                    weapon.weaponInstance.SetActive(false);
                }
            }
        }

        public Transform GetHandTransform()
        {
            return handTransform;
        }

        public GameObject GetCurrentWeaponInstance()
        {
            foreach (EquippedWeapon weapon in weaponInventory)
            {
                if (weapon.weaponScriptableObject == currentWeapon)
                {
                    return weapon.weaponInstance;
                }
            }
            return null;
        }


        public void AddWeaponToInventory(Weapon weaponScriptableObject, GameObject weaponInstance)
        {
            EquippedWeapon newWeapon = new EquippedWeapon(weaponScriptableObject, weaponInstance);
            weaponInventory.Add(newWeapon);
            currentWeapon = newWeapon.weaponScriptableObject;
            ActivateWeapon(currentWeapon);
            currentWeaponIndex = weaponInventory.Count;
            if (weaponsUI == null) { return; }
            weaponsUI.AddWeaponToUI(weaponScriptableObject,currentWeaponIndex);
        }

        public Weapon CurrentWeapon { get { return currentWeapon; } }



        public void SetCurrentWeapon(Weapon weapon)
        {
            currentWeapon = weapon;
        }

        public void SwitchWeapons()
        {
            if (weaponInventory.Count == 1) { return; }
            if (currentWeaponIndex == weaponInventory.Count)
            {
                currentWeaponIndex = 1;
            }
            else
            {
                currentWeaponIndex++;
            }
            currentWeapon = weaponInventory[currentWeaponIndex - 1].weaponScriptableObject;
            ActivateWeapon(currentWeapon);
            OnWeaponChanged();
            weaponsUI.SetActiveWeaponBackground(currentWeaponIndex);
            
        }
    }

}