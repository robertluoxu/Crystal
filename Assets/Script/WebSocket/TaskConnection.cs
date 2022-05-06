using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NativeWebSocket;
using System;


public class TaskConnection
{
    private static TaskConnection Instance;
    private static readonly object locker = new object();

    public static TaskConnection GetInstance() {
        if (Instance == null)
            {
                lock (locker)
                {
                    // 如果类的实例不存在则创建，否则直接返回
                    if (Instance == null)
                    {
                        Instance = new TaskConnection();
                    }
                }
            }
        return Instance;
    }
    // Start is called before the first frame update
    WebSocket websocket;

    public void Update()
    {
        if (websocket != null) {
            #if !UNITY_WEBGL || UNITY_EDITOR
            websocket.DispatchMessageQueue();
            #endif
        }
    }


    public async void StartWebSocket(Action<string> action, string userid){
        try
        {
            websocket = new WebSocket("ws://172.18.2.243:8888/" + userid);
            websocket.OnOpen += () =>
            {
                Debug.Log("Connection open!");
            };
            websocket.OnError += (e) =>
            {
                Debug.Log("Error! " + e);
            };
            websocket.OnClose += (e) =>
            {
                Debug.Log("Connection closed!");
            };
            websocket.OnMessage += (bytes) =>
            {
                // getting the message as a string
                var message = System.Text.Encoding.UTF8.GetString(bytes);
                Debug.Log("OnMessage! " + message);
                action(message);
            };  
            // waiting for messages
            await websocket.Connect();
        }
        catch (System.Exception ex)
        {
             // TODO
             throw ex;
        }
    }

    /// <summary>
    /// 发送消息
    /// </summary>
    /// <param name="playerMessage">消息体</param>
    /// <returns></returns>
    public async void SendWebSocketMessage(PlayerMessage playerMessage)
    {
        if (websocket.State == WebSocketState.Open)
        {
            var jsonStr = JsonUtility.ToJson(playerMessage);
            await websocket.SendText(jsonStr);
        }
    }

    /// <summary>
    /// 临时处理，发送任务编号即可
    /// </summary>
    /// <param name="taskId"></param>
    /// <returns></returns>
    public async void SendWebSocketMessage(string taskId){
        if (websocket.State == WebSocketState.Open)
        {
            await websocket.SendText(taskId);
        }
    }

    /// <summary>
    /// 注册客户端
    /// </summary>
    /// <param name="connId">客户端连接id</param>
    /// <param name="role">客户端角色</param>
    /// <returns></returns>
    public async void OnRegisterClient(string connId, Role role){
        if (websocket !=null && websocket.State == WebSocketState.Open)
        {
            PlayerMessage playerMessage = new PlayerMessage{
                Player = connId,
                Role = role,
                OnlineStatus = OnlineStatus.Online
            };
            var json = JsonUtility.ToJson(playerMessage);
            await websocket.SendText(json);
        }
    }

    /// <summary>
    /// 注销客户端
    /// </summary>
    /// <param name="connId">客户端id</param>
    /// <returns></returns>
    public async void UnRegisterClient(string connId){
        if (websocket !=null && websocket.State == WebSocketState.Open)
        {
            PlayerMessage playerMessage = new PlayerMessage{
                Player = connId,
                OnlineStatus = OnlineStatus.Offline
            };
            var json = JsonUtility.ToJson(playerMessage);
            await websocket.SendText(json);
        }
    }

    /// <summary>
    /// 系统退出
    /// </summary>
    /// <returns></returns>
    public async void OnApplicationQuit()
    {
        if (websocket !=  null) {
            await websocket.Close();
        }
    }
}

public class PlayerMessage{
    public string Player;
    public Role Role;
    public OnlineStatus OnlineStatus;
}

public enum OnlineStatus{
    Offline,
    Online,
    Unknown
}
