using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ProtoBuf;

namespace GameControllAction
{
    [ProtoContract]
    public class PlayerTransformAction : BasePlayerAction
    {
        //[ProtoMember(1)]
        //public int m_iPlayerId;

        [ProtoMember(2)]
        public float m_fHeadPosX;

        [ProtoMember(3)]
        public float m_fHeadPosY;

        [ProtoMember(4)]
        public float m_fHeadPosZ;

        //[ProtoMember(5)]
        //public float m_fHeadQutnX;

        [ProtoMember(6)]
        public float m_fHeadQutnY;

        //[ProtoMember(7)]
        //public float m_fHeadQutnZ;

        //[ProtoMember(8)]
        //public float m_fHeadQutnW;

        ///////////////////////////////////////////////////////////////////////
        [ProtoMember(9)]
        public float m_fArmLPosX;

        [ProtoMember(10)]
        public float m_fArmLPosY;

        [ProtoMember(11)]
        public float m_fArmLPosZ;

        [ProtoMember(12)]
        public float m_fArmLQutnX;

        [ProtoMember(13)]
        public float m_fArmLQutnY;

        [ProtoMember(14)]
        public float m_fArmLQutnZ;

        //[ProtoMember(15)]
        //public float m_fArmLQutnW;

        ///////////////////////////////////////////////////////////////////////
        [ProtoMember(16)]
        public float m_fArmRPosX;

        [ProtoMember(17)]
        public float m_fArmRPosY;

        [ProtoMember(18)]
        public float m_fArmRPosZ;

        [ProtoMember(19)]
        public float m_fArmRQutnX;

        [ProtoMember(20)]
        public float m_fArmRQutnY;

        [ProtoMember(21)]
        public float m_fArmRQutnZ;

        ///////////////////////////////////////////////////////////////////////
        [ProtoMember(22)]
        public float m_TransformX;

        [ProtoMember(23)]
        public float m_TransformY;

        [ProtoMember(24)]
        public float m_TransformZ;

        ///////////////////////////////////////////////////////////////////////////
        //FootL
        [ProtoMember(25)]
        public float m_fFootLPosX;

        [ProtoMember(26)]
        public float m_fFootLPosY;

        [ProtoMember(27)]
        public float m_fFootLPosZ;

        [ProtoMember(28)]
        public float m_fFootLQutnX;

        [ProtoMember(29)]
        public float m_fFootLQutnY;

        [ProtoMember(30)]
        public float m_fFootLQutnZ;

        ///////////////////////////////////////////////////////////////////////
        //FootR
        [ProtoMember(31)]
        public float m_fFootRPosX;

        [ProtoMember(32)]
        public float m_fFootRPosY;

        [ProtoMember(33)]
        public float m_fFootRPosZ;

        [ProtoMember(34)]
        public float m_fFootRQutnX;

        [ProtoMember(35)]
        public float m_fFootRQutnY;

        [ProtoMember(36)]
        public float m_fFootRQutnZ;

        ///////////////////////////////////////////////////////////////////////
        //Other1 (物品1)
        [ProtoMember(37)]
        public float m_fOther1Active;

        [ProtoMember(38)]
        public float m_fOtherPos1X;

        [ProtoMember(39)]
        public float m_fOtherPos1Y;

        [ProtoMember(40)]
        public float m_fOtherPos1Z;

        [ProtoMember(41)]
        public float m_fOtherRQutn1X;

        [ProtoMember(42)]
        public float m_fOtherRQutn1Y;

        [ProtoMember(43)]
        public float m_fOtherRQutn1Z;

        ///////////////////////////////////////////////////////////////////////
        //Other2 (物品2)
        [ProtoMember(44)]
        public float m_fOther2Active;

        [ProtoMember(45)]
        public float m_fOtherPos2X;

        [ProtoMember(46)]
        public float m_fOtherPos2Y;

        [ProtoMember(47)]
        public float m_fOtherPos2Z;

        [ProtoMember(48)]
        public float m_fOtherRQutn2X;
                                  
        [ProtoMember(49)]         
        public float m_fOtherRQutn2Y;
                                  
        [ProtoMember(50)]         
        public float m_fOtherRQutn2Z;



        //[ProtoMember(22)]
        //public float m_fArmRQutnW;

        ///////////////////////////////////////////////////////////////////////
        //[ProtoMember(23)]
        //public float m_fBodyPosX;

        //[ProtoMember(24)]
        //public float m_fBodyPosY;

        //[ProtoMember(25)]
        //public float m_fBodyPosZ;

        //[ProtoMember(26)]
        //public float m_fBodyQutnX;

        //[ProtoMember(27)]
        //public float m_fBodyQutnY;

        //[ProtoMember(28)]
        //public float m_fBodyQutnZ;

        //[ProtoMember(29)]
        //public float m_fBodyQutnW;



        public PlayerTransformAction()
            : base()
        {
            m_iType = (int)GameEM.EM_PlayerAction.Transform;
        }

        public override void ProcessAction()
        {
            
        }
        
    }


}
