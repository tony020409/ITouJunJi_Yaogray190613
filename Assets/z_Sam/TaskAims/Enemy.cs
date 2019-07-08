using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sam;

public class Enemy : MonoBehaviour
{
    public GameObject Arrow;
    public SphereCollider SC;
    public GameObject[] ArrowArray;
    public SphereCollider[] SCArray;
    // Use this for initialization

    public int _iTargetIndex ; 
        
    public void getAttackSite(int k)
    {
        if (k > ArrowArray.Length)
        {
            MessageBox.ASSERT("getAttackSite k Ero");
            return;
        }
        for (int i = 0; i < ArrowArray.Length; i++)
        {
            ArrowArray[i].gameObject.SetActive(false);
            SCArray[i].enabled = false;
        }
        Arrow = ArrowArray[k];
        SC = SCArray[k];
        SCArray[k].enabled = true;
        _iTargetIndex = k;
     //   NeedlegunTrack n1 = GetComponent<NeedlegunTrack>();
     //  n1.AttackSite = n1.AttackSiteArray[k];
    }

    public GameObject f_GetCurTargetObj(int iIndex)
    {
        if (iIndex > ArrowArray.Length)
        {
            MessageBox.ASSERT("getAttackSite k Ero");
            return null;
        }
        for (int i = 0; i < ArrowArray.Length; i++)
        {
            ArrowArray[i].gameObject.SetActive(false);
            SCArray[i].enabled = false;
        }
        Arrow = ArrowArray[iIndex];
        SC = SCArray[iIndex];
        SCArray[iIndex].enabled = true;
        _iTargetIndex = iIndex;

        return Arrow;
    }



}

