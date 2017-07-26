using UnityEngine;
using System.Collections;
using System.Linq;
using System;

public class MessagesInfo : ScriptableObject, ICsvConfigurable
{
	[Serializable]
	public class Message
	{
		public string id;
		public string text;
	}

	public Message[] messages;

	public Message GetMessage(string id)
	{
		return messages.FirstOrDefault(m => m.id == id);
	}

	public string GetText(string id)
	{
		return messages.Where(m => m.id == id).Select(m => m.text).DefaultIfEmpty(id).First();
	}

	public string GetRandom()
	{
		return messages.RandomElement().text;
	}

	public void Configure (csv.Values values)
	{
		var dict = values.Raw;

		messages = new Message[dict.Count];
		int i = 0;
		foreach (var item in dict) {
			messages[i] = new Message {
				id = item.Key,
				text = item.Value,
			};
			i++;
		}
	}
}

