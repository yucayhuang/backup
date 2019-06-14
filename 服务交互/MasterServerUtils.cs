1. 服务端（创建服务）
	NetworkConnectionError state = Network.InitializeServer (playerCount, serverPort, useNAT);
	if(state == NetworkConnectionError.NoError) {
		MasterServer.ipAddress = customConfigIP;
		MasterServer.port = customConfigPort;
		MasterServer.RegisterHost(uniqueGameType, gameName, comment);
	}
	
2. 客户端
	Network.Connect (hostData);
	Network.Connect ( ip, port, password);












using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class MasterServerUtils  {

	public static string uniqueGameType = "__Unity_Master_Server_Lynda.com__";


	public static bool useCustomConfiguration = true;
	public static string customConfigIP = "127.0.0.1";
	public static int customConfigPort = 23466;

	private static void ConfigureMasterServer() {
		if(useCustomConfiguration) {
			MasterServer.ipAddress = customConfigIP;
			MasterServer.port = customConfigPort;
		}
	}

	public static void RequestHostList() {
		ConfigureMasterServer();
		MasterServer.RequestHostList(uniqueGameType);
	}

	public static List<HostData> ListHosts() {
		ConfigureMasterServer();
		HostData[] hostData = MasterServer.PollHostList();
		MasterServer.ClearHostList();
		return hostData.ToList();
	}

	public static void RegisterWithMasterServer(string gameName, string comment) {
		ConfigureMasterServer();
		MasterServer.RegisterHost(uniqueGameType, gameName, comment);
	}

	public static void OnFailedToConnectToMasterServer(NetworkConnectionError info) {
		Debug.Log("Unable to connect to master server! - " + info);
	}
}







//接下来，测试

using UnityEngine;
using System.Collections;


public class ServerHostAndJoin : MonoBehaviour {

	public int playerCount = 8;
	public int serverPort = 23467;
	bool useNAT = false;

	public bool lanOnly = true;
	string log = "";
	public bool displayLog = true;
	MasterServerInterface msInterface;

	public Vector2 position = new Vector2(0,0);
	public Vector2 logPosition = new Vector2(0,0);

	//Control the behavior of the script with this
	HostOrJoin hostOrJoin = HostOrJoin.NotSet;

	public enum HostOrJoin {
		Host,
		Join,
		NotSet
	}
	
	void TakeAction(HostOrJoin choice) {

		hostOrJoin = choice;

		//Start or join, not both.
		if(hostOrJoin == HostOrJoin.Host) {
			StartServer();
		} 
		
		if(hostOrJoin == HostOrJoin.Join) {
			//If we're joining, create a MasterServiceInterface to provide us with the interface for selecting a server
			msInterface = this.gameObject.AddComponent<MasterServerInterface>();
			//Provide a call back for when a server is selected.
			msInterface.OnServerSelected += HandleOnServerSelected;

		}
	}

	public void StartServer() {
		//Determine if we should use network address translation
		//Essentially, YES if server is behind a router
		//             NO is server has a public IP
		if (lanOnly == true)
			useNAT = false;
		else
			useNAT = !Network.HavePublicAddress();

		//Start a simple server and add it to the master server list.
		//This will allow other players to locate it.
		NetworkConnectionError state = Network.InitializeServer (playerCount, serverPort, useNAT);
		if(state == NetworkConnectionError.NoError) {
			MasterServerUtils.RegisterWithMasterServer("Michael's Game", "This is a comment about TestServer");

		} else {
			Log("Server: Couldn't initalize server! " + state);
		}
	}

	void HandleOnServerSelected (HostData selectedServer)
	{
		//When the "Connect" button is pressed in the MasterServerInterface behavior, this is called
		if(selectedServer != null) {
			//Join and hide the host list interface
			JoinServer(selectedServer);
		}
	}
	public void JoinServer(HostData hostData) {
		//Join a server given some HostData, this data is provied by the master server
		Log("Client: Attempting to join server");
		Network.Connect (hostData);
	}
	void OnPlayerConnected (NetworkPlayer player)
	{
		//Fired in every MonoBehaviour when a player connects to our server
		Log("Server: Player (" + player.ipAddress + ") connected!");
	}

	void OnServerInitialized() {
		//Fired in every MonoBehaviour when a server is started
		Log("Server: Server started.");
		if(msInterface != null)
			msInterface.enabled = false;
	}

	void OnPlayerDisconnected(NetworkPlayer player) {
		//Fired in every MonoBehaviour when a player disconnects from our server
		Log("Server: Player (" + player.ipAddress + ") disconnected!");
	}

	void OnConnectedToServer() {
		//Fired in every MonoBehaviour when a player connects to a server
		Log("Client: Connected to server!");
		if(msInterface != null)
			msInterface.enabled = false;
	}

	void OnDisconnectedFromServer(NetworkDisconnection info) {
		//Fired in every MonoBehaviour when a player disconnects from a server
		Log("Client: Disconnected from server: " + info);
		if(msInterface != null)
			msInterface.enabled = true;
	}

	private void Log(string message) {
		//Some simple logging on screen so we don't have to worry about the debug console.
		log += "\n" + message;
	}



	void OnGUI() {
		if(hostOrJoin == HostOrJoin.NotSet) {
			if(GUI.Button(new Rect(position.x, position.y, 220, 40), "Start Server")) {
				TakeAction(HostOrJoin.Host);
			}
			if(GUI.Button(new Rect(position.x, position.y + 45, 220, 40), "Join Server")) {
				TakeAction(HostOrJoin.Join);
			}
		}

		//Some simple logging on screen so we don't have to worry about the debug console.
		if(displayLog)
			GUI.TextArea(new Rect(logPosition.x, logPosition.y, 220, 300), log);
	}

}
