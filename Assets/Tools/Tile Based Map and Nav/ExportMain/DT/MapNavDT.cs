using UnityEngine;
using System.Collections;


[System.Serializable]
public class MapNavDT
{
    public short m_iIndex;
    /// <summary>
    /// 网格索引X
    /// </summary>
    public short m_iX;
    public short m_iY;

    /// <summary>
    /// 物理坐标X
    /// </summary>
    public float m_fU3dX;
    public float m_fU3dY;

    /// <summary>
    /// Tile类型
    /// </summary>
    public byte m_TileType;
    /// <summary>
    /// 邻近Tile
    /// </summary>
    public short[] m_aLinkTileNode;

    public byte[] m_aNodeLinkFACE2WAY;



}