using System;
using System.Collections;
using System.Text;
using TMPro;
using UnityEngine;

namespace DUSDJ
{
    public class UtilCoroutine
    {
        public static IEnumerator WaitAndAction(float time, Action action)
        {
            float t = 0;

            while (t < time)
            {
                t += Time.deltaTime;

                yield return null;
            }

            action();
        }


        public static IEnumerator TextPrint(float stepTime, string text, TextMeshProUGUI textBox, Action afterAction)
        {
            StringBuilder sb = new StringBuilder();

            int index = 0;
            int length = text.Length;

            float t = 0;
            while (index < length)
            {
                t = 0;

                while (t < stepTime)
                {
                    t += Time.deltaTime;
                    yield return null;
                }

                sb.Append(text[index]);
                index += 1;

                textBox.text = sb.ToString();
                yield return null;
            }

            afterAction?.Invoke();

        }
    }

}
