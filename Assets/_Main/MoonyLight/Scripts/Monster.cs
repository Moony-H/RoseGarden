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
    private GameObject effect;


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
        if (hpSystem.isAlive) {
            float x3Diff = transform.position.x - target.transform.position.x;
            if (x3Diff > 0f)
            {
                //¿ÞÂÊ
                transform.rotation = Quaternion.Euler(0f, 0f, 0f);
            }
            else {

                transform.rotation = Quaternion.Euler(0f, -180f, 0f);            
            }
            if (!isKnockBack)
                transform.position = Vector3.MoveTowards(transform.position, target.transform.position, speed);
        }


        
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.transform.CompareTag("Weapon"))
        {

            DUSDJ.EffectManager.Instance.SetTextEffect("Hit_Mon_Dammage", transform.position, string.Format("{0}", UnityEngine.Random.Range(193, 295)));

            Vector3 hitPosition = transform.position;
            hitPosition.z += 1;
            Instantiate(effect, hitPosition, Quaternion.Euler(Vector3.zero));



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

    }
    IEnumerator KnockBack() {
        isKnockBack = true;
        yield return new WaitForSeconds(knockBackTime);
        isKnockBack = false;
        knockBack = null;
        knockBackPoint = Vector2.zero;

    }



    


}
