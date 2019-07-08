using System.Collections;
using System.Collections.Generic;
using UnityEngine;
 
public class Create_Moster : MonoBehaviour
{

    [Header("註解用、怪物編號、要幾隻")]
    public string Note = "這是..."; //註解用
    public int id;                  //怪物編號
    public int amount = 0;          //要幾隻
    private int cipher = 0;         //計算用

    // Use this for initialization
    void Start () {}
	
	// Update is called once per frame
	void Update ()
    {
        if (id != null){
            Create();
        }
    }

    void Create(){
        for (int i = 0; i < amount; i++){
            BattleMain.GetInstance().CreateComputerMonter2(id);
            cipher++;
        }
        if (cipher >= amount){
            Destroy(this);
        }
    }
}
