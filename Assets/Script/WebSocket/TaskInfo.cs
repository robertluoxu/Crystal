using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class TaskInfo {
    /**
     * 流程引擎中的任务唯一标识符
     */
    public string taskId;
    /**
     * 业务中的任务id
     */
    public string bussinessTaskId;
    /**
     * 任务名称
     */
    public string taskName;

    /**
     * 任务描述
     */
    public string taskDescription;
    /**
     * 任务所属的阶段名
     */
    public string stageName;

    /**
     * 执行方标识
     */
    public string exectorId;

    /**
     * 接收方标识
     */
    public string receiveId;

    /**
     * 任务对应的动作的全名，如com.gsafety.action.workaction
     */
    public string actionClassName;
}