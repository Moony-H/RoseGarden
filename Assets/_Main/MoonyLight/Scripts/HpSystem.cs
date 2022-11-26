using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HpSystem : MonoBehaviour
{
    // Start is called before the first frame update
    public bool isAlive = true;
    public int hp = 100;


    public int Hp
    {
        get
        {
            return hp;
        }
        set
        {
            if(value <=0)
            {
                value = 0;
                hp = value;

                die();
                return;
            }

            hp = value;

        }
    }

    [SerializeField]
    private float dyingTime = 0f;


    public void loseHp(int damage) {

        Hp -= damage;
    }

    public void increaseHp(int heal) {

        hp += heal;
    }

    public void die() {

        Destroy(gameObject, dyingTime);
        isAlive = true;
    }
}
