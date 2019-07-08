using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BodyPartGetHurt : MonoBehaviour
{
    //設定被攻擊部位的血量
    public int Damage = 1;
    public TRexHP hpController;

    // Use this for initialization
    void Start()
    {
        //仔入最高層級物件中的TRexHP腳本
        hpController = transform.root.GetComponent<TRexHP>();
        GetComponent<CapsuleCollider>().isTrigger = true;
    }

    // Update is called once per frame
    void Update()
    {

    }
    void OnTriggerEnter(Collider other)
    {
        if (other.transform.gameObject.name.Contains("bullet"))
        {
            //子彈消失
            Destroy(other);
            //扣除部位血量
            hpController.HP -= Damage;
           // 血量角度與位置與子彈一樣
            hpController.Blood.transform.rotation= other.transform.rotation;
            hpController.Blood.transform.position = other.transform.position;
            //噴血
            hpController.Blood.gameObject.SetActive(false);
            hpController.Blood.gameObject.SetActive(true);
        }
    }

}
