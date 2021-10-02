Shader "Toon/LitPotionLiquid" {
	Properties {
		_Color ("Main Color", Color) = (0.5,0.5,0.5,1)
		_MainTex ("Base (RGB)", 2D) = "white" {}
		_Ramp ("Toon Ramp (RGB)", 2D) = "gray" {}
		
		//Specular
		_SColor("Specular Color", Color) = (0.5,0.5,0.5,1)
		_SRamp("Specular Ramp", 2D) = "gray" {}
		_SSize("Specular Size", Range(0.1, 10)) = 1
		_SOffset("Specular Offset", Range(0.5,1)) = 0.5
		
		//Rim
		_Rim("Rim", Range(0.1, 10)) = 1
		_RimSize("Rim Size", Range(0.1, 10)) = 1
		_RimRamp("Rim Ramp", 2D) = "gray" {}
	}

	SubShader {
 Tags{ "Queue" = "Transparent"}
        LOD 200
		Blend SrcAlpha OneMinusSrcAlpha 
		
		    Stencil
             {
                 Ref 1
                 Comp Equal
             }   
		
CGPROGRAM
#pragma surface surf ToonRamp vertex:vert keepalpha

sampler2D _Ramp;

// custom lighting function that uses a texture ramp based
// on angle between light direction and normal
#pragma lighting ToonRamp exclude_path:prepass
inline half4 LightingToonRamp (SurfaceOutput s, half3 lightDir, half atten)
{
	#ifndef USING_DIRECTIONAL_LIGHT
	lightDir = normalize(lightDir);
	#endif
	
	half d = dot (s.Normal, lightDir)*0.5 + 0.5;
	half3 ramp = tex2D (_Ramp, float2(d,d)).rgb;
	
	half4 c;
	c.rgb = s.Albedo * _LightColor0.rgb * ramp * (atten * 2);
	c.a = s.Alpha;
	return c;
}


sampler2D _MainTex;
float4 _Color;

//Specular
half4 _SColor;
sampler2D _SRamp;
half _SSize;
float _SOffset;

//Rim
half _Rim;
half _RimSize;
sampler2D _RimRamp;

struct Input {
	float2 uv_MainTex : TEXCOORD0;
	float3 lightDir : TEXCOORD1;
	float3 viewDir : TEXTCOORD2;
};

 void vert(inout appdata_full v, out Input o)
    {
        UNITY_INITIALIZE_OUTPUT(Input, o);
        o.lightDir = WorldSpaceLightDir(v.vertex);
		o.viewDir = WorldSpaceViewDir(v.vertex);
    }
	
void surf (Input IN, inout SurfaceOutput o) {
	//Specular
	half d = dot(o.Normal,IN.lightDir)*0.5+_SOffset;
	half4 ramp = tex2D(_SRamp, float2(d,d));
	half4 sCol = (step(_SSize, ramp.r))*ramp* d*_SColor;
	
	//Rim
	half rim = ((1 - abs(dot(o.Normal, IN.viewDir)))*_RimSize);
	rim = tex2D(_RimRamp, float2(rim,rim));
	
	half4 c = tex2D(_MainTex, IN.uv_MainTex) * _Color;
	o.Albedo = c.rgb;
	o.Albedo +=rim * _Rim;
	o.Albedo +=sCol;
	o.Alpha = c.a;
}
ENDCG

	} 

	Fallback "Diffuse"
}
