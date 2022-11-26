using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DUSDJ;

public class Portal : MonoBehaviour
{


    private GameObject npc;

    [SerializeField]
    private GameObject monster;

    [SerializeField]
    private float createTime = 1f;


    private Coroutine createMonster=null;
    void Start()
    {
        npc = GameObject.Find("NPC");
    }
    [Button]
    public void Test()
    {
        Debug.LogError("Button~~");

        AudioManager.Instance.PlayOneShot("attack_01");

        EffectManager.Instance.SetEffect("Efx_Sample", transform.position);



    }

    IEnumerator CreateMonster() {
        while (true) {
            yield return new WaitForSeconds(createTime);
            Vector2 v2Position=new Vector2(transform.position.x, transform.position.y);
            Instantiate(monster, Random.insideUnitCircle+ v2Position, Quaternion.Euler(0f,0f,0f));

        }

    }

    public void stopCreate() {
        if (createMonster != null) {
            StopCoroutine(createMonster);
        }
    }

    public void startCreate() {

        if (createMonster != null)
        {
            StopCoroutine(createMonster);
            createMonster = StartCoroutine(CreateMonster());

        }
        else {
            createMonster = StartCoroutine(CreateMonster());
        }
    }


}
