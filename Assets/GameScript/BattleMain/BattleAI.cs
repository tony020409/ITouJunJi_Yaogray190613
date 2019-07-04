using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleAI
{
    
    public RolePool m_RolePool = new RolePool();
   

    public BattleAI()
    {
    }

   
    public void f_Start()
    {
        
    }

    public void f_Stop()
    {
        
    }

    public void f_Update()
    {

        Data_Pool.m_TeamPool.f_Update();
      
    }

    public void f_Clear()
    {
        m_RolePool.f_Clear();
    }
    

}