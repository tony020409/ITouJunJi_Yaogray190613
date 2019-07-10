using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class sore3_change : MonoBehaviour {

    private float ts_x;
    private float ts_y;
    private float ts_z;
    public Text input_ts_x;
    public Text input_ts_y;
    public Text input_ts_z;



    private float ro_x;
    private float ro_y;
    private float ro_z;
    public Text input_ro_x;
    public Text input_ro_y;
    public Text input_ro_z;

    public Transform oil;

	// Use this for initialization
	void Start () {
        //input_ts_x.text = "-2.21";
        //input_ts_y.text = "-0.63";
        //input_ts_z.text = "-0.6";

        //input_ro_x.text = "16.261";
        //input_ro_y.text = "-35.262";
        //input_ro_z.text = "-13.68";
    }

    // Update is called once per frame
    void Update()
    {
        oil.localPosition = new Vector3(float.Parse(input_ts_x.text), float.Parse(input_ts_y.text), float.Parse(input_ts_z.text));
        oil.eulerAngles = new Vector3(float.Parse(input_ro_x.text), float.Parse(input_ro_y.text), float.Parse(input_ro_z.text));
    }
}
