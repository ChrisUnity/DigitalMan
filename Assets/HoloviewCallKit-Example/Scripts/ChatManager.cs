using Holoview;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ShowNowSdk.model;
using MyFramework;
namespace Holoview.Chat{
    public class ChatManager : ScriptSingleton<ChatManager>
    {
        //public PluginState ChatState = 0;

        public ChatState ClientChatState= ChatState.UNINITED ;
        public PluginState ServerChatState = PluginState.UNINITED;
        public string CallID;
        public List<User> FriendList;
        public User CurrentUser;
        public string ConnectUserName;
        public List<long> InviteUserList;
        #region Unity
        private void Awake()
        {
            
        }
        // Start is called before the first frame update
        void Start()
        {
            init();
            Login();
        }

        // Update is called once per frame
        void Update()
        {

        }
        private void OnDestroy()
        {
            EndInit();
        }
        #endregion
        #region 视音频逻辑
        [HideInInspector]
        public string appkey= "000001";
        [HideInInspector]
        public string appsecret= "1111111";

        void init()
        {
           // FrameworkEntry.Instance.GetManager<EventManager>().Subscribe(1, ChatEventArg);
            HoloviewCallKit.instance().onInitResult += HLK_OnInitResult;
            HoloviewCallKit.instance().onStateChange += HLK_OnStateChange;
            HoloviewCallKit.instance().onGetTokenResult += HLK_OnGetTokenResult;
            //登陆结果
            HoloviewCallKit.instance().onConnectResult += HLK_OnConnectResult;
            HoloviewCallKit.instance().onLoginResult += HLK_onLoginResult;
            HoloviewCallKit.instance().onRecvTextMessage += HLK_OnRecvTextMessage;
            HoloviewCallKit.instance().onRecvImageMessage += HLK_OnRecvImageMessage;
            HoloviewCallKit.instance().onRecvVideoMessage += HLK_OnRecvVideoMessage;
            //被叫接入
            HoloviewCallKit.instance().onRecvCallFull += HLK_OnRecvCallFull;
            HoloviewCallKit.instance().onCallConnect += HLK_OnCallConnect;
            //对方挂断
            HoloviewCallKit.instance().onCallDisconnect += HLK_OnCallDisconnect;
            HoloviewCallKit.instance().onRemoteUserEnter += HLK_OnRemoteUserEnter;
            HoloviewCallKit.instance().onRemoteUserLeft += HLK_OnRemoteUserLeft;
            //视音频刷新
            // HoloviewCallKit.instance().onVideoFrameData += HLK_OnVideoFrameData;

            HoloviewCallKit.instance().onRecvCmdMessage += HLK_onRecvCmdMessage;
            HoloviewCallKit.instance().onFriendResult += HLK_onFriendResult;
            HoloviewCallKit.instance().onGroupResult += HLK_onGroupResult;
            HoloviewCallKit.instance().onAssetList += HLK_onAssetList;
            HoloviewCallKit.instance().onAssetRealTimeData += HLK_onAssetRealTimeData;

#if !UNITY_EDITOR && UNITY_WSA
           // HoloviewCallKit.instance().onSelfTranslate += HCK_onSelfTranslate;
            //HoloviewCallKit.instance().onRecvArRectWithMartix += HCK_onRecvArRectWithMartix;
#endif
            Debug.Log("chat初始化");
            HoloviewCallKit.instance().Init("000001", "1111111");
           // HoloviewCallKit.instance().Init(appkey, appsecret);
            HoloviewCallKit.instance().SetServerType(0);
            ClientChatState = ChatState.INITED;
        }
        public string Username;
        public string Password;

        public void Login()
        {
            Debug.Log("chat登录");
            //HoloviewCallKit.instance().Login(Username, Password);
            HoloviewCallKit.instance().Login("13611112220", "111111");
            //ChatView.Instance.Login();
        }
        public void Logout()
        {
            Debug.Log("chat登出");
            HoloviewCallKit.instance().Disconnect();
        }
        /// 设置服务器类型
        /// </summary>
        /// <param name="serverType">0：线上服务器  1：服务器地址192.168.1.250</param>
        public void SetServerType(int serverType)
        {
            HoloviewCallKit.instance().SetServerType(serverType);
        }
        public void AcceptCall(string callId, List<long> subUserIds, bool publishStream, int translateMode, bool subAudioData, bool mrcEnabled)
        {
            if (ClientChatState == ChatState.IsCall)
            {
                return;
            }
            CallID = callId;
            Debug.Log("接听");
#if !UNITY_EDITOR && UNITY_WSA
            HoloviewCallKit.instance().AcceptCall(callId, subUserIds, publishStream, mrcEnabled);
#elif UNITY_EDITOR || UNITY_STANDALONE_WIN
            HoloviewCallKit.instance().AcceptCall(callId, subUserIds, false, false);
#endif

            ClientChatState = ChatState.IsCall;
        }
        /// <summary>
        /// 发起呼叫
        /// </summary>
        /// <param name="type">1：一对一 2：群组</param>
        /// <param name="targetid">目标id  一对一：目标用户id   群组：目标群组id</param>
        /// <param name="usersid">呼叫的成员列表</param>
        /// <param name="subusersid">订阅视频的成员列表</param>
        /// <param name="publishStream">是否推流</param>
        /// <param name="translateMode">翻译模式 0：不翻译 1：汉译英 2：英译汉</param>
        /// <param name="mediatype">通话类型  1：语音通话 2：视频通话</param>
        /// <param name="mrcEnabled">是否开启mrc拍摄</param>
        public void Call(ConversationType type, long targetid, List<long> usersid, List<long> subusersid, bool publishStream, int translateMode, int mediatype, bool subAudioData, bool mrcEnabled)
        {
#if !UNITY_EDITOR && UNITY_WSA
            CallID = HoloviewCallKit.instance().CallFull((ConversationType)type, targetid, usersid, subusersid, publishStream, (TranslateMode)translateMode, (CallMediaType)mediatype,mrcEnabled);
#elif UNITY_EDITOR || UNITY_STANDALONE_WIN
            CallID = HoloviewCallKit.instance().CallFull((ConversationType)type, targetid, usersid, subusersid, false, (TranslateMode)translateMode, (CallMediaType)mediatype, false);
#endif
            Debug.Log("Call");
            ClientChatState= ChatState.Calling;
        }
        public void Hang(string callid)
        {
            if ( ClientChatState!=ChatState.IsCall)
            {
                return;
            }
            Debug.Log("挂断");
            HoloviewCallKit.instance().HangCall(callid);
            ClientChatState = ChatState.CONNECT;
        }
        public void SendCMDMessage(int conversationType, long targetId, string cmd)
        {
            HoloviewCallKit.instance().SendCmdMessage((ConversationType)conversationType, targetId, cmd);
        }
        public string GetWifiName()
        {
            return HoloviewCallKit.instance().GetWifiName();
        }
        public void SendCmdMessage(int conversationType, long targetId, string cmd)
        {
            HoloviewCallKit.instance().SendCmdMessage((ConversationType)conversationType, targetId, cmd);

        }
        public void EndInit()
        {
            HoloviewCallKit.instance().onInitResult -= HLK_OnInitResult;
            HoloviewCallKit.instance().onStateChange -= HLK_OnStateChange;
            HoloviewCallKit.instance().onGetTokenResult -= HLK_OnGetTokenResult;
            HoloviewCallKit.instance().onLoginResult -= HLK_onLoginResult;

            HoloviewCallKit.instance().onConnectResult -= HLK_OnConnectResult;
            HoloviewCallKit.instance().onRecvTextMessage -= HLK_OnRecvTextMessage;
            HoloviewCallKit.instance().onRecvImageMessage -= HLK_OnRecvImageMessage;
            HoloviewCallKit.instance().onRecvVideoMessage -= HLK_OnRecvVideoMessage;

            HoloviewCallKit.instance().onRecvCallFull -= HLK_OnRecvCallFull;
            HoloviewCallKit.instance().onCallConnect -= HLK_OnCallConnect;
            HoloviewCallKit.instance().onCallDisconnect -= HLK_OnCallDisconnect;
            HoloviewCallKit.instance().onRemoteUserEnter -= HLK_OnRemoteUserEnter;
            HoloviewCallKit.instance().onRemoteUserLeft -= HLK_OnRemoteUserLeft;
            // HoloviewCallKit.instance().onVideoFrameData -= HLK_OnVideoFrameData;

            HoloviewCallKit.instance().onRecvCmdMessage -= HLK_onRecvCmdMessage;
            HoloviewCallKit.instance().onFriendResult -= HLK_onFriendResult;
            HoloviewCallKit.instance().onGroupResult -= HLK_onGroupResult;

            HoloviewCallKit.instance().onAssetList -= HLK_onAssetList;
            HoloviewCallKit.instance().onAssetRealTimeData -= HLK_onAssetRealTimeData;
#if !UNITY_EDITOR && UNITY_WSA
        //HoloviewCallKit.instance().onSelfTranslate -= HLK_onSelfTranslate;
        //HoloviewCallKit.instance().onRecvArRectWithMartix -= HLK_onRecvArRectWithMartix;
#endif
            HoloviewCallKit.instance().Disconnect();
        }
        #endregion
        #region HoloviewToolKit事件
        //初始化结果
        void HLK_OnInitResult(bool result)
        {
            Debug.Log("HLK_OnInitResult result: " + result);
            if (result)
            {
                
            }
        }
        //连接状态改变
        /// 0  初始化完成
        /// 1  未初始化
        /// 2  服务器连接
        /// 3  服务器断开连接（网络差或登出后返回，需要重新登陆）
        /// 4  异常
        /// 5  账户被踢（账户被踢后返回, 询问重新登陆还是切换账号）

        void HLK_OnStateChange(PluginState state)
        {
            Debug.Log("HLK_OnStateChange state: " + state.ToString());
            switch (state)
            {
                case PluginState.INITED:
                    ClientChatState = ChatState.INITED;
                    break;
                case PluginState.UNINITED:
                    ClientChatState = ChatState.UNINITED;
                    break;
                case PluginState.CONNECT:
                    ClientChatState = ChatState.CONNECT;
                    break;
                case PluginState.DISCONNECT:
                    ClientChatState = ChatState.DISCONNECT;
                    break;
                case PluginState.ERROR:
                    ClientChatState = ChatState.ERROR;
                    break;
                case PluginState.KICKEDOFF:
                    ClientChatState = ChatState.KICKEDOFF;
                    break;
            }
        }
        //
        void HLK_OnGetTokenResult(bool result, string token)
        {
            Debug.Log("HLK_OnGetTokenResult result: " + result + " token:" + token);
           
        }
        void HLK_OnConnectResult(bool result, long userid)
        {
            Debug.Log("HLK_OnConnectResult result: " + result + " userId:" + userid);
            if (result)
            {
                HoloviewCallKit.instance().GetToken(CurrentUser.id, CurrentUser.name, "http://portrait.jpg");

            }
            else
            {
                Debug.LogError("连接失败");
            }


        }
        void HLK_onLoginResult(bool result, User user)
        {
            Debug.Log("HLK_onLoginResult result: " + result + " userId:" + user.id + "useravata  : " + user.portrait);
            if (result)
            {
                CurrentUser = user;
                ChatEventArg e = ReferencePool.Acquire<ChatEventArg>();
                FrameworkEntry.Instance.GetManager<EventManager>().Fire(this, e.ChangeState(ChatEvent.OnLoginResult));

            }
            else
            {
                Debug.LogError("登陆失败");
            }
        }
        void HLK_OnRecvTextMessage(long senderId, long targetId, ConversationType conversationType, string content)
        {
            Debug.Log("HLK_OnRecvTextMessage");
            ChatEventArg e = ReferencePool.Acquire<ChatEventArg>();
            FrameworkEntry.Instance.GetManager<EventManager>().Fire(this, e.ChangeState(ChatEvent.OnRecvText, senderId, content));

        }
        void HLK_OnRecvImageMessage(long senderId, long targetId, ConversationType conversationType, string localThumbUrl, string localUrl, string remoteThumbUrl, string remoteUrl)
        {
            if (remoteUrl == null || remoteUrl.Equals(""))
            {
                return;
            }
            Debug.Log("HLK_OnRecvImageMessage");
            ChatEventArg e = ReferencePool.Acquire<ChatEventArg>();
            FrameworkEntry.Instance.GetManager<EventManager>().Fire(this, e.ChangeState(ChatEvent.OnRecvImage, senderId, remoteUrl));

        }
        void HLK_OnRecvVideoMessage(long senderId, long targetId, ConversationType conversationType, string thumbUrl, string videoUrl)
        {
            Debug.Log("HLK_OnRecvVideoMessage");
        }
        //接受到电话邀请后的处理
        void HLK_OnRecvCallFull(long inviterId, string callId, long targetId, ConversationType conversationType, CallMediaType mediaType, CallEngineType engineType, long[] participates)
        {
            Debug.Log("HLK_OnRecvCallFull");
            ClientChatState = ChatState.Calling;
            long friendId = 0;
            if (conversationType == Holoview.ConversationType.P2P)
            {
                ConnectUserName = "UnKnowUser";
                friendId = inviterId;
            }
            else if (conversationType == Holoview.ConversationType.GROUP)
            {
                ConnectUserName = "UnKnowGroup";
                friendId = targetId;
            }
            if (friendId != 0)
            {
                Debug.Log("需要添加名字 图片");
            }
            foreach (long id in participates)
            {
                InviteUserList.Add(id);
            }
            ChatEventArg e = ReferencePool.Acquire<ChatEventArg>();
            FrameworkEntry.Instance.GetManager<EventManager>().Fire(this, e.ChangeState(ChatEvent.OnCallRequest, callId));

        }
        void HLK_OnCallConnect(string callId)
        {
            Debug.Log("视频连接成功");
            ChatEventArg e = ReferencePool.Acquire<ChatEventArg>();
            FrameworkEntry.Instance.GetManager<EventManager>().Fire(this, e.ChangeState(ChatEvent.OnCallConnect, callId));
            ClientChatState = ChatState.IsCall;
        }
        void HLK_OnCallDisconnect(CallDisconnectedReason reason)
        {
            Debug.Log("视频断开连接");
            ChatEventArg e = ReferencePool.Acquire<ChatEventArg>();
            FrameworkEntry.Instance.GetManager<EventManager>().Fire(this, e.ChangeState(ChatEvent.OnCallDisconnect, reason));

            ClientChatState = ChatState.CONNECT;

        }
        void HLK_OnRemoteUserEnter(long userId, CallMediaType mediaType)
        {
            Debug.Log("呼叫的好友进入");
            ChatEventArg e = ReferencePool.Acquire<ChatEventArg>();
            FrameworkEntry.Instance.GetManager<EventManager>().Fire(this, e.ChangeState(ChatEvent.OnUserEnter, userId));
            ClientChatState = ChatState.CONNECT;
        }
        void HLK_OnRemoteUserLeft(long userId, CallDisconnectedReason reason)
        {
            Debug.Log("呼叫的好友离开");
            ChatEventArg e = ReferencePool.Acquire<ChatEventArg>();
            FrameworkEntry.Instance.GetManager<EventManager>().Fire(this, e.ChangeState(ChatEvent.OnUserLeft, userId));
            ClientChatState = ChatState.CONNECT;
        }
        void HLK_OnVideoFrameData(long userId, ChatItem.VideoData videoData)
        {
            if (userId == CurrentUser.id) return;
            for (int i = 0; i < ChatView.Instance.ChatItemList.Count; i++)
            {
                ChatItem chatItem;
                if (ChatView.Instance.ChatItemList.TryGetValue(userId, out chatItem))
                {
                    chatItem.VideoQueue.Enqueue(videoData);
                }
            }

        }
        void HLK_onVideoYuvFrameData(long userId, ChatItem.VideoDataYuv videoData)
        {

        }
        void HLK_onGroupResult(bool result, Group grp, List<User> memberList)
        {
            ChatEventArg e = ReferencePool.Acquire<ChatEventArg>();
            FrameworkEntry.Instance.GetManager<EventManager>().Fire(this, e.ChangeState(ChatEvent.OnGroupResult, memberList));

        }
        void HLK_onRecvCmdMessage(long senderId, long targetId, ConversationType conversationType, string cmd)
        {

        }
        void HLK_onFriendResult(bool result, List<User> friendList)
        {
            if (result)
            {
                ChatEventArg e = ReferencePool.Acquire<ChatEventArg>();
                FrameworkEntry.Instance.GetManager<EventManager>().Fire(this, e.ChangeState(ChatEvent.OnFriendResult, friendList));
                Debug.Log(friendList);
            }
        }
        void HCK_onSelfTranslate(string msg)
        {
            ChatEventArg e = ReferencePool.Acquire<ChatEventArg>();
            FrameworkEntry.Instance.GetManager<EventManager>().Fire(this, e.ChangeState(ChatEvent.OnRecvText, CurrentUser.id, msg));

        }
        void HLK_onRecvArRectWithMartix()
        {

        }
        void HLK_onAssetList(bool result, List<Asset> assetList)
        {
            ChatEventArg e = ReferencePool.Acquire<ChatEventArg>();
            FrameworkEntry.Instance.GetManager<EventManager>().Fire(this, e.ChangeState(ChatEvent.OnAssetList, assetList));

        }
        void HLK_onAssetRealTimeData(AssetRealTimeData assetRealTimeData)
        {

        }
        #endregion


    }
    public enum ChatState
    {
        InValid=0,
        INITED = 1,
        UNINITED = 2,
        CONNECT = 3,
        DISCONNECT = 4,
        ERROR = 5,
        KICKEDOFF = 6,
        IsCall=7,
        Calling=8
    }
    public enum ChatEvent
    {
        InValid=0,
        OnLoginResult, OnFriend, OnGroup,
        OnCallConnect, OnCallDisconnect, OnUserEnter, OnUserLeft, OnCallRequest,
        OnRecvText, OnRecvImage,OnAssetList, OnCallAudioData, OnGroupResult, OnFriendResult

    }
}


