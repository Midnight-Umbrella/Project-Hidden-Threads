using System.Collections.Generic;
using UnityEngine;

public class MessageLoader : MonoBehaviour
{
    public TextAsset csvFile;

    public List<Conversation> LoadConversations()
    {
        Dictionary<string, Conversation> convoDict = new Dictionary<string, Conversation>();

        string[] lines = csvFile.text.Split('\n');

        for (int i = 1; i < lines.Length; i++) // skip header
        {
            if (string.IsNullOrWhiteSpace(lines[i])) continue;

            string[] parts = lines[i].Split(',');

            string contact = parts[0].Trim();
            string sender = parts[1].Trim();
            string message = parts[2].Trim();
            string isLockedStr = parts[3].Trim();

            if (!convoDict.ContainsKey(contact))
            {
                bool isLocked = false;
                if (isLockedStr == "1")
                {
                    isLocked = true;
                }
                convoDict[contact] = new Conversation 
                { 
                    contact = contact,
                    isLocked = isLocked
                };
            }

            convoDict[contact].messages.Add(new Message
            {
                sender = sender,
                text = message
            });
        }

        return new List<Conversation>(convoDict.Values);
    }
}

[System.Serializable]
public class Message
{
    public string sender;
    public string text;
}

[System.Serializable]
public class Conversation
{
    public string contact;
    public List<Message> messages = new List<Message>();
    public bool isLocked;
}