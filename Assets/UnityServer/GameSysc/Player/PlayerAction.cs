//using ProtoBuf;
//using System;
//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;
////using UnityEngine;

//namespace GameSysc
//{

//    [ProtoContract]
//    public class PlayerAction : GameSysc.Action
//    {
//        [ProtoMember(10101)]
//        private int _emKey;

//        [ProtoMember(10102)]
//        private int _bKeyStatic;

//        public PlayerAction()
//        {
//            //m_iId = ccMath.f_CreateKeyId();
//            m_iType = 101;
//        }

//        public void f_KeyDown(int tKeyCode)
//        {
//            _emKey = tKeyCode;
//            _bKeyStatic = 1;
//        }

//        public void f_KeyUp(int tKeyCode)
//        {
//            _emKey = tKeyCode;
//            _bKeyStatic = 0;
//        }



//        public override void ProcessAction()
//        {
//            if (_bKeyStatic == 1)
//            {
//                Player.GetInstance().f_SaveKeyDown(_emKey);
//            }
//            else
//            {
//                Player.GetInstance().f_SaveKeyUp(_emKey);
//            }            
//        }



//    }

//}