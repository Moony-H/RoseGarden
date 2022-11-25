using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

namespace DUSDJ
{

    public class EffectManager : MonoBehaviour
    {
        #region SingleTon

        /* SingleTon */
        private static EffectManager instance;
        public static EffectManager Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = GameObject.FindObjectOfType(typeof(EffectManager)) as EffectManager;
                    if (!instance)
                    {
                        Debug.LogWarning("EffectManager Create");

                        var load = Resources.Load<EffectManager>("Managers/EffectManager");
                        instance = Instantiate(load);
                    }
                }

                return instance;
            }
        }

        #endregion

        public Dictionary<string, Effect> EffectDic;
        public Dictionary<string, List<Effect>> ListDic;

        private bool isInit = false;

        private CinemachineImpulseSource cinemachineImpulse;
        private float originSustainTime;

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


        public IEnumerator InitCoroutine()
        {
            if (isInit)
            {
                yield break;
            }

            Debug.Log("=====EffectManager Init=====");

            EffectDic = new Dictionary<string, Effect>();
            ListDic = new Dictionary<string, List<Effect>>();

            #region Resources Load

            Effect[] effects = Resources.LoadAll<Effect>("Effect");
            for (int i = 0; i < effects.Length; i++)
            {
                Debug.LogFormat("EffectDic Add : {0}", effects[i].name);
                EffectDic.Add(effects[i].name, effects[i]);
                ListDic.Add(effects[i].name, new List<Effect>());
            }

            #endregion


            // cinemachineImpulse
            cinemachineImpulse = GetComponent<CinemachineImpulseSource>();
            originSustainTime = cinemachineImpulse.m_ImpulseDefinition.m_TimeEnvelope.m_SustainTime;

            // Init End!
            isInit = true;
        }

        #endregion


        #region Pooling, SetEffect

        public void IncreasePool(string key, int num)
        {
            for (int i = 0; i < num; i++)
            {
                Effect e = Instantiate(EffectDic[key]);
                e.transform.SetParent(transform, false);
                ListDic[key].Add(e);
                e.gameObject.SetActive(false);
            }
        }

        public Effect SetEffect(string msg, Vector3 position)
        {
            if (EffectDic.ContainsKey(msg) == false)
            {
                Debug.LogWarning(string.Format("없는 이름으로 Effect 생성 : {0}", msg));
                return null;
            }

            while (true)
            {
                for (int i = 0; i < ListDic[msg].Count; i++)
                {
                    Effect e = ListDic[msg][i];

                    if (e.gameObject.activeSelf == false)
                    {
                        e.gameObject.SetActive(true);
                        e.SetEffect(position);

                        return e;
                    }

                }

                IncreasePool(msg, 6);
            }
        }

        public Effect SetEffect(string msg, Vector3 position, Quaternion rotation)
        {
            if (EffectDic.ContainsKey(msg) == false)
            {
                Debug.LogWarning(string.Format("없는 이름으로 Effect 생성 : {0}", msg));
                return null;
            }

            while (true)
            {
                for (int i = 0; i < ListDic[msg].Count; i++)
                {
                    Effect e = ListDic[msg][i];

                    if (e.gameObject.activeSelf == false)
                    {
                        e.gameObject.SetActive(true);
                        e.SetEffect(position, rotation);

                        return e;
                    }

                }

                IncreasePool(msg, 6);
            }
        }

        public Effect SetEffect(string msg, Vector3 position, Vector3 scale)
        {
            if (EffectDic.ContainsKey(msg) == false)
            {
                Debug.LogWarning(string.Format("없는 이름으로 Effect 생성 : {0}", msg));
                return null;
            }

            while (true)
            {
                for (int i = 0; i < ListDic[msg].Count; i++)
                {
                    Effect e = ListDic[msg][i];

                    if (e.gameObject.activeSelf == false)
                    {
                        e.gameObject.SetActive(true);
                        e.SetEffect(position, scale);

                        return e;
                    }

                }

                IncreasePool(msg, 6);
            }
        }

        #endregion



        #region Impulse

        public void SetImpulse()
        {

            cinemachineImpulse.m_ImpulseDefinition.m_TimeEnvelope.m_SustainTime = originSustainTime;
            cinemachineImpulse.GenerateImpulse();
        }

        public void SetImpulse(float duration)
        {
            cinemachineImpulse.m_ImpulseDefinition.m_TimeEnvelope.m_SustainTime = duration;
            cinemachineImpulse.GenerateImpulse();
        }

        #endregion


        #region Clean

        public void Clean()
        {
            foreach (var item in ListDic)
            {
                for (int i = 0; i < item.Value.Count; i++)
                {
                    item.Value[i].Clean();
                }

            }
        }

        #endregion

    }

}
