using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using UnityEngine;

namespace DUSDJ
{

    public class AudioObject : MonoBehaviour
    {
        private AudioSource audioSource;

        private IAudioOwner owner;
        private IState<GameManager> ownerGameState;

        public void Init(AudioClip clip, IAudioOwner audioOwner)
        {
            if (audioSource == null)
            {
                audioSource = gameObject.AddComponent<AudioSource>();
            }
            audioSource.clip = clip;

            audioSource.loop = false;
            audioSource.Play();
        }

        public void Clean()
        {
            if (ownerGameState != null)
            {
                ownerGameState.StateExitEvent -= Clean;
            }

            if (AudioManager.Instance.OwnerDic.ContainsKey(owner))
            {
                AudioManager.Instance.OwnerDic.Remove(owner);
            }

            audioSource.Stop();
            audioSource.clip = null;

            owner = null;
            ownerGameState = null;

            gameObject.SetActive(false);
        }
    }
}
