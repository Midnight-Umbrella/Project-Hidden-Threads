using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;
using static UnityEngine.EventSystems.EventTrigger;

public class BoxingGameManager : MonoBehaviour
{
    [SerializeField] private GameObject player;
    private PlayerBoxing playerBoxing;
    [SerializeField] private GameObject enemy;
    private EnemyBoxing enemyBoxing;
    [SerializeField] private GameObject finalPicture;
    [SerializeField] private TMP_Text finalText;
    [SerializeField] private ToBecontinued toBeContinued; 
    [SerializeField] private bool isBoxingGameDialogFinished = false;
    [SerializeField] private bool isEnemyDead = false;
    [SerializeField] private GameObject toBeContinuedPanel;
    private bool isTransitioning = false;
    private void Awake()
    {
        playerBoxing = player.GetComponent<PlayerBoxing>();
        enemyBoxing = enemy.GetComponent<EnemyBoxing>();
    }

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
        else if (playerBoxing.currentLife <= 0.0f)
        {
            Destroy(player);
            finalText.text = "Loss !";
            finalPicture.SetActive(true);
            SceneManager.LoadScene("BoxingScene");
        }
        else if (isBoxingGameDialogFinished && (DialogueManager.Instance == null || !DialogueManager.Instance.IsDialogueActive) && !isTransitioning)
        {
            isTransitioning = true;
            StartCoroutine(EndGame());
        }
    }

    private IEnumerator EndGame()
    {
        toBeContinuedPanel.SetActive(true); 
        toBeContinued.Play();
        float waitTime = toBeContinued.delayBeforeStart
                       + (toBeContinued.fullText.Length * toBeContinued.typingSpeed)
                       + 1f;
        yield return new WaitForSeconds(waitTime);
        SceneManager.LoadScene("Credits Scene");
    }
}