using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

namespace ML.UI
{
    public class SingleWeaponUI : MonoBehaviour
    {
        [SerializeField] private Image weaponImage;
        [SerializeField] private Image backgroundImage;

        public void SetWeaponImage(Sprite weaponSprite)
        {
            weaponImage.sprite = weaponSprite;
        }

        public void SetWeaponBackgroundColor(Color color)
        {
            backgroundImage.color = color;
        }

    }

}