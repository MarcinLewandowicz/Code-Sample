using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

namespace ML.UI
{

    public class DamageTextDisplay : MonoBehaviour
    {

        [SerializeField] private TextMeshProUGUI damageText;

        public void InitializeText(string text, Color color)
        {
            damageText.color = color;
            damageText.SetText(text);
            GetComponent<Animation>().Play();
        }

        //ANIMATION EVENT
        public void DisableAfterAnimation()
        {
            gameObject.SetActive(false);
        }
    }

}