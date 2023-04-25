using ML.Combat;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ML.UI
{
    public class WeaponsUI : MonoBehaviour
    {
        [SerializeField] private GameObject singleWeaponUIPrefab;
        [SerializeField] private List<SingleWeaponUI> weaponsUI = new List<SingleWeaponUI>();

        public void AddWeaponToUI(Weapon weapon, int weaponIndex)
        {
            GameObject newWeaponInstance = Instantiate(singleWeaponUIPrefab, transform);
            SingleWeaponUI newWeaponUI = newWeaponInstance.GetComponent<SingleWeaponUI>();
            newWeaponUI.SetWeaponImage(weapon.WeaponIcon);
            weaponsUI.Add(newWeaponUI);
            SetActiveWeaponBackground(weaponIndex);
        }

        public void SetActiveWeaponBackground(int weaponIndex)
        {
            for (int i = 0; i < weaponsUI.Count; i++)
            {
                if (i == weaponIndex - 1)
                {
                    weaponsUI[i].SetWeaponBackgroundColor(Color.green);
                }
                else
                {
                    weaponsUI[i].SetWeaponBackgroundColor(Color.red);
                }
            }
        }
    }

}