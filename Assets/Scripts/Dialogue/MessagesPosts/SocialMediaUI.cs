using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class SocialMediaUI : MonoBehaviour
{
    public PostLoader loader;
    public Transform contentParent;
    public GameObject postPrefab;

    void Start()
    {
        var posts = loader.LoadPosts();

        foreach (var post in posts)
        {
            GameObject obj = Instantiate(postPrefab, contentParent);

            obj.transform.Find("Username").GetComponent<TextMeshProUGUI>().text = post.user;

            obj.transform.Find("PostText").GetComponent<TextMeshProUGUI>().text = post.text;

            obj.transform.Find("Time").GetComponent<TextMeshProUGUI>().text = post.time;
        }
    }
}
