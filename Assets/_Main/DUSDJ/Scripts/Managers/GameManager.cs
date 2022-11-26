using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
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


        public int NPCLife = 10;

        private static bool restartTrigger = false;

        public static int nowStage = 100;
        /*
        private NPC npc;
        public NPC NPC
        {
            get
            {
                if (npc == null)
                {
                    npc = FindObjectOfType<NPC>();
                    if (npc == null)
                    {
                        Debug.LogError("npc is missing in this scene");
                        return null;
                    }
                }

                return npc;
            }
            set
            {
                npc = value;
            }
        }
        */

        private Player player;
        public Player Player
        {
            get
            {
                if(player == null)
                {
                    player = FindObjectOfType<Player>();
                    if (player == null)
                    {
                        Debug.LogError("Player is missing in this scene");
                        return null;
                    }
                }

                return player;
            }
            set
            {
                player = value;
            }
        }



        [Header("도입부 스킵")]
        public bool Skip = false;

        [Header("인게임 자동 BGM")]
        public string BGMName;



        private bool isInit = false;
        private TimerMachine TimerMachine;



        #region Awake & InitCoroutine

        private void Awake()
        {
            if (Instance == this)
            {

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

            /* Camera Manager */
            yield return CameraManager.Instance.InitCoroutine();

            /* InputManager */
            yield return InputManager.Instance.InitCoroutine();

            /* Init Machines */
            TimerMachine = GetComponentInChildren<TimerMachine>();
            
            /* Set PopOption */
            OptionManager.Instance.Init();


            isInit = true;


            if (restartTrigger)
            {
                restartTrigger = false;
                yield return AfterInit(true);                
            }
            else
            {
                yield return AfterInit();
            }            
        }


        public IEnumerator AfterInit(bool restartSkip = false)
        {
            /* Set BGM */
            AudioManager.Instance.SetBGM(BGMName);

            // Load Stage (최신스테이지 불러오기)
            nowStage = PlayerDataManager.Instance.NowPD.GetLastStage();
            
            // 엔딩가야됨
            if(nowStage <= 0)
            {
                Debug.LogError("스테이지키 끝남");
                yield break;
            }


            // Stage Start Dialogue
            // 스테이지 CSV데이터에서 가져온다
            var stageData = Database.Instance.StageDic[nowStage];


            if (!Skip && !restartSkip)
            {
                // 스테이지 다이얼로그 1
                yield return DialogueManager.Instance.Stage_Dialogue(stageData.Dialogue_Init);


                // 포탈 전부 한번씩 카메라 Follow
                yield return CameraFollowPortals();

                //NPC.Init();

                yield return new WaitForSeconds(1.0f);

                // 스테이지 다이얼로그 2
                yield return DialogueManager.Instance.Stage_Dialogue(stageData.Dialogue_AfterCamera);

            }
            else
            {
                // 포탈 전부 한번씩 카메라 Follow
                yield return CameraFollowPortals();
            }


            Debug.Log("Game Start!");
            yield return GameStartRoutine(stageData);
        }


        private IEnumerator GameStartRoutine(StructStage stageData)
        {
            yield return TimerMachine.SetTImerRoutine(stageData.Timer);


            /* Set Player, NPC, Portal, Input */
            NPCLife = 10;
            // NPC.Init();


            PortalManager.Instance.startCreate();

            Player.Init();

            InputManager.Instance.SetJoystick();            
        }

        #endregion

        [Header("시나리오")]
        private List<Portal> portals;
        public float CameraWait = 1.0f;

        private IEnumerator CameraFollowPortals()
        {
            var wait = new WaitForSeconds(CameraWait);

            portals = PortalManager.Instance.portals.ToList();

            foreach (var portal in portals)
            {
                CameraManager.Instance.SetCameraFollow(portal.transform);
                yield return wait;
            }


            // 마지막은 플레이어
            var p = FindObjectOfType<Player>();
            CameraManager.Instance.SetCameraFollow(p.transform);
        }



        #region Restart

        public void Restart()
        {            
            StopAllCoroutines();
            
            restartTrigger = true;

            LoadingSceneManager.LoadScene(2);
        }

        #endregion




        #region GameOver, GameClear

        public void GameOver()
        {
            AudioManager.Instance.BGMStop();

            Debug.Log("Stage Fail!");

            UIManager.Instance.PopGameOver.SetUI(true);
            InputManager.Instance.CleanJoystick();
        }

        public void GameClear()
        {
            AudioManager.Instance.BGMStop();

            Debug.Log("Stage Clear!");
            PlayerDataManager.Instance.NowPD.SetStageClear(nowStage);
            PlayerDataManager.Instance.SaveMachine.UpdateSaveData(EnumSave.Clear);

            UIManager.Instance.PopGameClear.SetUI(true);
            InputManager.Instance.CleanJoystick();
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



        [Button]
        public void TestStageClear()
        {   
            GameClear();
            
        }


        [Button]
        public void TestStageFail()
        {
            GameOver();
        }

        [Button]
        public void AudioGroupTest(string msg, int value)
        {
            for (int i = 0; i < value; i++)
            {
                AudioManager.Instance.PlayOneShotGroup(msg, 3, 0.3f);
            }
        }

        #endregion


    }

}
