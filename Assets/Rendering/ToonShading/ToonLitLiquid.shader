Shader "Toon/LitLiquid" {
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
		
		//Liquid
		_FillAmount ("Fill Amount", Range(0,1))=0.5
	}

 SubShader
    {
        Tags {"Queue"="Geometry"  "DisableBatching" = "True" }
 
                Pass
        {
		 Zwrite On
		 Cull Off // we want the front and back faces
		 AlphaToMask On // transparency
 
         CGPROGRAM
 
 
         #pragma vertex vert
         #pragma fragment frag
         // make fog work
         #pragma multi_compile_fog
 
         #include "UnityCG.cginc"
 
         struct appdata
         {
           float4 vertex : POSITION;
           float2 uv : TEXCOORD0;
		   float3 normal : NORMAL;	
         };
 
         struct v2f
         {
            float2 uv : TEXCOORD0;
            UNITY_FOG_COORDS(1)
            float4 vertex : SV_POSITION;
			float3 viewDir : COLOR;
		    float3 normal : COLOR2;		
			float filling : TEXCOORD2;
			float3 lightDir : TEXCOORD3;
         };
 
 
		 float4 RotateAroundYInDegrees (float4 vertex, float degrees)
         {
			float alpha = degrees * UNITY_PI / 180;
			float sina, cosa;
			sincos(alpha, sina, cosa);
			float2x2 m = float2x2(cosa, sina, -sina, cosa);
			return float4(vertex.yz , mul(m, vertex.xz)).xzyw ;				
         }
		 
		sampler2D _MainTex;
		 float4 _MainTex_ST;
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
		
		//Liquid
		half _FillAmount;

 
         v2f vert (appdata v)
         {
            v2f o;
 
            o.vertex = UnityObjectToClipPos(v.vertex);
            o.uv = TRANSFORM_TEX(v.uv, _MainTex);
            UNITY_TRANSFER_FOG(o,o.vertex);			

			o.lightDir = WorldSpaceLightDir(v.vertex);
			o.viewDir = WorldSpaceViewDir(v.vertex);
			o.normal = v.normal;
			
			//Filling
            float3 worldPos = mul (unity_ObjectToWorld, v.vertex.xyz);   
			float3 worldPosX= RotateAroundYInDegrees(float4(worldPos,0),360);
			float3 worldPosZ = float3 (worldPosX.y, worldPosX.z, worldPosX.x);		
			float3 worldPosAdjusted = worldPos + (worldPosX  * 1)+ (worldPosZ* 1); 
			o.filling =  worldPosAdjusted + _FillAmount;
			
            return o;
         }
 
         fixed4 frag (v2f i, fixed facing : VFACE) : SV_Target
         {
			// sample the texture
           fixed4 c = tex2D(_MainTex, i.uv) * _Color;
           // apply fog
           UNITY_APPLY_FOG(i.fogCoord, col);
 
		 //Specular
			half d = dot(i.normal,i.lightDir)*0.5+_SOffset;
			half4 ramp = tex2D(_SRamp, float2(d,d));
			half4 sCol = (step(_SSize, ramp.r))*ramp* d*_SColor;
			c+=sCol;
			
			//Rim
			half rim = ((1 - abs(dot(i.normal, i.viewDir)))*_RimSize);
			rim = tex2D(_RimRamp, float2(rim,rim));
			c+=rim;
 
 
		      // foam edge
		   //float4 foam = ( step(i.filling, 0.5) - step(i.filling, (0.5 - _Rim)))  ;
           //float4 foamColored = foam * (_FoamColor * 0.9);
		   // rest of the liquid
		   float4 result = step(i.filling, 0.5);
           float4 resultColored = result * c;
		   // both together, with the texture
           float4 finalResult = resultColored;				
		   finalResult.rgb += c;
 
		   // color of backfaces/ top
		   float4 topColor = fixed4(0,0,1,1) * (result);
		   //VFACE returns positive for front facing, negative for backfacing
		   return facing > 0 ? finalResult: topColor;
 
         }
         ENDCG
        }
 
    }
}
