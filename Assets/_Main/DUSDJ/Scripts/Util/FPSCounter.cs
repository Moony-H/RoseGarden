using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System;
using TMPro;

namespace DUSDJ
{
	public class FPSCounter : MonoBehaviour
	{
		float deltaTime = 0.0f;

		public TextMeshProUGUI FPSText;

		public static FPSCounter Instance;

		private void Awake()
		{
			Instance = this;
		}


		void Update()
		{
			deltaTime += (Time.unscaledDeltaTime - deltaTime) * 0.1f;

			float msec = deltaTime * 1000.0f;
			float fps = 1.0f / deltaTime;

			if (FPSText != null)
            {
				FPSText.text = string.Format("{0:0.0} ms ({1:0.} fps)", msec, fps);
			}
		}

		public void BtnOnOff()
		{
			gameObject.SetActive(!gameObject.activeSelf);
		}

	}
}


