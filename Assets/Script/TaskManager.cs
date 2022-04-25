using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class TaskManager : NetworkBehaviour
{
    private List<TaskMessage> taskManager;

    private GUIStyle colorStyle1 = new GUIStyle();

    private GUIStyle colorStyle2 = new GUIStyle();

    // Start is called before the first frame update
    void Start()
    {
        taskManager = new List<TaskMessage>();
        colorStyle1.normal.textColor = Color.white;
        colorStyle2.normal.textColor = Color.white;
    }

    public void OnPublishPoliceTask() {
        TaskMessage message = new TaskMessage{taskId = System.Guid.NewGuid().ToString(),
                                                taskName="驾驶直升机去往xx地点投石",
                                                taskContent ="解锁直升机驾驶权限，驾驶直升机去往xx地点投石" ,
                                                taskState = TaskState.Standby };
        this.CmdAssignTasks("Police",message);
    }

    public void OnPublishEngineerTask() {
        TaskMessage message = new TaskMessage{taskId = System.Guid.NewGuid().ToString(),
                                            taskName="挖掘机自动导航到xx地点填坑",
                                            taskContent ="开启挖掘据自动导航到xx地点，到达指定位置后可以自由活动",
                                            taskState = TaskState.Standby };
        this.CmdAssignTasks("Engineer",message);
    }
    
    private void OnGUI() {
        if (PlayerControl.localPlayerTransform != null && PlayerControl.localPlayerTransform.tag == "Drector") {
            GUI.Box (new Rect (Screen.width - 260, 10, 250, 150), "任务");
            GUI.Label (new Rect (Screen.width - 245, 30, 250, 30), "警察任务-驾驶直升机去往xx地点投石", colorStyle1);
            GUI.Label (new Rect (Screen.width - 245, 70, 250, 30), "工程任务-挖掘机自动导航到xx地点填坑",colorStyle2);
            GUILayout.BeginArea(new Rect(Screen.width - 260, 110, 250, 150));
            if(GUILayout.Button("发送警察任务")){
                this.OnPublishPoliceTask();
            };
            if(GUILayout.Button("发送工程任务")){
                this.OnPublishEngineerTask();
            };
            GUILayout.EndArea();
        }
        
        if(this.taskManager.Count > 0){
            GUI.Box (new Rect (Screen.width - 260, 10, 250, 150), "任务");
            int index = 0;
            foreach (TaskMessage message in taskManager)
            {
                GUI.Label (new Rect (Screen.width - 245, 30+80*index, 250, 50), message.taskName+":"+message.taskContent);
                if (GUI.Button(new Rect(Screen.width - 245, 30+80*index+40, 250, 50), "完成任务")){
                    CmdFinishTasks(PlayerControl.localPlayerTransform.tag);
                }
                index++;
            }
        }
    }


    [Command(requiresAuthority = false)]
    void CmdAssignTasks(string target, TaskMessage message) {
        List<NetworkConnectionToClient> connectionToClients = new List<NetworkConnectionToClient>(NetworkServer.connections.Values);
        for (int i = 0; i < connectionToClients.Count; i++)
        {
            if (connectionToClients[i].identity != null && connectionToClients[i].identity.tag == target)
            {
                TargetPlayer(connectionToClients[i], message);
                this.taskManager.Add(message);
            }
        }
    }

    [TargetRpc]
    void TargetPlayer(Mirror.NetworkConnection target, TaskMessage message) {
        this.taskManager.Add(message);
    }

    [Command(requiresAuthority = false)]
    void CmdFinishTasks(string target){
        List<NetworkConnectionToClient> connectionToClients = new List<NetworkConnectionToClient>(NetworkServer.connections.Values);
        for (int i = 0; i < connectionToClients.Count; i++)
        {
            if (connectionToClients[i].identity != null && connectionToClients[i].identity.tag == "Drector")
            {
                TargetFinishTasks(connectionToClients[i], target);
            }
        }
    }

    /// <summary>
    /// 发送给导演
    /// </summary>
    /// <param name="target"></param>
    /// <param name="message"></param>
    [TargetRpc]
    void TargetFinishTasks(Mirror.NetworkConnection target, string tag){
        if(tag == "Police"){
            colorStyle1.normal.textColor = Color.green;
        }
        if (tag == "Engineer") {
            colorStyle2.normal.textColor = Color.green;
        }
    }

    
}

public class TaskMessage {
    public string taskId;

    public string taskName;

    public string taskContent;

    public TaskState taskState;
}

public enum TaskState{
    Standby,
    Processing,
    Finish,
}
