using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Net;
using System.IO;
using System;

[System.Serializable]
public struct GunData
{
    public bool Adaptation;
    public bool change;
    public Vector3 GunPosition;
    public Vector3 GunRotation;
    public Vector3 HandleModelSize;
    public float SupplementRotation;
}