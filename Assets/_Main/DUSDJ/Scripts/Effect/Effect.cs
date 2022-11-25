using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace DUSDJ
{

    public class Effect : MonoBehaviour
    {
        public float Duration;  // ���ӽð�
        private float BaseDuration;

        public Transform FollowTarget;
        public Vector3 Adder;

        private SpriteRenderer spr;
        public Animator anim;
        private ParticleSystem ps;

        IEnumerator coroutine;

        private void Awake()
        {
            spr = GetComponent<SpriteRenderer>();
            anim = GetComponent<Animator>();
            ps = GetComponent<ParticleSystem>();

            BaseDuration = Duration;
        }

        public void SetEffect(Vector3 position)
        {
            Duration = BaseDuration;

            transform.position = position;

            Action();
        }

        public void SetEffect(Vector3 position, Vector3 scale)
        {
            transform.localScale = scale;

            SetEffect(position);
        }

        public void SetEffect(Vector3 position, Quaternion rotation)
        {
            transform.rotation = rotation;

            SetEffect(position);
        }

        public void SetEffect(Vector3 position, Vector3 scale, Quaternion rotation)
        {
            transform.rotation = rotation;

            SetEffect(position, scale);
        }

        public void Action()
        {
            if (coroutine != null)
            {
                StopCoroutine(coroutine);
            }

            if (ps != null)
            {
                coroutine = CheckIfAlive();
            }
            else
            {
                coroutine = UpdateCoroutine();
            }

            if (anim != null)
            {
                foreach (var item in anim.parameters)
                {
                    if (item.name == "Action")
                    {
                        anim.SetTrigger("Action");
                    }
                }
            }

            StartCoroutine(coroutine);
        }

        IEnumerator CheckIfAlive()
        {

            while (true && ps != null)
            {
                yield return new WaitForSeconds(0.5f);
                if (!ps.IsAlive(true))
                {
                    Clean();
                }
            }
        }

        IEnumerator UpdateCoroutine()
        {
            float t = 0;

            while (t < Duration)
            {
                if (FollowTarget != null)
                {
                    if (FollowTarget.gameObject.activeSelf == false)
                    {
                        Clean();
                        yield break;
                    }
                    else
                    {
                        transform.position = FollowTarget.position + Adder;
                    }
                }

                if (anim.updateMode == AnimatorUpdateMode.UnscaledTime)
                {
                    t += Time.unscaledDeltaTime;
                }
                else
                {
                    t += Time.deltaTime;
                }

                yield return null;
            }

            Clean();
        }

        public void Clean()
        {
            if (coroutine != null) StopCoroutine(coroutine);

            FollowTarget = null;
            Adder = Vector3.zero;

            gameObject.SetActive(false);
        }
    }

}