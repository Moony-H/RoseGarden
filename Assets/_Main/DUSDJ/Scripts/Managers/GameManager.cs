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

                        var load = Resources.Load<GameManager>("Managers");
                        instance = load;
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
            StartCoroutine(InitCoroutine());
        }

        public IEnumerator InitCoroutine()
        {
            if (isInit)
            {
                yield break;
            }

            Debug.Log("=====GameManager Init=====");

            /* Set BGM */
            AudioManager.Instance.SetBGM(BGMName);



            isInit = true;
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
