// Upgrade NOTE: replaced '_Object2World' with 'unity_ObjectToWorld'
// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

// Shader created with Shader Forge v1.17 
// Shader Forge (c) Neat Corporation / Joachim Holmer - http://www.acegikmo.com/shaderforge/
// Note: Manually altering this data may prevent you from opening it in Shader Forge
/*SF_DATA;ver:1.17;sub:START;pass:START;ps:flbk:,iptp:0,cusa:False,bamd:0,lico:1,lgpr:1,limd:3,spmd:1,trmd:0,grmd:0,uamb:True,mssp:True,bkdf:True,hqlp:False,rprd:True,enco:False,rmgx:True,rpth:1,hqsc:True,nrmq:1,nrsp:0,vomd:0,spxs:False,tesm:0,culm:0,bsrc:0,bdst:1,dpts:2,wrdp:True,dith:0,rfrpo:True,rfrpn:Refraction,coma:15,ufog:True,aust:True,igpj:False,qofs:0,qpre:1,rntp:1,fgom:False,fgoc:False,fgod:False,fgor:False,fgmd:0,fgcr:0.5,fgcg:0.5,fgcb:0.5,fgca:1,fgde:0.01,fgrn:0,fgrf:300,ofsf:0,ofsu:0,f2p0:False;n:type:ShaderForge.SFN_Final,id:2865,x:32880,y:32703,varname:node_2865,prsc:2|diff-6343-OUT,spec-358-OUT,gloss-1813-OUT,normal-701-OUT,difocc-3987-OUT,spcocc-3987-OUT;n:type:ShaderForge.SFN_Multiply,id:6343,x:32179,y:32552,varname:node_6343,prsc:2|A-4484-OUT,B-6665-RGB;n:type:ShaderForge.SFN_Color,id:6665,x:31964,y:32572,ptovrint:False,ptlb:Color,ptin:_Color,varname:_Color,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,c1:0.5019608,c2:0.5019608,c3:0.5019608,c4:1;n:type:ShaderForge.SFN_Slider,id:358,x:32394,y:32789,ptovrint:False,ptlb:Metallic,ptin:_Metallic,varname:node_358,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,min:0,cur:0,max:1;n:type:ShaderForge.SFN_Slider,id:1813,x:32394,y:32880,ptovrint:False,ptlb:Gloss,ptin:_Gloss,varname:_Metallic_copy,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,min:0,cur:0,max:1;n:type:ShaderForge.SFN_Lerp,id:701,x:32332,y:33348,varname:node_701,prsc:2|A-5341-OUT,B-3865-OUT,T-6385-OUT;n:type:ShaderForge.SFN_Tex2d,id:3353,x:31016,y:31814,varname:node_3353,prsc:2,tex:5b1dd272519103d4582d7f14c4e7db96,ntxv:0,isnm:False|TEX-5275-TEX;n:type:ShaderForge.SFN_Tex2d,id:9673,x:31356,y:32347,varname:node_9673,prsc:2,ntxv:0,isnm:False|UVIN-1886-OUT,TEX-9963-TEX;n:type:ShaderForge.SFN_ComponentMask,id:6512,x:31089,y:32821,varname:node_6512,prsc:2,cc1:1,cc2:-1,cc3:-1,cc4:-1|IN-781-OUT;n:type:ShaderForge.SFN_Clamp01,id:9883,x:31284,y:32831,varname:node_9883,prsc:2|IN-6512-OUT;n:type:ShaderForge.SFN_Round,id:8094,x:31676,y:32820,varname:node_8094,prsc:2|IN-4418-OUT;n:type:ShaderForge.SFN_Tex2dAsset,id:5275,x:30789,y:31781,ptovrint:False,ptlb:Aldebo,ptin:_Aldebo,varname:node_5275,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,tex:5b1dd272519103d4582d7f14c4e7db96,ntxv:0,isnm:False;n:type:ShaderForge.SFN_Lerp,id:4484,x:31952,y:32331,varname:node_4484,prsc:2|A-1150-OUT,B-8863-OUT,T-5144-OUT;n:type:ShaderForge.SFN_Tex2dAsset,id:9963,x:31113,y:32408,ptovrint:False,ptlb:Mask RGBA,ptin:_MaskRGBA,varname:node_9963,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,ntxv:0,isnm:False;n:type:ShaderForge.SFN_Tex2dAsset,id:4775,x:30992,y:33222,ptovrint:False,ptlb:Normalmap,ptin:_Normalmap,varname:node_4775,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,ntxv:3,isnm:True;n:type:ShaderForge.SFN_Tex2dAsset,id:4203,x:30856,y:33897,ptovrint:False,ptlb:Mask Bump,ptin:_MaskBump,varname:node_4203,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,ntxv:3,isnm:True;n:type:ShaderForge.SFN_Tex2d,id:3919,x:31016,y:31980,varname:node_3919,prsc:2,tex:5740fd4692000774690e68cd4171f1eb,ntxv:0,isnm:False|UVIN-1657-OUT,TEX-8890-TEX;n:type:ShaderForge.SFN_Tex2dAsset,id:8890,x:30789,y:31963,ptovrint:False,ptlb:Detail,ptin:_Detail,varname:node_8890,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,tex:5740fd4692000774690e68cd4171f1eb,ntxv:0,isnm:False;n:type:ShaderForge.SFN_Tex2d,id:5196,x:30908,y:33456,ptovrint:False,ptlb:Detail Bump,ptin:_DetailBump,varname:node_5196,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,ntxv:3,isnm:True|UVIN-1657-OUT;n:type:ShaderForge.SFN_Tex2d,id:7208,x:31172,y:33239,varname:node_7208,prsc:2,ntxv:0,isnm:False|TEX-4775-TEX;n:type:ShaderForge.SFN_Blend,id:9492,x:31585,y:31809,varname:node_9492,prsc:2,blmd:10,clmp:True|SRC-5663-OUT,DST-3353-RGB;n:type:ShaderForge.SFN_Multiply,id:1657,x:30754,y:33184,varname:node_1657,prsc:2|A-4608-UVOUT,B-8528-OUT;n:type:ShaderForge.SFN_TexCoord,id:4608,x:30536,y:33073,varname:node_4608,prsc:2,uv:0;n:type:ShaderForge.SFN_Slider,id:8528,x:30448,y:33316,ptovrint:False,ptlb:Detail scale,ptin:_Detailscale,varname:node_8528,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,min:0,cur:2,max:20;n:type:ShaderForge.SFN_Tex2d,id:5495,x:31273,y:33869,varname:node_5495,prsc:2,ntxv:0,isnm:False|UVIN-1886-OUT,TEX-4203-TEX;n:type:ShaderForge.SFN_Tex2d,id:4309,x:32332,y:32995,ptovrint:False,ptlb:AO,ptin:_AO,varname:node_4309,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,ntxv:0,isnm:False;n:type:ShaderForge.SFN_NormalBlend,id:5341,x:31756,y:33339,varname:node_5341,prsc:2|BSE-9708-OUT,DTL-7510-OUT;n:type:ShaderForge.SFN_NormalBlend,id:3865,x:31792,y:33541,varname:node_3865,prsc:2|BSE-9708-OUT,DTL-374-OUT;n:type:ShaderForge.SFN_NormalVector,id:781,x:30919,y:32821,prsc:2,pt:True;n:type:ShaderForge.SFN_NormalVector,id:5963,x:30919,y:32956,prsc:2,pt:False;n:type:ShaderForge.SFN_ComponentMask,id:8735,x:31089,y:32972,varname:node_8735,prsc:2,cc1:1,cc2:-1,cc3:-1,cc4:-1|IN-5963-OUT;n:type:ShaderForge.SFN_Clamp01,id:6593,x:31292,y:32993,varname:node_6593,prsc:2|IN-8735-OUT;n:type:ShaderForge.SFN_Round,id:6385,x:32035,y:33285,varname:node_6385,prsc:2|IN-6593-OUT;n:type:ShaderForge.SFN_Multiply,id:8863,x:31578,y:32378,varname:node_8863,prsc:2|A-9673-RGB,B-1802-RGB;n:type:ShaderForge.SFN_Color,id:1802,x:31419,y:32552,ptovrint:False,ptlb:Mask Color,ptin:_MaskColor,varname:_Color_copy,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,c1:1,c2:1,c3:1,c4:1;n:type:ShaderForge.SFN_Vector3,id:7130,x:31080,y:33602,varname:node_7130,prsc:2,v1:0,v2:0,v3:1;n:type:ShaderForge.SFN_Lerp,id:8732,x:31384,y:33460,varname:node_8732,prsc:2|A-7130-OUT,B-5196-RGB,T-1907-OUT;n:type:ShaderForge.SFN_Slider,id:1907,x:30963,y:33746,ptovrint:False,ptlb:Detail Bump Int,ptin:_DetailBumpInt,varname:node_1907,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,min:0,cur:1,max:2;n:type:ShaderForge.SFN_Lerp,id:5663,x:31276,y:31990,varname:node_5663,prsc:2|A-4276-OUT,B-3919-RGB,T-3132-OUT;n:type:ShaderForge.SFN_Vector3,id:4276,x:30975,y:32120,varname:node_4276,prsc:2,v1:0.5,v2:0.5,v3:0.5;n:type:ShaderForge.SFN_Slider,id:3132,x:30739,y:32335,ptovrint:False,ptlb:Detail Diff Int,ptin:_DetailDiffInt,varname:node_3132,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,min:0,cur:0,max:1;n:type:ShaderForge.SFN_Multiply,id:5144,x:31645,y:32514,varname:node_5144,prsc:2|A-9673-A,B-8764-OUT;n:type:ShaderForge.SFN_Normalize,id:7510,x:31554,y:33399,varname:node_7510,prsc:2|IN-8732-OUT;n:type:ShaderForge.SFN_Slider,id:9969,x:30377,y:33597,ptovrint:False,ptlb:Mask scale,ptin:_Maskscale,varname:_Detailscale_copy,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,min:0,cur:2,max:20;n:type:ShaderForge.SFN_Multiply,id:1886,x:30782,y:33573,varname:node_1886,prsc:2|A-2546-UVOUT,B-9969-OUT;n:type:ShaderForge.SFN_Slider,id:2999,x:31406,y:33120,ptovrint:False,ptlb:Mask Power,ptin:_MaskPower,varname:node_2999,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,min:0,cur:1.5,max:1.5;n:type:ShaderForge.SFN_TexCoord,id:2546,x:30503,y:33441,varname:node_2546,prsc:2,uv:0;n:type:ShaderForge.SFN_Multiply,id:4418,x:31676,y:32956,varname:node_4418,prsc:2|A-2999-OUT,B-9883-OUT;n:type:ShaderForge.SFN_Lerp,id:374,x:31545,y:33829,varname:node_374,prsc:2|A-7130-OUT,B-5495-RGB,T-513-OUT;n:type:ShaderForge.SFN_Slider,id:513,x:31088,y:34036,ptovrint:False,ptlb:Mask Bump Int,ptin:_MaskBumpInt,varname:_DetailBumpInt_copy,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,min:0,cur:1,max:2;n:type:ShaderForge.SFN_Lerp,id:4817,x:32529,y:33097,varname:node_4817,prsc:2|A-4309-RGB,B-368-OUT,T-5044-OUT;n:type:ShaderForge.SFN_Vector3,id:368,x:32332,y:33153,varname:node_368,prsc:2,v1:1,v2:1,v3:1;n:type:ShaderForge.SFN_Slider,id:9412,x:32175,y:33260,ptovrint:False,ptlb:AO Power,ptin:_AOPower,varname:_MaskPower_copy,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,min:0,cur:1,max:3;n:type:ShaderForge.SFN_ComponentMask,id:3987,x:32692,y:33097,varname:node_3987,prsc:2,cc1:0,cc2:-1,cc3:-1,cc4:-1|IN-4817-OUT;n:type:ShaderForge.SFN_OneMinus,id:5044,x:32549,y:33306,varname:node_5044,prsc:2|IN-9412-OUT;n:type:ShaderForge.SFN_SwitchProperty,id:1150,x:31796,y:31984,ptovrint:False,ptlb:Detail Blend Dodge,ptin:_DetailBlendDodge,varname:node_1150,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,on:True|A-9492-OUT,B-1156-OUT;n:type:ShaderForge.SFN_Blend,id:1156,x:31585,y:32063,varname:node_1156,prsc:2,blmd:13,clmp:True|SRC-9824-OUT,DST-3353-RGB;n:type:ShaderForge.SFN_Lerp,id:9824,x:31276,y:32130,varname:node_9824,prsc:2|A-4276-OUT,B-3919-RGB,T-3132-OUT;n:type:ShaderForge.SFN_SwitchProperty,id:8764,x:31774,y:32675,ptovrint:False,ptlb:Smooth Blend,ptin:_SmoothBlend,varname:node_8764,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,on:True|A-8094-OUT,B-4418-OUT;n:type:ShaderForge.SFN_Lerp,id:9708,x:31406,y:33281,varname:node_9708,prsc:2|A-7130-OUT,B-7208-RGB,T-6639-OUT;n:type:ShaderForge.SFN_Slider,id:6639,x:31093,y:33398,ptovrint:False,ptlb:Normalmap Int,ptin:_NormalmapInt,varname:node_6639,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,min:0,cur:1,max:2;proporder:358-1813-6665-5275-6639-4775-9412-4309-1802-2999-513-9969-8764-9963-4203-3132-1907-8528-1150-8890-5196;pass:END;sub:END;*/

Shader "MK4/Rock" {
    Properties {
        _Metallic ("Metallic", Range(0, 1)) = 0
        _Gloss ("Gloss", Range(0, 1)) = 0
        _Color ("Color", Color) = (0.5019608,0.5019608,0.5019608,1)
        _Aldebo ("Aldebo", 2D) = "white" {}
        _NormalmapInt ("Normalmap Int", Range(0, 2)) = 1
        _Normalmap ("Normalmap", 2D) = "bump" {}
        _AOPower ("AO Power", Range(0, 3)) = 1
        _AO ("AO", 2D) = "white" {}
        _MaskColor ("Mask Color", Color) = (1,1,1,1)
        _MaskPower ("Mask Power", Range(0, 1.5)) = 1.5
        _MaskBumpInt ("Mask Bump Int", Range(0, 2)) = 1
        _Maskscale ("Mask scale", Range(0, 20)) = 2
        [MaterialToggle] _SmoothBlend ("Smooth Blend", Float ) = 0
        _MaskRGBA ("Mask RGBA", 2D) = "white" {}
        _MaskBump ("Mask Bump", 2D) = "bump" {}
        _DetailDiffInt ("Detail Diff Int", Range(0, 1)) = 0
        _DetailBumpInt ("Detail Bump Int", Range(0, 2)) = 1
        _Detailscale ("Detail scale", Range(0, 20)) = 2
        [MaterialToggle] _DetailBlendDodge ("Detail Blend Dodge", Float ) = 0.3882353
        _Detail ("Detail", 2D) = "white" {}
        _DetailBump ("Detail Bump", 2D) = "bump" {}
    }
    SubShader {
        Tags {
            "RenderType"="Opaque"
        }
        Pass {
            Name "DEFERRED"
            Tags {
                "LightMode"="Deferred"
            }
            
            
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #define UNITY_PASS_DEFERRED
            #define SHOULD_SAMPLE_SH ( defined (LIGHTMAP_OFF) && defined(DYNAMICLIGHTMAP_OFF) )
            #define _GLOSSYENV 1
            #include "UnityCG.cginc"
            #include "Lighting.cginc"
            #include "UnityPBSLighting.cginc"
            #include "UnityStandardBRDF.cginc"
            #pragma fragmentoption ARB_precision_hint_fastest
            #pragma multi_compile_shadowcaster
            #pragma multi_compile ___ UNITY_HDR_ON
            #pragma multi_compile LIGHTMAP_OFF LIGHTMAP_ON
            #pragma multi_compile DIRLIGHTMAP_OFF DIRLIGHTMAP_COMBINED DIRLIGHTMAP_SEPARATE
            #pragma multi_compile DYNAMICLIGHTMAP_OFF DYNAMICLIGHTMAP_ON
            #pragma multi_compile_fog
            #pragma exclude_renderers gles3 metal d3d11_9x xbox360 xboxone ps3 ps4 psp2 
            #pragma target 3.0
            uniform float4 _Color;
            uniform float _Metallic;
            uniform float _Gloss;
            uniform sampler2D _Aldebo; uniform float4 _Aldebo_ST;
            uniform sampler2D _MaskRGBA; uniform float4 _MaskRGBA_ST;
            uniform sampler2D _Normalmap; uniform float4 _Normalmap_ST;
            uniform sampler2D _MaskBump; uniform float4 _MaskBump_ST;
            uniform sampler2D _Detail; uniform float4 _Detail_ST;
            uniform sampler2D _DetailBump; uniform float4 _DetailBump_ST;
            uniform float _Detailscale;
            uniform sampler2D _AO; uniform float4 _AO_ST;
            uniform float4 _MaskColor;
            uniform float _DetailBumpInt;
            uniform float _DetailDiffInt;
            uniform float _Maskscale;
            uniform float _MaskPower;
            uniform float _MaskBumpInt;
            uniform float _AOPower;
            uniform fixed _DetailBlendDodge;
            uniform fixed _SmoothBlend;
            uniform float _NormalmapInt;
            struct VertexInput {
                float4 vertex : POSITION;
                float3 normal : NORMAL;
                float4 tangent : TANGENT;
                float2 texcoord0 : TEXCOORD0;
                float2 texcoord1 : TEXCOORD1;
                float2 texcoord2 : TEXCOORD2;
            };
            struct VertexOutput {
                float4 pos : SV_POSITION;
                float2 uv0 : TEXCOORD0;
                float2 uv1 : TEXCOORD1;
                float2 uv2 : TEXCOORD2;
                float4 posWorld : TEXCOORD3;
                float3 normalDir : TEXCOORD4;
                float3 tangentDir : TEXCOORD5;
                float3 bitangentDir : TEXCOORD6;
                #if defined(LIGHTMAP_ON) || defined(UNITY_SHOULD_SAMPLE_SH)
                    float4 ambientOrLightmapUV : TEXCOORD7;
                #endif
            };
            VertexOutput vert (VertexInput v) {
                VertexOutput o = (VertexOutput)0;
                o.uv0 = v.texcoord0;
                o.uv1 = v.texcoord1;
                o.uv2 = v.texcoord2;
                #ifdef LIGHTMAP_ON
                    o.ambientOrLightmapUV.xy = v.texcoord1.xy * unity_LightmapST.xy + unity_LightmapST.zw;
                    o.ambientOrLightmapUV.zw = 0;
                #elif UNITY_SHOULD_SAMPLE_SH
            #endif
            #ifdef DYNAMICLIGHTMAP_ON
                o.ambientOrLightmapUV.zw = v.texcoord2.xy * unity_DynamicLightmapST.xy + unity_DynamicLightmapST.zw;
            #endif
            o.normalDir = UnityObjectToWorldNormal(v.normal);
            o.tangentDir = normalize( mul( unity_ObjectToWorld, float4( v.tangent.xyz, 0.0 ) ).xyz );
            o.bitangentDir = normalize(cross(o.normalDir, o.tangentDir) * v.tangent.w);
            o.posWorld = mul(unity_ObjectToWorld, v.vertex);
            o.pos = UnityObjectToClipPos(v.vertex);
            return o;
        }
        void frag(
            VertexOutput i,
            out half4 outDiffuse : SV_Target0,
            out half4 outSpecSmoothness : SV_Target1,
            out half4 outNormal : SV_Target2,
            out half4 outEmission : SV_Target3 )
        {
            i.normalDir = normalize(i.normalDir);
            float3x3 tangentTransform = float3x3( i.tangentDir, i.bitangentDir, i.normalDir);
/// Vectors:
            float3 viewDirection = normalize(_WorldSpaceCameraPos.xyz - i.posWorld.xyz);
            float3 node_7130 = float3(0,0,1);
            float3 node_7208 = UnpackNormal(tex2D(_Normalmap,TRANSFORM_TEX(i.uv0, _Normalmap)));
            float3 node_9708 = lerp(node_7130,node_7208.rgb,_NormalmapInt);
            float2 node_1657 = (i.uv0*_Detailscale);
            float3 _DetailBump_var = UnpackNormal(tex2D(_DetailBump,TRANSFORM_TEX(node_1657, _DetailBump)));
            float3 node_5341_nrm_base = node_9708 + float3(0,0,1);
            float3 node_5341_nrm_detail = normalize(lerp(node_7130,_DetailBump_var.rgb,_DetailBumpInt)) * float3(-1,-1,1);
            float3 node_5341_nrm_combined = node_5341_nrm_base*dot(node_5341_nrm_base, node_5341_nrm_detail)/node_5341_nrm_base.z - node_5341_nrm_detail;
            float3 node_5341 = node_5341_nrm_combined;
            float2 node_1886 = (i.uv0*_Maskscale);
            float3 node_5495 = UnpackNormal(tex2D(_MaskBump,TRANSFORM_TEX(node_1886, _MaskBump)));
            float3 node_3865_nrm_base = node_9708 + float3(0,0,1);
            float3 node_3865_nrm_detail = lerp(node_7130,node_5495.rgb,_MaskBumpInt) * float3(-1,-1,1);
            float3 node_3865_nrm_combined = node_3865_nrm_base*dot(node_3865_nrm_base, node_3865_nrm_detail)/node_3865_nrm_base.z - node_3865_nrm_detail;
            float3 node_3865 = node_3865_nrm_combined;
            float3 normalLocal = lerp(node_5341,node_3865,round(saturate(i.normalDir.g)));
            float3 normalDirection = normalize(mul( normalLocal, tangentTransform )); // Perturbed normals
            float3 viewReflectDirection = reflect( -viewDirection, normalDirection );
// Lighting:
            float Pi = 3.141592654;
            float InvPi = 0.31830988618;
///// Gloss:
            float gloss = _Gloss;
/// GI Data:
            UnityLight light; // Dummy light
            light.color = 0;
            light.dir = half3(0,1,0);
            light.ndotl = max(0,dot(normalDirection,light.dir));
            UnityGIInput d;
            d.light = light;
            d.worldPos = i.posWorld.xyz;
            d.worldViewDir = viewDirection;
            d.atten = 1;
            #if defined(LIGHTMAP_ON) || defined(DYNAMICLIGHTMAP_ON)
                d.ambient = 0;
                d.lightmapUV = i.ambientOrLightmapUV;
            #else
                d.ambient = i.ambientOrLightmapUV;
            #endif
            d.boxMax[0] = unity_SpecCube0_BoxMax;
            d.boxMin[0] = unity_SpecCube0_BoxMin;
            d.probePosition[0] = unity_SpecCube0_ProbePosition;
            d.probeHDR[0] = unity_SpecCube0_HDR;
            d.boxMax[1] = unity_SpecCube1_BoxMax;
            d.boxMin[1] = unity_SpecCube1_BoxMin;
            d.probePosition[1] = unity_SpecCube1_ProbePosition;
            d.probeHDR[1] = unity_SpecCube1_HDR;
            UnityGI gi = UnityGlobalIllumination (d, 1, gloss, normalDirection);
// Specular:
            float3 node_4276 = float3(0.5,0.5,0.5);
            float4 node_3919 = tex2D(_Detail,TRANSFORM_TEX(node_1657, _Detail));
            float4 node_3353 = tex2D(_Aldebo,TRANSFORM_TEX(i.uv0, _Aldebo));
            float4 node_9673 = tex2D(_MaskRGBA,TRANSFORM_TEX(node_1886, _MaskRGBA));
            float node_4418 = (_MaskPower*saturate(normalDirection.g));
            float3 diffuseColor = (lerp(lerp( saturate(( node_3353.rgb > 0.5 ? (1.0-(1.0-2.0*(node_3353.rgb-0.5))*(1.0-lerp(node_4276,node_3919.rgb,_DetailDiffInt))) : (2.0*node_3353.rgb*lerp(node_4276,node_3919.rgb,_DetailDiffInt)) )), saturate(( lerp(node_4276,node_3919.rgb,_DetailDiffInt) > 0.5 ? (node_3353.rgb/((1.0-lerp(node_4276,node_3919.rgb,_DetailDiffInt))*2.0)) : (1.0-(((1.0-node_3353.rgb)*0.5)/lerp(node_4276,node_3919.rgb,_DetailDiffInt))))), _DetailBlendDodge ),(node_9673.rgb*_MaskColor.rgb),(node_9673.a*lerp( round(node_4418), node_4418, _SmoothBlend )))*_Color.rgb); // Need this for specular when using metallic
            float specularMonochrome;
            float3 specularColor;
            diffuseColor = DiffuseAndSpecularFromMetallic( diffuseColor, _Metallic, specularColor, specularMonochrome );
            specularMonochrome = 1-specularMonochrome;
            float NdotV = max(0.0,dot( normalDirection, viewDirection ));
            half grazingTerm = saturate( gloss + specularMonochrome );
            float3 indirectSpecular = (gi.indirect.specular);
            indirectSpecular *= FresnelLerp (specularColor, grazingTerm, NdotV);
/// Diffuse:
            float3 indirectDiffuse = float3(0,0,0);
            indirectDiffuse += gi.indirect.diffuse;
            float4 _AO_var = tex2D(_AO,TRANSFORM_TEX(i.uv0, _AO));
            float node_3987 = lerp(_AO_var.rgb,float3(1,1,1),(1.0 - _AOPower)).r;
            indirectDiffuse *= node_3987; // Diffuse AO
// Final Color:
            outDiffuse = half4( diffuseColor, node_3987 );
            outSpecSmoothness = half4( specularColor, gloss );
            outNormal = half4( normalDirection * 0.5 + 0.5, 1 );
            outEmission = half4(0,0,0,1);
            outEmission.rgb += indirectSpecular * node_3987;
            outEmission.rgb += indirectDiffuse * diffuseColor;
            #ifndef UNITY_HDR_ON
                outEmission.rgb = exp2(-outEmission.rgb);
            #endif
        }
        ENDCG
    }
    Pass {
        Name "FORWARD"
        Tags {
            "LightMode"="ForwardBase"
        }
        
        
        CGPROGRAM
        #pragma vertex vert
        #pragma fragment frag
        #define UNITY_PASS_FORWARDBASE
        #define SHOULD_SAMPLE_SH ( defined (LIGHTMAP_OFF) && defined(DYNAMICLIGHTMAP_OFF) )
        #define _GLOSSYENV 1
        #include "UnityCG.cginc"
        #include "AutoLight.cginc"
        #include "Lighting.cginc"
        #include "UnityPBSLighting.cginc"
        #include "UnityStandardBRDF.cginc"
        #pragma multi_compile_fwdbase_fullshadows
        #pragma multi_compile LIGHTMAP_OFF LIGHTMAP_ON
        #pragma multi_compile DIRLIGHTMAP_OFF DIRLIGHTMAP_COMBINED DIRLIGHTMAP_SEPARATE
        #pragma multi_compile DYNAMICLIGHTMAP_OFF DYNAMICLIGHTMAP_ON
        #pragma multi_compile_fog
        #pragma exclude_renderers gles3 metal d3d11_9x xbox360 xboxone ps3 ps4 psp2 
        #pragma target 3.0
        uniform float4 _Color;
        uniform float _Metallic;
        uniform float _Gloss;
        uniform sampler2D _Aldebo; uniform float4 _Aldebo_ST;
        uniform sampler2D _MaskRGBA; uniform float4 _MaskRGBA_ST;
        uniform sampler2D _Normalmap; uniform float4 _Normalmap_ST;
        uniform sampler2D _MaskBump; uniform float4 _MaskBump_ST;
        uniform sampler2D _Detail; uniform float4 _Detail_ST;
        uniform sampler2D _DetailBump; uniform float4 _DetailBump_ST;
        uniform float _Detailscale;
        uniform sampler2D _AO; uniform float4 _AO_ST;
        uniform float4 _MaskColor;
        uniform float _DetailBumpInt;
        uniform float _DetailDiffInt;
        uniform float _Maskscale;
        uniform float _MaskPower;
        uniform float _MaskBumpInt;
        uniform float _AOPower;
        uniform fixed _DetailBlendDodge;
        uniform fixed _SmoothBlend;
        uniform float _NormalmapInt;
        struct VertexInput {
            float4 vertex : POSITION;
            float3 normal : NORMAL;
            float4 tangent : TANGENT;
            float2 texcoord0 : TEXCOORD0;
            float2 texcoord1 : TEXCOORD1;
            float2 texcoord2 : TEXCOORD2;
        };
        struct VertexOutput {
            float4 pos : SV_POSITION;
            float2 uv0 : TEXCOORD0;
            float2 uv1 : TEXCOORD1;
            float2 uv2 : TEXCOORD2;
            float4 posWorld : TEXCOORD3;
            float3 normalDir : TEXCOORD4;
            float3 tangentDir : TEXCOORD5;
            float3 bitangentDir : TEXCOORD6;
            LIGHTING_COORDS(7,8)
            UNITY_FOG_COORDS(9)
            #if defined(LIGHTMAP_ON) || defined(UNITY_SHOULD_SAMPLE_SH)
                float4 ambientOrLightmapUV : TEXCOORD10;
            #endif
        };
        VertexOutput vert (VertexInput v) {
            VertexOutput o = (VertexOutput)0;
            o.uv0 = v.texcoord0;
            o.uv1 = v.texcoord1;
            o.uv2 = v.texcoord2;
            #ifdef LIGHTMAP_ON
                o.ambientOrLightmapUV.xy = v.texcoord1.xy * unity_LightmapST.xy + unity_LightmapST.zw;
                o.ambientOrLightmapUV.zw = 0;
            #elif UNITY_SHOULD_SAMPLE_SH
        #endif
        #ifdef DYNAMICLIGHTMAP_ON
            o.ambientOrLightmapUV.zw = v.texcoord2.xy * unity_DynamicLightmapST.xy + unity_DynamicLightmapST.zw;
        #endif
        o.normalDir = UnityObjectToWorldNormal(v.normal);
        o.tangentDir = normalize( mul( unity_ObjectToWorld, float4( v.tangent.xyz, 0.0 ) ).xyz );
        o.bitangentDir = normalize(cross(o.normalDir, o.tangentDir) * v.tangent.w);
        o.posWorld = mul(unity_ObjectToWorld, v.vertex);
        float3 lightColor = _LightColor0.rgb;
        o.pos = UnityObjectToClipPos(v.vertex);
        UNITY_TRANSFER_FOG(o,o.pos);
        TRANSFER_VERTEX_TO_FRAGMENT(o)
        return o;
    }
    float4 frag(VertexOutput i) : COLOR {
        i.normalDir = normalize(i.normalDir);
        float3x3 tangentTransform = float3x3( i.tangentDir, i.bitangentDir, i.normalDir);
// Vectors:
        float3 viewDirection = normalize(_WorldSpaceCameraPos.xyz - i.posWorld.xyz);
        float3 node_7130 = float3(0,0,1);
        float3 node_7208 = UnpackNormal(tex2D(_Normalmap,TRANSFORM_TEX(i.uv0, _Normalmap)));
        float3 node_9708 = lerp(node_7130,node_7208.rgb,_NormalmapInt);
        float2 node_1657 = (i.uv0*_Detailscale);
        float3 _DetailBump_var = UnpackNormal(tex2D(_DetailBump,TRANSFORM_TEX(node_1657, _DetailBump)));
        float3 node_5341_nrm_base = node_9708 + float3(0,0,1);
        float3 node_5341_nrm_detail = normalize(lerp(node_7130,_DetailBump_var.rgb,_DetailBumpInt)) * float3(-1,-1,1);
        float3 node_5341_nrm_combined = node_5341_nrm_base*dot(node_5341_nrm_base, node_5341_nrm_detail)/node_5341_nrm_base.z - node_5341_nrm_detail;
        float3 node_5341 = node_5341_nrm_combined;
        float2 node_1886 = (i.uv0*_Maskscale);
        float3 node_5495 = UnpackNormal(tex2D(_MaskBump,TRANSFORM_TEX(node_1886, _MaskBump)));
        float3 node_3865_nrm_base = node_9708 + float3(0,0,1);
        float3 node_3865_nrm_detail = lerp(node_7130,node_5495.rgb,_MaskBumpInt) * float3(-1,-1,1);
        float3 node_3865_nrm_combined = node_3865_nrm_base*dot(node_3865_nrm_base, node_3865_nrm_detail)/node_3865_nrm_base.z - node_3865_nrm_detail;
        float3 node_3865 = node_3865_nrm_combined;
        float3 normalLocal = lerp(node_5341,node_3865,round(saturate(i.normalDir.g)));
        float3 normalDirection = normalize(mul( normalLocal, tangentTransform )); // Perturbed normals
        float3 viewReflectDirection = reflect( -viewDirection, normalDirection );
        float3 lightDirection = normalize(_WorldSpaceLightPos0.xyz);
        float3 lightColor = _LightColor0.rgb;
        float3 halfDirection = normalize(viewDirection+lightDirection);
// Lighting:
        float attenuation = LIGHT_ATTENUATION(i);
        float3 attenColor = attenuation * _LightColor0.xyz;
        float Pi = 3.141592654;
        float InvPi = 0.31830988618;
// Gloss:
        float gloss = _Gloss;
        float specPow = exp2( gloss * 10.0+1.0);
// GI Data:
        UnityLight light;
        #ifdef LIGHTMAP_OFF
            light.color = lightColor;
            light.dir = lightDirection;
            light.ndotl = LambertTerm (normalDirection, light.dir);
        #else
            light.color = half3(0.f, 0.f, 0.f);
            light.ndotl = 0.0f;
            light.dir = half3(0.f, 0.f, 0.f);
        #endif
        UnityGIInput d;
        d.light = light;
        d.worldPos = i.posWorld.xyz;
        d.worldViewDir = viewDirection;
        d.atten = attenuation;
        #if defined(LIGHTMAP_ON) || defined(DYNAMICLIGHTMAP_ON)
            d.ambient = 0;
            d.lightmapUV = i.ambientOrLightmapUV;
        #else
            d.ambient = i.ambientOrLightmapUV;
        #endif
        d.boxMax[0] = unity_SpecCube0_BoxMax;
        d.boxMin[0] = unity_SpecCube0_BoxMin;
        d.probePosition[0] = unity_SpecCube0_ProbePosition;
        d.probeHDR[0] = unity_SpecCube0_HDR;
        d.boxMax[1] = unity_SpecCube1_BoxMax;
        d.boxMin[1] = unity_SpecCube1_BoxMin;
        d.probePosition[1] = unity_SpecCube1_ProbePosition;
        d.probeHDR[1] = unity_SpecCube1_HDR;
        UnityGI gi = UnityGlobalIllumination (d, 1, gloss, normalDirection);
        lightDirection = gi.light.dir;
        lightColor = gi.light.color;
// Specular:
        float NdotL = max(0, dot( normalDirection, lightDirection ));
        float4 _AO_var = tex2D(_AO,TRANSFORM_TEX(i.uv0, _AO));
        float node_3987 = lerp(_AO_var.rgb,float3(1,1,1),(1.0 - _AOPower)).r;
        float3 specularAO = node_3987;
        float LdotH = max(0.0,dot(lightDirection, halfDirection));
        float3 node_4276 = float3(0.5,0.5,0.5);
        float4 node_3919 = tex2D(_Detail,TRANSFORM_TEX(node_1657, _Detail));
        float4 node_3353 = tex2D(_Aldebo,TRANSFORM_TEX(i.uv0, _Aldebo));
        float4 node_9673 = tex2D(_MaskRGBA,TRANSFORM_TEX(node_1886, _MaskRGBA));
        float node_4418 = (_MaskPower*saturate(normalDirection.g));
        float3 diffuseColor = (lerp(lerp( saturate(( node_3353.rgb > 0.5 ? (1.0-(1.0-2.0*(node_3353.rgb-0.5))*(1.0-lerp(node_4276,node_3919.rgb,_DetailDiffInt))) : (2.0*node_3353.rgb*lerp(node_4276,node_3919.rgb,_DetailDiffInt)) )), saturate(( lerp(node_4276,node_3919.rgb,_DetailDiffInt) > 0.5 ? (node_3353.rgb/((1.0-lerp(node_4276,node_3919.rgb,_DetailDiffInt))*2.0)) : (1.0-(((1.0-node_3353.rgb)*0.5)/lerp(node_4276,node_3919.rgb,_DetailDiffInt))))), _DetailBlendDodge ),(node_9673.rgb*_MaskColor.rgb),(node_9673.a*lerp( round(node_4418), node_4418, _SmoothBlend )))*_Color.rgb); // Need this for specular when using metallic
        float specularMonochrome;
        float3 specularColor;
        diffuseColor = DiffuseAndSpecularFromMetallic( diffuseColor, _Metallic, specularColor, specularMonochrome );
        specularMonochrome = 1-specularMonochrome;
        float NdotV = max(0.0,dot( normalDirection, viewDirection ));
        float NdotH = max(0.0,dot( normalDirection, halfDirection ));
        float VdotH = max(0.0,dot( viewDirection, halfDirection ));
        float visTerm = SmithBeckmannVisibilityTerm( NdotL, NdotV, 1.0-gloss );
        float normTerm = max(0.0, NDFBlinnPhongNormalizedTerm(NdotH, RoughnessToSpecPower(1.0-gloss)));
        float specularPBL = max(0, (NdotL*visTerm*normTerm) * unity_LightGammaCorrectionConsts_PIDiv4 );
        float3 directSpecular = 1 * pow(max(0,dot(halfDirection,normalDirection)),specPow)*specularPBL*lightColor*FresnelTerm(specularColor, LdotH);
        half grazingTerm = saturate( gloss + specularMonochrome );
        float3 indirectSpecular = (gi.indirect.specular) * specularAO;
        indirectSpecular *= FresnelLerp (specularColor, grazingTerm, NdotV);
        float3 specular = (directSpecular + indirectSpecular);
// Diffuse:
        NdotL = max(0.0,dot( normalDirection, lightDirection ));
        half fd90 = 0.5 + 2 * LdotH * LdotH * (1-gloss);
        float3 directDiffuse = ((1 +(fd90 - 1)*pow((1.00001-NdotL), 5)) * (1 + (fd90 - 1)*pow((1.00001-NdotV), 5)) * NdotL) * attenColor;
        float3 indirectDiffuse = float3(0,0,0);
        indirectDiffuse += gi.indirect.diffuse;
        indirectDiffuse *= node_3987; // Diffuse AO
        float3 diffuse = (directDiffuse + indirectDiffuse) * diffuseColor;
// Final Color:
        float3 finalColor = diffuse + specular;
        fixed4 finalRGBA = fixed4(finalColor,1);
        UNITY_APPLY_FOG(i.fogCoord, finalRGBA);
        return finalRGBA;
    }
    ENDCG
}
Pass {
    Name "FORWARD_DELTA"
    Tags {
        "LightMode"="ForwardAdd"
    }
    Blend One One
    
    
    CGPROGRAM
    #pragma vertex vert
    #pragma fragment frag
    #define UNITY_PASS_FORWARDADD
    #define SHOULD_SAMPLE_SH ( defined (LIGHTMAP_OFF) && defined(DYNAMICLIGHTMAP_OFF) )
    #define _GLOSSYENV 1
    #include "UnityCG.cginc"
    #include "AutoLight.cginc"
    #include "Lighting.cginc"
    #include "UnityPBSLighting.cginc"
    #include "UnityStandardBRDF.cginc"
    #pragma multi_compile_fwdadd_fullshadows
    #pragma multi_compile LIGHTMAP_OFF LIGHTMAP_ON
    #pragma multi_compile DIRLIGHTMAP_OFF DIRLIGHTMAP_COMBINED DIRLIGHTMAP_SEPARATE
    #pragma multi_compile DYNAMICLIGHTMAP_OFF DYNAMICLIGHTMAP_ON
    #pragma multi_compile_fog
    #pragma exclude_renderers gles3 metal d3d11_9x xbox360 xboxone ps3 ps4 psp2 
    #pragma target 3.0
    uniform float4 _Color;
    uniform float _Metallic;
    uniform float _Gloss;
    uniform sampler2D _Aldebo; uniform float4 _Aldebo_ST;
    uniform sampler2D _MaskRGBA; uniform float4 _MaskRGBA_ST;
    uniform sampler2D _Normalmap; uniform float4 _Normalmap_ST;
    uniform sampler2D _MaskBump; uniform float4 _MaskBump_ST;
    uniform sampler2D _Detail; uniform float4 _Detail_ST;
    uniform sampler2D _DetailBump; uniform float4 _DetailBump_ST;
    uniform float _Detailscale;
    uniform float4 _MaskColor;
    uniform float _DetailBumpInt;
    uniform float _DetailDiffInt;
    uniform float _Maskscale;
    uniform float _MaskPower;
    uniform float _MaskBumpInt;
    uniform fixed _DetailBlendDodge;
    uniform fixed _SmoothBlend;
    uniform float _NormalmapInt;
    struct VertexInput {
        float4 vertex : POSITION;
        float3 normal : NORMAL;
        float4 tangent : TANGENT;
        float2 texcoord0 : TEXCOORD0;
        float2 texcoord1 : TEXCOORD1;
        float2 texcoord2 : TEXCOORD2;
    };
    struct VertexOutput {
        float4 pos : SV_POSITION;
        float2 uv0 : TEXCOORD0;
        float2 uv1 : TEXCOORD1;
        float2 uv2 : TEXCOORD2;
        float4 posWorld : TEXCOORD3;
        float3 normalDir : TEXCOORD4;
        float3 tangentDir : TEXCOORD5;
        float3 bitangentDir : TEXCOORD6;
        LIGHTING_COORDS(7,8)
    };
    VertexOutput vert (VertexInput v) {
        VertexOutput o = (VertexOutput)0;
        o.uv0 = v.texcoord0;
        o.uv1 = v.texcoord1;
        o.uv2 = v.texcoord2;
        o.normalDir = UnityObjectToWorldNormal(v.normal);
        o.tangentDir = normalize( mul( unity_ObjectToWorld, float4( v.tangent.xyz, 0.0 ) ).xyz );
        o.bitangentDir = normalize(cross(o.normalDir, o.tangentDir) * v.tangent.w);
        o.posWorld = mul(unity_ObjectToWorld, v.vertex);
        float3 lightColor = _LightColor0.rgb;
        o.pos = UnityObjectToClipPos(v.vertex);
        TRANSFER_VERTEX_TO_FRAGMENT(o)
        return o;
    }
    float4 frag(VertexOutput i) : COLOR {
        i.normalDir = normalize(i.normalDir);
        float3x3 tangentTransform = float3x3( i.tangentDir, i.bitangentDir, i.normalDir);
// Vectors:
        float3 viewDirection = normalize(_WorldSpaceCameraPos.xyz - i.posWorld.xyz);
        float3 node_7130 = float3(0,0,1);
        float3 node_7208 = UnpackNormal(tex2D(_Normalmap,TRANSFORM_TEX(i.uv0, _Normalmap)));
        float3 node_9708 = lerp(node_7130,node_7208.rgb,_NormalmapInt);
        float2 node_1657 = (i.uv0*_Detailscale);
        float3 _DetailBump_var = UnpackNormal(tex2D(_DetailBump,TRANSFORM_TEX(node_1657, _DetailBump)));
        float3 node_5341_nrm_base = node_9708 + float3(0,0,1);
        float3 node_5341_nrm_detail = normalize(lerp(node_7130,_DetailBump_var.rgb,_DetailBumpInt)) * float3(-1,-1,1);
        float3 node_5341_nrm_combined = node_5341_nrm_base*dot(node_5341_nrm_base, node_5341_nrm_detail)/node_5341_nrm_base.z - node_5341_nrm_detail;
        float3 node_5341 = node_5341_nrm_combined;
        float2 node_1886 = (i.uv0*_Maskscale);
        float3 node_5495 = UnpackNormal(tex2D(_MaskBump,TRANSFORM_TEX(node_1886, _MaskBump)));
        float3 node_3865_nrm_base = node_9708 + float3(0,0,1);
        float3 node_3865_nrm_detail = lerp(node_7130,node_5495.rgb,_MaskBumpInt) * float3(-1,-1,1);
        float3 node_3865_nrm_combined = node_3865_nrm_base*dot(node_3865_nrm_base, node_3865_nrm_detail)/node_3865_nrm_base.z - node_3865_nrm_detail;
        float3 node_3865 = node_3865_nrm_combined;
        float3 normalLocal = lerp(node_5341,node_3865,round(saturate(i.normalDir.g)));
        float3 normalDirection = normalize(mul( normalLocal, tangentTransform )); // Perturbed normals
        float3 lightDirection = normalize(lerp(_WorldSpaceLightPos0.xyz, _WorldSpaceLightPos0.xyz - i.posWorld.xyz,_WorldSpaceLightPos0.w));
        float3 lightColor = _LightColor0.rgb;
        float3 halfDirection = normalize(viewDirection+lightDirection);
// Lighting:
        float attenuation = LIGHT_ATTENUATION(i);
        float3 attenColor = attenuation * _LightColor0.xyz;
        float Pi = 3.141592654;
        float InvPi = 0.31830988618;
// Gloss:
        float gloss = _Gloss;
        float specPow = exp2( gloss * 10.0+1.0);
// Specular:
        float NdotL = max(0, dot( normalDirection, lightDirection ));
        float LdotH = max(0.0,dot(lightDirection, halfDirection));
        float3 node_4276 = float3(0.5,0.5,0.5);
        float4 node_3919 = tex2D(_Detail,TRANSFORM_TEX(node_1657, _Detail));
        float4 node_3353 = tex2D(_Aldebo,TRANSFORM_TEX(i.uv0, _Aldebo));
        float4 node_9673 = tex2D(_MaskRGBA,TRANSFORM_TEX(node_1886, _MaskRGBA));
        float node_4418 = (_MaskPower*saturate(normalDirection.g));
        float3 diffuseColor = (lerp(lerp( saturate(( node_3353.rgb > 0.5 ? (1.0-(1.0-2.0*(node_3353.rgb-0.5))*(1.0-lerp(node_4276,node_3919.rgb,_DetailDiffInt))) : (2.0*node_3353.rgb*lerp(node_4276,node_3919.rgb,_DetailDiffInt)) )), saturate(( lerp(node_4276,node_3919.rgb,_DetailDiffInt) > 0.5 ? (node_3353.rgb/((1.0-lerp(node_4276,node_3919.rgb,_DetailDiffInt))*2.0)) : (1.0-(((1.0-node_3353.rgb)*0.5)/lerp(node_4276,node_3919.rgb,_DetailDiffInt))))), _DetailBlendDodge ),(node_9673.rgb*_MaskColor.rgb),(node_9673.a*lerp( round(node_4418), node_4418, _SmoothBlend )))*_Color.rgb); // Need this for specular when using metallic
        float specularMonochrome;
        float3 specularColor;
        diffuseColor = DiffuseAndSpecularFromMetallic( diffuseColor, _Metallic, specularColor, specularMonochrome );
        specularMonochrome = 1-specularMonochrome;
        float NdotV = max(0.0,dot( normalDirection, viewDirection ));
        float NdotH = max(0.0,dot( normalDirection, halfDirection ));
        float VdotH = max(0.0,dot( viewDirection, halfDirection ));
        float visTerm = SmithBeckmannVisibilityTerm( NdotL, NdotV, 1.0-gloss );
        float normTerm = max(0.0, NDFBlinnPhongNormalizedTerm(NdotH, RoughnessToSpecPower(1.0-gloss)));
        float specularPBL = max(0, (NdotL*visTerm*normTerm) * unity_LightGammaCorrectionConsts_PIDiv4 );
        float3 directSpecular = attenColor * pow(max(0,dot(halfDirection,normalDirection)),specPow)*specularPBL*lightColor*FresnelTerm(specularColor, LdotH);
        float3 specular = directSpecular;
// Diffuse:
        NdotL = max(0.0,dot( normalDirection, lightDirection ));
        half fd90 = 0.5 + 2 * LdotH * LdotH * (1-gloss);
        float3 directDiffuse = ((1 +(fd90 - 1)*pow((1.00001-NdotL), 5)) * (1 + (fd90 - 1)*pow((1.00001-NdotV), 5)) * NdotL) * attenColor;
        float3 diffuse = directDiffuse * diffuseColor;
// Final Color:
        float3 finalColor = diffuse + specular;
        return fixed4(finalColor * 1,0);
    }
    ENDCG
}
Pass {
    Name "Meta"
    Tags {
        "LightMode"="Meta"
    }
    Cull Off
    
    CGPROGRAM
    #pragma vertex vert
    #pragma fragment frag
    #define UNITY_PASS_META 1
    #define SHOULD_SAMPLE_SH ( defined (LIGHTMAP_OFF) && defined(DYNAMICLIGHTMAP_OFF) )
    #define _GLOSSYENV 1
    #include "UnityCG.cginc"
    #include "Lighting.cginc"
    #include "UnityPBSLighting.cginc"
    #include "UnityStandardBRDF.cginc"
    #include "UnityMetaPass.cginc"
    #pragma fragmentoption ARB_precision_hint_fastest
    #pragma multi_compile_shadowcaster
    #pragma multi_compile LIGHTMAP_OFF LIGHTMAP_ON
    #pragma multi_compile DIRLIGHTMAP_OFF DIRLIGHTMAP_COMBINED DIRLIGHTMAP_SEPARATE
    #pragma multi_compile DYNAMICLIGHTMAP_OFF DYNAMICLIGHTMAP_ON
    #pragma multi_compile_fog
    #pragma exclude_renderers gles3 metal d3d11_9x xbox360 xboxone ps3 ps4 psp2 
    #pragma target 3.0
    uniform float4 _Color;
    uniform float _Metallic;
    uniform float _Gloss;
    uniform sampler2D _Aldebo; uniform float4 _Aldebo_ST;
    uniform sampler2D _MaskRGBA; uniform float4 _MaskRGBA_ST;
    uniform sampler2D _Detail; uniform float4 _Detail_ST;
    uniform float _Detailscale;
    uniform float4 _MaskColor;
    uniform float _DetailDiffInt;
    uniform float _Maskscale;
    uniform float _MaskPower;
    uniform fixed _DetailBlendDodge;
    uniform fixed _SmoothBlend;
    struct VertexInput {
        float4 vertex : POSITION;
        float3 normal : NORMAL;
        float2 texcoord0 : TEXCOORD0;
        float2 texcoord1 : TEXCOORD1;
        float2 texcoord2 : TEXCOORD2;
    };
    struct VertexOutput {
        float4 pos : SV_POSITION;
        float2 uv0 : TEXCOORD0;
        float2 uv1 : TEXCOORD1;
        float2 uv2 : TEXCOORD2;
        float4 posWorld : TEXCOORD3;
        float3 normalDir : TEXCOORD4;
    };
    VertexOutput vert (VertexInput v) {
        VertexOutput o = (VertexOutput)0;
        o.uv0 = v.texcoord0;
        o.uv1 = v.texcoord1;
        o.uv2 = v.texcoord2;
        o.normalDir = UnityObjectToWorldNormal(v.normal);
        o.posWorld = mul(unity_ObjectToWorld, v.vertex);
        o.pos = UnityMetaVertexPosition(v.vertex, v.texcoord1.xy, v.texcoord2.xy, unity_LightmapST, unity_DynamicLightmapST );
        return o;
    }
    float4 frag(VertexOutput i) : SV_Target {
        i.normalDir = normalize(i.normalDir);
// Vectors:
        float3 viewDirection = normalize(_WorldSpaceCameraPos.xyz - i.posWorld.xyz);
        float3 normalDirection = i.normalDir;
        UnityMetaInput o;
        UNITY_INITIALIZE_OUTPUT( UnityMetaInput, o );
        
        o.Emission = 0;
        
        float3 node_4276 = float3(0.5,0.5,0.5);
        float2 node_1657 = (i.uv0*_Detailscale);
        float4 node_3919 = tex2D(_Detail,TRANSFORM_TEX(node_1657, _Detail));
        float4 node_3353 = tex2D(_Aldebo,TRANSFORM_TEX(i.uv0, _Aldebo));
        float2 node_1886 = (i.uv0*_Maskscale);
        float4 node_9673 = tex2D(_MaskRGBA,TRANSFORM_TEX(node_1886, _MaskRGBA));
        float node_4418 = (_MaskPower*saturate(normalDirection.g));
        float3 diffColor = (lerp(lerp( saturate(( node_3353.rgb > 0.5 ? (1.0-(1.0-2.0*(node_3353.rgb-0.5))*(1.0-lerp(node_4276,node_3919.rgb,_DetailDiffInt))) : (2.0*node_3353.rgb*lerp(node_4276,node_3919.rgb,_DetailDiffInt)) )), saturate(( lerp(node_4276,node_3919.rgb,_DetailDiffInt) > 0.5 ? (node_3353.rgb/((1.0-lerp(node_4276,node_3919.rgb,_DetailDiffInt))*2.0)) : (1.0-(((1.0-node_3353.rgb)*0.5)/lerp(node_4276,node_3919.rgb,_DetailDiffInt))))), _DetailBlendDodge ),(node_9673.rgb*_MaskColor.rgb),(node_9673.a*lerp( round(node_4418), node_4418, _SmoothBlend )))*_Color.rgb);
        float specularMonochrome;
        float3 specColor;
        diffColor = DiffuseAndSpecularFromMetallic( diffColor, _Metallic, specColor, specularMonochrome );
        float roughness = 1.0 - _Gloss;
        o.Albedo = diffColor + specColor * roughness * roughness * 0.5;
        
        return UnityMetaFragment( o );
    }
    ENDCG
}
}
FallBack "Diffuse"
CustomEditor "ShaderForgeMaterialInspector"
}
