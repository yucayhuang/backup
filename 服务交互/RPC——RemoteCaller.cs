using UnityEngine;
using System.Collections;
using System;

[RequireComponent (typeof (NetworkView))]
public class RemoteCaller : MonoBehaviour {

	public bool displayLog = true;
	string log = "";
	public string myName = "byte";
	
	// Update is called once per frame
	void Update () {
		if(Input.GetKeyDown(KeyCode.Space)) {
			SendGreeting(myName);
		}
	}

	void SendGreeting(string myName) {
		networkView.RPC("PrintGreeting", RPCMode.Others, myName);
		Log("Greeting Sent");
	}

	[RPC]
	public void PrintGreeting(string name) {
		Log(name + " says: Hello!");
	}


	private void Log(string message) {
		//Some simple logging on screen so we don't have to worry about the debug console.
		log += "\n" + message;
	}
	
	void OnGUI() {
		//Some simple logging on screen so we don't have to worry about the debug console.
		if(displayLog)
			GUI.TextArea(new Rect(220, 0, 230, 600), log);
	}
}
