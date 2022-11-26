using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;

namespace DUSDJ
{
    [Serializable]
    public class PlayerData
    {

        public List<int> ClearStage;


        public PlayerData()
        {
            ClearStage = new List<int>();
        }


        public void SetStageClear(int key)
        {
            if (ClearStage.Contains(key))
            {
                return;
            }

            ClearStage.Add(key);
        }

        public bool CheckStageClear(int key)
        {
            if (ClearStage.Contains(key))
            {
                return true;
            }

            return false;
        }

        public int GetLastStage()
        {
            // DB의 스테이지 데이터 순회해서 다음 인덱스를 찾는다.
            var dic = Database.Instance.StageDic;


            // 세이브데이터 없으면 인덱스 첫번째 값            
            if (ClearStage == null || ClearStage.Count == 0)
            {
                Debug.Log("세이브데이터 없음 : 첫번째 스테이지");
                return dic.First().Key;
            }

            foreach (var key in dic.Keys)
            {
                if (ClearStage.Contains(key))
                {
                    continue;
                }

                Debug.LogFormat("다음 스테이지 : {0}", key);
                return key;
            }

            // 다음 인덱스 없으면 마지막 스테이지로
            Debug.Log("다음 스테이지 없음");
            return dic.Last().Key;
        }
    }

}

