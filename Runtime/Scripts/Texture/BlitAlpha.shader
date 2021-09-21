Shader "Blitter/BlitAlpha"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _Color("Multiplicative color", Color) = (1.0, 1.0, 1.0, 1.0)
    }
    SubShader
    {
        
        // Tags { "RenderType"="Opaque" }
        // LOD 100

        Tags {"Queue"="Transparent" "IgnoreProjector"="true" "RenderType"="Transparent"}
        ZWrite Off Blend SrcAlpha OneMinusSrcAlpha Cull Off
        
        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            
            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };
            
            struct v2f
            {
                float2 uv : TEXCOORD0;
                float4 vertex : SV_POSITION;
            };
            
            sampler _MainTex;
            uniform float4 _Color;
            
            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                return o;
            }
            
            float4 frag (v2f i) : SV_Target
            {
                float4 input_col = tex2D(_MainTex, i.uv) * _Color;
                input_col.a = _Color.a;
                /* post-processing here */
                return input_col;
            }
            ENDCG
        }
    }
}

// Shader "Blitter/BlitAlpha" {
    //     Properties
    //     {
        //         _MainTex ("Texture", any) = "" {}
        //         _Color("Multiplicative color", Color) = (1.0, 1.0, 1.0, 1.0)
    //     }
    //     SubShader {
        //         Pass {
            //             Tags {"Queue"="Transparent" "IgnoreProjector"="true" "RenderType"="Transparent"}
            //             ZWrite Off Blend SrcAlpha OneMinusSrcAlpha Cull Off

            //             CGPROGRAM
            //             #pragma vertex vert
            //             #pragma fragment frag
            //             #include "UnityCG.cginc"

            //             UNITY_DECLARE_SCREENSPACE_TEXTURE(_MainTex);
            //             uniform float4 _MainTex_ST;
            //             uniform float4 _Color;

            //             struct appdata_t {
                //                 float4 vertex : POSITION;
                //                 float2 texcoord : TEXCOORD0;
                //                 UNITY_VERTEX_INPUT_INSTANCE_ID
            //             };

            //             struct v2f {
                //                 float4 vertex : SV_POSITION;
                //                 float2 texcoord : TEXCOORD0;
                //                 UNITY_VERTEX_OUTPUT_STEREO
            //             };

            //             v2f vert (appdata_t v)
            //             {
                //                 v2f o;
                //                 UNITY_SETUP_INSTANCE_ID(v);
                //                 UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO(o);
                //                 o.vertex = UnityObjectToClipPos(v.vertex);
                //                 o.texcoord = TRANSFORM_TEX(v.texcoord.xy, _MainTex);
                //                 return o;
            //             }

            //             fixed4 frag (v2f i) : SV_Target
            //             {
                //                 UNITY_SETUP_STEREO_EYE_INDEX_POST_VERTEX(i);
                //                 fixed4 col = UNITY_SAMPLE_SCREENSPACE_TEXTURE(_MainTex, i.texcoord) * _Color;
                //                 col.a =  _Color.a;
                //                 return col;
            //             }
            //             ENDCG

        //         }
    //     }
    //     Fallback Off
// }
