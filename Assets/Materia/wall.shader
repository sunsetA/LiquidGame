Shader "Custom/UnlitExpandPixel" {
    Properties {
        _MainTex ("Texture", 2D) = "white" {}
        _Color ("Color", Color) = (1,1,1,1)
        _Xtiling ("X Tiling", Range(1,10)) = 1
        _ExpandFactor("DiscardBottom",Range(1,3))=1
    }
 
    SubShader {
        Tags {"Queue"="Transparent" "RenderType"="Transparent" }
        LOD 100
 
        Pass {
            ZWrite Off
            Blend SrcAlpha OneMinusSrcAlpha
            Cull Off
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"
 
            struct appdata_t {
                float4 vertex : POSITION;
                float4 color : COLOR;
                float2 uv : TEXCOORD0;
            };
 
            struct v2f {
                float2 uv : TEXCOORD0;
                float4 color : COLOR;
                float4 vertex : SV_POSITION;
                float height : TEXCOORD1; // 添加一个用于存储高度的纹理坐标
            };
                        sampler2D _MainTex;
            float4 _MainTex_ST;
            float4 _Color;
            int _Xtiling;
            float _ExpandFactor;

            v2f vert (appdata_t v) {
                v2f o;
                // o.vertex = UnityObjectToClipPos(v.vertex*_ExpandFactor);
                o.vertex = UnityObjectToClipPos(float3(v.vertex.x, v.vertex.y * _ExpandFactor, v.vertex.z));
                o.uv = float2(v.uv.x * _Xtiling, v.uv.y);
                o.color = v.color * _Color;
                o.height=v.vertex.y;
                return o;
            }
 
            fixed4 frag (v2f i) : SV_Target {
                fixed4 col = tex2D(_MainTex, i.uv.xy);
                col *= i.color;
                col.a *= _Color.a;
                col.a*=abs(sin(_Time.y*3)) ;
                if(col.a<0.7)
                {
                    col.a=0.7;
                    }
                   // col.a = saturate((col.a - 0.3) / (1 - 0.3)); // 使用saturate()函数将col.a限制在0.3-1之间
                return col;
            }
            ENDCG
        }
    }
    }
 
  