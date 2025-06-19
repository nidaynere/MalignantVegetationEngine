
float4 _globalWindDirectionNormal;
float _globalWindDirectionPower;
float _globalWindDirectionStutter;
float4 _globalWindNoiseTiling;

void noiseSampler_half(
    in float4 objectWorldPosition, 
    in float4 vertexPosition,
    in float vertexBasedNoisePower,
    out float2 uv)
{
    uv = _globalWindNoiseTiling.xy + float2(
    objectWorldPosition.x + _TimeParameters.x * _globalWindDirectionStutter,
    objectWorldPosition.z);
    
    uv.x += vertexPosition.x * vertexBasedNoisePower;
}

void f_half (
    in float4 objectWorldPosition, 
    in float4 vertexWorldPosition, 
    in float4 vertexObjectPosition,
    in float startFromRadius,
    in float startFromHeight,
    in float heightWindPower,

    in float noise,

    out float4 resultWorldPosition,
    out float4 debugColor
)
{
    float2 v2D = float2(vertexWorldPosition.x, vertexWorldPosition.z);
    float2 o2D = float2(objectWorldPosition.x, objectWorldPosition.z);

    float dist = distance(v2D, o2D);
    float distPower = max(0, dist - startFromRadius);
    float heightPower = heightWindPower * max(0, vertexObjectPosition.y - startFromHeight);
    float vertexWindPower = noise * (sign(distPower) * heightPower);

    debugColor = lerp(float4(0, 0, 0, 0), float4(1, 1, 1, 1), vertexWindPower * 10);
    
    resultWorldPosition = vertexWorldPosition + vertexWindPower * _globalWindDirectionNormal * _globalWindDirectionPower;
}