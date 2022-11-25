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

    private float velocity = 0f;

    [SerializeField]
    private float attackRange = 7f;
    private float _attackRange = 0f;



    private bool isInputBlocked = false;

    private bool isAttack = false;
    private bool attackAcc = false;
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

            _attackRange = Mathf.Lerp(_attackRange, 0f, Time.deltaTime * 4f);
            characterMove = _attackRange * characterDir * _speed;
            return;
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
        attackAcc = true;
        _attackRange = attackRange;
        yield return new WaitForSeconds(0.4f);
        isAttack = false;
        _attackRange = 0f;
        UnblouckInput();
    }
}
