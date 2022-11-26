using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [Header("¸Ê ÇÑ°è")]
    public float MaxY;
    public float MinY;

    public float MaxX;
    public float MinX;


    // Start is called before the first frame update

    public Vector2 characterDir = Vector2.right;
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
    private Animator weaponAnimator;

    [SerializeField]
    private GameObject Weapon;

    [SerializeField]
    private GameObject playerDirection;

    [SerializeField]
    private GameObject model;


    [SerializeField]
    public int[] comboAttackDamage = new int[3] { 1,2,3};

    [SerializeField]
    private Animator characterAnimator;

    public bool isRunning=false;
    public int attackDamage=0;




    public bool isInit = false;

    public void Init()
    {
        _speed = Speed;

        isInit = true;
    }


    private bool IsMapLimit(Vector2 dir)
    {
        if(dir.y > 0 && transform.position.y >= MaxY)
        {
            return true;
        }

        if (dir.y < 0 && transform.position.y <= MinY)
        {
            return true;
        }

        if (dir.x > 0 && transform.position.x >= MaxX)
        {
            return true;
        }

        if (dir.x < 0 && transform.position.x <= MinX)
        {
            return true;
        }


        return false;
    }

    // Update is called once per frame
    void Update()
    {


        if (!isInit)
        {
            return;
        }

        Debug.DrawRay(transform.position, new Vector3(characterDir.x, characterDir.y, 0f), Color.red);


        float weaponeRotationY = 0;
        if (characterDir.x < 0)
            weaponeRotationY = 180f;
        else if (characterDir.x > 0)
            weaponeRotationY = 0f;

        if (Weapon == null) {
            Debug.LogWarning("weapon is null");
        }
        Weapon.transform.rotation =Quaternion.Euler( 0, weaponeRotationY, Vector2.Angle(Vector2.down, characterDir));
        playerDirection.transform.rotation = Quaternion.Euler(58.353f,0f,Vector2.SignedAngle(Vector2.up,characterDir));;;




        if (isStopped)
            return;


        if (!isInputBlocked)
        {
            X = Input.GetAxisRaw("Horizontal");
            Y = Input.GetAxisRaw("Vertical");

            if (DUSDJ.InputManager.Instance.UseMobile)
            {
                characterAxis = DUSDJ.InputManager.GetStickVector();
            }
            else
            {
                characterAxis = new Vector2(X, Y);
            }

            if (characterAxis != Vector2.zero)
            {
                isRunning = true;
                characterDir = characterAxis.normalized;

                if (IsMapLimit(characterDir))
                {
                    characterMove = Vector2.zero;
                }
                else
                {
                    characterMove = characterDir * _speed;
                }
                
            }
            else {
                isRunning = false;
                characterMove = Vector2.zero;
            }
                
            
        }

        if (characterDir.x > 0)
        {
            model.transform.rotation = Quaternion.Euler(0, 0, 0);
        }
        else if (characterDir.x < 0) {

            model.transform.rotation = Quaternion.Euler(0, 180, 0);
        }


        if (isAttack)
        {
            _attackSpeed = Mathf.Lerp(_attackSpeed, 0f, Time.deltaTime * 4f / 7f * attackSpeed);
            characterMove = _attackSpeed * characterDir;
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




        if (!isAttack)
        {
            if ((Input.GetKeyDown(KeyCode.Space) || DUSDJ.InputManager.BtnTrigger))
            {
                if (Vector2.zero != characterDir)
                {
                    if (attackCoroutine == null)
                    {
                        attackCoroutine = StartCoroutine(Attack());
                    }
                }
            }
        }
        DUSDJ.InputManager.BtnTrigger = false;

        weaponAnimator.SetInteger("attackType", attackType);
        characterAnimator.SetInteger("attackType", attackType);
        characterAnimator.SetBool("isRunning", isRunning);
    }

    private void FixedUpdate()
    {
        transform.Translate(characterMove * Time.deltaTime);
        
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

        attackDamage=comboAttackDamage[attackType - 1];

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
            yield return new WaitForSeconds(0.4f);
            canNextAttack = false;
            attackType = 0;

        }
        else {

            //ÈÄµô·¹ÀÌ
            yield return new WaitForSeconds(attackTime * 0.15f);
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
