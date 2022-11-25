using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace DUSDJ
{
    public class PanDialogue : MonoBehaviour
    {

        public GameObject Dimded;

        [Serializable]
        public struct DialoguePositionBox
        {
            public GameObject Object;
            public TextMeshProUGUI NameText;
            public TextMeshProUGUI ContentText;
            public Image CharacterImage;
            
        }
        public DialoguePositionBox Box_Left;
        public DialoguePositionBox Box_Right;

        public void Init()
        {
            gameObject.SetActive(true);
            Dimded.SetActive(false);
            ClearBox(EnumDialoguePosition.Left);
            ClearBox(EnumDialoguePosition.Center);
            ClearBox(EnumDialoguePosition.Right);
        }


        public void SetUI(bool onOff)
        {
            if (onOff)
            {
                Dimded.SetActive(true);
            }
            else
            {
                Dimded.SetActive(false);
            }            
        }

        public DialoguePositionBox PrintDialogue(StructDialogue sd)
        {
            
            DialoguePositionBox box = Box_Left;

            switch (sd.DialoguePosition)
            {
                case EnumDialoguePosition.Left:
                    box = Box_Left;
                    break;


                case EnumDialoguePosition.Center:
                    break;


                case EnumDialoguePosition.Right:
                    box = Box_Right;
                    break;
            }

            box.Object.SetActive(true);
            box.CharacterImage.sprite = Database.Instance.GetSpriteFromImageDic(sd.Sprite);
            box.NameText.text = sd.Name_Cha;
            box.ContentText.text = string.Empty;

            return box;
        }


        public void ClearBox(EnumDialoguePosition dialoguePosition)
        {
            DialoguePositionBox box = Box_Left;

            switch (dialoguePosition)
            {
                case EnumDialoguePosition.Left:
                    box = Box_Left;
                    break;
                case EnumDialoguePosition.Center:
                    break;
                case EnumDialoguePosition.Right:
                    box = Box_Right;
                    break;
            }


            box.Object.SetActive(false);
            box.CharacterImage.sprite = null;
            box.NameText.text = string.Empty;
            box.ContentText.text = string.Empty;
        }
    }
}
