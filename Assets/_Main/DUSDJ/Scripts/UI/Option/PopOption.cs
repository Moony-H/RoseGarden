using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace DUSDJ
{
    public class PopOption : MonoBehaviour
    {

        [Header("胶农费官")]
        public Slider ScrollBgm;
        public Slider ScrollEfx;

        [Header("胶农费官 Fill")]
        public Image ScrollBgmFill;
        public Image ScrollEfxFill;



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


        public void BtnChangeBGMVolume(float value)
        {
            OptionManager.Instance.BtnChangeBGMVolume(value);
        }

        public void BtnChangeEfxVolume(float value)
        {
            OptionManager.Instance.BtnChangeEfxVolume(value);
        }

        public void SetBGMVolume(float value)
        {
            OptionManager.Instance.SetBGMVolume(value);
        }

        public void SetEfxVolume(float value)
        {
            OptionManager.Instance.SetEfxVolume(value);
        }

    }

}

