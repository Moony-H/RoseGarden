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
        /// 세이브데이터 초기값 Init
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
                    // 클리어 정보 : 스테이지 클리어 등
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
        /// 전체 세이브
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
        /// 경로에 JSON 파일로 저장
        /// </summary>
        public void SaveData()
        {
            string jsonData = JsonUtility.ToJson(SavePD, true);
            // 암호화
            if (PDM.JSONCrypto)
            {
                jsonData = AESCryptoUtil.AESEncrypt128(jsonData);
            }

            // 세이브 경로
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
        /// 경로에서 Json 파일 읽기
        /// </summary>
        public bool LoadPlayerDataFromJson()
        {
            // 세이브 경로
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

                // 복호화
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
                Debug.Log("불러올 파일 없음.");
                return false;
            }

            catch (Exception)
            {
                Debug.LogError("뭐 문제 생겼는데요?");
                return false;
            }
        }

        #endregion



        #region Load Data Apply

        /// <summary>
        /// 불러온 데이터 적용
        /// </summary>
        public void LoadDataApply()
        {
            DataLoading = true;

            PlayerData pd = PDM.NowPD;

            // 클리어 정보
            pd.ClearStage = LoadPD.ClearStage;

            /* 적용 끝 */
            DataLoading = false;

            UpdateSaveDataAll();
        }

        #endregion

    }



}

