using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace DUSDJ
{
    public class GameManager : MonoBehaviour
    {
        #region SingleTon
        /* SingleTon */
        private static GameManager instance;
        public static GameManager Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = GameObject.FindObjectOfType(typeof(GameManager)) as GameManager;
                    if (!instance)
                    {
                        Debug.LogError("GameManager Create");

                        var load = Resources.Load<GameManager>("Managers/GameManager");
                        instance = Instantiate(load);
                    }
                }

                return instance;
            }
        }

        #endregion



        [Header("인게임 자동 BGM")]
        public string BGMName;



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


        public void Init()
        {
            Debug.LogWarning("InGame Init!");

            StartCoroutine(InitCoroutine());
        }

        public IEnumerator InitCoroutine()
        {
            if (isInit)
            {
                yield break;
            }

            Debug.Log("=====GameManager Init=====");


            /* InGame UI Manager */
            yield return UIManager.Instance.InitCoroutine();



            /* Set BGM */
            AudioManager.Instance.SetBGM(BGMName);

            /* Set PopOption */
            OptionManager.Instance.Init();


            isInit = true;


            yield return AfterInit();
        }


        public IEnumerator AfterInit()
        {
            // Stage Start Dialogue
            // 스테이지 CSV데이터에서 가져와야하지만 임시로 0 고정

            yield return DialogueManager.Instance.Stage_Dialogue(0);


            yield return null;
        }

        #endregion




        #region Testing

        private void Update()
        {
            if(isInit == false)
            {
                return;
            }

            if (Input.GetKeyDown(KeyCode.I))
            {
                Debug.Log("Key I : PlaySound");
                AudioManager.Instance.PlayOneShot("attack_01");
            }

            if (Input.GetKeyDown(KeyCode.O))
            {
                Debug.Log("Key O : Effect");
                EffectManager.Instance.SetEffect("Efx_Sample", Vector3.zero);
            }
        }


        #endregion


    }

}
