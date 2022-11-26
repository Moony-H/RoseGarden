using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;


namespace DUSDJ
{

    public class SaveMachine
    {
        const string loadData = "_PlayerData.json";

        public PlayerData SavePD;
        public PlayerData LoadPD;

        private PlayerDataManager PDM;

        private bool DataLoading;
        private bool SavePathForTest;


        public SaveMachine(PlayerDataManager pdm, bool savePathForTest)
        {
            PDM = pdm;
            SavePathForTest = savePathForTest;

            SaveDataInit();
        }

        /// <summary>
        /// ���̺굥���� �ʱⰪ Init
        /// </summary>
        public void SaveDataInit()
        {
            SavePD = new PlayerData();
            LoadPD = null;

            DataLoading = false;
        }


        #region Update Save Data

        private void UpdateSaveDataMethods(EnumSave es)
        {
            PlayerData pd = PDM.NowPD;

            switch (es)
            {
                case EnumSave.Clear:
                    // Ŭ���� ���� : �������� Ŭ���� ��
                    SavePD.ClearStage = pd.ClearStage;
                    break;
            }
        }

        public void UpdateSaveData(EnumSave es)
        {
            if (DataLoading)
            {
                return;
            }

            UpdateSaveDataMethods(es);

            SaveData();
        }

        /// <summary>
        /// ��ü ���̺�
        /// </summary>
        public void UpdateSaveDataAll()
        {
            if (DataLoading)
            {
                return;
            }

            foreach (EnumSave es in Enum.GetValues(typeof(EnumSave)))
            {
                UpdateSaveDataMethods(es);
            }

            SaveData();
        }

        #endregion




        #region To Json, From Json

        /// <summary>
        /// ��ο� JSON ���Ϸ� ����
        /// </summary>
        public void SaveData()
        {
            string jsonData = JsonUtility.ToJson(SavePD, true);
            // ��ȣȭ
            if (PDM.JSONCrypto)
            {
                jsonData = AESCryptoUtil.AESEncrypt128(jsonData);
            }

            // ���̺� ���
            string mobilePath = Path.Combine(Application.persistentDataPath, loadData); //"playerData.json"
            string deskTopPath = Path.Combine(Application.dataPath, loadData); //"playerData.json"

            string path = mobilePath;

            if (SavePathForTest)
            {
                path = deskTopPath;
            }
            File.WriteAllText(path, jsonData);
        }


        /// <summary>
        /// ��ο��� Json ���� �б�
        /// </summary>
        public bool LoadPlayerDataFromJson()
        {
            // ���̺� ���
            string mobilePath = Path.Combine(Application.persistentDataPath, loadData);
            string deskTopPath = Path.Combine(Application.dataPath, loadData);

            string path = mobilePath;

            if (SavePathForTest)
            {
                path = deskTopPath;
            }

            try
            {
                string jsonData = File.ReadAllText(path);

                // ��ȣȭ
                if (PDM.JSONCrypto)
                {
                    jsonData = AESCryptoUtil.AESDecrypt128(jsonData);
                }

                LoadPD = JsonUtility.FromJson<PlayerData>(jsonData);

                LoadDataApply();
                return true;
            }

            catch (FileNotFoundException)
            {
                Debug.Log("�ҷ��� ���� ����.");
                return false;
            }

            catch (Exception)
            {
                Debug.LogError("�� ���� ����µ���?");
                return false;
            }
        }

        #endregion



        #region Load Data Apply

        /// <summary>
        /// �ҷ��� ������ ����
        /// </summary>
        public void LoadDataApply()
        {
            DataLoading = true;

            PlayerData pd = PDM.NowPD;

            // Ŭ���� ����
            pd.ClearStage = LoadPD.ClearStage;

            /* ���� �� */
            DataLoading = false;

            UpdateSaveDataAll();
        }

        #endregion

    }



}

