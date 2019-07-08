using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//方向定义        
//        A上
//    H       B右上
//G               C右
//    F       D右下
//        E下
//ABCDEFGH对应为01234567
public enum FACE2WAY
{
	/// <summary>
	/// A上
	/// </summary>
	eWayU = 0,

	/// <summary>
	/// B右上
	/// </summary>
	eWayRU = 1,

	/// <summary>
	/// C右
	/// </summary>
	eWayR = 2,

	/// <summary>
	/// D右下
	/// </summary>
	eWayRD = 3,

	/// <summary>
	/// E下
	/// </summary>
	eWayD = 4,

	/// <summary>
	/// F左下
	/// </summary>
	eWayLD = 5,

	/// <summary>
	/// G左
	/// </summary>
	eWayL = 6,

	/// <summary>
	/// H左上
	/// </summary>
	eWayLU = 7,


}