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
    private GameObject enemy;  //敌人对象
    private float boxingHitDistance=1.2f;  //打击距离
    private float damage;  //打击伤害
    [SerializeField]
    private Slider playerLifeSlider;  //玩家生命值滑块
    [Header("Audio")]
    [SerializeField] private AudioSource HarperAudioSource;
    [SerializeField] private AudioClip[] PunchHitClipS;
    [SerializeField] private float HitVolume = 1f;
    [SerializeField] private AudioClip PunchMissClip;
    [SerializeField] private float MissVolume = 1f;




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
            //向左移动
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
        if ((Input.GetMouseButtonDown(0) || Input.GetKeyDown(KeyCode.F)) && !ani.GetCurrentAnimatorStateInfo(0).IsName("PlayerBoxing"))
        {
            ani.SetTrigger("Boxing");
            //如果在判定击中距离内并且朝向正确则判定击中
            if (Vector3.Magnitude(enemy.transform.position - transform.position) < boxingHitDistance && (((enemy.transform.position.x - transform.position.x < 0) && ani.GetBool("isRight")) || ((enemy.transform.position.x - transform.position.x >= 0) && (!ani.GetBool("isRight")))))
            {
                enemy.GetComponent<EnemyBoxing>().currentLife -= damage;
                AudioClip PunchHitClip = GetRandomClip(PunchHitClipS);
                if (HarperAudioSource != null && AudioController.Instance != null && PunchHitClip != null)
                    AudioController.Instance.PlaySFXOnSource(HarperAudioSource, PunchHitClip, HitVolume);
                else if (AudioController.Instance != null)
                    AudioController.Instance.PlaySFXAtPosition(PunchMissClip, transform.position, MissVolume);
            }
            else
            {
                if (HarperAudioSource != null && AudioController.Instance != null)
                    AudioController.Instance.PlaySFXOnSource(HarperAudioSource, PunchMissClip, MissVolume);
                else if (AudioController.Instance != null)
                    AudioController.Instance.PlaySFXAtPosition(PunchMissClip, transform.position, MissVolume);
            }
        }
        //更新血条
        playerLifeSlider.value = currentLife / maxLife;
    }
    private AudioClip GetRandomClip(AudioClip[] clips)
    {
        if (clips == null || clips.Length == 0) return null;
        return clips[Random.Range(0, clips.Length)];
    }
}