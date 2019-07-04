using UnityEngine;

[System.Serializable]
public struct PoseData
{
    public bool m_Enabled;     //遊戲是否使用文件去調整位置
    public Vector3 m_Position; //Tracker或手把的位置
    public Vector3 m_Rotation; //Tracker或手把的旋轉
}



[System.Serializable]
public struct PoseData_Defult
{
    public Vector3 m_Defult_Position_Gun;       //槍枝預設的位置
    public Vector3 m_Defult_Rotation_Gun;       //槍枝預設的旋轉
    public Vector3 m_Defult_Position_Tracker_1; //Tracker_1 預設的位置
    public Vector3 m_Defult_Rotation_Tracker_1; //Tracker_1 預設的旋轉
    public Vector3 m_Defult_Position_Tracker_2; //Tracker_2 預設的位置
    public Vector3 m_Defult_Rotation_Tracker_2; //Tracker_2 預設的旋轉
}
