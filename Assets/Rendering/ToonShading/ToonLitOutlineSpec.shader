Shader "Toon/Lit Outline Specular" {
	Properties {
		_Color ("Main Color", Color) = (0.5,0.5,0.5,1)
		[HDR] _OutlineColor ("Outline Color", Color) = (0,0,0,1)
		_Outline ("Outline width", Range (0, 0.005)) = .0015
		_MainTex ("Base (RGB)", 2D) = "white" {}
		_Ramp ("Toon Ramp (RGB)", 2D) = "gray" {} 

		[HDR] _Emission ("Emission", Color) = (0.5,0.5,0.5,1)
		
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
		Tags { "RenderType"="Opaque" }
		UsePass "Toon/LitSpecularRim/FORWARD"
		UsePass "Toon/Basic Outline/OUTLINE"
	} 
	
	Fallback "Toon/Lit"
}
