using System.Collections;
using UnityEngine;

public class _b03_DestroyTimer : MonoBehaviour
{
    [Space(5.0f)]
    [TextArea(1, 2)]
    public string Note =
        "這程式是用來自動執行自毀的\n" +
        "當過了dTime秒後,該物件銷毀。";

    [Header("輸入等待時間")]
    public float dTime;

    void Update()
    {
        StartCoroutine("WaitDTimer");
    }

    //倒數程式
    IEnumerator WaitDTimer()
    {
        //當過了sTime秒後,就執行指定的程式
        yield return new WaitForSeconds(dTime*Time.deltaTime);
        Destroy(gameObject);
    }
}
