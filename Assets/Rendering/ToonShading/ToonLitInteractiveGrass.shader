Shader "Toon/InteractiveGrass" {
	Properties {
		_Color ("Main Color", Color) = (0.5,0.5,0.5,1)
		_MainTex ("Base (RGB)", 2D) = "white" {}
		_Ramp ("Toon Ramp (RGB)", 2D) = "gray" {}
		_Radius("Bending Radius", Float) = 1
			_Displacement("Displacement",Float) = 1.75
			_Rigidness("Rigidness",Range(0,10)) = 1.3
			_Frenquency("Wave Frenquecy",Range(0,10)) = 0.4
			_Sway("Max Sway",Range(0,1)) = 0.1
	}

	SubShader {
		Tags { "RenderType"="Opaque" }
		LOD 200
		
CGPROGRAM
#pragma surface surf ToonRamp vertex:vert addshadow

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
	c.a = 0;
	return c;
}


sampler2D _MainTex;
float4 _Color;

struct Input {
	float2 uv_MainTex : TEXCOORD0;
	float4 color;
};

//Bending
uniform float2 _BenderPosition;
half _Radius;
half _Displacement;

//Wobbling
half _Rigidness;
half _Frenquency;
half _Sway;

void vert(inout appdata_full v) {
	float3 worldPos = mul(unity_ObjectToWorld, v.vertex).xyz;

	//Wobbling
	float2 sway = float2(sin(worldPos.x / _Rigidness + _Time.z * _Frenquency) *  v.color.r, sin(worldPos.z / _Rigidness + _Time.y * _Frenquency) *  v.color.r);
	v.vertex.xz += sway * _Sway;

	//Bending
		float dist = distance(_BenderPosition, worldPos.xz);
		float dispRatio = 1 - saturate(dist / _Radius);
		float2 sphereDisp = (worldPos.xz - _BenderPosition) * dispRatio * _Displacement * v.color.r;
		v.vertex.xz += sphereDisp;
}

void surf (Input IN, inout SurfaceOutput o) {
	half4 c = tex2D(_MainTex, IN.uv_MainTex) * _Color;
	o.Albedo = c.rgb;
	o.Alpha = c.a;
}
ENDCG

	} 

	Fallback "Diffuse"
}
