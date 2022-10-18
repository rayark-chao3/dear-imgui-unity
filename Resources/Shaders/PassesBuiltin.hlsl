#ifndef DEARIMGUI_BUILTIN_INCLUDED
#define DEARIMGUI_BUILTIN_INCLUDED

#include "UnityCG.cginc"
#include "Packages/com.realgames.dear-imgui/Resources/Shaders/Common.hlsl"

sampler2D _Tex;

Varyings ImGuiPassVertex(ImVert input)
{
    Varyings output  = (Varyings)0;
    output.vertex    = UnityObjectToClipPos(float4(input.vertex, 0, 1));
    output.uv        = float2(input.uv.x, 1 - input.uv.y);
    output.color     = input.color;
    return output;
}

half4 ImGuiPassFrag(Varyings input) : SV_Target
{
    return input.color * tex2D(_Tex, input.uv);
}

#endif
