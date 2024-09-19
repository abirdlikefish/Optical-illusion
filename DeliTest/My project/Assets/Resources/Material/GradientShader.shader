Shader "Custom/GradientDirection"
{
    Properties
    {
        _ColorBase ("_ColorBase", Color) = (1,0,0,1)
    }
    SubShader
    {
        LOD 300
        Tags { "RenderType"="Opaque" }

        Pass
        {  
            CGPROGRAM
            #pragma target 4.6
            #pragma vertex vert
            #pragma fragment frag
            
            #include "UnityCG.cginc"

            struct appdata_t
            {
                float4 vertex : POSITION; // 顶点位置
            };

            struct v2f
            {
                float4 pos : SV_POSITION; // 裁剪空间位置
                float3 worldPos : TEXCOORD0; // 
                float3 centerWorldPos : TEXCOORD1;
            };

            fixed4 _ColorBase;
            
            StructuredBuffer<float3> positionBuffer;
            StructuredBuffer<float4> colorBuffer;
            v2f vert (appdata_t v)
            {
                v2f o;

                // 对象空间到世界空间的转换
                o.worldPos = mul(unity_ObjectToWorld, v.vertex).xyz;

                float4 objectCenter = float4(0, 0, 0, 1);  // 中心点在对象空间是 (0,0,0)
                o.centerWorldPos = mul(unity_ObjectToWorld, objectCenter).xyz;  // 将中心点转换到世界空间

                // 将对象坐标转换为裁剪坐标
                o.pos = UnityObjectToClipPos(v.vertex);
                return o;
            }

            float4 frag (v2f i) : SV_Target
            {
                float t;
                float4 curColor = _ColorBase;
                for (int idx = 0; idx < positionBuffer.Length; idx++) 
                { 
                    //计算顶点世界坐标在positionBuffer上的投影长度
                    if(dot(i.worldPos - i.centerWorldPos, positionBuffer[idx].xyz) > 0)
                    {
                        float len = dot(i.worldPos - i.centerWorldPos, positionBuffer[idx].xyz) / length(positionBuffer[idx].xyz);
                        curColor = lerp(curColor, colorBuffer[idx],len);
                    }
                    
                }
                return curColor;
            }
            ENDCG
        }
    }
     FallBack "Diffuse"
}