using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace DUSDJ
{

    public class PopGameOver : MonoBehaviour
    {
        public void Init()
        {
            SetUI(false);
        }

        public void SetUI(bool onOff)
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


