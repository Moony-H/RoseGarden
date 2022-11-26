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


        public void BtnGoToTitle()
        {
            LoadingSceneManager.LoadScene(0);
        }

        public void Restart()
        {
            SetUI(false);

            GameManager.Instance.Restart();
        }

    }


}


