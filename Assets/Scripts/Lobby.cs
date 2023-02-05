using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Lobby : MonoBehaviour
{
	private InputField inputField;

	public void Init()
	{
		inputField = transform.GetChild(0).GetComponent<InputField>();
	}

	public void OnJoinRoom()
	{
		LocalClient.SendRequest("join", int.Parse(inputField.text));
	}

	public void OnCreateRoom()
	{
		LocalClient.SendRequest("create", null);
	}
}
