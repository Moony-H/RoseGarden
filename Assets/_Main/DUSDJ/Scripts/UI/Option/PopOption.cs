using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DUSDJ
{
    public class PopOption : MonoBehaviour
    {
        public void UISet(bool onOff)
        {
            if (onOff)
            {
                gameObject.SetActive(true);
            }
            else
            {
                gameObject.SetActive(false);
            }

        }
    }

}

