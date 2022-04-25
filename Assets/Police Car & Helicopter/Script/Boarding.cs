using System.Collections;
using System.Collections.Generic;
using Mirror;
using UnityEngine;

public class Boarding : NetworkBehaviour
{
    private GameObject msgObject; 
    private GameObject helicopterObject; 
    private GameObject mainCamera; 
    private GameObject chanObject;
    private bool insideTheHelicopter = false;

    public GameObject cube;

    // Start is called before the first frame update
    void Start()
    {
        msgObject = GameObject.Find ("HelicopterMsg");
        helicopterObject = GameObject.Find ("Helicopter");
        mainCamera = GameObject.Find("MainCamera");
        chanObject = GameObject.Find("PersonChan");
        msgObject.GetComponent<Renderer>().enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (msgObject.GetComponent<Renderer>().enabled && Input.GetKeyDown(KeyCode.E))
        {
            CmdSyncPlayer(PlayerControl.localPlayerNetId,false);
            msgObject.GetComponent<Renderer>().enabled = false;
            helicopterObject.AddComponent<HelicopterMove>();
            mainCamera.SendMessage("SetTarget", helicopterObject.transform);
            insideTheHelicopter = true;
            PlayerControl.localPlayerTransform.gameObject.SetActive(false);
        }

        if (insideTheHelicopter && Input.GetKeyDown(KeyCode.Q))
        {
            PlayerControl.localPlayerTransform.gameObject.SetActive(true);
            Destroy(helicopterObject.GetComponent<HelicopterMove>());
            PlayerControl.localPlayerTransform.position = new Vector3(helicopterObject.transform.position.x + 3 ,helicopterObject.transform.position.y,
                helicopterObject.transform.position.z - 2);
            mainCamera.SendMessage("SetTarget", PlayerControl.localPlayerTransform);
            insideTheHelicopter = false;
            CmdSyncPlayer(PlayerControl.localPlayerNetId,true);
        }
        if (PlayerControl.localPlayerNetId != 0) {
            CmdSyncLocation(PlayerControl.localPlayerNetId,transform.position.x,transform.position.y,transform.position.z);
        }

         if (insideTheHelicopter && Input.GetKeyDown(KeyCode.T))
        {
            Instantiate(cube, transform.position, transform.rotation);//transform.position  cube 的位置， transform.rotation的角度
        }
    }

    void OnCollisionEnter(Collision other) {
        if (other.gameObject.name == "PersonChan(Clone)") {
            msgObject.GetComponent<Renderer>().enabled = true;
        }
    }

    void OnCollisionExit(Collision other) {
        msgObject.GetComponent<Renderer>().enabled = false;
    }

    [Command(requiresAuthority = false)]
    void CmdSyncLocation(uint objNetId,float x,float y,float z){
        RpcSyncLocation(objNetId,x,y,z);
    }

    [ClientRpc]
    void RpcSyncLocation(uint objNetId, float x,float y,float z) {
        if (!insideTheHelicopter) {
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
