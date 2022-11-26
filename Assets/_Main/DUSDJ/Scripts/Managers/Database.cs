using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace DUSDJ
{
    public class Database : MonoBehaviour
    {
        #region SingleTon

        /* SingleTon */
        private static Database instance;

        public static Database Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = GameObject.FindObjectOfType(typeof(Database)) as Database;
                    if (!instance)
                    {
                        Debug.LogWarning("Database Create");

                        var load = Resources.Load<Database>("Managers/Database");
                        instance = Instantiate(load);
                    }
                }

                return instance;
            }
        }

        #endregion


        private bool isInit = false;


        // 이미지 테이블
        public Dictionary<string, Sprite> SpriteDic;


        // '모든' 다이얼로그 테이블
        public Dictionary<int, List<StructDialogue>> DialogueDic;

        // 스테이지 테이블
        public Dictionary<int, StructStage> StageDic;




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





        #region Getters

        public Sprite GetSpriteFromImageDic(string key)
        {
            if (SpriteDic.ContainsKey(key) == false)
            {
                Debug.LogWarning("ImageDic.ContainsKey(key) == false : " + key);

                // 더미 이미지 리턴
                return null;
            }

            return SpriteDic[key];
        }

        #endregion







        public IEnumerator InitCoroutine()
        {
            if (isInit)
            {
                yield break;
            }

            Debug.Log("=====Database Init=====");

            List<IEnumerator> workList = new List<IEnumerator>();

            // Sprite Dicionary
            workList.Add(SetSpriteDictionary());

            // Stage Dicionary
            workList.Add(SetStageDictionary());

            // Dialogue Dicionary
            workList.Add(SetDialogueDictionary());



            int workCount = workList.Count;

            #region CSV Load

            for (int i = 0; i < workList.Count; i++)
            {
                // 실행시간 측정
                // System.Diagnostics.Stopwatch watch = new System.Diagnostics.Stopwatch();
                // watch.Start();
                yield return StartCoroutine(workList[i]);
                // watch.Stop();
                // UnityEngine.Debug.LogError(watch.ElapsedMilliseconds + " ms");            
            }

            Debug.Log("=====Database Init End=====");

            #endregion

            isInit = true;

            yield return null;
        }




        private IEnumerator SetSpriteDictionary()
        {
            if(SpriteDic == null)
            {
                Debug.Log("SpriteDic");
                SpriteDic = new Dictionary<string, Sprite>();


                var load = Resources.LoadAll<Sprite>("Img");
                foreach (var spr in load)
                {
                    if (SpriteDic.ContainsKey(spr.name))
                    {
                        Debug.LogErrorFormat("SpriteDic Duplication : {0}", spr.name);
                        continue;
                    }

                    SpriteDic.Add(spr.name, spr);
                }
            }
           

            yield return null;
        }




        private IEnumerator SetStageDictionary()
        {
            if (StageDic == null)
            {
                Debug.Log("StageDic");
                StageDic = new Dictionary<int, StructStage>();

                var csv = Resources.Load<TextAsset>("CSV/StageTable");
                List<Dictionary<string, object>> table = new List<Dictionary<string, object>>();
                
                table = CSVReader.Read(csv);


                // Struct Dialogue for every index
                for (int j = 0; j < table.Count; j++)
                {
                    if (string.IsNullOrEmpty(table[j]["Key"].ToString()))
                    {
                        continue;
                    }

                    StructStage ss = new StructStage();
                    int.TryParse(table[j]["Key"].ToString(), out ss.Key);
                    int.TryParse(table[j]["Timer"].ToString(), out ss.Timer);

                    var split = table[j]["Dialogue_Init"].ToString().Split('_');                    
                    ss.Dialogue_Init = int.Parse(split[split.Length - 1]);

                    split = table[j]["Dialogue_AfterCamera"].ToString().Split('_');
                    ss.Dialogue_AfterCamera = int.Parse(split[split.Length - 1]);

                    split = table[j]["Dialogue_Clear"].ToString().Split('_');
                    ss.Dialogue_Clear = int.Parse(split[split.Length - 1]);

                    split = table[j]["Dialogue_Fail"].ToString().Split('_');
                    ss.Dialogue_Fail = int.Parse(split[split.Length - 1]);
                    
                    if (StageDic.ContainsKey(ss.Key))
                    {
                        Debug.LogErrorFormat("stage key duplicatoin : {0}", ss.Key);
                    }

                    StageDic.Add(ss.Key, ss);
                }
            }

            yield return null;
        }




        private IEnumerator SetDialogueDictionary()
        {
            if (DialogueDic == null)
            {
                Debug.Log("DialogueDic");
                DialogueDic = new Dictionary<int, List<StructDialogue>>();

                var csvs = Resources.LoadAll<TextAsset>("Dialogue");
                List<Dictionary<string, object>> table = new List<Dictionary<string, object>>();


                foreach (var result in csvs)
                {
                    var split = result.name.Split('_');
                    int dialogueIndex = int.Parse(split[split.Length-1]);
                    table = CSVReader.Read(result);

                    List<StructDialogue> list = new List<StructDialogue>();

                    // Struct Dialogue for every index
                    for (int j = 0; j < table.Count; j++)
                    {
                        if (string.IsNullOrEmpty(table[j]["Index"].ToString()))
                        {
                            continue;
                        }

                        StructDialogue sd = new StructDialogue();
                        int.TryParse(table[j]["Index"].ToString(), out sd.Index);
                        Enum.TryParse(table[j]["DialoguePosition"].ToString(), out sd.DialoguePosition);                        
                        sd.Name_Cha = table[j]["Name_Cha"].ToString();
                        sd.Sprite = table[j]["Sprite"].ToString();
                        sd.Content = table[j]["Content"].ToString();

                        list.Add(sd);
                    }

                    if (DialogueDic.ContainsKey(dialogueIndex))
                    {
                        Debug.LogErrorFormat("dialogueIndex duplicatoin : {0}", dialogueIndex);
                        continue;
                    }

                    DialogueDic.Add(dialogueIndex, list);
                }
            }

            yield return null;
        }




    }

}

