
using ProtoBuf;
using System;
using ccU3DEngine;


namespace GameSysc
{

    [ProtoContract]
    public class NoAction : Action
    {
        public NoAction()
            : base()
        {
            m_iType = -99;
        }

        public override void ProcessAction()
        {

        }
    }


}