using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    // Start is called before the first frame update

    public Vector2 characterDir = Vector2.zero;
    public Vector2 characterMove = Vector2.zero;
    private Vector2 characterAxis = Vector2.zero;

    private float X = 0f;
    private float Y = 0f;

    [SerializeField]
    private float Speed = 1f;
    private float _speed;
    private bool isStopped = false;


    [SerializeField]
    private float attackSpeed = 7f;
    private float _attackSpeed = 0f;

    [SerializeField]
    private float attackRange = 0.5f;

    [SerializeField]
    private float attackTime = 0.4f;
    private bool isInputBlocked = false;

    private bool isAttack = false;
    private bool isAttackCollider = false;

    [SerializeField]
    float pushValue = 20f;

    void Start()
    {
        _speed = Speed;
    }

    // Update is called once per frame
    void Update()
    {

        Debug.DrawRay(transform.position, new Vector3(characterDir.x, characterDir.y, 0f), Color.red);

        if (isStopped)
            return;


        if (!isInputBlocked)
        {
            X = Input.GetAxisRaw("Horizontal");
            Y = Input.GetAxisRaw("Vertical");
            characterAxis = new Vector2(X, Y);
            characterDir = characterAxis.normalized;
            characterMove = characterDir * _speed;
        }


        if (isAttack)
        {
            _attackSpeed = Mathf.Lerp(_attackSpeed, 0f, Time.deltaTime * 4f / 7f * attackSpeed);
            characterMove = _attackSpeed * characterDir * _speed;
            Collider2D[] colls=null;
            if (isAttackCollider) {
                Vector2 attack = new Vector2(transform.position.x, transform.position.y);
                colls = Physics2D.OverlapCircleAll(attack+characterDir, attackRange);

                foreach (Collider2D coll in colls) {
                    if (coll.tag == "monster") {
                        Destroy(coll.gameObject);
                        
                    }

                
                }
            }
        }








        if (Input.GetKeyDown(KeyCode.Space))
        {
            Debug.Log("Character pushed");
            StartCoroutine(Attack());
        }
    }

    private void FixedUpdate()
    {

        transform.Translate(characterMove * Time.deltaTime);
    }

    private void OnDrawGizmos()
    {
        if (isAttackCollider) {
            Vector2 attack = new Vector2(transform.position.x, transform.position.y)+ characterDir;
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(new Vector3(attack.x, attack.y, 0), attackRange);

        }
    }


    public void StopCharacter()
    {
        isStopped = true;
        _speed = 0f;
    }

    public void RunCharacter()
    {
        isStopped = false;
        _speed = Speed;
    }

    public void BlockInput()
    {
        isInputBlocked = true;
    }

    public void UnblouckInput()
    {
        isInputBlocked = false;
    }


    IEnumerator Attack()
    {
        Debug.Log("Attack start");
        isAttack = true;

        BlockInput();
        _attackSpeed = attackSpeed;
        isAttackCollider = true;
        yield return new WaitForSeconds(attackTime *0.7f);
        isAttackCollider = false;
        yield return new WaitForSeconds(attackTime * 0.3f);
        isAttack = false;
        _attackSpeed = 0f;
        UnblouckInput();
    }
}
