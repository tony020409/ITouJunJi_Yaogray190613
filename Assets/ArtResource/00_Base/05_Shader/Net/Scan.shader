Shader "World of Zero/Scan"
{
	Properties
	{
		_MainTex("Texture", 2D) = "white" {}
		_ScanDistance("Scan Distance", float) = 0
		_ScanWidth("Scan Width", float) = 10
		_ScanColor("Scan Color", Color) = (1, 1, 1, 0)
	}
		SubShader
		{
			// Cull Off
			// ZTest Always

			Pass
			{
				CGPROGRAM
				#pragma vertex vert
				#pragma fragment frag
				#include "UnityCG.cginc"

				// 顶点着色器需要的数据类型
				struct appdata
				{
					float4 vertex : POSITION;
					float2 uv : TEXCOORD0;
				};

		// 顶点着色器处理后传递给片段着色器的数据类型
		struct v2f
		{
			float4 vertex : SV_POSITION;
			float2 uv : TEXCOORD0;
			float2 uv_depth : TEXCOORD1;
		};

		// 内置变量：相机的世界坐标
		float4 _CameraWS;
		// 内置变量：主贴图像素尺寸大小，值是Vector4(1 / width, 1 / height, width, height)
		float4 _MainTex_TexelSize;

		// 顶点着色器
		v2f vert(appdata v)
		{
			v2f o;
			o.vertex = UnityObjectToClipPos(v.vertex);
			o.uv = v.uv;
			o.uv_depth = v.uv;
			return o;
		}

		// 主贴图
		sampler2D _MainTex;

		// 内置变量：深度图
		sampler2D _CameraDepthTexture;

		// 扫描距离
		float _ScanDistance;

		// 扫描网宽度
		float _ScanWidth;

		// 扫描网的颜色
		float4 _ScanColor;

		// 指定返回值类型是被SV_Target限定的类型
		half4 frag(v2f i) : SV_Target
		{
			// 片段着色器着色（图像采样）
			half4 col = tex2D(_MainTex, i.uv);

			// 获取深度信息
			float depth = UNITY_SAMPLE_DEPTH(tex2D(_CameraDepthTexture, i.uv));

			// 将深度值转换为线性的值 （0~1之间）
			float linear01Depth = Linear01Depth(depth);

			// 绘制需要变色的扫描区域
			if (
				linear01Depth < _ScanDistance &&
				linear01Depth > _ScanDistance - _ScanWidth &&
				linear01Depth < 1)
			{
				float diff = 1 - (_ScanDistance - linear01Depth) / (_ScanWidth);
				_ScanColor *= diff;
				return col + _ScanColor;
			}

			return col;
		}
		ENDCG
	}
		}
}
