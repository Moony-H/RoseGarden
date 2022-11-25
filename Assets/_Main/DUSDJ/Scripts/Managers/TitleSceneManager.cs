using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace DUSDJ
{
    public class TitleSceneManager : MonoBehaviour
    {
        public static bool isLoaded = false;

        public GameObject ForceLoadingObject;



        private void Awake()
        {
            if (isLoaded)
            {
                return;
            }

            // Scene 0 = Title
            if(SceneManager.GetActiveScene().buildIndex == 0)
            {
                StartCoroutine(InitRoutine());                
            }
            // Else = Load And Init GameManager
            else
            {
                StartCoroutine(InitRoutine(false));
            }            
        }

        public IEnumerator InitRoutine(bool isTitle = true)
        {
            // GameScene : Loading Object Set Acitve
            if (!isTitle)
            {
                ForceLoadingObject.SetActive(true);
            }
            else
            {
                ForceLoadingObject.SetActive(false);
            }

            
            // Resource Managers
            yield return AudioManager.Instance.InitCoroutine();
            yield return EffectManager.Instance.InitCoroutine();

            yield return new WaitForSeconds(2.0f);

            // OptionManager
            OptionManager.Instance.Init();




            isLoaded = true;

            // GameScene : Init
            if (!isTitle)
            {
                ForceLoadingObject.SetActive(false);
                GameManager.Instance.Init();
            }
        }
    }
}
