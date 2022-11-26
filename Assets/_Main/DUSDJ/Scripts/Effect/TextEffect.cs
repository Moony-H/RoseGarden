using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

namespace DUSDJ
{
    public class TextEffect : Effect
    {
        public TextMeshPro textBox;

        protected override void Awake()
        {
            base.Awake();


            textBox = GetComponentInChildren<TextMeshPro>();
        }

        public void SetEffect(Vector3 position, string text)
        {
            textBox.text = text;


            base.SetEffect(position);
        }

    }
}
