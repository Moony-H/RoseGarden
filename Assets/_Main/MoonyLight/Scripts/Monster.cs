using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Monster : MonoBehaviour
{
    private HpSystem hpSystem;
    private Player player;
    private GameObject npc;

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



    private void Awake()
    {
        hpSystem = GetComponent<HpSystem>();
        player = GameObject.Find("Player").GetComponent<Player>();
        npc = GameObject.Find("NPC");
        _knockBackSpeed = knockBackSpeed;
    }

    private void FixedUpdate()
    {
        if (hpSystem.isAlive) {
            if (!isKnockBack)
                transform.position = Vector3.MoveTowards(transform.position, npc.transform.position, speed);
            else
            {
                Debug.Log("knock back");

                //_knockBackSpeed = Mathf.Lerp(_knockBackSpeed, 0f, Time.deltaTime*knockBackDecrease);

                //transform.Translate(_knockBackSpeed * knockBackPoint);
            }
        }


        
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.transform.CompareTag("Weapon"))
        {
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
