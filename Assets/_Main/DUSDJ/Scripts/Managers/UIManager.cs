using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace DUSDJ
{

    public class UIManager : MonoBehaviour
    {
        #region SingleTon
        /* SingleTon */
        private static UIManager instance;
        public static UIManager Instance
        { 
            get
            {
                if (instance == null)
                {
                    instance = GameObject.FindObjectOfType(typeof(UIManager)) as UIManager;
                    if (!instance)
                    {
                        Debug.LogError("UIManager Create");

                        var load = Resources.Load<UIManager>("Managers/UIManager");
                        instance = Instantiate(load);
                    }
                }

                return instance;
            }
        }

        #endregion




        #region UIs

        [HideInInspector]
        public PanDialogue PanDialogue;



        #endregion




        private bool isInit = false;



        #region Awake & InitCoroutine

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

            Debug.Log("=====UIManager Init=====");


            if(PanDialogue == null)
            {
                PanDialogue = FindObjectOfType<PanDialogue>(true);
                PanDialogue.Init();
            }




            isInit = true;
        }



        #endregion


    }


}


