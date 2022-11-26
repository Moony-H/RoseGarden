using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DUSDJ
{
    public class UIBtnOption : MonoBehaviour
    {
        public void OnClick(bool value)
        {
            OptionManager.Instance.SetOption(value);
        }
    }
}


