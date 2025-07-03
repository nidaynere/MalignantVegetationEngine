
#pragma multi_compile _ _CONDITION1

float4 _globalWindNormalStart;
float4 _globalWindNormalEnd;
float _globalWindDirectionPower;
float _globalWindDirectionStutter;
float4 _globalWindNoiseTiling;

struct InteractionBufferElement
{
    float3 Position;
    float Radius;
    float Power;
};

StructuredBuffer<InteractionBufferElement> _InteractionBuffer;

void noiseTiling_half(out float2 tiling)
{
    tiling = _globalWindNoiseTiling;
}

void noiseOffset_half(
    in float4 objectWorldPosition, 
    in float4 vertexPosition,
    in float vertexBasedNoisePower,
    out float2 offset)
{
    offset = float2(
    objectWorldPosition.x + _TimeParameters.x * _globalWindDirectionStutter,
    objectWorldPosition.z);
    offset.x += vertexPosition.x * vertexBasedNoisePower;
}

float2 calcInteraction(in float2 vertexPosition2D, float objectY, float interactionYDeduction)
{
    float2 interactionFinal = float2(0, 0);
    
    float deduction = pow(1 / (1 + interactionYDeduction), (1 + max(0, objectY)) * 2);
    
    for (int i = 0; i < 512; i++)
    {
        InteractionBufferElement interaction = _InteractionBuffer[i];

        float2 interactionPos2D = float2(interaction.Position.x, interaction.Position.z);
        float2 interactionDirection = (vertexPosition2D - interactionPos2D);
        float distance2D = length(interactionDirection);
    
        float radialPower = max(0, interaction.Radius - distance2D);
        if (radialPower <= 0)
        {
            continue;
        }
        
        float interactionPower = pow(radialPower, 2);
        interactionFinal += interactionPower * interactionDirection * deduction * interaction.Power;
    }
    
    interactionFinal = min(interactionFinal, float2(1, 1));
    
    return interactionFinal;
}

void f_half (
    in float4 objectWorldPosition, 
    in float4 vertexWorldPosition, 
    in float4 vertexObjectPosition,
    in float startFromRadius,
    in float startFromHeight,
    in float heightWindPower,
    in float defaultWindPower,
    in float interactionPower,
    in float interactionYDeduction,

    in float noise,

    out float4 resultWorldPosition
)
{
    float4 windNormal = lerp(_globalWindNormalStart, _globalWindNormalEnd, (noise - 0.5) * 2);
    
    float2 v2D = float2(vertexWorldPosition.x, vertexWorldPosition.z);
    float2 o2D = float2(objectWorldPosition.x, objectWorldPosition.z);

    float dist = sqrt(distance(v2D, o2D));
    float distPower = max(0, dist - startFromRadius);
    float heightPower = heightWindPower * max(0, vertexObjectPosition.y - startFromHeight);
    float vertexBendPower = noise * (sign(distPower) * (heightPower + defaultWindPower));

    resultWorldPosition = vertexWorldPosition + vertexBendPower * windNormal * _globalWindDirectionPower;
    
#ifdef _CONDITION1
    if (interactionPower > 0)
    {
        float noiseMod = (1 + noise) / 2;
        float2 interaction2D = calcInteraction(v2D, vertexObjectPosition.y, interactionYDeduction);
        float4 interactionFinal = noiseMod * float4(interaction2D.x, 0, interaction2D.y, 0) * vertexBendPower * interactionPower / 2;
        interactionFinal.y -= noiseMod * length(interaction2D) * vertexBendPower * heightPower * interactionPower;
        resultWorldPosition += interactionFinal;
    }
#endif
}

