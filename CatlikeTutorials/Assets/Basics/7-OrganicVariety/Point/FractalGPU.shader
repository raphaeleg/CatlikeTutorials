Shader "Custom/FractalGPU"
{
    SubShader
    {
        Tags { "RenderType"="Opaque" }
        LOD 200

        CGPROGRAM
        #pragma surface ConfigureSurface Standard fullforwardshadows addshadow
        #pragma instancing_options assumeuniformscaling procedural:ConfigureProcedural
        #pragma editor_sync_compilation
        #pragma target 4.5

        #include "FractalGPU.hlsl"
        
        struct Input
        {
            float3 worldPos; // empty struct doesn't compile
        };

        float _Smoothness;

        void ConfigureSurface (Input input, inout SurfaceOutputStandard surface) {
            surface.Albedo = GetFractalColor().rgb;
            surface.Smoothness = GetFractalColor().a;
        }
        ENDCG
    }
    FallBack "Diffuse"
}
