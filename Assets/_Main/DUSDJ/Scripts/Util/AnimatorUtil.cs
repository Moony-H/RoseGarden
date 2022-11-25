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
        /// 오디오매니저 통해 사운드 재생
        /// </summary>
        /// <param name="msg"></param>
        public void PlaySound(string msg)
        {
            AudioManager.Instance.PlayOneShot(msg);
        }

        /// <summary>
        /// 사운드 재생
        /// 주인이 죽었을 시 제거
        /// 스테이트 변경시 제거
        /// </summary>
        public void PlaySoundInStateWithOwner(string msg)
        {
            AudioManager.Instance.PlayWithClearEvent(msg, true, GetComponentInParent<IAudioOwner>());
        }

        /// <summary>
        /// 사운드 재생
        /// 스테이트 변경시 제거
        public void PlaySoundInState(string msg)
        {
            AudioManager.Instance.PlayWithClearEvent(msg, true);
        }

        /// <summary>
        /// 사운드 재생
        /// 주인이 죽었을 시 제거
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
