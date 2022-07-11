float Zoom;
int MaxIterations;
float2 Aspect;
float2 Pan;
float2 Constant;

float Depth(float2 zInput)
{
    int iterations = 0;
    float2 z = zInput;
    float zxSqr = 0;
    float zySqr = 0;

    do
    {
        zxSqr = z.x * z.x;
        zySqr = z.y * z.y;
        z = float2(zxSqr - zySqr, 2 * z.x * z.y) + Constant;
        iterations++;
    } while (zxSqr + zySqr <= 4.0 && iterations < MaxIterations);

    return (float(iterations) + 1 - (log(log(sqrt(zxSqr + zySqr))) / log(2.0))) / float(MaxIterations);
}

float4 PixelShaderFunction(float2 coords: TEXCOORD0) : COLOR0
{
    float2 pixel = (coords - 0.5) * Zoom * Aspect - Pan;
    float color = Depth(pixel);
    return float4(sin(color * 4), sin(color * 5), sin(color * 6), 1);
}

technique Technique1
{
    pass Pass1
    {
        PixelShader = compile ps_3_0 PixelShaderFunction();
    }
}