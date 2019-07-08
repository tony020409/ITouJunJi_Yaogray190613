using UnityEngine;
using System.Collections;
using UnityEditor;
using System.IO;

public class CreateMapDataTools
{
    static void CreatePath(string path)
    {
        string NewPath = path.Replace("\\", "/");

        string[] strs = NewPath.Split('/');
        string p = "";

        for (int i = 0; i < strs.Length; ++i)
        {
            p += strs[i];

            if (i != strs.Length - 1)
            {
                p += "/";
            }

            if (!Path.HasExtension(p))
            {
                if (!Directory.Exists(p))
                    Directory.CreateDirectory(p);
            }
        }

    }

    static void PackSC1(string strFileName, string strDestPath, string p, System.Type type)
    {
        PackSC(strFileName, strDestPath, p, BuildTarget.iOS, type);
        PackSC(strFileName, strDestPath, p, BuildTarget.Android, type);
        PackSC(strFileName, strDestPath, p, BuildTarget.StandaloneWindows, type);
    }

    static void PackSC(string strFileName, string strDestPath, string p, BuildTarget target, System.Type type)
    {
        UnityEngine.Object tmpObject = AssetDatabase.LoadAssetAtPath(p, type);

        //打包版本
        //string dest = "Assetbundles/" + Platform.GetPlatformFolder(target) + "/SC/" + strFileName + ".bytes";
        string dest = strDestPath + "/" + target.ToString() + "/" + strFileName + ".bytes";
        dest = dest.Replace("\\", "/");
        dest = dest.Replace("//", "/");
        //dest = dest.ToLower();

        CreatePath(dest);

        //Debug.Log("dest Path = " + dest);
        tmpObject.name = strFileName;
        if (BuildPipeline.BuildAssetBundle(tmpObject, null, dest, BuildAssetBundleOptions.CollectDependencies | BuildAssetBundleOptions.CompleteAssets, target))    //BuildAssetBundleOptions.CollectDependencies | BuildAssetBundleOptions.CompleteAssets
        {
        }
        else
        {
            Debug.LogError("BuildPipeline.BuildAssetBundle 失败 " + dest);
        }
        //DoMd5(dest);

        Debug.Log(target.ToString() + " 打包成功 " + dest);
    }

    static bool CreateMapData(MapNavData tMapNavData)
    {
        Object selectedObject = Selection.activeObject;
        if (selectedObject != null)
        {
            if (selectedObject.name == "MapNav")
            {
                MapNav tMapNav = ((GameObject)selectedObject).GetComponent<MapNav>();
                for (int i = 0; i < tMapNav.nodes.Length; i++ )
                {
                    MapNavDT tMapNavDT = new MapNavDT();
                    TileNode tTileNode = tMapNav.nodes[i];

                    tMapNavDT.m_iIndex = (short)tTileNode.idx;
                    tMapNavDT.m_iX = (short)tTileNode.m_iIndexX;
                    tMapNavDT.m_iY = (short)tTileNode.m_iIndexY;
                    tMapNavDT.m_fU3dX = tTileNode.transform.position.x;
                    tMapNavDT.m_fU3dY = tTileNode.transform.position.z;
                    tMapNavDT.m_TileType = (byte)tTileNode.tileTypeMask;
                    int iAA = 0;
                    for (int j = 0; j < tTileNode.nodeLinks.Length; j++)
                    {
                        if (tTileNode.nodeLinks[j] != null)
                        {
                            iAA++;
                        }
                    }
                    tMapNavDT.m_aLinkTileNode = new short[iAA];
                    tMapNavDT.m_aNodeLinkFACE2WAY = new byte[iAA];
                    iAA = 0;
                    for (int j = 0; j < tTileNode.nodeLinks.Length; j ++ )
                    {
                        if (tTileNode.nodeLinks[j] != null)
                        {
                            tMapNavDT.m_aLinkTileNode[iAA] = (short)tTileNode.nodeLinks[j].idx;
                            tMapNavDT.m_aNodeLinkFACE2WAY[iAA] = (byte)tTileNode.m_aNodeLinkFACE2WAY[j];
							iAA++;
                        }
                    }
                    tMapNavData.m_aData.Add(tMapNavDT);
                }
                return true;
            }
        }
        return false;
    }

    static void Exec( )
    {
        string strDestPath = "G:\\NightMarketCloud_4\\svn\\Project\\NMC217\\Assets\\Resources\\Map\\MapNavData";
        //string strFileName = Path.GetFileNameWithoutExtension(strScrPath);
        string time = "A";   // Common.CurrTimeString;
        string p = "Assets\\AAAtime.asset";

        MapNavData tMapNavData = ScriptableObject.CreateInstance<MapNavData>();
        if (CreateMapData(tMapNavData))
        {
            AssetDatabase.CreateAsset(tMapNavData, p);
            PackSC1("MapNavData", strDestPath, p, typeof(MapNavData));
        }

    }

   [MenuItem("Window/Map and Nav/CreateMapData")]
    public static void BuildStaticForWindows()
    {
        Exec();
    }


}