using System.Collections.Generic;
using UnityEngine;

public class PostLoader : MonoBehaviour
{
    public TextAsset csvFile;

    public List<Post> LoadPosts()
    {
        List<Post> posts = new List<Post>();

        string[] lines = csvFile.text.Split('\n');

        for (int i = 1; i < lines.Length; i++) // skip header
        {
            if (string.IsNullOrWhiteSpace(lines[i])) continue;

            string[] parts = lines[i].Split(',');

            string user = parts[0].Trim();
            string post = parts[1].Trim();
            string time = parts[2].Trim();

            

            posts.Add(new Post
            {
                user = user,
                text = post,
                time = time
            });
        }

        return posts;
    }
}

[System.Serializable]
public class Post
{
    public string user;
    public string text;
    public string time;
}

