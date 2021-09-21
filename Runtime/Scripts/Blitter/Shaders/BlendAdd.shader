Shader "Blitter/BlendAdd"
{
    Properties
    {
        [NoScaleOffset] _MainTex ("Texture", 2D) = "white" {}
        [NoScaleOffset] _Mask ("Mask", 2D) = "white" {}
        [NoScaleOffset] _Back ("Back", 2D) = "white" {}
        _Color("Tint Color", Color) = (1.0, 1.0, 1.0, 1.0)
        _Alpha ("Opacity", Range(0.0, 1.0)) = 1.0
    }
    SubShader
    {
        Tags {"Queue"="Transparent" "IgnoreProjector"="true" "RenderType"="Transparent"}
        ZWrite Off Blend SrcAlpha OneMinusSrcAlpha Cull Off ZTest Never
        
        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            #include "UnityCG.cginc"
            #include "PSBlendModes.cginc"
            
            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };
            
            struct v2f
            {
                float2 uv : TEXCOORD0;
                float4 vertex : SV_POSITION;
                float2 buv : TEXCOORD1;
            };
            
            sampler _MainTex;
            sampler _Mask;
            sampler _Back;
            uniform float4 _Color;
            float _Alpha;
            
            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                o.buv = ComputeGrabScreenPos(o.vertex);
                return o;
            }
            
            float4 frag (v2f i) : SV_Target
            {
                float4 back = tex2D(_Back, i.buv);
                float4 input = tex2D(_MainTex, i.uv) * _Color;
                float4 mask = tex2D(_Mask, i.uv);
                float4 result = input * _Alpha;
                result = Add(back, result);
                result.a = mask.r;
                return result;
            }
            ENDCG
        }
    }
}