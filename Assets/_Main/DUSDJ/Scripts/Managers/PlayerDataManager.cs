using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace DUSDJ
{
    public class PlayerDataManager : MonoBehaviour
    {
        #region SingleTon
        /* SingleTon */
        private static PlayerDataManager instance;
        public static PlayerDataManager Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = GameObject.FindObjectOfType(typeof(PlayerDataManager)) as PlayerDataManager;
                    if (!instance)
                    {
                        Debug.LogError("PlayerDataManager Create");

                        var load = Resources.Load<PlayerDataManager>("Managers/PlayerDataManager");
                        instance = Instantiate(load);
                    }
                }

                return instance;
            }
        }

        #endregion

        private bool isInit = false;



        #region Data Save & Load

        [Space]
        [Header("JSON암호화여부")]
        public bool JSONCrypto = false;

        // 현재 플레이어 데이터
        public PlayerData NowPD;

        [HideInInspector] public SaveMachine SaveMachine;

        private bool IsLoadedOnce = false;

        #endregion




        private IEnumerator InitSaveData()
        {
            bool savePathForTest = false;
#if (UNITY_EDITOR)
            savePathForTest = true;
#else
        savePathForTest = false;
#endif

            if (SaveMachine == null)
            {
                SaveMachine = new SaveMachine(Instance, savePathForTest);
            }

            yield return null;
        }

        private IEnumerator LoadPlayerData()
        {
            if (SaveMachine.LoadPD != null)
            {
                yield break;
            }

            bool isLoad = SaveMachine.LoadPlayerDataFromJson();

            if (isLoad)
            {
                Debug.Log("데이터 불러오기 완료");
            }
            else
            {
                Debug.LogWarning("불러올 데이터 없음");
            }

            yield return null;
        }




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

            Debug.Log("=====PDM Init=====");

            // Player Data Load
            List<IEnumerator> workList = new List<IEnumerator>();

            workList.Add(InitSaveData());
            workList.Add(LoadPlayerData());
            
            int workCount = workList.Count;

            for (int i = 0; i < workList.Count; i++)
            {
                yield return StartCoroutine(workList[i]);
            }

            Debug.Log("=====PDM Init End=====");
        }

    }
}
