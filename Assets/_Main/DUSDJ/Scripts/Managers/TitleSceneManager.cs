using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace DUSDJ
{
    public class TitleSceneManager : MonoBehaviour
    {
        #region SingleTon
        /* SingleTon */
        private static TitleSceneManager instance;
        public static TitleSceneManager Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = GameObject.FindObjectOfType(typeof(TitleSceneManager)) as TitleSceneManager;
                    if (!instance)
                    {
                        Debug.LogError("TitleSceneManager Null");
                        return null;
                    }
                }

                return instance;
            }
        }

        #endregion

        public static bool IsLoaded = false;


        public GameObject ForceLoadingObject;



        private void Awake()
        {
            Debug.LogWarning("Awake!");

            if (Instance == this)
            {
                DontDestroyOnLoad(this);
            }
            else
            {
                if (IsLoaded)
                {
                    // Loaded & GameScene
                    if (SceneManager.GetActiveScene().buildIndex == 2)
                    {
                        GameManager.Instance.Init();
                        return;
                    }
                }

                Destroy(gameObject);

                return;
            }

            if (IsLoaded)
            {
                return;
            }
            
            instance = this;

            // Scene 0 = Title
            if (SceneManager.GetActiveScene().buildIndex == 0)
            {
                StartCoroutine(InitRoutine());                
            }
            // Else = Load And Init GameManager
            else
            {
                StartCoroutine(InitRoutine(false));
            }            
        }

        public IEnumerator InitRoutine(bool isTitle = true)
        {
            Debug.LogWarning("Title Init!");

            // GameScene : Loading Object Set Acitve
            if (!isTitle)
            {
                ForceLoadingObject.SetActive(true);
            }
            else
            {
                ForceLoadingObject.SetActive(false);
            }

            // Player Data Init (Load)
            yield return PlayerDataManager.Instance.InitCoroutine();


            // Database Init
            yield return Database.Instance.InitCoroutine();


            // Dialogue Manager Init
            // yield return DialogueManager.Instance.InitCoroutine();

            // Resource Managers
            yield return AudioManager.Instance.InitCoroutine();
            yield return EffectManager.Instance.InitCoroutine();

            // OptionManager
            OptionManager.Instance.Init();


            IsLoaded = true;

            // GameScene : Init
            if (!isTitle)
            {
                ForceLoadingObject.SetActive(false);
                GameManager.Instance.Init();
            }
        }



        public void BtnLoadScene(int index)
        {
            LoadingSceneManager.LoadScene(index);
        }

        /// <summary>
        /// 게임 종료
        /// </summary>
        public void BtnQuitApplication()
        {
            // 앱 종료
#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
#else
            UnityEngine.Application.Quit();
#endif
        }


    }
}
