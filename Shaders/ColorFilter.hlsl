float4 _globalColorFilterColor;
float _globalColorFilterIntensity;

void colorFilter_half(
    in float4 objectPosition, 
    in float startFromRadius,
    in float filterRange,
    in float4 color, 
    out float4 colorWithFilter)
{
    float height = sqrt(objectPosition.y);
    float2 dir = float2(objectPosition.x, objectPosition.z);
    float mul = max(0, length(dir) - startFromRadius);
    float intensitySign = sign(mul);
    
    colorWithFilter = lerp
    (
            color,
            _globalColorFilterColor, 
            min(1, filterRange * intensitySign * height * _globalColorFilterIntensity)
    );
}