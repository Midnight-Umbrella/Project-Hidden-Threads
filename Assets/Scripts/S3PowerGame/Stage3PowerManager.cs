using DG.Tweening;
using System.Collections;
using System.Security.Cryptography;
using UnityEngine;
using UnityEngine.SceneManagement;
using static UnityEngine.EventSystems.EventTrigger;

public class Stage3PowerManager : MonoBehaviour
{
    public static Stage3PowerManager Instance { get; private set; }

    [Header("References")]
    [SerializeField] private GameObject darkOverlay;
    [SerializeField] private GameObject miniGamePanel;
    [SerializeField] private CameraMovement cm;
    [SerializeField] private Transform chairTrans;
    [SerializeField] private GameObject mrCrocker;
    [SerializeField] private Transform mrCrockerTarget;

    [Header("Optional Lights")]
    [SerializeField] private GameObject[] bulbGlowObjects;

    [Header("State")]
    [SerializeField] private bool powerRestored = false;
    [SerializeField] private float lightTurnOnDelay = 0.15f;
    [SerializeField] private bool isStage3DialogFinished=false;

    public bool PowerRestored => powerRestored;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
    }

    private void Start()
    {
        if (miniGamePanel != null)
            miniGamePanel.SetActive(false);

        if (powerRestored)
        {
            if (darkOverlay != null)
                darkOverlay.SetActive(false);

            TurnOnAllLightsImmediate();
        }
        else
        {
            if (darkOverlay != null)
                darkOverlay.SetActive(true);

            TurnOffAllLights();
        }
    }
    void Update()
    {
        if (isStage3DialogFinished && !DialogueManager.Instance.IsDialogueActive)
        {
            SceneManager.LoadScene("BoxingScene");
            return;
        }
    }

    public void OpenMiniGame()
    {
        if (powerRestored) return;

        if (miniGamePanel != null)
            miniGamePanel.SetActive(true);

        Time.timeScale = 0f;
    }

    public void CloseMiniGame()
    {
        if (miniGamePanel != null)
            miniGamePanel.SetActive(false);

        Time.timeScale = 1f;
    }

    public void RestorePower()
    {
        if (powerRestored) return;

        powerRestored = true;
        CloseMiniGame();
        StartCoroutine(RestorePowerRoutine());
    }

    private IEnumerator RestorePowerRoutine()
    {
        if (darkOverlay != null)
            darkOverlay.SetActive(false);

        for (int i = 0; i < bulbGlowObjects.Length; i++)
        {
            if (bulbGlowObjects[i] != null)
                bulbGlowObjects[i].SetActive(true);

            yield return new WaitForSecondsRealtime(lightTurnOnDelay);
        }
        //摄像机移动到椅子
        Sequence tween = DOTween.Sequence();
        //先延迟一下
        tween.AppendInterval(2.0f);
        tween.AppendCallback(() => {
            //关闭摄像机对玩家的聚焦
            cm.focusing = false;  
        });
        //移动摄像机到椅子
        tween.Append(Camera.main.transform.DOMove(new Vector3(chairTrans.position.x,chairTrans.position.y,-10.0f),5.0f));
        //延迟一下
        tween.AppendInterval(1.0f);
        //MrCrocker走向目标点
        tween.AppendCallback(() => {
            mrCrocker.GetComponent<Animator>().SetBool("WalkLeft",true);
        });
        tween.Append(mrCrocker.transform.DOMove(mrCrockerTarget.position,5.0f));
        //到达目标点后停止
        tween.AppendCallback(() =>
        {
            mrCrocker.GetComponent<Animator>().SetBool("WalkLeft", false);
        });
        //开启对话
        tween.AppendCallback(() => {
            DialogueManager.Instance.StartDialogue("STAGE3_1", "MrCrocker");
            isStage3DialogFinished = true;
        });
        //tween.AppendCallback(() => {
        //    SceneManager.LoadScene("BoxingScene");
        //});
        
    }

    private void TurnOffAllLights()
    {
        for (int i = 0; i < bulbGlowObjects.Length; i++)
        {
            if (bulbGlowObjects[i] != null)
                bulbGlowObjects[i].SetActive(false);
        }
    }

    private void TurnOnAllLightsImmediate()
    {
        for (int i = 0; i < bulbGlowObjects.Length; i++)
        {
            if (bulbGlowObjects[i] != null)
                bulbGlowObjects[i].SetActive(true);
        }
    }
}