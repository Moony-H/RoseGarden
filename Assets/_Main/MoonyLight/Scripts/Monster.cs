using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Monster : MonoBehaviour
{
    private HpSystem hpSystem;
    private Player player;

    [SerializeField]
    private float speed = 0.1f;

    [SerializeField]
    private float knockBackTime = 1f;

    public bool isKnockBack = false;

    private Coroutine knockBack = null;

    [SerializeField]
    private float knockBackSpeed = 1f;
    private float _knockBackSpeed;

    [SerializeField]
    private float knockBackDecrease = 2f;
    private Vector2 knockBackPoint=Vector2.zero;

    [SerializeField]
    private GameObject target;

    [SerializeField]
    private GameObject hitEffect;

    [SerializeField]
    private GameObject deathEffect;

    [SerializeField]
    private Animator hitAnim;

    private bool death = false;

    private int animType=0;

    private bool stopMove = false;

    [SerializeField]
    private float expTime = 5f;

    private void Awake()
    {
        hpSystem = GetComponent<HpSystem>();
        player = GameObject.Find("Player").GetComponent<Player>();
        if (target == null) {
            Debug.Log("find");
            target = GameObject.Find("NPC");
        }

            

        _knockBackSpeed = knockBackSpeed;

    }

    private void FixedUpdate()
    {
        if (hpSystem.isAlive)
        {
            float x3Diff = transform.position.x - target.transform.position.x;
            if (x3Diff > 0f)
            {
                //¿ÞÂÊ
                transform.rotation = Quaternion.Euler(0f, 0f, 0f);
            }
            else
            {

                transform.rotation = Quaternion.Euler(0f, -180f, 0f);
            }
            if (!isKnockBack) {
                transform.position = Vector3.MoveTowards(transform.position, target.transform.position, speed);
            }
                
        }
        else {

            Debug.LogError("death monster");
        }
//        else if(!death) {
//            Debug.Log("death");
//            death = true;
//            Instantiate(deathEffect, transform.position, Quaternion.Euler(Vector3.zero));
//        }
        if (hitAnim != null)
        {
            hitAnim.SetBool("isHit", isKnockBack);
            hitAnim.SetInteger("animType", animType);
            Debug.Log("type: " + animType);
        }


        
            


    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.transform.CompareTag("Weapon"))
        {

            DUSDJ.EffectManager.Instance.SetTextEffect("Hit_Mon_Dammage", transform.position, string.Format("{0}", Random.Range(193, 295)));

            DUSDJ.AudioManager.Instance.PlayOneShotGroup("gwvCB2D", 3, 0.05f);

            Vector3 hitPosition = transform.position;
            hitPosition.z += 1;
            Instantiate(hitEffect, hitPosition, Quaternion.Euler(Vector3.zero));


            hpSystem.loseHp(player.attackDamage);
            Debug.Log("attacked: " + player.attackDamage.ToString());
            if (knockBack == null)
                knockBack = StartCoroutine(KnockBack());
            else
            {
                StopCoroutine(knockBack);
                isKnockBack = false;
                knockBack = StartCoroutine(KnockBack());
            }
        }
        else if (collision.transform.CompareTag("Barrier")) {

            Debug.Log("hit barrier");
            stopMove = true;

            animType = 1;
            StartCoroutine(ExplosionTIme());
        }


    }
    IEnumerator KnockBack() {
        isKnockBack = true;
        yield return new WaitForSeconds(knockBackTime);
        isKnockBack = false;
        knockBack = null;
        knockBackPoint = Vector2.zero;

    }

    IEnumerator ExplosionTIme() {

        yield return new WaitForSeconds(expTime);
        animType = 2;
        yield return new WaitForSeconds(3f);
        Destroy(gameObject);
    }



    


}
