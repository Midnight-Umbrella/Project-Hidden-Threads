using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyBoxing : MonoBehaviour
{
    [SerializeField]
    private float maxLife;  //敌人最大生命值
    public float currentLife;  //敌人的当前生命值
    public float speed;  //敌人的移动速度
    [SerializeField]
    private Animator ani;  //敌人的动画组件
    [SerializeField]
    private GameObject player;  //主角玩家的物体
    private float controlDistance=1.0f;  //目标达到的与玩家的距离
    private float boxingHitDistance = 1.2f;  //判定击中的距离
    private float minBoxingInterval = 1.0f;  //出拳的最小时间间隔
    private float maxBoxingInterval = 4.0f;  //出拳的最大时间间隔
    private float damage;  //敌人的攻击力
    [SerializeField]
    private Slider enemyLifeSlider;  //敌人生命条
    


    void Awake()
    {
        currentLife = maxLife;
        damage = 10.0f;
    }
    void Start()
    {
        StartCoroutine(CheckAndBoxing());
    }
    void Update()
    {
        //如果距离过大则向玩家移动
        if (Vector3.Magnitude(player.transform.position - transform.position) > controlDistance)
        {
            //判断移动方向
            Vector3 dir = (player.transform.position.x - transform.position.x) >= 0 ? Vector3.right : Vector3.left;
            //执行移动
            transform.Translate(dir * speed * Time.deltaTime);
            //设置动画
            if (dir == Vector3.right)
            {
                if (ani.GetBool("isRight"))
                {
                    ani.SetBool("isRight", false);
                }
                    if (!ani.GetBool("isWalking"))
                {
                    ani.SetBool("isWalking", true);
                }
            }
            else if (dir == Vector3.left)
            {
                if (!ani.GetBool("isRight"))
                {
                    ani.SetBool("isRight", true);
                }
                    if (!ani.GetBool("isWalking"))
                {
                    ani.SetBool("isWalking", true);
                }
            }

        }
        else {
            ani.SetBool("isWalking",false);
        }
            //更新生命条
            enemyLifeSlider.value = currentLife / maxLife;
    }

    IEnumerator CheckAndBoxing()
    {
        while (true) {
            //如果与玩家距离在判定击中距离范围内则出拳
            if (Vector3.Magnitude(player.transform.position-transform.position)<boxingHitDistance)
            {
                //判断面对方向
                Vector3 dir = (player.transform.position.x - transform.position.x) >= 0 ? Vector3.right : Vector3.left;
                //设置动画方向
                if (dir == Vector3.right && ani.GetBool("isRight"))
                {
                    ani.SetBool("isRight", false);
                }
                else if (dir == Vector3.left && (!ani.GetBool("isRight")))
                {
                    ani.SetBool("isRight", true);
                }
                //播放出拳动画
                ani.SetTrigger("Boxing");
                //如果在判定击中距离内并且朝向正确则判定击中
                if (Vector3.Magnitude(transform.position - player.transform.position) < boxingHitDistance && (((player.transform.position.x - transform.position.x < 0) && ani.GetBool("isRight")) || ((player.transform.position.x - transform.position.x >= 0) && (!ani.GetBool("isRight")))))
                {
                    player.GetComponent<PlayerBoxing>().currentLife -= damage;
                }
            }
            yield return new WaitForSeconds(Random.Range(minBoxingInterval,maxBoxingInterval));
        }
    }
}
