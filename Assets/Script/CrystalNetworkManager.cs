using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using System;

public class CrystalNetworkManager :  NetworkManager
{
    private Role chooseRole;

    public override void OnStartServer()
    {
        base.OnStartServer();
        NetworkServer.RegisterHandler<CharacterCreatorMessage>(OnCreateCharacter);
    }

    public override void OnClientConnect(NetworkConnection conn)
    {
        //send the message here
        //the message should be defined above this class in a NetworkMessage
        CharacterCreatorMessage characterMessage = new CharacterCreatorMessage
        {
            //Character info here
            role = chooseRole
        };
        NetworkClient.Send(characterMessage);

        base.OnClientConnect(conn);
    }

    public override void OnServerAddPlayer(NetworkConnectionToClient conn)
    {
        base.OnServerAddPlayer(conn);
    }

    void OnCreateCharacter(NetworkConnectionToClient conn, CharacterCreatorMessage message)
    {
        GameObject gameObject=null;
        switch(message.role) {
            case Role.Police:
                gameObject = Instantiate(NetworkManager.singleton.spawnPrefabs[0]);
                gameObject.transform.position = new Vector3(-299.06f,69.96f,52.43f);
                break;
            case Role.Engineer:
                gameObject = Instantiate(NetworkManager.singleton.spawnPrefabs[1]);
                gameObject.transform.position = new Vector3(-364.31f,71.05f,54.8f);
                break;
            case Role.Drector:
                gameObject = Instantiate(NetworkManager.singleton.spawnPrefabs[2]);
                break;
        }
        NetworkServer.AddPlayerForConnection(conn, gameObject);
    }

    void OnGUI() {
      if(PlayerControl.localPlayerTransform == null){
        if (GUILayout.Button("startServer"))
        {
            this.StartServer();
        }
        if (GUILayout.Button("警察"))
        {
            chooseRole = Role.Police;
            this.StartClient();
        }
        if (GUILayout.Button("工程师"))
        {
            chooseRole = Role.Engineer;
            this.StartClient();
        }
        if (GUILayout.Button("导演"))
        {
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
