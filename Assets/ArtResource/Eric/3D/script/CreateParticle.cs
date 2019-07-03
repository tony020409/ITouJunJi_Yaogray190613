using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[System.Serializable]
public class ChangObj{
	public GameObject obj;
	public GameObject Parent;
	public float DestroyTime;
    public float NullParentTime;
}

public class CreateParticle : MonoBehaviour {
	public ChangObj[] events;
    private List<GameObject> tempList;
    private GameObject tempobj;
    private int i;
    void Start() {
        tempList = new List<GameObject>();
    }
    public void CreateObj(int index){
		if(index>=events.Length){
			Debug.Log( "給的參數大於設定項目" );
			return;
		}
		if(events[index].Parent == null){
			tempobj = Instantiate(events[index].obj)as GameObject;
            tempList .Add(tempobj);
            i = tempList.Count-1;
        } else if (events[index].Parent != null) {
            tempobj = Instantiate(events[index].obj) as GameObject;
            tempobj.transform.parent = events[index].Parent.transform;
            tempobj.transform.localPosition = Vector3.zero;
            tempobj.transform.localRotation = events[index].Parent.transform.localRotation;
            if (events[index].NullParentTime != 0) {
                StartCoroutine(LeaveParent(tempobj,events[index].NullParentTime));
            }
            tempList.Add(tempobj);
            i = tempList.Count-1;
        }

        if (events[index].DestroyTime != 0) {
            Destroy(tempList[i] ,events[index].DestroyTime);
            tempList.RemoveAt(i);
        }
	}

    private IEnumerator LeaveParent(GameObject g , float t) {
        yield return new WaitForSeconds(t);
        if (g) {
            g.transform.parent = null;
        }
    }
}



