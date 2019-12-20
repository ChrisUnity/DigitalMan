//
//  HoloUnityApi.h
//  HoloviewIosPlugin
//
//  Created by zhenhui yang on 2019/3/29.
//  Copyright © 2019年 zhenhui yang. All rights reserved.
//

#import <Foundation/Foundation.h>

NS_ASSUME_NONNULL_BEGIN

#if defined (__cplusplus)
extern "C"
{
#endif
    // 初始化回调
    // bool => result
    typedef  void(*InitCallbackPtr)(bool);
    
    // --------------------- 接收消息回调 --------------------- //
    /**
     TextMessageCallbackPtr
     
     uint64_t senderId
     uint64_t targetId
     int conversationType
     char* content
     */
    typedef  void(*TextMessageCallbackPtr)(uint64_t, uint64_t, int, char*);
    
    /**
     ImageMessageCallbackPtr
     
     uint64_t senderId
     uint64_t targetId
     int conversationType
     char* localThumbUrl
     char* localUrl
     char* remoteThumbUrl
     char* remoteUrl
     */
    typedef  void(*ImageMessageCallbackPtr)(uint64_t, uint64_t, int, char*, char*, char*, char*);
    /**
     ImageMessageCallbackPtr
     
     uint64_t senderId
     uint64_t targetId
     int conversationType
     char* thumbUrl
     char* videoUrl
     */
    typedef  void(*VideoMessageCallbackPtr)(uint64_t, uint64_t, int, char*, char*);
    typedef  void(*VoiceMessageCallbackPtr)(uint64_t, uint64_t, int, char*, int);
    typedef  void(*LocationMessageCallbackPtr)(uint64_t, uint64_t, int, double, double, char*, char*);

    typedef  void(*JoinCallRoomCallbackPtr)(bool result, uint64_t roomid, char* roomName);
    typedef  void(*CallRoomNotifyCallbackPtr)(uint64_t, uint64_t, char* user, char* targetUsers, char* operation, char* showText);

    // --------------------- 视频相关回调 --------------------- //
    /**
     VideoRecvCallbackPtr
     
     uint64_t inviterId
     char* callId
     uint64_t targetId
     int conversationType
     int mediaType
     int engineType
     char* participates
     */
    typedef  void(*VideoRecvCallbackPtr)(uint64_t , char*, uint64_t, int, int, int, char*);
    /**
     VideoConnectCallbackPtr
     
     char* callId
     */
    typedef  void(*VideoConnectCallbackPtr)(char*);
    /**
     VideoDisconnectCallbackPtr
     
     int reason
     */
    typedef  void(*VideoDisconnectCallbackPtr)(int);
    /**
     VideoUserEnterCallbackPtr
     
     uint64_t userId
     int mediaType
     */
    typedef  void(*VideoUserEnterCallbackPtr)(uint64_t, int);
    /**
     VideoUserLeftCallbackPtr
     
     uint64_t userId
     int reason
     */
    typedef  void(*VideoUserLeftCallbackPtr)(uint64_t, int);
    
    /**
     VideoFrameCallbackPtr
     
     uint64_t userId
     uint8_t* buf
     int bufLen
     int width
     int height
     */
    typedef  void(*VideoFrameCallbackPtr)(uint64_t , uint8_t*, int, int, int);
    
    /**
     VideoFrameYuvCallbackPtr
     
     uint64_t userId
     uint8_t* dataY
     int lenY
     uint8_t* dataU
     int lenU
     uint8_t* dataV
     int lenV
     int width
     int height
     */
    typedef  void(*VideoFrameYuvCallbackPtr)(uint64_t , uint8_t*, int, uint8_t*, int, uint8_t*, int, int, int);
    
    
    /**
     状态回调
     
     int state
     */
    typedef  void(*PluginStateCallbackPtr)(int);
    
    
    /**
     获取token回调
     
     bool result
     char* token
     */
    typedef  void(*CetTokenCallbackPtr)(bool, char*);
    
    /**
     连接回调
     
     bool result
     uint64_t userId
     */
    typedef  void(*ConnectCallbackPtr)(bool, uint64_t);
    
    typedef void(*GroupCallbackPtr)(char* group, char* members);
    
    typedef void(*FriendCallbackPtr) (char* friends);
    
    void _init(char* appkey, char* appscrect, InitCallbackPtr initCallback, TextMessageCallbackPtr textMsgCallback,
               ImageMessageCallbackPtr imageMsgCallback, VideoMessageCallbackPtr videoMsgCallback,
               VoiceMessageCallbackPtr voiceMsgCallback, LocationMessageCallbackPtr locMsgCallback,
               CallRoomNotifyCallbackPtr callRoomNotifyCallback,
               VideoRecvCallbackPtr videoRecvCallback, VideoConnectCallbackPtr videoConnectCallback,
               VideoDisconnectCallbackPtr videoDisConnectCallback, VideoUserEnterCallbackPtr videoUserEnterCallback,
               VideoUserLeftCallbackPtr videoUserLeftCallback, VideoFrameCallbackPtr videoFrameCallback,VideoFrameYuvCallbackPtr videoFrameYuvCallback,
               PluginStateCallbackPtr pluginStateCallback, GroupCallbackPtr groupCallback, FriendCallbackPtr friendCallback);
    void _getToken(uint64_t uid, char* uname, char* uportrait, CetTokenCallbackPtr callback);
    void _connect(char* token, ConnectCallbackPtr callback);
    void _disconnect(void);
    char* _startCall(int conversationType ,uint64_t targetId,char* userIds, int mediaType, int engineType);
    void _acceptCall(char* callId);
    void _hangCall(char* callId);
    void _sendTextMessage(uint64_t targetId, int conversationType, char* content);
    void _sendImageMessage(uint64_t targetId, int conversationType, char* localThumbUrl, char* localUrl);
    void _sendVideoMessage(uint64_t targetId, int conversationType, char* thumbUrl, char* videoUrl, int videoLength);
    void _sendVoiceMessage(uint64_t targetId, int conversationType, char* voiceUrl, int voiceLength);
    void _sendLocationMessage(uint64_t targetId, int conversationType, double lat, double lng, char* poi, char* thumbUrl);

    void _setYuvDataEnable(void);
    
    void _loginWithAccount(char* account, char* password, ConnectCallbackPtr callback);
    void _loginWithToken(char* token, char* appkey, ConnectCallbackPtr callback);
    void _joinCallRoom(char* roomCode, JoinCallRoomCallbackPtr callback);
    
    void _logToiOS(const char* debugMessage);
    void _test(VideoFrameCallbackPtr callback);
#if defined (__cplusplus)
}
#endif

NS_ASSUME_NONNULL_END
