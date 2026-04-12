using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using static UnityEngine.EventSystems.EventTrigger;

public class BoxingGameManager : MonoBehaviour
{
    [SerializeField]
    private GameObject player;
    private PlayerBoxing playerBoxing;
    [SerializeField]
    private GameObject enemy;
    private EnemyBoxing enemyBoxing;
    [SerializeField]
    private GameObject finalPicture;  //拳击游戏结束画面物体
    [SerializeField]
    private Text finalText;  //拳击游戏结束结果文字
    [SerializeField]
    private bool isBoxingGameDialogFinished=false;
    [SerializeField]
    private bool isEnemyDead = false;


    private void Awake()
    {
        playerBoxing=player.GetComponent<PlayerBoxing>();
        enemyBoxing=enemy.GetComponent<EnemyBoxing>();
    }
    // Update is called once per frame
    void Update()
    {
        if (enemyBoxing.currentLife <= 0.0f && !isEnemyDead)
        {
            Destroy(enemy);
            isEnemyDead = true;
            finalText.text = "Victory !";
            finalPicture.SetActive(true);
            DialogueManager.Instance.StartDialogue("STAGE3_2", "MrCrocker");
            isBoxingGameDialogFinished = true;
        }
        else if (playerBoxing.currentLife<=0.0f)
        {
            Destroy(player);
            finalText.text = "Loss !";
            finalPicture.SetActive(true);
            SceneManager.LoadScene("BoxingScene");
        }
        else if (isBoxingGameDialogFinished && !DialogueManager.Instance.IsDialogueActive)
        {
            SceneManager.LoadScene("Credits Scene");
            return;
        }
    }
}
