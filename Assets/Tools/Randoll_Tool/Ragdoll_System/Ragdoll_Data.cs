using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ragdoll_Data : ScriptableObjectDescription
{
    [Header("Joint")]
    [Header("關節約束開關")]
    public bool Joint_enable = true;
    [Header("約束距離")]
    public float Joint_projection_Distance = 0.5f;
    [Header("約束角度")]
    public float Joint_projection_Angle = 180;
    [Header("擊中力道")]
    public float TransformDirection = 2;
    [Header("Rigbody")]
    [Header("是否要統一質量與阻力，若無勾選可忽略Mass與Drag設定")] 
    public bool IsRigbody_the_Same;
    [Header("全身質量")]
    public float Mass;
    [Header("全身阻力")]
    public float Drag;
    [Header("最小擊中力道")]
    public float minTransformDirection;
    [Header("最大擊中力道")]
    public float maxTransformDirection;
    
}
