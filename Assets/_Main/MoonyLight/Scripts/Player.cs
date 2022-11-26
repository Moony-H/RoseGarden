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
    private bool isCanAttackCollider = false;
    private bool canNextAttack = false;
    private int attackType = 0;

    private Coroutine attackCoroutine = null;
    private Coroutine nextAttackCoroutine = null;

    [SerializeField]
    private float CanNextAttackDelay = 1f;

    [SerializeField]
    private Animator weaponeAnimator;

    [SerializeField]
    private GameObject weapone;

    void Start()
    {
        _speed = Speed;
    }

    // Update is called once per frame
    void Update()
    {

        Debug.DrawRay(transform.position, new Vector3(characterDir.x, characterDir.y, 0f), Color.red);

        weapone.transform.rotation =Quaternion.Euler( 0,0,Vector2.SignedAngle(Vector2.right, characterDir)+90f);

        

        if (isStopped)
            return;


        if (!isInputBlocked)
        {
            X = Input.GetAxisRaw("Horizontal");
            Y = Input.GetAxisRaw("Vertical");
            characterAxis = new Vector2(X, Y);
            if (characterAxis != Vector2.zero)
            {
                characterDir = characterAxis.normalized;
                characterMove = characterDir * _speed;

            }
            else {
                characterMove = Vector2.zero;
            }
                
            
        }


        if (isAttack)
        {
            _attackSpeed = Mathf.Lerp(_attackSpeed, 0f, Time.deltaTime * 4f / 7f * attackSpeed);
            characterMove = _attackSpeed * characterDir * _speed;
 //           Collider2D[] colls=null;
 //           if (isCanAttackCollider) {
 //               Vector2 attack = new Vector2(transform.position.x, transform.position.y);
 //               colls = Physics2D.OverlapCircleAll(attack+characterDir*attackRange, attackRange);
 //
 //               foreach (Collider2D coll in colls) {
 //                   if (coll.tag == "monster") {
 //                       Destroy(coll.gameObject);
 //                       
 //                   }
 //
 //               
 //               }
 //           }
        }







        if (!isAttack) {
            if (Input.GetKeyDown(KeyCode.Space)&&Vector2.zero!=characterDir)
            {
                if(attackCoroutine==null)
                    attackCoroutine=StartCoroutine(Attack());
            }
        }
        weaponeAnimator.SetInteger("attackType", attackType);
    }

    private void FixedUpdate()
    {

        transform.Translate(characterMove * Time.deltaTime);
        
    }

    private void OnDrawGizmos()
    {
        if (isCanAttackCollider) {
            Vector2 attack = new Vector2(transform.position.x, transform.position.y)+ characterDir* attackRange;
            if (isAttack) {
                if (attackType == 1)
                {
                    Gizmos.color = Color.yellow;
                }
                else if (attackType == 2)
                {
                    Gizmos.color = Color.red;
                }
                else if(attackType==3){

                    Gizmos.color = Color.green;
                }
            
            }
                
            
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
        //Debug.Log("attack");
        
        isAttack = true;

        attackType++;


        if (canNextAttack)
        {
            if (nextAttackCoroutine != null) {
                StopCoroutine(nextAttackCoroutine);
            }
            
        }
        BlockInput();
        _attackSpeed = attackSpeed;
        isCanAttackCollider = true;
        yield return new WaitForSeconds(attackTime *0.7f);
        isCanAttackCollider = false;
        
        
        
        
        

        if (attackType > 2)
        {
            yield return new WaitForSeconds(1f);
            canNextAttack = false;
            attackType = 0;

        }
        else {

            //ÈÄµô·¹ÀÌ
            yield return new WaitForSeconds(attackTime * 0.3f);
            if (nextAttackCoroutine == null) {
                nextAttackCoroutine = StartCoroutine(NextAttack());
            }
            else{
                StopCoroutine(nextAttackCoroutine);
                canNextAttack = false;
                nextAttackCoroutine=StartCoroutine(NextAttack());
            }

                
            
            
        }
        UnblouckInput();

        isAttack = false;
        _attackSpeed = 0f;
        attackCoroutine=null;
        //Debug.Log("attack end");
    }

    IEnumerator NextAttack() {
        canNextAttack = true;
        yield return new WaitForSeconds(CanNextAttackDelay);
        canNextAttack = false;
        nextAttackCoroutine = null;
        attackType = 0;
        
    }
}
