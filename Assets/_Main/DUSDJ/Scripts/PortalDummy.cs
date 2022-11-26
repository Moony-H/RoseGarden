using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DUSDJ
{
    public class PortalDummy : MonoBehaviour
    {
        public string EffectName;

        [Button]
        public void SetText(string str)
        {
            EffectManager.Instance.SetTextEffect(EffectName, transform.position, str);
        }
    }
}


