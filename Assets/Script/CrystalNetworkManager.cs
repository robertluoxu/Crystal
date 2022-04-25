using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using System;

public class CrystalNetworkManager :  NetworkManager
{
    private Role chooseRole;

    public string address = "localhost";


    public override void OnStartServer()
    {
        base.OnStartServer();
        NetworkServer.RegisterHandler<CharacterCreatorMessage>(OnCreateCharacter);
    }

    public override void OnClientConnect(NetworkConnection conn)
    {
        base.OnClientConnect(conn);

        //send the message here
        //the message should be defined above this class in a NetworkMessage
        CharacterCreatorMessage characterMessage = new CharacterCreatorMessage
        {
            //Character info here
            role = chooseRole
        };
        NetworkClient.Send(characterMessage);
    }

    void OnCreateCharacter(NetworkConnection conn, CharacterCreatorMessage message)
    {
        GameObject gameObject=null;
        switch(message.role) {
            case Role.Police:
                gameObject = Instantiate(NetworkManager.singleton.spawnPrefabs[0]);
                break;
            case Role.Engineer:
                gameObject = Instantiate(NetworkManager.singleton.spawnPrefabs[1]);
                break;
            case Role.Drector:
                gameObject = Instantiate(NetworkManager.singleton.spawnPrefabs[2]);
                break;
        }
        NetworkServer.AddPlayerForConnection((NetworkConnectionToClient)conn, gameObject);
    }


    void OnGUI() {
      if(PlayerControl.localPlayerTransform == null){
        if (GUILayout.Button("startServer"))
        {
            this.StartServer();
        }
        if (GUILayout.Button("警察"))
        {
            this.networkAddress = this.address;
            chooseRole = Role.Police;
            this.StartClient();
        }
        if (GUILayout.Button("工程师"))
        {
            this.networkAddress = this.address;
            chooseRole = Role.Engineer;
            this.StartClient();
        }
        if (GUILayout.Button("导演"))
        {
            this.networkAddress = this.address;
            chooseRole = Role.Drector;
            this.StartClient();
        }
      }
      
      GUIStyle myStyle = new GUIStyle();
      myStyle.fontSize = 30;
      myStyle.normal.textColor = Color.red;
      
      if(chooseRole != Role.None) {
          GUI.Box (new Rect (Screen.height / 2 + 200, 0, 300, 100), "当前角色");
          GUI.Label(new Rect(Screen.height / 2 + 270, 50, 0, 20), chooseRole.ToString(), myStyle);
      }
  }
}



public struct CharacterCreatorMessage : NetworkMessage
{
    public Role role;
}

public enum Role
{
    None,
    Police,
    Engineer,
    Drector
}
