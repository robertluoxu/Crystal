using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;



public class AutoNavigation : MonoBehaviour
{
    //定义接收导航网络组件
    private NavMeshAgent nav;
    //坐标点列表
    private List<Vector3> destpoints;
    //导航下一个坐标点
    private int nextindex;
    //离导航坐标点的距离 
    private float calcdist = 2f;
    private float dist = 0f;

    private bool startNav = false;

    void Start()
    {
        //将定义的路线加入到List列表中
        destpoints = new List<Vector3>();
        destpoints.Add(GameObject.Find("Street 4 Prefab (2)").transform.position);
        destpoints.Add(GameObject.Find("Street 4 Prefab (47)").transform.position);
        destpoints.Add(GameObject.Find("Street 4 Prefab (125)").transform.position);
        //获取当前车辆的NavMeshAgent
        nav = this.transform.GetComponent<NavMeshAgent>();
 
 
        //计算最近的点，获取下一点的序号
        Vector3 navpoint = this.transform.position;
        for (int i = 0; i < destpoints.Count; ++i)
        {
            //首先判断点在当前位置的前方还是后方，如果是后方不做计算
            Vector3 dir = destpoints[i] - navpoint;
            float dot = Vector3.Dot(transform.forward, dir);
            //判断点在前方时才计算最近的点的距离
            if (dot > 0)
            {
                float tmpdist = Vector3.Distance(destpoints[i], navpoint);
                if (dist == 0)
                {
                    dist = tmpdist;
                    nextindex = i;
                }
                else if (dist > tmpdist)
                {
                    dist = tmpdist;
                    nextindex = i;
                }
            }
        }
    }

    
    void Update()
    {
        if (this.startNav) {
            //判断距离是否在到达范围内，如果在走到一下个点
            if (Vector3.Distance(this.transform.position, destpoints[nextindex])< calcdist)
            {
                if (nextindex == destpoints.Count - 1)
                {
                    nextindex = 0;
                    this.nav.Stop();
                    this.startNav = false;
                }
                else
                {
                    nextindex++;
                }
            }
            nav.SetDestination(destpoints[nextindex]);
        }
    }

    void OnGUI() {
        if (PlayerControl.localPlayerTransform.tag == "Engineer") {
            GUILayout.BeginArea(new Rect(Screen.width - 560, 110, 250, 150));
            if(GUILayout.Button("开启导航")){
                this.startNav = true;
                this.nav.Resume();
            };
            if(GUILayout.Button("停止导航")){
                this.startNav = false;
                this.nav.Stop();
            };
            GUILayout.EndArea();
        }
    }
}
