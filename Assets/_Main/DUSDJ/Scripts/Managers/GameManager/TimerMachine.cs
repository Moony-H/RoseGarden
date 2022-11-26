using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DUSDJ
{
    public class TimerMachine : MonoBehaviour
    {
        public int MaxTimer = 180;

        private float nowTimer = 0;
        public float NowTimer
        {
            get
            {
                return nowTimer;
            }
            set
            {
                if(value < 0)
                {
                    value = 0;
                }

                if(value > 180)
                {
                    value = MaxTimer;
                    TimerOver();
                }

                nowTimer = value;
                UIManager.Instance.UITimer.SetTimerBar(nowTimer / MaxTimer);
            }
        }

        public bool TimerPause = false;
        private IEnumerator routine;

        public IEnumerator SetTImerRoutine(int maxTimer = 180)
        {
            MaxTimer = maxTimer;
            NowTimer = 0;

            // UIManager.Instance.UITimer.SetUI(true);

            yield return UIManager.Instance.UITimer.SetUIWithTween();


            if (routine != null)
            {
                StopCoroutine(routine);
            }
            routine = TimerRoutine();
            StartCoroutine(routine);            
        }

        public void SetTimer(int maxTimer = 180)
        {
            MaxTimer = maxTimer;
            NowTimer = 0;

            UIManager.Instance.UITimer.SetUI(true);

            if (routine != null)
            {
                StopCoroutine(routine);
            }
            routine = TimerRoutine();
            StartCoroutine(routine);
        }


        public void CleanTimer()
        {
            MaxTimer = 180;
            NowTimer = 0;

            UIManager.Instance.UITimer.SetUI(false);
        }

        public IEnumerator TimerRoutine()
        {
            while (true)
            {
                if (TimerPause)
                {
                    yield return null;
                    continue;
                }


                NowTimer += Time.deltaTime;
                yield return null;
            }
        }


        public void TimerOver()
        {
            if (routine != null)
            {
                StopCoroutine(routine);
            }
            routine = null;

            // Max! 
            Debug.Log("Timer End!");
        }
    }

}

