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
        transform.position = new Vector3(-299.06f,69.96f,52.43f);
        if (transform.tag == "Police") {
            mainCamera.SendMessage("OpenCameraDrop", false);
            mainCamera.SendMessage("SetTarget", transform);
        }
        else if(transform.tag == "Engineer"){
            transform.position = new Vector3(-364.31f,71.05f,54.8f);
            mainCamera.SendMessage("OpenCameraDrop", false);
            mainCamera.SendMessage("SetTarget", transform);
        }
        else {
            mainCamera.SendMessage("OpenCameraDrop", true);
        }
    }
}
