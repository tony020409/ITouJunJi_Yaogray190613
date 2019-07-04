using ProtoBuf;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// 发送平均数
/// 为了让一个客户端向其他客户端发送平均数，Action接口修改为一个有两个字段的抽象类
/// </summary>
[ProtoContract]             //[Serializable]
                            //[ProtoInclude(100, typeof(NoAction))]
                            //[ProtoInclude(101, typeof(PlayerAction))]
public abstract class BaseAction
{


        public BaseAction()
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
        [ProtoMember(3)]
        public int m_iCurGameSyscFrame;
        /// <summary>
        /// Action类型 0=NoAction 1=其它类型
        /// </summary>
        [ProtoMember(4)]
        public int m_iType;

        public void f_Save(int iUserId, int iCurGameSyscFrame)
        {
            m_iUserId = iUserId;
            m_iCurGameSyscFrame = iCurGameSyscFrame;
        }

        ////////////////////////////////////////////////////////////////////
        //以下为自定义数据内容




        //[ProtoMember(5)]
        //public int NetworkAverage { get; set; }
        //[ProtoMember(6)]
        //public int RuntimeAverage { get; set; }



        public abstract void f_Update();


 }