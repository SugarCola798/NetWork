Shader "Custom/ToonBody"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _BaseMap ("Base Map", 2D) = "white"{}
        _LightMap ("LightMap", 2D) = "white" {}
        [Toggle(_USE_LIGHTMAP)] _USE_LIGHTMAP("USE LIGHTMAP", Range(0, 1)) = 1
    }
    
    SubShader
    {
        Tags
        {
            "RenderPipeline" = "UniversalRenderPipeline"
            "RenderType" = "Opaque"
        }
        
        HLSLINCLUDE
        #pragma multi_compile _MAIN_LIGHT_SHADOWS // 主光源阴影
        #pragma multi_compile _MAIN_LIGHT_SHADOWS_CASCADE // 主光源阴影级联
        #pragma multi_compile _MAIN_LIGHT_SHADOWS_SCREEN // 主光源阴影屏幕空间

        #pragma multi_compile_fragment _LIGHT_LAYERS // 光照层
        #pragma multi_compile_fragment _LIGHT_COOKIES // 光照饼干
        #pragma multi_compile_fragment _SCREEN_SPACE_OCCLUSION // 屏幕空间遮挡
        #pragma multi_compile_fragment _ADDITIONAL_LIGHT_SHADOWS // 额外光源阴影
        #pragma multi_compile_fragment _SHADOWS_SOFT // 阴影软化
        #pragma multi_compile_fragment _REFLECTION_PROBE_BLENDING // 反射探针混合
        #pragma multi_compile_fragment _REFLECTION_PROBE_BOX_PROJECTION // 反射探针盒投影

        #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl" // 核心库
        #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Lighting.hlsl" // 光照库
        
        #pragma shader_feature_local _USE_LIGHTMAP
        CBUFFER_START(UnityPerMaterial) //常量缓冲区
            sampler2D _BaseMap;
            sampler2D _LightMap;
        CBUFFER_END
        ENDHLSL

        Pass
        {
            Name "UniversalForward"
            Tags
            {
                "LightMode" = "UniversalForward"
            }
            
            HLSLPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            struct input
            {
                float4 positionOS : POSITION;
                float2 uv0 : TEXCOORD0;
                float3 normalOS : NORMAL;
            };

            struct Varings
            {
                float4 pos : SV_POSITION;
                float2 uv0 : TEXCOORD0;
                float3 normalWS : TEXCOORD1;
            };
            
            Varings vert(input i)
            {
                Varings output;
                VertexPositionInputs input = GetVertexPositionInputs(i.positionOS.xyz);
                output.pos = input.positionCS;
                output.uv0 = i.uv0;
                VertexNormalInputs normal_inputs = GetVertexNormalInputs(i.normalOS);
                output.normalWS = normal_inputs.normalWS;
                return output;
            }
            
            half4 frag(Varings v) : SV_TARGET
            {
                Light light = GetMainLight();
                half3 N = normalize(v.normalWS);
                half3 L = normalize(light.direction);
                half lambert = dot(N,L) * 0.5 + 0.5;
                half halfLambert = pow(lambert, 2);
                float4 baseMap = tex2D(_BaseMap, v.uv0);
                float4 lightMap = tex2D(_LightMap, v.uv0);
               
                #if _USE_LIGHTMAP
                     half ambient = lightMap.g; //环境光    
                #else
                      half ambient = halfLambert; //环境光
                #endif
                
                half shadow = (ambient + halfLambert) * 0.5f;
                float3 finalColor = baseMap.rgb * halfLambert * (shadow + 0.2);
                return float4(finalColor, 1);
            }
            ENDHLSL
        }
    }
}
