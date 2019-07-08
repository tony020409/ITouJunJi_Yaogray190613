using System;
using System.Collections.Generic;
using ccU3DEngine;

namespace GameSysc
{
    public class ServiceBase
    {
        protected ccSocketMessagePoolV2 _ccSocketMessagePoolV2 = new ccSocketMessagePoolV2();

        public ServiceBase()
        {
            InitMessage();
        }

        protected virtual void InitMessage()
        {

        }
        
        //public void f_Router(aaa12334 stHead, byte[] bytes, int iPos)
        //{
        //    _ccSocketMessagePoolV2.f_Router(stHead, bytes, iPos);
        //}




    }


}