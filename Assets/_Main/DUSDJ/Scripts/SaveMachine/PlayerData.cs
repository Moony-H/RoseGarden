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
            // DB�� �������� ������ ��ȸ�ؼ� ���� �ε����� ã�´�.
            var dic = Database.Instance.StageDic;


            // ���̺굥���� ������ �ε��� ù��° ��            
            if (ClearStage == null || ClearStage.Count == 0)
            {
                Debug.Log("���̺굥���� ���� : ù��° ��������");
                return dic.First().Key;
            }

            foreach (var key in dic.Keys)
            {
                if (ClearStage.Contains(key))
                {
                    continue;
                }

                Debug.LogFormat("���� �������� : {0}", key);
                return key;
            }

            // ���� �ε��� ������ ������ ����������
            Debug.Log("���� �������� ����");
            return dic.Last().Key;
        }
    }

}

