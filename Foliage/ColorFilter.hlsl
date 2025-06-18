float4 _globalColorFilterColor;
float _globalColorFilterIntensity;

void colorFilter_half(
    in float4 objectPosition, 
    in float4 color, 
    out float4 colorWithFilter)
{
    float height = objectPosition.y;

    colorWithFilter = lerp(color, _globalColorFilterColor, height * _globalColorFilterIntensity);
}