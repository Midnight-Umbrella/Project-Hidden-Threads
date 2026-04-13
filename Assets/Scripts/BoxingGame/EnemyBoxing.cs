using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyBoxing : MonoBehaviour
{
    [SerializeField]
    private float maxLife;  //           ֵ
    public float currentLife;  //   ˵ĵ ǰ    ֵ
    public float speed;  //   ˵  ƶ  ٶ 
    [SerializeField]
    private Animator ani;  //   ˵Ķ      
    [SerializeField]
    private GameObject player;  //      ҵ     
    private float controlDistance=1.0f;  //Ŀ  ﵽ      ҵľ   
    private float boxingHitDistance = 1.2f;  // ж    еľ   
    private float minBoxingInterval = 1.0f;  //  ȭ    Сʱ    
    private float maxBoxingInterval = 4.0f;  //  ȭ     ʱ    
    private float damage;  //   ˵Ĺ     
    [SerializeField]
    private Slider enemyLifeSlider;  //          
    [Header("Audio")]
    [SerializeField] private AudioSource CrockerAudioSource;
    [SerializeField] private AudioClip PunchHitClip;
    [SerializeField] private float HitVolume = 1f;
    [SerializeField] private AudioClip PunchMissClip;
    [SerializeField] private float MissVolume = 1f;
    


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
        //                  ƶ 
        if (Vector3.Magnitude(player.transform.position - transform.position) > controlDistance)
        {
            // ж  ƶ     
            Vector3 dir = (player.transform.position.x - transform.position.x) >= 0 ? Vector3.right : Vector3.left;
            //ִ   ƶ 
            transform.Translate(dir * speed * Time.deltaTime);
            //   ö   
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
            //          
            enemyLifeSlider.value = currentLife / maxLife;
    }

    IEnumerator CheckAndBoxing()
    {
        while (true) {
            //       Ҿ      ж    о  뷶Χ     ȭ
            if (Vector3.Magnitude(player.transform.position-transform.position)<boxingHitDistance)
            {
                // ж   Է   
                Vector3 dir = (player.transform.position.x - transform.position.x) >= 0 ? Vector3.right : Vector3.left;
                //   ö       
                if (dir == Vector3.right && ani.GetBool("isRight"))
                {
                    ani.SetBool("isRight", false);
                }
                else if (dir == Vector3.left && (!ani.GetBool("isRight")))
                {
                    ani.SetBool("isRight", true);
                }
                //   ų ȭ    
                ani.SetTrigger("Boxing");
                //      ж    о    ڲ  ҳ     ȷ   ж     
                if (Vector3.Magnitude(transform.position - player.transform.position) < boxingHitDistance && (((player.transform.position.x - transform.position.x < 0) && ani.GetBool("isRight")) || ((player.transform.position.x - transform.position.x >= 0) && (!ani.GetBool("isRight")))))
                {
                    player.GetComponent<PlayerBoxing>().currentLife -= damage;
                    if (CrockerAudioSource != null && AudioController.Instance != null)
                        AudioController.Instance.PlaySFXOnSource(CrockerAudioSource, PunchHitClip, HitVolume);
                    else if (AudioController.Instance != null)
                        AudioController.Instance.PlaySFXAtPosition(PunchHitClip, transform.position, HitVolume);
                } else
                {
                    if (CrockerAudioSource != null && AudioController.Instance != null)
                        AudioController.Instance.PlaySFXOnSource(CrockerAudioSource, PunchMissClip, MissVolume);
                    else if (AudioController.Instance != null)
                        AudioController.Instance.PlaySFXAtPosition(PunchMissClip, transform.position, MissVolume);
                }           
            }
            yield return new WaitForSeconds(Random.Range(minBoxingInterval,maxBoxingInterval));
        }
    }
}