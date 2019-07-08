using ccU3DEngine;
using ProtoBuf;
using System;


namespace GameControllAction
{

    /// <summary>
    /// 发送平均数
    /// 为了让一个客户端向其他客户端发送平均数，Action接口修改为一个有两个字段的抽象类
    /// </summary>
    [ProtoContract]             //[Serializable]
    [ProtoInclude(200, typeof(PlayerTransformAction))]
    [ProtoInclude(201, typeof(PlayerRotateAction))]
    public abstract class BasePlayerAction
    {
        public BasePlayerAction()
        {
            m_iId = ccMath.f_CreateKeyId();
            m_iType = 0;
        }
        /// <summary>
        /// 唯一Id
        /// </summary>
        [ProtoMember(1)]
        public int m_iId;
        /// <summary>
        /// 所属玩家Id
        /// </summary>
        [ProtoMember(2)]
        public int m_iUserId;
        /// <summary>
        /// 当前游戏帧
        /// </summary>
        //[ProtoMember(3)]
        //public int m_iCurGameSyscFrame;
        /// <summary>
        /// Action类型 0=NoAction 1=其它类型
        /// </summary>
        [ProtoMember(4)]
        public int m_iType;

        public void f_Save(int iUserId, int iCurGameSyscFrame)
        {
            m_iUserId = iUserId;
            //m_iCurGameSyscFrame = iCurGameSyscFrame;
        }

        ////////////////////////////////////////////////////////////////////
        //以下为自定义数据内容
                
        public abstract void ProcessAction();


    }

}