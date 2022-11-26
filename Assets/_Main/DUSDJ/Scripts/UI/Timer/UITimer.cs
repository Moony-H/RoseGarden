using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;
using System.Collections;

namespace DUSDJ
{
    public class UITimer : MonoBehaviour
    {
        public Image Bar;


        public void Init()
        {
            // Tweeen Value Init
            InitTween();

            gameObject.SetActive(false);

            Bar.fillAmount = 0f;
        }


        public void SetUI(bool onOff)
        {
            if (onOff)
            {
                gameObject.SetActive(true);
            }
            else
            {
                gameObject.SetActive(false);
            }
        }

        public void SetTimerBar(float value)
        {
            Bar.fillAmount = value;
        }



        #region  UI Tween


        [Header("트윈커브")]
        public AnimationCurve tweenCurve;
        public float tweenTime = 0.4f;

        private float tweenOffSetY;
        private RectTransform rt;

        private void InitTween()
        {
            rt = GetComponent<RectTransform>();
            tweenOffSetY = 1 * rt.rect.height;
        }


        public IEnumerator SetUIWithTween()
        {
            gameObject.SetActive(true);

            var originY = rt.anchoredPosition.y;
            rt.anchoredPosition = new Vector2(rt.anchoredPosition.x, originY + tweenOffSetY);
            var tw = rt.DOAnchorPosY(originY, tweenTime).SetEase(tweenCurve);

            yield return tw.WaitForCompletion();
        }

        #endregion
    }
}
