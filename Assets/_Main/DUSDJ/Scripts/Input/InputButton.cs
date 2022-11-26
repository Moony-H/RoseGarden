using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace DUSDJ
{
    public class InputButton : MonoBehaviour
    {
        #region Btn & Long Click

        [Header("��Ŭ�� ��� �ð�")]
        public float LongClickUpdateTime = 0.75f;
        private WaitForSeconds enterTime = null;
        private WaitForSeconds updateTime = null;

        [HideInInspector] public bool IsClick = false;
        private bool routineRunned = false;
        private IEnumerator routine;

        private Vector2 beginDragPoint;

        public void BtnDown()
        {
            // ��Ŭ�� üũ ����
            IsClick = true;
            routineRunned = false;

            StopRoutine();
            routine = BtnUpdate();
            StartCoroutine(routine);

        }

        public void BtnUp()
        {
            if (!IsClick)
            {
                return;
            }

            CancelRoutine();

            /*
            // �չ�ư Update�� �������� OnClick
            if (routineRunned == false)
            {
                OnClick();
            }
            */
        }

        /*
         * 
        public void OnBeginDrag(BaseEventData data)
        {
            PointerEventData pointerData = data as PointerEventData;
            beginDragPoint = pointerData.position;
        }
        public void OnDrag(BaseEventData data)
        {
            PointerEventData pointerData = data as PointerEventData;

            if (!Vector3.Equals(beginDragPoint, pointerData.position))
            {
                CancelRoutine();
            }
        }
        
         */

        #region LongButton Routine

        private void CancelRoutine()
        {
            IsClick = false;
            StopRoutine();
        }

        private bool StopRoutine()
        {
            if (routine != null)
            {
                StopCoroutine(routine);
                routine = null;

                return true;
            }

            return false;
        }

        public IEnumerator BtnUpdate()
        {
            // �ϴ� ����
            OnClick();

            updateTime = new WaitForSeconds(LongClickUpdateTime);

            yield return updateTime;
            routineRunned = true;

            while (IsClick)
            {
                // �չ�ư���� ���� �Է�
                OnClick();

                yield return updateTime;
            }
        }

        #endregion


        #endregion


        public void OnClick()
        {
            InputManager.Instance.OnClick();
        }
    }

}

