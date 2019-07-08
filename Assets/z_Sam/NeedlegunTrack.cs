using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
namespace Sam
{

    public enum Aimscategory
    {
        SmallPter_Big,//大翼手龍
        Trex,//暴龍
    }
    public class NeedlegunTrack : MonoBehaviour
    {
        public GameObject AttackSite;
        public GameObject []AttackSiteArray;
        public Aimscategory aimscategory;
        public float totalAttackpower;
        public int attackfrequency;
        public float TotalAttackQuantity = 50;
        public float HurtDecreasing = 3;//攻擊遞減
        public List<float> Attackaddition; //攻擊加成

        // Use this for initialization
        void Start()
        {
            //for (int i = 0; i < 1000; i++)
            //{
            //    Attackaddition.Add(TotalAttackQuantity - (i * HurtDecreasing));
            //}
        }
        // Update is called once per frame
        void Update()
        {

        }

        public void Calculation()
        {
            float f = 0;
            for (int i = 0; i < attackfrequency; i++)
            {
                f += Attackaddition[i];
            }

            totalAttackpower = f;
        }
    }
}
