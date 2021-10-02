Shader "Toon/LitPotionMask" {
	SubShader
    {

        Pass
        {    
           
             Stencil
             {
                 Ref 1
                 Comp Always
                 Pass Replace
             }            

        ZWrite On
        ColorMask 0
        }
    }
}
