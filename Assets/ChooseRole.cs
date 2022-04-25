using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class ChooseRole : MonoBehaviour
{
    private NetworkManager networkManager;
    void Start() {
        networkManager = transform.GetComponent<NetworkManager>();
    }

    public void chooseRole(uint roleId) {
        switch(roleId) {
            case 0:
                
                break;
            case 1:

                break;
            case 2:

                break;
        }
    }

    private void choosePolice(){


        // networkManager.networkAddress = "localhost";
        // networkManager.StartClient();
    }
}
