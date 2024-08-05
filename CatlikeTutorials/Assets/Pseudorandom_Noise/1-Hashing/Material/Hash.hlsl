



void ShaderGraphFunction_float(float3 In, out float3 Out, out float3 Color) {
    Out = In;
    Color = GetHashColor();
}

void ShaderGraphFunction_half(half3 In, out half3 Out, out half3 Color) {
    Out = In;
    Color = GetHashColor();
}