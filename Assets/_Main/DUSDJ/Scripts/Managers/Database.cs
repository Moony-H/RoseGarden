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

