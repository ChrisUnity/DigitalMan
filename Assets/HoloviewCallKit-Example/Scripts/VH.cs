using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ShowNow;

public class VH : MonoBehaviour
{
    public Animator a;
    public enum DigitalManState
    {
        walk,
        squat,
        stand,
        greet,
        idle
    }
    public Vector3 InitTransform;
    public Vector3 CurrentTransform;
    public Vector3 LastTransform;
    public DigitalManState State = DigitalManState.idle;

    #region 动作检测
    void MotionDetection()
    {
        CurrentTransform = ResourceManager.Instance.HololensCamera.transform.position;
        Debug.Log(LastTransform);
        Debug.Log(CurrentTransform);

        if (Vector2.Distance(new Vector2(CurrentTransform.x, CurrentTransform.z), new Vector2(LastTransform.x, LastTransform.z)) > 0.15f)
        {
            //正在行走
            DMWalk(CurrentTransform.x.ToString(), CurrentTransform.y.ToString(), CurrentTransform.z.ToString());
            NetHelper.Instance.SyncCMD("DMWalk", new string[] { CurrentTransform.x.ToString(), InitTransform.y.ToString(), CurrentTransform.z.ToString() });
        }
        else if ((LastTransform.y - CurrentTransform.y) > 0.15f)
        {
            //正在蹲下
            NetHelper.Instance.SyncCMD("DMSquat", null);
            DMSquat();

        }
        else if ((LastTransform.y - CurrentTransform.y) < -0.15f)
        {
            //正在站起来
            NetHelper.Instance.SyncCMD("DMStand", null);
        }
        else
        {
            //站立
            NetHelper.Instance.SyncCMD("DMIdle", null);
        }
        LastTransform= ResourceManager.Instance.HololensCamera.transform.position;
    }



    #endregion
    public void DMWalk(string x, string y, string z)
    {
        transform.position = new Vector3(float.Parse(x), float.Parse(y), float.Parse(z));
        if (State == DigitalManState.walk) return;
        State = DigitalManState.walk;
        a.SetTrigger("walk");
    }
    public void DMIdle()
    {
        if (State == DigitalManState.walk) return;
        State = DigitalManState.walk;
        a.SetTrigger("idle");
    }
    public void DMSquat()
    {
        if (State == DigitalManState.walk) return;
        State = DigitalManState.walk;
        a.SetTrigger("sit");
    }
    public void DMStand()
    {
        if (State == DigitalManState.walk) return;
        State = DigitalManState.walk;
        a.SetTrigger("sitstand");
    }

    private void OnEnable()
    {
        State = DigitalManState.idle;
        InitTransform = ResourceManager.Instance.HololensCamera.transform.position;
        CurrentTransform = InitTransform;
        LastTransform = InitTransform;
        InvokeRepeating("MotionDetection", 0, 1f);
    }
    // Start is called before the first frame update
    void Start()
    {
        NetHelper.Instance.RegistCmdHandler(this);
    }
    public float time=0;
    // Update is called once per frame
    void Update()
    {
        //time += Time.deltaTime;
        //if (time > 1)
        //{
        //    MotionDetection();
        //    time = 0;
        //}
    }
    private void OnDisable()
    {
        CancelInvoke("MotionDetection");

    }
}
