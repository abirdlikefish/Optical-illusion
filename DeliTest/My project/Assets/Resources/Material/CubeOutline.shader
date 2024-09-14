Shader "Custom/CubeOutline"
{
    Properties 
    {

        _Color("Color",Color)=(1.0,1.0,1.0,1.0)
        _EdgeColor("Edge Color",Color)=(1.0,1.0,1.0,1.0)
        _Width("Width",Range(0,1))=0.2

        _ColorA ("Color A", Color) = (1,0,0,1)
        _ColorB ("Color B", Color) = (0,0,1,1)
    }
    SubShader
    {

        Tags { 
        "Queue"="Transparent" 
        "IgnoreProjector"="True" 
        "RenderType"="Transparent" 
        }
        Blend SrcAlpha OneMinusSrcAlpha
        LOD 200
        Cull Front
        zWrite off
        Pass
        {

            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #pragma target 3.0
            #include "UnityCG.cginc"

            struct a2v {
                half4 uv : TEXCOORD0 ;
                half4 vertex : POSITION ;
            };

            struct v2f{
                half4 pos : SV_POSITION ;
                half4 uv : TEXCOORD0  ;            
            };
            fixed4 _Color;
            fixed4 _EdgeColor;
            float _Width;
    
            v2f vert(a2v v)
            {
                v2f o;
                o.uv = v.uv;
                o.pos=UnityObjectToClipPos(v.vertex);
                return o;
            }


            fixed4 frag(v2f i) : COLOR
            {
                fixed4 col;
                float lx = step(_Width, i.uv.x);
                float ly = step(_Width, i.uv.y);
                float hx = step(i.uv.x, 1.0 - _Width);
                float hy = step(i.uv.y, 1.0 - _Width);
                col = lerp(_EdgeColor, _Color, lx*ly*hx*hy);
                return col;
            }
        ENDCG
        }
        Blend SrcAlpha OneMinusSrcAlpha
        LOD 200 
        Cull Back
        zWrite off
        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #pragma target 3.0
            #include "UnityCG.cginc"

            struct a2v {
                half4 uv : TEXCOORD0 ;
                half4 vertex : POSITION ;
            };

            struct v2f{
                half4 pos : SV_POSITION ;
                half4 uv : TEXCOORD0  ;            
            };
            fixed4 _Color;
            fixed4 _EdgeColor;
            float _Width;
    
            v2f vert(a2v v)
            {
                v2f o;
                o.uv = v.uv;
                o.pos=UnityObjectToClipPos(v.vertex);
                return o;
            }


            fixed4 frag(v2f i) : COLOR
            {
                fixed4 col;
                float lx = step(_Width, i.uv.x);
                float ly = step(_Width, i.uv.y);
                float hx = step(i.uv.x, 1.0 - _Width);
                float hy = step(i.uv.y, 1.0 - _Width);
                col = lerp(_EdgeColor, _Color, lx*ly*hx*hy);
                return col;
        }
        ENDCG
        }
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }
        LOD 200

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            #include "UnityCG.cginc"

            struct appdata_t
            {
                float4 vertex : POSITION;
                float3 normal : NORMAL;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                float4 vertex : SV_POSITION;
            };

            fixed4 _ColorA;
            fixed4 _ColorB;

            v2f vert (appdata_t v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.vertex.xz; // 使用 xz 坐标来计算渐变
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                float t = (i.uv.x + 1.0) * 0.5; // 归一化 x 坐标
                return lerp(_ColorA, _ColorB, t); // 线性插值
            }
            ENDCG
        }
    }

    FallBack "Diffuse"
}