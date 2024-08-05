Shader "Custom/Hash"
{
    Properties {
        _Smoothness ("Smoothness", Range(0,1)) = 0.5
    }

    SubShader {
        Tags { "RenderType"="Opaque" }
        LOD 200

        CGPROGRAM
        #pragma surface ConfigureSurface Standard fullforwardshadows addshadow
        #pragma instancing_options assumeuniformscaling procedural:ConfigureProcedural
        #pragma editor_sync_compilation
        #pragma target 4.5

        struct Input
        {
            float3 worldPos;
        };
        float _Smoothness;
        float4 _Config;
        #if defined(UNITY_PROCEDURAL_INSTANCING_ENABLED)
	        StructuredBuffer<uint> _Hashes;
        #endif

        void ConfigureProcedural() { // directly derive the instance's position from its identifier
            #if defined(UNITY_PROCEDURAL_INSTANCING_ENABLED)
		        float v = floor(_Config.y * unity_InstanceID) + 0.00001;	// GPUs don't have integer divisions
		        float u = unity_InstanceID - _Config.x * v;	
		
		        // use the UV coordinates to place the instance on the XZ plane, 
		        // offset and scaled such that it remains inside the unit cube at the origin.
		        unity_ObjectToWorld = 0.0;
		        unity_ObjectToWorld._m03_m13_m23_m33 = float4(
			        _Config.y * (u + 0.5) - 0.5,
			        _Config.z * ((1.0 / 255.0) * (_Hashes[unity_InstanceID] >> 24) - 0.5),
			        _Config.y * (v + 0.5) - 0.5,
			        1.0
		        );
		        unity_ObjectToWorld._m00_m11_m22 = _Config.y;
            #endif
        }

        float3 GetHashColor(){ // retrieves the hash and uses it to produce an RGB color
            #if defined(UNITY_PROCEDURAL_INSTANCING_ENABLED)
		        uint hash = _Hashes[unity_InstanceID];
                return (1.0 / 255.0) * float3(
			        hash & 255,
			        (hash >> 8) & 255,
			        (hash >> 16) & 255
		        );
            #else
                return 1.0;
            #endif
        }

        void ConfigureSurface (Input input, inout SurfaceOutputStandard surface) {
            surface.Smoothness = _Smoothness;
            surface.Albedo = GetHashColor();
        }

        ENDCG
    }
    FallBack "Diffuse"
}