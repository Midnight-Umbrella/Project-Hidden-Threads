using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.UI;


public class PlayerBoxing : MonoBehaviour
{
    public float speed;  //主角移动速度
    [SerializeField]
    private float maxLife;  //玩家最大生命值
    public float currentLife;  //主角当前生命值
    [SerializeField]
    private Animator ani;
    [SerializeField]
    private GameObject enemy;  //敌人物体
    private float boxingHitDistance=1.2f;  //判定击中的距离
    private float damage;  //玩家的攻击力
    [SerializeField]
    private Slider playerLifeSlider;  //玩家生命条


    private void Awake()
    {
        currentLife = maxLife;
        damage = 10.0f;
    }
    void Update()
    {
        //AD键左右移动
        if (Input.GetKey(KeyCode.A))
        {
            //向左移动
            transform.Translate(Vector3.left * speed * Time.deltaTime);
            //设置动画
            if (!ani.GetBool("isRight"))
            {
                ani.SetBool("isRight", true);
            }
            if (!ani.GetBool("isWalking"))
            {
                ani.SetBool("isWalking", true);
            }
        }
        else if (Input.GetKey(KeyCode.D))
        {
            //向右移动
            transform.Translate(Vector3.right * speed * Time.deltaTime);
            //设置动画
            if (ani.GetBool("isRight"))
            {
                ani.SetBool("isRight", false);
            }
            if (!ani.GetBool("isWalking"))
            {
                ani.SetBool("isWalking", true);
            }
        }
        else {
            ani.SetBool("isWalking",false);
        }

        //J键出拳
        if (Input.GetMouseButtonDown(0))
        {
            ani.SetTrigger("Boxing");
            //如果在判定击中距离内并且朝向正确则判定击中
            if (Vector3.Magnitude(enemy.transform.position - transform.position) < boxingHitDistance && (((enemy.transform.position.x - transform.position.x < 0) && ani.GetBool("isRight")) || ((enemy.transform.position.x - transform.position.x >= 0) && (!ani.GetBool("isRight")))))
            {
                enemy.GetComponent<EnemyBoxing>().currentLife -= damage;
            }
        }
        //更新血条
        playerLifeSlider.value = currentLife / maxLife;
    }
}