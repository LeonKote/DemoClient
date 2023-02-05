using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using UnityEngine;
using UnityEngine.UI;

public class LocalClient : MonoBehaviour
{
	[DllImport("__Internal")]
	private static extern void StartClient();

	[DllImport("__Internal")]
	private static extern void Send(string message);

	[DllImport("__Internal")]
	private static extern void Log(string message);

	private GameObject loginForm;
	private GameObject lobbyForm;
	private GameObject roomForm;
	private Button loginButton;
	private Room room;

	// Start is called before the first frame update
	void Start()
	{
		loginForm = transform.GetChild(0).gameObject;
		lobbyForm = transform.GetChild(1).gameObject;
		roomForm = transform.GetChild(2).gameObject;

		loginForm.GetComponent<Login>().Init();
		lobbyForm.GetComponent<Lobby>().Init();
		roomForm.GetComponent<Room>().Init();

		loginButton = loginForm.transform.GetChild(1).GetComponent<Button>();
		room = roomForm.GetComponent<Room>();

		StartClient();
	}

	public void OnResponse(string message)
	{
		Log(message);

		JObject response = JObject.Parse(message);

		switch (response.Properties().First().Name)
		{
			case "websocket":
				if ((string)response["websocket"] == "OK")
					loginButton.interactable = true;
				break;
			case "authResult":
				if ((string)response["authResult"] == "OK")
				{
					loginForm.SetActive(false);
					lobbyForm.SetActive(true);
				}
				break;
			case "roomJoin":
				lobbyForm.SetActive(false);
				roomForm.SetActive(true);
				room.SetRoomCode((int)response["roomJoin"]["code"]);
				room.SetClients(JsonConvert.DeserializeObject<Client[]>(response["roomJoin"]["clients"].ToString()));
				break;
			case "clientJoin":
				room.AddClient(JsonConvert.DeserializeObject<Client>(response["clientJoin"].ToString()));
				break;
			case "clientLeave":
				room.RemoveClient(JsonConvert.DeserializeObject<Client>(response["clientLeave"].ToString()));
				break;
			case "clientMessage":
				room.ClientMessage((int)response["clientMessage"]["id"], (string)response["clientMessage"]["text"]);
				break;
		}
	}

	public static void SendRequest(string key, string value)
	{
		JObject request = new JObject();
		request[key] = value;
		Send(request.ToString());
	}

	public static void SendRequest(string key, int value)
	{
		JObject request = new JObject();
		request[key] = value;
		Send(request.ToString());
	}
}
