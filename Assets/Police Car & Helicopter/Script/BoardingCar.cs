using System.Collections;
using System.Collections.Generic;
using Mirror;
using UnityChan;
using UnityEngine;
using static PlayerControl;

public class BoardingCar : NetworkBehaviour
{
    private GameObject carMsgObject;
    private GameObject chanObject;
    private GameObject car;
    private bool insideTheCar = false;
    private GameObject mainCamera;

    // Start is called before the first frame update
    void Start()
    {
        carMsgObject = GameObject.Find ("CarMsg");
        chanObject = GameObject.Find ("PersonChan");
        car = GameObject.Find("PoliceCar");
        mainCamera = GameObject.Find("MainCamera");
        carMsgObject.GetComponent<Renderer>().enabled = false;
    }

    void Update() {
        if (carMsgObject.GetComponent<Renderer>().enabled && Input.GetKeyDown(KeyCode.E))
        {
            CmdSyncPlayer(PlayerControl.localPlayerNetId,false);
            car.AddComponent<TransportationMove>();
            mainCamera.SendMessage("SetTarget", car.transform);
            insideTheCar = true;
            PlayerControl.localPlayerTransform.gameObject.SetActive(false);
        }

        if (insideTheCar && Input.GetKeyDown(KeyCode.Q))
        {
            CmdSyncPlayer(PlayerControl.localPlayerNetId,true);
            PlayerControl.localPlayerTransform.gameObject.SetActive(true);
            Destroy(car.GetComponent<TransportationMove>());
            PlayerControl.localPlayerTransform.position = new Vector3(car.transform.position.x + 3 ,car.transform.position.y,car.transform.position.z - 2);
            mainCamera.SendMessage("SetTarget", PlayerControl.localPlayerTransform);
            insideTheCar = false;
        }

        if (PlayerControl.localPlayerNetId != 0){
            CmdSyncLocation(PlayerControl.localPlayerNetId,transform.position.x,transform.position.y,transform.position.z);
        }
    }

    void OnCollisionEnter(Collision other) {
        if (other.gameObject.name == "PersonChan(Clone)") {
            carMsgObject.GetComponent<Renderer>().enabled = true;
        }
    }

    void OnCollisionExit(Collision other) {
        carMsgObject.GetComponent<Renderer>().enabled = false;
    }
    
    [Command(requiresAuthority = false)]
    void CmdSyncLocation(uint objNetId,float x,float y,float z){
        RpcSyncLocation(objNetId,x,y,z);
    }

    [ClientRpc]
    void RpcSyncLocation(uint objNetId, float x,float y,float z) {
        if (!insideTheCar) {
            Vector3 vector3 = new Vector3(x,y,z);
            transform.position = vector3;
        }
    }

    [Command(requiresAuthority = false)]
    void CmdSyncPlayer(uint objNetId,bool active){
        RpcSyncPlayer(objNetId,active);
    }

    [ClientRpc]
    void RpcSyncPlayer(uint objNetId,bool active) {
        if (NetworkIdentity.spawned.TryGetValue(objNetId, out NetworkIdentity identity))
        {
            identity.gameObject.SetActive(active);
        }
    }
}
