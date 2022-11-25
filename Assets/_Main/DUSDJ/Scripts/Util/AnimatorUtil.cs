using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace DUSDJ
{

    public class AnimatorUtil : MonoBehaviour
    {
        public void Clean()
        {
            gameObject.SetActive(false);
        }

        /// <summary>
        /// ������Ŵ��� ���� ���� ���
        /// </summary>
        /// <param name="msg"></param>
        public void PlaySound(string msg)
        {
            AudioManager.Instance.PlayOneShot(msg);
        }

        /// <summary>
        /// ���� ���
        /// ������ �׾��� �� ����
        /// ������Ʈ ����� ����
        /// </summary>
        public void PlaySoundInStateWithOwner(string msg)
        {
            AudioManager.Instance.PlayWithClearEvent(msg, true, GetComponentInParent<IAudioOwner>());
        }

        /// <summary>
        /// ���� ���
        /// ������Ʈ ����� ����
        public void PlaySoundInState(string msg)
        {
            AudioManager.Instance.PlayWithClearEvent(msg, true);
        }

        /// <summary>
        /// ���� ���
        /// ������ �׾��� �� ����
        public void PlaySoundWithOwner(string msg)
        {
            AudioManager.Instance.PlayWithClearEvent(msg, false, GetComponentInParent<IAudioOwner>());
        }

        public void SendMessageToObject(string msg)
        {
            SendMessageUpwards(msg);
        }
    }
}
