using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

namespace DUSDJ
{
    public class OptionManager : MonoBehaviour
    {

        #region SingleTon

        /* SingleTon */
        private static OptionManager instance;
        public static OptionManager Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = GameObject.FindObjectOfType(typeof(OptionManager)) as OptionManager;
                    if (!instance)
                    {
                        Debug.LogError("OptionManager Create");

                        var load = Resources.Load<OptionManager>("Managers");
                        instance = load;                       
                    }
                }

                return instance;
            }
        }

        #endregion



        #region UI
        [Header("옵션창")]
        public PopOption PopOption;

        [Header("스크롤바")]
        public Scrollbar ScrollBgm;
        public Scrollbar ScrollEfx;

        [Header("스크롤바 Fill")]
        public Image ScrollBgmFill;
        public Image ScrollEfxFill;

        // [Header("게임종료 확인 모달")]
        // public TitleModal QuitGameModal;


        #endregion







        #region Awake & Init

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

        public void Init()
        {
            LoadVolume();

            SetOption(false);
        }



        #endregion



        #region Option Volume


        public void SetOption(bool onOff)
        {
            if(PopOption == null)
            {
                PopOption = FindObjectOfType<PopOption>();

                if(PopOption == null)
                {
                    Debug.LogError("PopOption Null in this Scene!");
                    return;
                }
            }


            // On
            if (onOff)
            {
                // Local Load
                LoadVolume();

                // Update ScrollBar
                ScrollBgm.value = AudioManager.Instance.GetBGMVolume();
                ScrollEfx.value = AudioManager.Instance.GetEfxVolume();

                // 씬에 따른 버튼 비활성 처리
                // SetButtonsByScene();

                PopOption.UISet(true);
            }
            // Off
            else
            {
                PopOption.UISet(false);

                // 인게임일 경우 정지 해제
                // 인게임 정지 해제 (인게임이 원래 정지상태였다면 해제X)
                /*
                if (SceneManager.GetActiveScene().buildIndex == 2)
                {
                    if (isInGameAlreadyPaused == false)
                    {
                        GameManager.Instance.ResumeGame();
                    }
                }
                */
            }

        }


        #endregion


        #region Local Load

        public void LoadVolume()
        {
            SetBGMVolume(PlayerPrefs.GetFloat("BgmVolume", 1.0f));
            SetEfxVolume(PlayerPrefs.GetFloat("EfxVolume", 1.0f));
        }

        #endregion


        #region Button Event

        public void BtnChangeBGMVolume(float value)
        {
            float result = Mathf.Clamp01(ScrollBgm.value + value);
            ScrollBgm.value = result;

            AudioManager.Instance.SetBGMVolume(result);
        }

        public void BtnChangeEfxVolume(float value)
        {
            float result = Mathf.Clamp01(ScrollEfx.value + value);
            ScrollEfx.value = result;

            AudioManager.Instance.SetEfxVolume(result);
        }

        public void SetBGMVolume(float value)
        {
            ScrollBgmFill.fillAmount = value;

            PlayerPrefs.SetFloat("BgmVolume", value);

            AudioManager.Instance.SetBGMVolume(value);
        }

        public void SetEfxVolume(float value)
        {
            ScrollEfxFill.fillAmount = value;

            PlayerPrefs.SetFloat("EfxVolume", value);

            AudioManager.Instance.SetEfxVolume(value);
        }


        #endregion
    }

}

