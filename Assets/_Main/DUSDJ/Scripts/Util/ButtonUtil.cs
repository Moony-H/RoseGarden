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
        /// ������Ŵ��� ���� ���� ���
        /// </summary>
        /// <param name="msg"></param>
        public void PlaySound(string msg)
        {
            AudioManager.Instance.PlayOneShot(msg);
        }
    }
}


