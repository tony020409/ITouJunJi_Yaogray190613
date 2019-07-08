// ====================================================================================================================
//
// Created by Leslie Young
// http://www.plyoung.com/ or http://plyoung.wordpress.com/
// ====================================================================================================================

using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using System.Diagnostics;

[ExecuteInEditMode]
public class MapNav : MonoBehaviour 
{
    private bool _bInitOK = false;

	// ====================================================================================================================
	#region inspector properties

	public bool gizmoDrawNodes = true;
	public bool gizmoDrawLinks = true;
	public bool gizmoColorCoding = true;

    /// <summary>
    /// ﹚竡Tile┮糷
    /// </summary>
	public int tilesLayer = 0;							// on what layer is tiles

    /// <summary>
    /// ﹚竡瓜ン┮糷ノめ秈︽翴阑矪瞶
    /// </summary>
	public int unitsLayer = 0;							// on what layer is units
	public TilesLayout tilesLayout = TilesLayout.Hex;	// kind of tile layout

    /// <summary>
    /// space between tiles 1
    /// </summary>
	public float tileSpacing = 1f;						// space between tiles

    /// <summary>
    /// tile 1Τ
    /// </summary>
	public float tileSize = 1f;							// how big is a tile

    /// <summary>
    /// 玂TIleGameObject
    /// </summary>
	public GameObject[] nodesCache;	// (note) I actually want TileNode, but Unity crash with buge maps if I cache the component and not GameObject, here
	public int nodesXCount = 0, nodesYCount = 0;

	#endregion
	
    // ====================================================================================================================
	#region vars

	public enum TilesLayout : byte
    {
        /// <summary>
        /// 6綟﹡
        /// </summary>
        Hex = 0,
        /// <summary>
        /// 4綟﹡
        /// </summary>
        Square_4 = 1,
        /// <summary>
        /// 8綟﹡
        /// </summary>
        Square_8 = 2 }

	// get a TileNode at index of nodesCache
	public TileNode this[int index]
	{ 
		get 
		{
			if (nodesCache == null) return null;
			if (index < 0) return null;
			if (index >= nodesCache.Length) return null;
			if (nodesCache[index] == null) return null;
			return nodesCache[index].GetComponent<TileNode>(); 
		} 
	}

	// shortcut to getting the length of nodesCache. Used together with "TileNode this[int index]"
	// above it makes MapNav act like an array of nodes
	public int Length 
	{ 
		get 
		{
			if (nodesCache == null) return 0;
			return nodesCache.Length; 
		} 
	}

    // this is inited in LinkNodes() but only if nodesCache is set
    //玂ΤTIleTileNode癸禜
    public TileNode[] nodes;// { get; set; }

    public Dictionary<int, TileNode> m_aDicNodes = new Dictionary<int, TileNode>();
    //public Dictionary<int, TileNode> m_aDicPathNodes = new Dictionary<int, TileNode>();
    //public Dictionary<int, TileNode> m_aDicBuildNodes = new Dictionary<int, TileNode>();
    //public Dictionary<int, TileNode> m_aDicWallNodes = new Dictionary<int, TileNode>();

	#endregion
	
        // ====================================================================================================================
	#region pub

    void Awake() 
	{
		LinkNodes();
		ShowAllTileNodes(false);
        _bInitOK = true;
        //InvokeRepeating("f_TTTT", 5, 1);
	}

	/// <summary>hide and disable or show/enable all TileNodes</summary>
	public void ShowAllTileNodes(bool show)
	{
        if (_bInitOK) return;
		if (nodes == null) return;

        int i = 0;
		foreach (TileNode n in nodes)
		{
            if (n == null)
            {
                continue;
            }            
			//n.Show(show);
            n.f_InitTileSprite();

            m_aDicNodes.Add(n.idx, n);
            //if (n.tileTypeMask == TileNode.TileType.Normal)
            //{
            //    m_aDicWallNodes.Add(n.idx, n);
            //}
            //else if (n.tileTypeMask == TileNode.TileType.Path)
            //{
            //    m_aDicPathNodes.Add(n.idx, n);
            //}
            //else if (n.tileTypeMask == TileNode.TileType.Build)
            //{
            //    m_aDicBuildNodes.Add(n.idx, n);
            //}
            n.f_UnShow();
		}
	}


    //public Dictionary<int, TileNode> f_GetAllWallNode()
    //{
    //    return m_aDicWallNodes;
    //}

    //public Dictionary<int, TileNode> f_GetAllNormalNode()
    //{
    //    return m_aDicPathNodes;
    //}

    //public Dictionary<int, TileNode> f_GetAllBuildNode()
    //{
    //    return m_aDicBuildNodes;
    //}

    
	/// <summary>Only touches the markers which means it does not "enable/disable" the tile by touching the collider too</summary>
	public void ShowTileNodeMarkers(bool show)
	{
		if (nodesCache == null) return;
		foreach (GameObject go in nodesCache)
		{
			if (go == null) continue;
            TileNode tTileNode = go.GetComponent<TileNode>();
			//go.GetComponent<Renderer>().enabled = show;
            if (show)
            {
                tTileNode.f_Show();
            }
            else
            {
                tTileNode.f_UnShow();
            }
		}
	}


    /// <summary>
    /// 莉眔㏄瞅竚翴
    /// </summary>
    /// <param name="tTileNode"></param>
    /// <param name="validNodesLayer"></param>
    /// <returns></returns>
    public TileNode f_GetAroundFreePos(TileNode tTileNode, TileNode.TileType validNodesLayer)
    {
        if (CheckTileIsFree(tTileNode, null))
        {
            if (tTileNode.tileTypeMask == validNodesLayer)
            {
                return tTileNode;
            }
        }
        
        List<TileNode> aData = f_GetAroundPos(tTileNode, validNodesLayer, 1);
        for (int i = 0; i < aData.Count; i++)
        {
            if (CheckTileIsFree(aData[i], null))
            {
                return aData[i];
            }
        }

        return null;
    }

    private bool CheckTileIsFree(TileNode tTileNode, List<int> aData)
    {
        if (tTileNode == null)
        {
            return false;
        }
        return tTileNode.f_CheckIsFree(aData);
    }

    private bool CheckTileIsFree(TileNode tTileNode, int iData)
    {
        if (tTileNode == null)
        {
            return false;
        }
        return tTileNode.f_CheckIsFree(iData);
    }

    private List<TileNode> f_GetAroundPos(TileNode tTileNode, TileNode.TileType validNodesLayer, int iSize)
    {
        int iIndexX = tTileNode.m_iIndexX;
        int iIndexY = tTileNode.m_iIndexY;
        List<TileNode> aData = new List<TileNode>();
        if (tilesLayout == TilesLayout.Square_4)
        {
            //0
            TileNode tNode = f_GetNodeForIndexXY(iIndexX, iIndexY + iSize);
            if (tNode != null)
            {
                aData.Add(tNode);
            }
            //1
            tNode = f_GetNodeForIndexXY(iIndexX + iSize, iIndexY + iSize);
            if (tNode != null)
            {
                aData.Add(tNode);
            }
            //3
            tNode = f_GetNodeForIndexXY(iIndexX + iSize, iIndexY);
            if (tNode != null)
            {
                aData.Add(tNode);
            }
            //4
            tNode = f_GetNodeForIndexXY(iIndexX + iSize, iIndexY - iSize);
            if (tNode != null)
            {
                aData.Add(tNode);
            }
            //6
            tNode = f_GetNodeForIndexXY(iIndexX, iIndexY - iSize);
            if (tNode != null)
            {
                aData.Add(tNode);
            }
            //7
            tNode = f_GetNodeForIndexXY(iIndexX - iSize, iIndexY - iSize);
            if (tNode != null)
            {
                aData.Add(tNode);
            }
            //9
            tNode = f_GetNodeForIndexXY(iIndexX - iSize, iIndexY);
            if (tNode != null)
            {
                aData.Add(tNode);
            }
            //11
            tNode = f_GetNodeForIndexXY(iIndexX - iSize, iIndexY + iSize);
            if (tNode != null)
            {
                aData.Add(tNode);
            }
        }
        return aData;
    }


    /// <summary>
    /// 莉眔㏄瞅︽ǐ翴碝隔
    /// </summary>
    /// <param name="tTileNode"></param>
    /// <param name="validNodesLayer"></param>
    /// <returns></returns>
    public List<TileNode> f_GetAroundFreePosForPath(TileNode fromNode, TileNode toNode, int iSize, TileNode.TileType validNodesLayer, int iToNodeData, List<int> aIgnoreTileData)
    {
        if (fromNode == null || toNode == null) return null;
        List<TileNode> aPath = null;
        List<TileNode> aData = f_GetAroundPos(toNode, validNodesLayer, iSize);
        for (int i = 0; i < aData.Count; i++)
        {
            if (CheckTileIsFree(aData[i], iToNodeData))
            {
                aPath = GetPath(fromNode, aData[i], TileNode.TileType.Normal, aIgnoreTileData);
                break;
            }
        }
        return aPath;
    }


    int iIndex;
    List<TileNode> _aTmpTileNode = new List<TileNode>();
    Dictionary<int, TileNode> _aOpenList = new Dictionary<int, TileNode>();
    Dictionary<int, TileNode> _aCloseList = new Dictionary<int, TileNode>();  
    public List<TileNode> GetPath(TileNode fromNode, TileNode toNode, TileNode.TileType validNodesLayer, List<int> aIgnoreTileData)
    {
        if (fromNode == null || toNode == null) return null;

        // reset
        foreach (TileNode ttt in _aTmpTileNode)
        {
            ttt.SetPathParent(null, 0, 0);
        }
        _aTmpTileNode.Clear();

        const int DistanceConst = 10;
        _aOpenList.Clear();
        _aCloseList.Clear();

        TileNode tn = fromNode;
        _aOpenList.Add(tn.idx, tn);

        while (_aOpenList.Count > 0)
        {
            // find one with lowest F score
            tn = null;
            foreach (KeyValuePair<int, TileNode> ol in _aOpenList)
            {
                if (tn == null)
                {
                    tn = ol.Value;
                }
                else if (ol.Value.PathF < tn.PathF)
                {
                    tn = ol.Value;
                }
            }

            // drop it from open list and add to close list
            _aCloseList.Add(tn.idx, tn);
            _aOpenList.Remove(tn.idx);

            if (tn == toNode) 
                break; // reached destination, break

            iIndex = -1;
            // find neighbours of this point
            foreach (TileNode n in tn.nodeLinks)
            {
                iIndex++;
                // ignore null,
                if (n == null) continue;

                // in close set
                if (_aCloseList.ContainsKey(n.idx)) continue;

                // check if it is a tile node that can be used
                if (validNodesLayer > 0)
                {
                    if ((n.tileTypeMask & validNodesLayer) == validNodesLayer)
                    {
                        // now check if another unit is occupying the same layer/level on the node
                        if (n.GetUnitInLevel(validNodesLayer) != null) continue;
                    }

                    // else, not a valid node
                    else continue;
                }                
                if (!n.f_CheckIsHave(aIgnoreTileData))
                {
                    continue;
                }
                // check if there is link switch between the two nodes and if it is on or off
                if (tn.linkOnOffSwitch != null)
                {
                    if (tn.linkOnOffSwitch.LinkIsOn(n) == 0) continue;
                }
                if (n.linkOnOffSwitch != null)
                {
                    if (n.linkOnOffSwitch.LinkIsOn(tn) == 0) continue;
                }
                int G = G = tn.PathG + DistanceConst;
                int H = 0;
                if (tilesLayout == TilesLayout.Square_8)
                {
                    // calc G & H                   
                    H = Mathf.Abs(toNode.m_iDis - n.m_iDis);
                    //BDFH
                    if (n.m_aNodeLinkFACE2WAY[iIndex] == FACE2WAY.eWayRU ||
                        n.m_aNodeLinkFACE2WAY[iIndex] == FACE2WAY.eWayRD ||
                        n.m_aNodeLinkFACE2WAY[iIndex] == FACE2WAY.eWayLD ||
                        n.m_aNodeLinkFACE2WAY[iIndex] == FACE2WAY.eWayLU)
                    {
                        H -= 1;
                    }
                }
                else
                {
                    H = DistanceConst * Mathf.Abs((int)Vector3.Distance(n.transform.position, toNode.transform.position));
                }
                // check if there are movement modifiers
                if (n.movesMod != null)
                {
                    foreach (TNEMovementModifier.MovementInfo m in n.movesMod.moveInfos)
                    {
                        if (m.tileType == validNodesLayer) G += DistanceConst * m.movesModifier;
                    }
                }

                // apply
                if (_aOpenList.ContainsKey(n.idx))
                {	// open list contains the neighbour
                    // check if G score to the neighbour will be lower if followed from this point
                    if (G < n.PathG)
                    {
                        n.SetPathParent(tn, G, H);
                        _aTmpTileNode.Add(n);
                    }
                }
                else
                {	// add neighbour to open list
                    n.SetPathParent(tn, G, H);
                    _aOpenList.Add(n.idx, n);
                    _aTmpTileNode.Add(n);
                }
            }
        }

        // start at dest and build path
        List<TileNode> path = new List<TileNode>();
        tn = toNode;
        while (tn.PathParent != null)
        {
            path.Add(tn);
            tn = tn.PathParent;
        }

        if (path.Count > 0)
        {	// the path is calculated in reverse, swop it around
            // and then return the array of the elements
            path.Reverse();
            return path;
        }

        // else return null
        return null;
    }

	#endregion
	
    // ====================================================================================================================
	#region create / setup tools


	/// <summary>Creates a new grid of tile nodes of x by y count</summary>
    public static void CreateTileNodes(GameObject nodeFab, MapNav map, MapNav.TilesLayout layout, float tileSpacing, float tileSize, TileNode.TileType initialMask, int xCount, int yCount)
    {
        if (xCount <= 0 || yCount <= 0) return;

        map.tilesLayout = layout;
        map.tileSpacing = tileSpacing;
        map.tileSize = tileSize;

        // first delete the old nodes
        List<GameObject> remove = new List<GameObject>();
        foreach (Transform t in map.transform)
        {	// just make sure it is a node in case there are other kinds of objects under MapNav object
            if (t.name.Contains("node")) remove.Add(t.gameObject);
        }
        remove.ForEach(go => DestroyImmediate(go));

        // now create new nodes

        map.nodesXCount = xCount;
        map.nodesYCount = yCount;
        map.nodesCache = new GameObject[map.nodesXCount * map.nodesYCount];

        int count = 0;
        bool atoffs = false;
        float offs = 0f;
        float xOffs = map.tileSpacing;
        float yOffs = map.tileSpacing;

        if (map.tilesLayout == MapNav.TilesLayout.Hex)
        {	// calculate offset to correctly plate hextiles
            xOffs = Mathf.Sqrt(3f) * map.tileSpacing * 0.5f;
            offs = xOffs * 0.5f;
            yOffs = yOffs * 0.75f;
        }
        else if (map.tilesLayout == MapNav.TilesLayout.Square_8)
        {
            xOffs = Mathf.Sqrt(3f) * map.tileSpacing * 0.58f;       //xOffs	1.004589	float
            offs = xOffs * 0.5f;                                    //offs	0.5022947	float
            yOffs = yOffs * 0.25f;                                  //yOffs	0.24	float
        }
        //float xAA = 0.0045f;
        for (int y = 0; y < yCount; y++)
        {
            for (int x = 0; x < xCount; x++)
            {
                // create the node
                GameObject go = (GameObject)GameObject.Instantiate(nodeFab);
                go.name = "node" + count.ToString();
                go.layer = map.tilesLayer;

                // parent under MapNav and position the node
                //承Tile癸禜璸衡Г夹
                go.transform.parent = map.transform;
                //go.transform.localPosition = new Vector3(x * xOffs + (atoffs ? offs : 0f) - xAA * y, 0f, y * yOffs);
                go.transform.localPosition = new Vector3(x * xOffs + (atoffs ? offs : 0f), 0f, y * yOffs);
                go.transform.localScale = new Vector3(map.tileSize, map.tileSize, map.tileSize);

                // update TileNode component
                TileNode n = go.GetComponent<TileNode>();
                n.idx = count;
                n.m_iIndexX = x;
                n.m_iIndexY = y;
                n.parent = map;
                n.tileTypeMask = initialMask;
                n.tag = "TagTileNode";
                n.f_InitTileSprite();

                // turn off the collider, don't need it now
                go.GetComponent<Collider>().enabled = false;

                // cache the node
                map.nodesCache[count] = go;
                count++;
            }
            atoffs = !atoffs;
        }


    }

    //public static void CreateTileNodes(GameObject nodeFab, MapNav map, MapNav.TilesLayout layout, float tileSpacing, float tileSize, TileNode.TileType initialMask, int xCount, int yCount)
    //{
    //    if (xCount <= 0 || yCount <= 0) return;

    //    map.tilesLayout = layout;
    //    map.tileSpacing = tileSpacing;
    //    map.tileSize = tileSize;

    //    // first delete the old nodes
    //    List<GameObject> remove = new List<GameObject>();
    //    foreach (Transform t in map.transform)
    //    {	// just make sure it is a node in case there are other kinds of objects under MapNav object
    //        if (t.name.Contains("node")) remove.Add(t.gameObject);
    //    }
    //    remove.ForEach(go => DestroyImmediate(go));

    //    // now create new nodes

    //    map.nodesXCount = xCount;
    //    map.nodesYCount = yCount;
    //    map.nodesCache = new GameObject[map.nodesXCount * map.nodesYCount];

    //    int count = 0;
    //    bool atoffs = false;
    //    float offs = 0f;
    //    float xOffs = map.tileSpacing;
    //    float yOffs = map.tileSpacing;

    //    if (map.tilesLayout == MapNav.TilesLayout.Hex)
    //    {	// calculate offset to correctly plate hextiles
    //        xOffs = Mathf.Sqrt(3f) * map.tileSpacing * 0.5f;
    //        offs = xOffs * 0.5f;
    //        yOffs = yOffs * 0.75f;
    //    }

    //    for (int y = 0; y < yCount; y++)
    //    {
    //        for (int x = 0; x < xCount; x++)
    //        {
    //            // create the node
    //            GameObject go = (GameObject)GameObject.Instantiate(nodeFab);
    //            go.name = "node" + count.ToString();
    //            go.layer = map.tilesLayer;

    //            // parent under MapNav and position the node
    //            go.transform.parent = map.transform;
    //            go.transform.localPosition = new Vector3(x * xOffs + (atoffs ? offs : 0f), 0f, y * yOffs);
    //            go.transform.localScale = new Vector3(map.tileSize, map.tileSize, map.tileSize);

    //            // update TileNode component
    //            TileNode n = go.GetComponent<TileNode>();
    //            n.idx = count;
    //            n.parent = map;
    //            n.tileTypeMask = initialMask;

    //            // turn off the collider, don't need it now
    //            go.GetComponent<Collider>().enabled = false;

    //            // cache the node
    //            map.nodesCache[count] = go;
    //            count++;
    //        }
    //        atoffs = !atoffs;
    //    }
    //}

	/// <summary>Links TileNodes with their neighbouring nodes</summary>
    public void LinkNodes()
    {
        if (_bInitOK)
        {
            return;
        }
        if (nodesCache == null) return;
        if (nodesCache.Length == 0) return;
        if (nodesCache.Length != nodesXCount * nodesYCount)
        {
            MessageBox.DEBUG(string.Format("The number of cached nodes {0} != {1} which was expected", nodesCache.Length, (nodesXCount * nodesYCount)));
            return;
        }

        nodes = new TileNode[nodesCache.Length];

        bool atoffs = false;
        int i = 0;

        // === link nodes with their neighbours (hex tile pattern)
        if (tilesLayout == TilesLayout.Hex)
        {
            atoffs = false;
            for (int y = 0; y < nodesYCount; y++)
            {
                for (int x = 0; x < nodesXCount; x++)
                {
                    i = y * nodesXCount + x;
                    if (nodesCache[i] == null) { nodes[i] = null; continue; }

                    TileNode n = nodesCache[i].GetComponent<TileNode>();
                    nodes[i] = n; // cache the component
                    n.nodeLinks = new TileNode[6] { null, null, null, null, null, null };

                    // link with previous node
                    if (x - 1 >= 0) n.nodeLinks[0] = nodesCache[i - 1] == null ? null : nodesCache[i - 1].GetComponent<TileNode>();
                    // link with next node
                    if (x + 1 < nodesXCount) n.nodeLinks[1] = nodesCache[i + 1] == null ? null : nodesCache[i + 1].GetComponent<TileNode>();

                    // link with nodes in previous row
                    if (y > 0)
                    {
                        // prev row, same column
                        n.nodeLinks[2] = nodesCache[i - nodesXCount] == null ? null : nodesCache[i - nodesXCount].GetComponent<TileNode>(); ;
                        if (atoffs)
                        {	// prev row, next column
                            if (x + 1 < nodesXCount) n.nodeLinks[4] = nodesCache[i - nodesXCount + 1] == null ? null : nodesCache[i - nodesXCount + 1].GetComponent<TileNode>();
                        }
                        else
                        {	// prev row, prev column
                            if (x - 1 >= 0) n.nodeLinks[4] = nodesCache[i - nodesXCount - 1] == null ? null : nodesCache[i - nodesXCount - 1].GetComponent<TileNode>();
                        }
                    }

                    // link with nodes in next row
                    if (y + 1 < nodesYCount)
                    {
                        // next row, same column
                        n.nodeLinks[3] = nodesCache[i + nodesXCount] == null ? null : nodesCache[i + nodesXCount].GetComponent<TileNode>();
                        if (atoffs)
                        {	// prev row, next column
                            if (x + 1 < nodesXCount) n.nodeLinks[5] = nodesCache[i + nodesXCount + 1] == null ? null : nodesCache[i + nodesXCount + 1].GetComponent<TileNode>();
                        }
                        else
                        {	// prev row, prev column
                            if (x - 1 >= 0) n.nodeLinks[5] = nodesCache[i + nodesXCount - 1] == null ? null : nodesCache[i + nodesXCount - 1].GetComponent<TileNode>();
                        }
                    }
                }
                atoffs = !atoffs;
            }
        }

        // === link nodes with their neighbours (square tile pattern with 4 neighbours)
        if (tilesLayout == TilesLayout.Square_4)
        {
            for (int y = 0; y < nodesYCount; y++)
            {
                for (int x = 0; x < nodesXCount; x++)
                {
                    i = y * nodesXCount + x;
                    if (nodesCache[i] == null) { nodes[i] = null; continue; }

                    TileNode n = nodesCache[i].GetComponent<TileNode>();
                    nodes[i] = n; // cache the component
                    n.nodeLinks = new TileNode[4] { null, null, null, null };

                    // link with previous node
                    if (x - 1 >= 0) n.nodeLinks[0] = nodesCache[i - 1] == null ? null : nodesCache[i - 1].GetComponent<TileNode>();
                    // link with next node
                    if (x + 1 < nodesXCount) n.nodeLinks[1] = nodesCache[i + 1] == null ? null : nodesCache[i + 1].GetComponent<TileNode>();
                    // prev row, same column
                    if (y > 0) n.nodeLinks[2] = nodesCache[i - nodesXCount] == null ? null : nodesCache[i - nodesXCount].GetComponent<TileNode>();
                    // next row, same column
                    if (y + 1 < nodesYCount) n.nodeLinks[3] = nodesCache[i + nodesXCount] == null ? null : nodesCache[i + nodesXCount].GetComponent<TileNode>(); ;
                }
            }
        }

        //=== link nodes with their neighbours (square tile pattern with 8 neighbours)
        int iLinkX = 0, iLinkY = 0, i45 = 0;
        int iDisY = 0;
        if (tilesLayout == TilesLayout.Square_8)
        {
            for (int y = 0; y < nodesYCount; y++)
            {
                i45 = y % 2;
                if (i45 == 0)
                {
                    //iDisY = 0;
                }
                else
                {
                    iDisY += 1;
                }
                for (int x = 0; x < nodesXCount; x++)
                {
                    i = y * nodesXCount + x;
                    if (nodesCache[i] == null) { nodes[i] = null; continue; }

                    TileNode n = nodesCache[i].GetComponent<TileNode>();                    
                    nodes[i] = n; // cache the component
                    n.nodeLinks = new TileNode[8] { null, null, null, null, null, null, null, null };
                    n.m_aNodeLinkFACE2WAY = new FACE2WAY[8];

                    TileNodeLink tTileNodeLink = n.GetComponent<TileNodeLink>();
                    if (tTileNodeLink == null)
                    {
                        tTileNodeLink = n.gameObject.AddComponent<TileNodeLink>();
                    }                    
                    n.m_iDis = x + iDisY;

                    ////A
                    //iLinkX = x;
                    //iLinkY = y + 2;
                    //n.nodeLinks[0] = GetTileNode(iLinkX, iLinkY);
                    //n.m_aNodeLinkFACE2WAY[0] = FACE2WAY.eWayU;

                    //B
                    if (i45 == 0)
                    {
                        iLinkX = x;
                        iLinkY = y + 1;
                    }
                    else
                    {
                        iLinkX = x + 1;
                        iLinkY = y + 1;
                    }
                    n.nodeLinks[1] = GetTileNode(iLinkX, iLinkY);
                    n.m_aNodeLinkFACE2WAY[1] = FACE2WAY.eWayRU;

                    ////C
                    //iLinkX = x + 1;
                    //iLinkY = y;
                    //n.nodeLinks[2] = GetTileNode(iLinkX, iLinkY);
                    //n.m_aNodeLinkFACE2WAY[2] = FACE2WAY.eWayR;

                    //D
                    if (i45 == 0)
                    {
                        iLinkX = x;
                        iLinkY = y - 1;
                    }
                    else
                    {
                        iLinkX = x + 1;
                        iLinkY = y - 1;
                    }
                    n.nodeLinks[3] = GetTileNode(iLinkX, iLinkY);
                    n.m_aNodeLinkFACE2WAY[3] = FACE2WAY.eWayRD;

                    ////E
                    //iLinkX = x;
                    //iLinkY = y - 2;
                    //n.nodeLinks[4] = GetTileNode(iLinkX, iLinkY);
                    //n.m_aNodeLinkFACE2WAY[4] = FACE2WAY.eWayD;

                    //F
                    if (i45 == 0)
                    {
                        iLinkX = x - 1;
                        iLinkY = y - 1;
                    }
                    else
                    {
                        iLinkX = x;
                        iLinkY = y - 1;
                    }
                    n.nodeLinks[5] = GetTileNode(iLinkX, iLinkY);
                    n.m_aNodeLinkFACE2WAY[5] = FACE2WAY.eWayLD;

                    ////G
                    //iLinkX = x - 1;
                    //iLinkY = y;                   
                    //n.nodeLinks[6] = GetTileNode(iLinkX, iLinkY);
                    //n.m_aNodeLinkFACE2WAY[6] = FACE2WAY.eWayL;

                    //H
                    if (i45 == 0)
                    {
                        iLinkX = x - 1;
                        iLinkY = y + 1;
                    }
                    else
                    {
                        iLinkX = x;
                        iLinkY = y + 1;
                    }
                    n.nodeLinks[7] = GetTileNode(iLinkX, iLinkY);
                    n.m_aNodeLinkFACE2WAY[7] = FACE2WAY.eWayLU;

                    tTileNodeLink.m_aLinkTileNode = n.nodeLinks;

                    //// link with previous node
                    //if (x - 1 >= 0)
                    //{
                    //    n.nodeLinks[0] = nodesCache[i - 1] == null ? null : nodesCache[i - 1].GetComponent<TileNode>();
                    //}
                    //// link with next node
                    //if (x + 1 < nodesXCount)
                    //{
                    //    n.nodeLinks[1] = nodesCache[i + 1] == null ? null : nodesCache[i + 1].GetComponent<TileNode>();
                    //}
                    //// link with nodes in previous row
                    //if (y > 0)
                    //{
                    //    // prev row, same column
                    //    n.nodeLinks[2] = nodesCache[i - nodesXCount] == null ? null : nodesCache[i - nodesXCount].GetComponent<TileNode>();
                    //    // prev row, prev column
                    //    if (x - 1 >= 0)
                    //    {
                    //        n.nodeLinks[4] = nodesCache[i - nodesXCount - 1] == null ? null : nodesCache[i - nodesXCount - 1].GetComponent<TileNode>();
                    //    }
                    //    // prev row, next column
                    //    if (x + 1 < nodesXCount)
                    //    {
                    //        n.nodeLinks[6] = nodesCache[i - nodesXCount + 1] == null ? null : nodesCache[i - nodesXCount + 1].GetComponent<TileNode>();
                    //    }
                    //}
                    //// link with nodes in next row
                    //if (y + 1 < nodesYCount)
                    //{
                    //    // next row, same column
                    //    n.nodeLinks[3] = nodesCache[i + nodesXCount] == null ? null : nodesCache[i + nodesXCount].GetComponent<TileNode>();
                    //    // prev row, prev column
                    //    if (x - 1 >= 0)
                    //    {
                    //        n.nodeLinks[5] = nodesCache[i + nodesXCount - 1] == null ? null : nodesCache[i + nodesXCount - 1].GetComponent<TileNode>();
                    //    }
                    //    // prev row, next column
                    //    if (x + 1 < nodesXCount)
                    //    {
                    //        n.nodeLinks[7] = nodesCache[i + nodesXCount + 1] == null ? null : nodesCache[i + nodesXCount + 1].GetComponent<TileNode>();
                    //    }
                    //}
                }
            }
        }

    }

    //public void LinkNodes()
    //{
    //    if (nodesCache == null) return;
    //    if (nodesCache.Length == 0) return;
    //    if (nodesCache.Length != nodesXCount * nodesYCount)
    //    {
    //        UnityEngine.MessageBox.DEBUG(string.Format("The number of cached nodes {0} != {1} which was expected", nodesCache.Length, (nodesXCount * nodesYCount)));
    //        return;
    //    }

    //    nodes = new TileNode[nodesCache.Length];

    //    bool atoffs = false;
    //    int i = 0;

    //    // === link nodes with their neighbours (hex tile pattern)
    //    if (tilesLayout == TilesLayout.Hex)
    //    {
    //        atoffs = false;
    //        for (int y = 0; y < nodesYCount; y++)
    //        {
    //            for (int x = 0; x < nodesXCount; x++)
    //            {
    //                i = y * nodesXCount + x;
    //                if (nodesCache[i] == null) { nodes[i] = null; continue; }

    //                TileNode n = nodesCache[i].GetComponent<TileNode>();
    //                nodes[i] = n; // cache the component
    //                n.nodeLinks = new TileNode[6] { null, null, null, null, null, null };

    //                // link with previous node
    //                if (x - 1 >= 0) n.nodeLinks[0] = nodesCache[i - 1] == null ? null : nodesCache[i - 1].GetComponent<TileNode>();
    //                // link with next node
    //                if (x + 1 < nodesXCount) n.nodeLinks[1] = nodesCache[i + 1] == null ? null : nodesCache[i + 1].GetComponent<TileNode>();

    //                // link with nodes in previous row
    //                if (y > 0)
    //                {
    //                    // prev row, same column
    //                    n.nodeLinks[2] = nodesCache[i - nodesXCount] == null ? null : nodesCache[i - nodesXCount].GetComponent<TileNode>(); ;
    //                    if (atoffs)
    //                    {	// prev row, next column
    //                        if (x + 1 < nodesXCount) n.nodeLinks[4] = nodesCache[i - nodesXCount + 1] == null ? null : nodesCache[i - nodesXCount + 1].GetComponent<TileNode>();
    //                    }
    //                    else
    //                    {	// prev row, prev column
    //                        if (x - 1 >= 0) n.nodeLinks[4] = nodesCache[i - nodesXCount - 1] == null ? null : nodesCache[i - nodesXCount - 1].GetComponent<TileNode>();
    //                    }
    //                }

    //                // link with nodes in next row
    //                if (y + 1 < nodesYCount)
    //                {
    //                    // next row, same column
    //                    n.nodeLinks[3] = nodesCache[i + nodesXCount] == null ? null : nodesCache[i + nodesXCount].GetComponent<TileNode>();
    //                    if (atoffs)
    //                    {	// prev row, next column
    //                        if (x + 1 < nodesXCount) n.nodeLinks[5] = nodesCache[i + nodesXCount + 1] == null ? null : nodesCache[i + nodesXCount + 1].GetComponent<TileNode>();
    //                    }
    //                    else
    //                    {	// prev row, prev column
    //                        if (x - 1 >= 0) n.nodeLinks[5] = nodesCache[i + nodesXCount - 1] == null ? null : nodesCache[i + nodesXCount - 1].GetComponent<TileNode>();
    //                    }
    //                }
    //            }
    //            atoffs = !atoffs;
    //        }
    //    }

    //    // === link nodes with their neighbours (square tile pattern with 4 neighbours)
    //    if (tilesLayout == TilesLayout.Square_4)
    //    {
    //        for (int y = 0; y < nodesYCount; y++)
    //        {
    //            for (int x = 0; x < nodesXCount; x++)
    //            {
    //                i = y * nodesXCount + x;
    //                if (nodesCache[i] == null) { nodes[i] = null; continue; }

    //                TileNode n = nodesCache[i].GetComponent<TileNode>();
    //                nodes[i] = n; // cache the component
    //                n.nodeLinks = new TileNode[4] { null, null, null, null };

    //                // link with previous node
    //                if (x - 1 >= 0) n.nodeLinks[0] = nodesCache[i - 1] == null ? null : nodesCache[i - 1].GetComponent<TileNode>();
    //                // link with next node
    //                if (x + 1 < nodesXCount) n.nodeLinks[1] = nodesCache[i + 1] == null ? null : nodesCache[i + 1].GetComponent<TileNode>();
    //                // prev row, same column
    //                if (y > 0) n.nodeLinks[2] = nodesCache[i - nodesXCount] == null ? null : nodesCache[i - nodesXCount].GetComponent<TileNode>();
    //                // next row, same column
    //                if (y + 1 < nodesYCount) n.nodeLinks[3] = nodesCache[i + nodesXCount] == null ? null : nodesCache[i + nodesXCount].GetComponent<TileNode>(); ;
    //            }
    //        }
    //    }

    //    //=== link nodes with their neighbours (square tile pattern with 8 neighbours)
    //    if (tilesLayout == TilesLayout.Square_8)
    //    {
    //        for (int y = 0; y < nodesYCount; y++)
    //        {
    //            for (int x = 0; x < nodesXCount; x++)
    //            {
    //                i = y * nodesXCount + x;
    //                if (nodesCache[i] == null) { nodes[i] = null; continue; }

    //                TileNode n = nodesCache[i].GetComponent<TileNode>();
    //                nodes[i] = n; // cache the component
    //                n.nodeLinks = new TileNode[8] { null, null, null, null, null, null, null, null };

    //                // link with previous node
    //                if (x - 1 >= 0)
    //                {
    //                    n.nodeLinks[0] = nodesCache[i - 1] == null ? null : nodesCache[i - 1].GetComponent<TileNode>();
    //                }
    //                // link with next node
    //                if (x + 1 < nodesXCount)
    //                {
    //                    n.nodeLinks[1] = nodesCache[i + 1] == null ? null : nodesCache[i + 1].GetComponent<TileNode>();
    //                }
    //                // link with nodes in previous row
    //                if (y > 0)
    //                {
    //                    // prev row, same column
    //                    n.nodeLinks[2] = nodesCache[i - nodesXCount] == null ? null : nodesCache[i - nodesXCount].GetComponent<TileNode>();
    //                    // prev row, prev column
    //                    if (x - 1 >= 0)
    //                    {
    //                        n.nodeLinks[4] = nodesCache[i - nodesXCount - 1] == null ? null : nodesCache[i - nodesXCount - 1].GetComponent<TileNode>();
    //                    }
    //                    // prev row, next column
    //                    if (x + 1 < nodesXCount)
    //                    {
    //                        n.nodeLinks[6] = nodesCache[i - nodesXCount + 1] == null ? null : nodesCache[i - nodesXCount + 1].GetComponent<TileNode>();
    //                    }
    //                }
    //                // link with nodes in next row
    //                if (y + 1 < nodesYCount)
    //                {
    //                    // next row, same column
    //                    n.nodeLinks[3] = nodesCache[i + nodesXCount] == null ? null : nodesCache[i + nodesXCount].GetComponent<TileNode>();
    //                    // prev row, prev column
    //                    if (x - 1 >= 0)
    //                    {
    //                        n.nodeLinks[5] = nodesCache[i + nodesXCount - 1] == null ? null : nodesCache[i + nodesXCount - 1].GetComponent<TileNode>();
    //                    }
    //                    // prev row, next column
    //                    if (x + 1 < nodesXCount)
    //                    {
    //                        n.nodeLinks[7] = nodesCache[i + nodesXCount + 1] == null ? null : nodesCache[i + nodesXCount + 1].GetComponent<TileNode>();
    //                    }
    //                }
    //            }
    //        }
    //    }

    //}

    private TileNode GetTileNode(int iX, int iY)
    {        
        if (iX < 0 || iY < 0)
        {
            return null;
        }
        if (iX == nodesXCount || iY == nodesYCount)
        {
            return null;
        }
        if (iX < nodesXCount && iY < nodesYCount)
        {
            return nodesCache[GetIndexForXY(iX, iY)].GetComponent<TileNode>();
        }
        return null;
    }

    private int GetIndexForXY(int iX, int iY)
    {
        return iY * nodesXCount + iX;
    }

	/// <summary>Reset all tilenodes' masks to the given type</summary>
	public void SetAllNodeMasksTo(TileNode.TileType mask)
	{
		if (nodesCache == null) return;
		foreach (GameObject go in nodesCache)
		{
			if (go == null) continue;
			TileNode n = go.GetComponent<TileNode>();
			if (n != null) n.tileTypeMask = mask;
		}
	}

	public void SetTileNodeMasks(TileNode.TileType mask, Transform parent)
	{
		AddOrSetTileNodeMasks(false, mask, parent, 10f);
	}

	public void SetTileNodeMasks(TileNode.TileType mask, int testAgainstCollidersLayer)
	{
		AddOrSetTileNodeMasks(false, mask, 1<<testAgainstCollidersLayer, 100f);
	}

	public void AddToTileNodeMasks(TileNode.TileType mask, Transform parent)
	{
		AddOrSetTileNodeMasks(true, mask, parent, 10f);
	}

	public void AddToTileNodeMasks(TileNode.TileType mask, int testAgainstCollidersLayer)
	{
		AddOrSetTileNodeMasks(true, mask, 1<<testAgainstCollidersLayer, 100f);
	}

	/// <summary>
	/// Add a mask value to TileNode masks (or reset to given value), but only those under the child transforms 
	/// of the parent transform. Note that this casts a ray from the transform down to the nodes and if it touches
	/// any node's collider, it will make changes to that node. Use offsetY as an offest added to the transform's
	/// y position to cast the ray from. Usefull if you know some of your transforms might be at positions lower
	/// or inside the tilenode colliders.
	/// </summary>
	public void AddOrSetTileNodeMasks(bool isAdd, TileNode.TileType mask, Transform parent, float offsetY)
	{
		if (nodesCache == null || parent == null) return;

		// gonna need the colliders on for this, so turn 'em on
		foreach (GameObject go in nodesCache)
		{
			if (go == null) continue;
			go.GetComponent<Collider>().enabled = true;
		}

		// run through the child transforms
		foreach (Transform tr in parent.transform)
		{
			LayerMask rayMask = (1 << tilesLayer);
			Vector3 pos = tr.position; pos.y = offsetY;
			RaycastHit hit;
			if (Physics.Raycast(pos, -Vector3.up, out hit, offsetY * 2f, rayMask))
			{
				TileNode node = hit.collider.GetComponent<TileNode>();
				if (node != null)
				{
					if (isAdd) node.tileTypeMask = (node.tileTypeMask | mask);
					else node.tileTypeMask = mask;
				}
			}
		}

		// done, disable the colliders
		foreach (GameObject go in nodesCache)
		{
			if (go == null) continue;
			go.GetComponent<Collider>().enabled = false;
		}
	}

	public void AddOrSetTileNodeMasks(bool isAdd, TileNode.TileType mask, LayerMask testAgainstCollidersLayerMask, float offsetY)
	{
		if (nodesCache == null) return;

		// run through nodes, cast a ray against provided mask and make changes to node if needed
		foreach (GameObject go in nodesCache)
		{
			if (go == null) continue;

			Vector3 pos = go.transform.position; pos.y = offsetY;
			RaycastHit hit;
			if (Physics.Raycast(pos, -Vector3.up, out hit, offsetY * 2f, testAgainstCollidersLayerMask))
			{
				TileNode node = go.GetComponent<TileNode>();
				if (node != null)
				{
					if (isAdd) node.tileTypeMask = (node.tileTypeMask | mask);
					else node.tileTypeMask = mask;
				}
			}
		}
	}

	/// <summary>
	/// Cast rays down from each node (node height + a little height) and check if hit a collider on the given.
	/// The node's height is adjusted to that of the hit offset, if anything hit in the layer.
	/// If unlinkIsActive is true, then nodes that did not hit any layer will be deleted.
	/// </summary>
	public void SetupNodeHeights(LayerMask checkAgainstLayer, bool unlinkIsActive)
	{
		if (nodesCache == null) return;
		if (nodesCache.Length == 0) return;

		foreach (GameObject go in nodesCache)
		{
			if (go == null) continue;
			Transform tr = go.transform;
			LayerMask rayMask = (1 << checkAgainstLayer);
			Vector3 pos = tr.position; pos.y = 100f;
			RaycastHit hit;
			if (Physics.Raycast(pos, -Vector3.up, out hit, 200f, rayMask))
			{
				pos.y = hit.point.y;
				tr.position = pos;
			}
			else if (unlinkIsActive)
			{
				// node is not over something usefull (like terrain), delete it
				TileNode node = go.GetComponent<TileNode>();
				node.Unlink();
                #if UNITY_EDITOR
				DestroyImmediate(go);
                #else
				Destroy(go);
                #endif
			}
		}
	}

	#endregion
	
    
    // ====================================================================================================================
    #region 耎甶よ猭

    public TileNode f_GetNodeForIndex(int iIndex)
    {
        if (!m_aDicNodes.ContainsKey(iIndex))
        {
            //MessageBox.DEBUG("f_GetNodeForIndex 无此Index对应的TileNode信息" + iIndex);
            return null;
        }
        return m_aDicNodes[iIndex];
    }

    /// <summary>
    /// 8よ锣传よ猭
    /// </summary>
    /// <param name="Pos"></param>
    /// <returns></returns>
    public TileNode f_GetTileNodeForPosition(Vector3 Pos)
    {
        Vector3 v3LocalPos = transform.InverseTransformPoint(Pos);
        int iIndex = 0;
        if (tilesLayout == MapNav.TilesLayout.Square_4)
        {
            int iIndexY = (int)(v3LocalPos.z / Get_yOffs() + 1);         
            int iIndexX = (int)((v3LocalPos.x) / Get_xOffs() + 1);
            iIndex = f_GetIndexForIndexXY(iIndexX, iIndexY);

        }
        else
        {
            float fF = 1f;
            int iIndexY = (int)(v3LocalPos.z / Get_yOffs() + 1);
            if (iIndexY % 2 == 0)
            {
                fF = 0;
            }
            int iIndexX = (int)((v3LocalPos.x - fF) / (Get_xOffs() * 2) + 1);
            iIndex = f_GetIndexForIndexXY(iIndexX, iIndexY);
        }
        //MessageBox.DEBUG(iIndex + " f_GetTileNodeForPosition " + v3LocalPos + " iIndexX " + iIndexX + " iIndexY " + iIndexY);

        return f_GetNodeForIndex(iIndex);
    }

    //public Vector2Map f_GetNodeForPositon(Vector3 Pos)
    //{
    //    Vector3 v3LocalPos = transform.InverseTransformPoint(Pos);

    //    int iIndexX = (int)(v3LocalPos.x / Get_xOffs() + 0.5);
    //    int iIndexY = (int)(v3LocalPos.z / Get_yOffs() + 0.5);

    //    Vector2Map __IndexPos = new Vector2Map(iIndexX, iIndexY);

    //    //MessageBox.DEBUG("HitMap" + Pos + ">>" + v3LocalPos + ">>" + __IndexPos.x + " " + __IndexPos.y);

    //    return __IndexPos;
    //}
        
    public int f_GetIndexForIndexXY(int iIndexX, int iIndexY)
    {
        //return iIndexY * nodesYCount + iIndexX;
		return iIndexY * nodesXCount + iIndexX;
    }

    public TileNode f_GetNodeForIndexXY(int iIndexX, int iIndexY)
    {
        return f_GetNodeForIndex(f_GetIndexForIndexXY(iIndexX, iIndexY));
    }

    public Vector2Map f_GetPosForIndex(int iIndex)
    {
        Vector2Map Pos = new Vector2Map();
        Pos.x = iIndex % nodesXCount;
        Pos.y = iIndex / nodesXCount;
        return Pos;
    }

    public float Get_xOffs()
    {
        float xOffs = 0;
        if (tilesLayout == MapNav.TilesLayout.Hex)
        {	// calculate offset to correctly plate hextiles
            xOffs = Mathf.Sqrt(3f) * tileSpacing * 0.5f;
            //             offs = xOffs * 0.5f;
            //             yOffs = yOffs * 0.75f;
        }
        else if (tilesLayout == MapNav.TilesLayout.Square_8)
        {
            xOffs = Mathf.Sqrt(3f) * tileSpacing * 0.58f;       //xOffs	1.004589	float
            xOffs = xOffs * 0.5f;                                    //offs	0.5022947	float
        }
        else if (tilesLayout == MapNav.TilesLayout.Square_4)
        {
            xOffs = 1;
        }

        return xOffs;
    }

    public float Get_yOffs()
    {
        float yOffs = tileSpacing;
        if (tilesLayout == MapNav.TilesLayout.Hex)
        {	// calculate offset to correctly plate hextiles
            //xOffs = Mathf.Sqrt(3f) * tileSpacing * 0.5f;
            //offs = xOffs * 0.5f;
            yOffs = 0.75f;
        }
        else if (tilesLayout == MapNav.TilesLayout.Square_8)
        {
            //xOffs = Mathf.Sqrt(3f) * tileSpacing * 0.58f;       //xOffs	1.004589	float
            //offs = xOffs * 0.5f;                                    //offs	0.5022947	float
            yOffs = yOffs * 0.25f;                              //yOffs	0.24	float
        }
        else if (tilesLayout == MapNav.TilesLayout.Square_4)
        {
            yOffs = 1;
        }
        return yOffs;
    }


    //public void f_TileNodeColliderOP(bool show)
    //{
    //    if (nodes == null) return;

    //    foreach (TileNode n in nodes)
    //    {
    //        if (n == null)
    //        {
    //            continue;
    //        }
    //        n.ColliderOP(show);
    //    }
    //}

    public void f_TileNodeShow(bool show)
    {
        if (nodes == null) return;

        foreach (TileNode n in nodes)
        {
            if (n == null)
            {
                continue;
            }
            if (show)
            {
                n.f_Show();
            }
            else
            {
                n.f_UnShow();
            }            
        }
    }

    public void f_EditTileNodeShow(bool show)
    {
        if (nodes == null) return;

        foreach (TileNode n in nodes)
        {
            if (n == null)
            {
                continue;
            }
            if (show)
            {
                n.f_EditShow();
            }
            else
            {
                n.f_UnShow();
            }            
        }
    }

    public void f_UpdateTileNodeShow()
    {
        if (nodes == null) return;

        foreach (TileNode n in nodes)
        {
            if (n == null)
            {
                continue;
            }
            n.f_UpdateTileNodeShow();
        }
    }



    #endregion

    // ====================================================================================================================
    List<GameObject> _aGameObj = new List<GameObject>();
    void CreateTestBall(int iIndex, Vector3 Pos)
    {
        Pos.y = 10;
        GameObject obj1 = GameObject.CreatePrimitive(PrimitiveType.Cube);
        obj1.transform.position = Pos;
        obj1.transform.localScale = new Vector3(0.2f, 0.2f, 0.2f);
        obj1.name = "" + iIndex;
        _aGameObj.Add(obj1);
    }


    //void Update()
    //{
    //    if (Input.GetMouseButton(0))
    //    {
    //        f_TTTT();
    //    }


    //}

    //List<TileNode> aPathNode = null;
    //TileNode tTileNodeS, tTileNodeE;
    //public void f_TTTT()
    //{
    //    UnityEngine.Debug.Log("-------------------------------------------------------");

    //    for (int i = (_aGameObj.Count - 1); i >= 0; i--)
    //    {
    //        Destroy(_aGameObj[i]);
    //        _aGameObj.RemoveAt(i);
    //    }
    //    if (aPathNode != null)
    //    {
    //        tTileNodeS.f_UnShow();
    //        tTileNodeE.f_UnShow();
    //        for (int i = 0; i < aPathNode.Count; i++)
    //        {
    //            aPathNode[i].f_UnShow();
    //        }
    //    }

    //    //int iNodeS = 100;// UnityEngine.Random.Range(10, 100);
    //    //int iNodeE = 502;// UnityEngine.Random.Range(45 * 30, 45 * 44);      
    //    int iNodeS = 100;// UnityEngine.Random.Range(10, 100);
    //    int iNodeE = UnityEngine.Random.Range(1100, 99 * 50);      
        

    //    tTileNodeS = f_GetNodeForIndex(iNodeS);
    //    tTileNodeE = f_GetNodeForIndex(iNodeE);

    //    System.Diagnostics.Stopwatch stopwatch = new Stopwatch();

    //    stopwatch.Start(); //  开始监视代码运行时间
    //    aPathNode = GetPath(tTileNodeS, tTileNodeE, TileNode.TileType.Normal);
    //    stopwatch.Stop(); //  停止监视

    //    TimeSpan timespan = stopwatch.Elapsed; //  获取当前实例测量得出的总时间
    //    double hours = timespan.TotalHours; // 总小时
    //    double minutes = timespan.TotalMinutes;  // 总分钟
    //    double seconds = timespan.TotalSeconds;  //  总秒数
    //    double milliseconds = timespan.TotalMilliseconds;  //  总毫秒数
    //    UnityEngine.Debug.Log(iNodeS + ">>" + iNodeE + "------ seconds:" + seconds + " milliseconds:" + milliseconds + "-------------------");
    //    UnityEngine.Debug.Log("AAAAAAAAA" + timespan.ToString());
        

    //    //stopwatch.Start(); //  开始监视代码运行时间
    //    //TileNode[] aaaa = GetPath(tTileNodeS, tTileNodeE, TileNode.TileType.Normal);
    //    //stopwatch.Stop(); //  停止监视

    //    //timespan = stopwatch.Elapsed; //  获取当前实例测量得出的总时间
    //    //hours = timespan.TotalHours; // 总小时
    //    //minutes = timespan.TotalMinutes;  // 总分钟
    //    //seconds = timespan.TotalSeconds;  //  总秒数
    //    //milliseconds = timespan.TotalMilliseconds;  //  总毫秒数

    //    //UnityEngine.Debug.Log(iNodeS + ">>" + iNodeE + "------ seconds:" + seconds + " milliseconds:" + milliseconds + "-------------------");
    //    //UnityEngine.Debug.Log("------" + timespan.ToString());


    //    if (aPathNode == null)
    //    {
    //        UnityEngine.Debug.Log("失败");
    //    }

    //    tTileNodeS.f_Show();
    //    tTileNodeE.f_Show();
    //    Vector3[] aArray = new Vector3[aPathNode.Count];
    //    for (int i = 0; i < aPathNode.Count; i++)
    //    {
    //        aArray[i] = aPathNode[i].transform.position;
    //        aPathNode[i].f_Show();
    //        CreateTestBall(i, aArray[i]);
    //    }
        

    //}


}
