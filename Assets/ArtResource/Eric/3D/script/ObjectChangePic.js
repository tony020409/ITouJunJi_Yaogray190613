#pragma strict

var event : ChangObj[];

function ObjChangePic( a : int ){
	if( a >= event.Length ){
		Debug.Log( "給的參數大於設定項目" );
		return;
	}
	if( event[a].Obj == null || event[a].Pic == null ){
		Debug.Log( a + "項目未設定完整" );
		return;
	}
	event[a].Obj.material.mainTexture = event[a].Pic;
}

class ChangObj{
	var Obj : Renderer;
	var Pic : Texture;
}