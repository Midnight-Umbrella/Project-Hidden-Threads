using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.UI;


public class PlayerBoxing : MonoBehaviour
{
    public float speed;  //     ƶ  ٶ 
    [SerializeField]
    private float maxLife;  //          ֵ
    public float currentLife;  //   ǵ ǰ    ֵ
    [SerializeField]
    private Animator ani;
    [SerializeField]
    private GameObject enemy;  //        
    private float boxingHitDistance=1.2f;  // ж    еľ   
    private float damage;  //  ҵĹ     
    [SerializeField]
    private Slider playerLifeSlider;  //         
    [Header("Audio")]
    [SerializeField] private AudioSource HarperAudioSource;
    [SerializeField] private AudioClip PunchHitClip;
    [SerializeField] private float HitVolume = 1f;
    [SerializeField] private AudioClip PunchMissClip;
    [SerializeField] private float MissVolume = 1f;
    [SerializeField] private GameObject punchPrompt;



    private void Awake()
    {
        currentLife = maxLife;
        damage = 10.0f;
    }
    void Update()
    {
        //AD       ƶ 
        if (Input.GetKey(KeyCode.A))
        {
            //     ƶ 
            transform.Translate(Vector3.left * speed * Time.deltaTime);
            //   ö   
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
            //     ƶ 
            transform.Translate(Vector3.right * speed * Time.deltaTime);
            //   ö   
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

        //J    ȭ
        if ((Input.GetMouseButtonDown(0) || Input.GetKeyDown(KeyCode.F)) && !ani.GetCurrentAnimatorStateInfo(0).IsName("PlayerBoxing"))
        {
            punchPrompt.SetActive(false);
            ani.SetTrigger("Boxing");
            //      ж    о    ڲ  ҳ     ȷ   ж     
            if (Vector3.Magnitude(enemy.transform.position - transform.position) < boxingHitDistance && (((enemy.transform.position.x - transform.position.x < 0) && ani.GetBool("isRight")) || ((enemy.transform.position.x - transform.position.x >= 0) && (!ani.GetBool("isRight")))))
            {
                enemy.GetComponent<EnemyBoxing>().currentLife -= damage;
                if (HarperAudioSource != null && AudioController.Instance != null)
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
        //    Ѫ  
        playerLifeSlider.value = currentLife / maxLife;
    }
}