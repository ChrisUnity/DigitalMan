using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MyFramework;
using ShowNow;

public delegate void CallCompleteHandler();
public class ProjectEntry : ScriptSingleton <ProjectEntry>
{
    public CallCompleteHandler CallComplete;
    // Start is called before the first frame update
    void Start()
    {
        CallComplete += call;
        //NetHelper.Instance.JoinRoom();
    }
    void ProjectStart()
    {

    }
    public CallKitExample CallKitExample;
    // Update is called once per frame
    void Update()
    {
        if (CallKitExample.Operations.Count > 0)
        {
            ResourceManager.Instance.VMP.SetActive(true);
            CallKitExample.Operations.Clear();

        }
    }
    void call()
    {
        //vmh.SetActive(true);
        Debug.Log("show vh");
        ResourceManager.Instance.VMP.transform.position = Vector3.zero;
    }

}
