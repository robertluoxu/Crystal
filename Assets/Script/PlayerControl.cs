using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControl : NetworkBehaviour 
{
    public static Transform localPlayerTransform;
    public static uint localPlayerNetId;
    private GameObject mainCamera;

    public override void OnStartLocalPlayer() {
        mainCamera = GameObject.Find("MainCamera");
        localPlayerTransform = transform;
        localPlayerNetId = netId;
        if (transform.tag == "Police") {
            mainCamera.SendMessage("OpenCameraDrop", false);
            mainCamera.SendMessage("SetTarget", transform);
        }
        else if(transform.tag == "Engineer"){
            mainCamera.SendMessage("OpenCameraDrop", false);
            mainCamera.SendMessage("SetTarget", transform);
        }
        else {
            mainCamera.SendMessage("OpenCameraDrop", true);
        }
    }
}
