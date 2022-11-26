using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DUSDJ
{

    public class DialogueManager : MonoBehaviour
    {
        #region SingleTon

        /* SingleTon */
        private static DialogueManager instance;
        public static DialogueManager Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = GameObject.FindObjectOfType(typeof(DialogueManager)) as DialogueManager;
                    if (!instance)
                    {
                        Debug.LogWarning("DialogueManager Create");

                        var load = Resources.Load<DialogueManager>("Managers/DialogueManager");
                        instance = Instantiate(load);
                    }
                }

                return instance;
            }
        }

        #endregion



        private bool isInit = false;



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

        /*
        public IEnumerator InitCoroutine()
        {
            if (isInit)
            {
                yield break;
            }

            // Set Chara Data Dic
            CharaDic = new Dictionary<string, ScriptableDialogueCharacter>();
            var CharaDatas = Resources.LoadAll<ScriptableDialogueCharacter>("DialogueCharacters");

            foreach (var c in CharaDatas)
            {
                if (CharaDic.ContainsKey(c.name))
                {
                    Debug.LogErrorFormat("ScriptableDialogueCharacter duplication : {0}");
                    continue;
                }

                CharaDic.Add(c.name, c);
            }


            isInit = true;

            yield return null;
        }
        */

        #endregion


        #region Dialogue System

        public IEnumerator DialogueCoroutine;

        private IEnumerator nextFlow;
        // private EnumDialogueStartCondition DialogueStartCondition;



        private List<StructDialogue> dialogueList = new List<StructDialogue>();
        private int index = 0;

        private int LastIndex = -1;
        private EnumDialoguePosition LastPosition;



        private void AddDialogue(StructDialogue copy)
        {
            StructDialogue sd = new StructDialogue();

            sd.DialoguePosition = copy.DialoguePosition;
            sd.Name_Cha = copy.Name_Cha;
            sd.Sprite = copy.Sprite;
            sd.Content = copy.Content;

            dialogueList.Add(sd);            
        }





        public IEnumerator Stage_Dialogue(int dialogueKey)
        {
            if (Database.Instance.DialogueDic.ContainsKey(dialogueKey) == false)
            {
                Debug.LogError("dialogue Key Error!");
                yield break;
            }

            dialogueList.Clear();


            /* 다이얼로그 불러오기 */
            var table = Database.Instance.DialogueDic[dialogueKey];

            // 다이얼로그 위한 UI 초기화
            var pd = UIManager.Instance.PanDialogue;
            pd.SetUI(true);


            StructDialogue sd = new StructDialogue();
            sd.Index = 0;
            sd.DialoguePosition = EnumDialoguePosition.Left;
            sd.Name_Cha = string.Empty;
            sd.Sprite = string.Empty;
            sd.Content = string.Empty;


            // 다이얼로그 생성
            for (int i = 0; i < table.Count; i++)
            {
                var copy = table[i];

                sd.DialoguePosition = copy.DialoguePosition;
                sd.Name_Cha = copy.Name_Cha;
                sd.Sprite = copy.Sprite;
                sd.Content = copy.Content;

                AddDialogue(sd);
            }

            // 시작
            Action();

            // 입력존버
            while (IsEnded() == false)
            {
                //TODO : 입력 수정하자
                if (Input.GetMouseButtonDown(0))
                {
                    Action();
                }

                yield return null;
            }

            // 대화끝
            Clear();
        }





        public bool IsEnded()
        {
            return index > dialogueList.Count;
        }

        public void Action()
        {
            if (DialogueCoroutine != null)
            {
                StopCoroutine(DialogueCoroutine);
                DialogueCoroutine = null;
                ForceClearLastDialogue();

                return;
            }

            if (index >= dialogueList.Count)
            {
                index += 1;
                return;
            }

            PrintDialogue(dialogueList[index]);
            LastPosition = dialogueList[index].DialoguePosition;

            LastIndex = index;
            index += 1;
        }


        public void PrintDialogue(StructDialogue sd)
        {
            var pd = UIManager.Instance.PanDialogue;

            var box = pd.PrintDialogue(sd);

            DialogueCoroutine = UtilCoroutine.TextPrint(0.05f, sd.Content, box.ContentText, () =>
            {
                DialogueCoroutine = null;
            });

            StartCoroutine(DialogueCoroutine);
        }

        public void ForceClearLastDialogue()
        {
            if (LastIndex < 0)
            {
                return;
            }

            var pd = UIManager.Instance.PanDialogue;

            switch (LastPosition)
            {
                case EnumDialoguePosition.Left:
                    pd.Box_Left.ContentText.text = dialogueList[index - 1].Content;
                    break;

                case EnumDialoguePosition.Right:
                    pd.Box_Right.ContentText.text = dialogueList[index - 1].Content;
                    break;
            }
        }


        public void Clear()
        {
            index = 0;
            LastIndex = -1;
            dialogueList.Clear();

            var pd = UIManager.Instance.PanDialogue;

            pd.SetUI(false);
            pd.ClearBox(EnumDialoguePosition.Left);
            pd.ClearBox(EnumDialoguePosition.Right);
        }


        #endregion



    }
}