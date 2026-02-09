using UnityEngine;

public class ClueBootstrap : MonoBehaviour
{
    public static ClueJournal Journal { get; private set; }

    private void Awake()
    {
        if (Journal != null)
        {
            Destroy(gameObject);
            return;
        }

        Journal = new ClueJournal();
        DontDestroyOnLoad(gameObject);
    }
}
