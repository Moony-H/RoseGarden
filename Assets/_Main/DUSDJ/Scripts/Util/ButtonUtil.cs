using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace DUSDJ
{
    public class ButtonUtil : MonoBehaviour
    {
        /// <summary>
        /// 오디오매니저 통해 사운드 재생
        /// </summary>
        /// <param name="msg"></param>
        public void PlaySound(string msg)
        {
            AudioManager.Instance.PlayOneShot(msg);
        }
    }
}


