using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DUSDJ
{

    public enum EnumAudioClearEvent
    {
        NowGameStateChange,
    }

    public class AudioManager : MonoBehaviour
    {
        #region SingleTon
        /* SingleTon */
        private static AudioManager instance;
        public static AudioManager Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = GameObject.FindObjectOfType(typeof(AudioManager)) as AudioManager;
                    if (!instance)
                    {
                        Debug.LogWarning("audioManager Create");

                        var load = Resources.Load<AudioManager>("Managers/AudioManager");                        
                        instance = Instantiate(load);
                    }
                }

                return instance;
            }
        }

        #endregion

        public Dictionary<string, AudioClip> AudioDic;

        public Dictionary<IAudioOwner, AudioObject> OwnerDic;

        private AudioSource audioSourceBgm;
        private AudioSource audioSourceEfx;

        private bool isInit = false;

        #region Awake & InitCoroutine

        private void Awake()
        {
            if (Instance == this)
            {
                DontDestroyOnLoad(this);
            }
            else
            {
                Destroy(gameObject);

                return;
            }

        }


        public IEnumerator InitCoroutine()
        {
            if (isInit)
            {
                yield break;
            }

            Debug.Log("=====AudioManager Init=====");

            AudioDic = new Dictionary<string, AudioClip>();
            OwnerDic = new Dictionary<IAudioOwner, AudioObject>();

            if (audioSourceBgm == null)
            {
                audioSourceBgm = gameObject.AddComponent<AudioSource>();
            }

            if (audioSourceEfx == null)
            {
                audioSourceEfx = gameObject.AddComponent<AudioSource>();
            }


            #region Resource Load

            AudioClip[] clips = Resources.LoadAll<AudioClip>("Audio");
            for (int i = 0; i < clips.Length; i++)
            {
                Debug.LogFormat("AudioDic Add : {0}", clips[i].name);
                AudioDic.Add(clips[i].name, clips[i]);
            }

            #endregion

            isInit = true;
        }


        #endregion


        #region PlayOneShot

        public void PlayOneShot(string msg)
        {
            if (AudioDic == null)
            {
                return;
            }

            if (AudioDic.ContainsKey(msg) == false)
            {
                Debug.LogError(string.Format("Key : {0} 오디오소스가 없음", msg));
                return;
            }

            audioSourceEfx.PlayOneShot(AudioDic[msg]);
        }


        

        public void PlayOneShot(string msg, int max = 3, float lifeTime = 0.1f)
        {
            if (AudioDic == null)
            {
                return;
            }

            if (AudioDic.ContainsKey(msg) == false)
            {
                Debug.LogError(string.Format("Key : {0} 오디오소스가 없음", msg));
                return;
            }

            audioSourceEfx.PlayOneShot(AudioDic[msg]);
        }

        #endregion


        #region AudioOwner Play, Dead

        /// <summary>
        /// 이벤트 조건에 따라 자동 제거됨.
        /// </summary>
        /// <param name="msg"></param>
        /// <param name=""></param>
        public void PlayWithClearEvent(string msg, bool nowStateOwner, IAudioOwner owner = null)
        {
            if (AudioDic.ContainsKey(msg) == false)
            {
                Debug.LogError(string.Format("Key : {0} 오디오소스가 없음", msg));
                return;
            }

            AudioObject ao = new GameObject().AddComponent<AudioObject>();
            ao.Init(AudioDic[msg], owner);
        }


        public void AudioOwnerDead(IAudioOwner owner)
        {
            if (OwnerDic.ContainsKey(owner))
            {
                OwnerDic[owner].Clean();
            }
        }

        #endregion




        #region BGM

        public void SetBGM(string msg)
        {
            Debug.Log("SetBGM");

            if (audioSourceBgm == null)
            {
                Debug.LogError("audioSource null");
                return;
            }

            /*
            if (audioSourceBgm.clip == AudioDic[msg])
            {
                return;
            }
            */
            audioSourceBgm.Stop();
            audioSourceBgm.clip = AudioDic[msg];
            audioSourceBgm.loop = true;
            audioSourceBgm.Play();
        }

        public void BGMStop()
        {
            audioSourceBgm.Stop();
        }

        public void BGMStart()
        {
            audioSourceBgm.Play();
        }


        #endregion


        #region Volume Control (BGM, SFX 합침)


        public void SetBGMVolume(float value)
        {
            audioSourceBgm.volume = value;
        }

        public void SetEfxVolume(float value)
        {
            audioSourceEfx.volume = value;
        }

        public float GetBGMVolume()
        {
            return audioSourceBgm.volume;
        }

        public float GetEfxVolume()
        {
            return audioSourceEfx.volume;
        }


        #endregion


        public void Clean()
        {
            BGMStop();

        }
    }
}

