using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IPlayerMono
{
    
    void f_Update(int iDeltaTime);
    
    bool m_bIsComplete { get; set; }
       

}