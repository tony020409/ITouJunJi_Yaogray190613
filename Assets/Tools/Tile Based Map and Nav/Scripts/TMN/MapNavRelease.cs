using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;

public class MapNavRelease
{

    private bool _bInitOK = false;

    public Dictionary<int, TileNode> m_aDicNodes = new Dictionary<int, TileNode>();

    public int nodesXCount = 0, nodesYCount = 0;


    private IEnumerator LoadData()
    {
        //string AssetBundlesOutputPath = Application.dataPath;
        //AssetBundlesOutputPath = AssetBundlesOutputPath + "\\Resources\\Map\\MapNavData";
        string AssetBundlesOutputPath = "Map\\MapNavData";
        string strTargetPlatform = "";
#if UNITY_WEBPLAYER
         strTargetPlatform = "WebPlayer";
#elif UNITY_ANDROID
         strTargetPlatform = "Android";
#elif UNITY_IPHONE
         strTargetPlatform = "IOS";
#else
        strTargetPlatform = "StandaloneWindows";
#endif
        AssetBundlesOutputPath = Path.Combine(AssetBundlesOutputPath, strTargetPlatform) + "\\MapNavData";

        TextAsset bindata = Resources.Load(AssetBundlesOutputPath) as TextAsset;

        AssetBundleCreateRequest tAbcRequest = AssetBundle.LoadFromMemoryAsync(bindata.bytes);

        yield return tAbcRequest;

        MapNavData tMapNavData = tAbcRequest.assetBundle.mainAsset as MapNavData;
        tAbcRequest.assetBundle.Unload(false);
        tAbcRequest = null;

        CreateMapData(tMapNavData);

        TileNode FormTileNode = f_GetNodeForIndex(0);
        TileNode ToTileNode = f_GetNodeForIndex(1);
        List<TileNode> aPathNode = GetPath(FormTileNode, ToTileNode, TileNode.TileType.Normal);

        Debug.Log("aPathNode " + aPathNode.Count);
    }

    private void CreateMapData(MapNavData tMapNavData)
    {
        for (int i = 0; i < tMapNavData.m_aData.Count; i++)
        {
            MapNavDT tMapNavDT = tMapNavData.m_aData[i];
            GameObject Obj = new GameObject();
            Obj.name = "" + tMapNavDT.m_iIndex;
            TileNode tTileNode = Obj.AddComponent<TileNode>();
            tTileNode.f_CreateForMapData(tMapNavDT);
            m_aDicNodes.Add(tTileNode.idx, tTileNode);
        }

        foreach (KeyValuePair<int, TileNode> tItem in m_aDicNodes)
        {
            short[] aLinkNode = tItem.Value.f_GeteLinkData();
            TileNode[] nodeLinks = new TileNode[aLinkNode.Length];
            for (int i = 0; i < aLinkNode.Length; i++ )
            {
                nodeLinks[i] = f_GetNodeForIndex(aLinkNode[i]);
            }
            tItem.Value.f_SaveLinkNode(nodeLinks);
        }

    }

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
            //return nodesCache[GetIndexForXY(iX, iY)].GetComponent<TileNode>();
        }
        return null;
    }

    private int GetIndexForXY(int iX, int iY)
    {
        return iY * nodesXCount + iX;
    }


#region 扩展方法

    public TileNode f_GetNodeForIndex(int iIndex)
    {
        if (!m_aDicNodes.ContainsKey(iIndex))
        {
            MessageBox.DEBUG("f_GetNodeForIndex 无此Index对应的TileNode信息" + iIndex);
            return null;
        }
        return m_aDicNodes[iIndex];
    }



    int iIndex;
    List<TileNode> _aTmpTileNode = new List<TileNode>();
    Dictionary<int, TileNode> _aOpenList = new Dictionary<int, TileNode>();
    Dictionary<int, TileNode> _aCloseList = new Dictionary<int, TileNode>();
    public List<TileNode> GetPath(TileNode fromNode, TileNode toNode, TileNode.TileType validNodesLayer)
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

                // check if there is link switch between the two nodes and if it is on or off
                //if (tn.linkOnOffSwitch != null)
                //{
                //    if (tn.linkOnOffSwitch.LinkIsOn(n) == 0) continue;
                //}
                //if (n.linkOnOffSwitch != null)
                //{
                //    if (n.linkOnOffSwitch.LinkIsOn(tn) == 0) continue;
                //}
                
                // calc G & H
                int G = tn.PathG + DistanceConst;
                int H = Mathf.Abs(toNode.m_iDis - n.m_iDis);

                //H = DistanceConst * Mathf.Abs((int)Vector3.Distance(n.transform.position, toNode.transform.position));

                //BDFH
                if (n.m_aNodeLinkFACE2WAY.Length > iIndex)
                {
                    if (n.m_aNodeLinkFACE2WAY[iIndex] == FACE2WAY.eWayRU ||
                        n.m_aNodeLinkFACE2WAY[iIndex] == FACE2WAY.eWayRD ||
                        n.m_aNodeLinkFACE2WAY[iIndex] == FACE2WAY.eWayLD ||
                        n.m_aNodeLinkFACE2WAY[iIndex] == FACE2WAY.eWayLU)
                    {
                        H -= 1;
                    }
                }
                // check if there are movement modifiers
                //if (n.movesMod != null)
                //{
                //    foreach (TNEMovementModifier.MovementInfo m in n.movesMod.moveInfos)
                //    {
                //        if (m.tileType == validNodesLayer) G += DistanceConst * m.movesModifier;
                //    }
                //}

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





}