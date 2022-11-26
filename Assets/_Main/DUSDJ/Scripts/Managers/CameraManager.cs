using Cinemachine;
using Sirenix.OdinInspector;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;


namespace DUSDJ
{
    public class CameraManager : MonoBehaviour
    {

        #region SingleTon
        /* SingleTon */
        private static CameraManager instance;
        public static CameraManager Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = GameObject.FindObjectOfType(typeof(CameraManager)) as CameraManager;
                    if (!instance)
                    {
                        Debug.LogError("CameraManager Create");

                        var load = Resources.Load<CameraManager>("Managers/CameraManager");
                        instance = Instantiate(load);
                    }
                }

                return instance;
            }
        }

        #endregion





        private CinemachineVirtualCamera mainCamera;
        public CinemachineVirtualCamera MainCamera
        {
            get
            {
                if(mainCamera == null)
                {
                    MainCamera = FindObjectOfType<CinemachineVirtualCamera>();
                }


                return mainCamera;
            }


            set
            {
                mainCamera = value;
            }
        }



        private bool isInit = false;





        #region Awake & InitCoroutine

        private void Awake()
        {
            if (Instance == this)
            {

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


            MainCamera = FindObjectOfType<CinemachineVirtualCamera>();


            isInit = true;


            yield return null;
        }



        #endregion




        [Button]
        public void SetCameraFollow(Transform target)
        {
            MainCamera.Follow = target;
        }

    }
}
