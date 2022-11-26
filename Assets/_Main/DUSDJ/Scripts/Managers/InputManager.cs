using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DUSDJ
{
    public class InputManager : MonoBehaviour
    {
        #region SingleTon

        /* SingleTon */
        private static InputManager instance;
        public static InputManager Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = GameObject.FindObjectOfType(typeof(InputManager)) as InputManager;
                    if (!instance)
                    {
                        Debug.LogWarning("InputManager Create");

                        var load = Resources.Load<InputManager>("Managers/InputManager");
                        instance = Instantiate(load);
                    }
                }

                return instance;
            }
        }

        #endregion

        public bool UseMobile = false;

        public static bool InputTrigger;

        private bool isInit = false;

        public VariableJoystick Stick;

        private CopyP player;


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

            Debug.Log("=====InputManager Init=====");

            if(Stick == null)
            {
                Stick = FindObjectOfType<VariableJoystick>(true);
                Stick.OnPointerUp(null);
            }

            player = FindObjectOfType<CopyP>();
            player.Init();

            // Init End!
            isInit = true;
        }


        #endregion



        #region Set & Clean Joystick

        public void SetJoystick()
        {
            InputTrigger = true;

            UIManager.Instance.UIJoystick.SetUI(true);
        }

        public void CleanJoystick()
        {
            ResetJoyStick();

            InputTrigger = false;

            UIManager.Instance.UIJoystick.SetUI(false);
        }

        public void ResetJoyStick()
        {
            Stick.OnPointerUp(null);
        }

        #endregion



        #region Update & Inputs

        public static bool btnTrigger = false;


        public static Vector2 GetStickVector()
        {
            if(Instance == null)
            {
                Debug.LogError("InputManager Error");
                return Vector2.zero;
            }

            return new Vector2(Instance.Stick.Horizontal, Instance.Stick.Vertical);
        }


        public void OnClick()
        {
            btnTrigger = true;
        }


        #endregion


    }

}

