using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Holoview;
using Holoview.Chat;
using MyFramework;
using Holoview;

public class ChatView : ScriptSingleton<ChatView>
{
    User CurrentUser;
    public Dictionary<long, ChatItem> ChatItemList;
    public List<User> FriendList = new List<User>();
    public long FriendID= 1000171296750;
    private void Awake()
    {
        FrameworkEntry.Instance.GetManager<EventManager>().Subscribe(1, handlerEvent);
    }


    public void Login()
    {
        //ResourceManager.Instance.LoginPanel.SetActive(false);
        Debug.Log("login");
        ChatManager.Instance.Login();
        
    }
    public void CallUser()
    {
        Debug.Log("calluser");
        ChatManager.Instance.Call(ConversationType.P2P, FriendID, new List<long> { FriendID }, new List<long> { FriendID, 1000171296769 }, false, 1, 1, false, true);
    }
    public void CallGroup()
    {
        Debug.Log("callgroup");
        ChatManager.Instance.Call(ConversationType.GROUP, FriendID, new List<long> { FriendID }, new List<long> { FriendID , 1000171296769 }, true, 1, 2, false, true);
        
    }
    void handlerEvent(object sender, GlobalEventArgs e)
    {
        
    }
}
