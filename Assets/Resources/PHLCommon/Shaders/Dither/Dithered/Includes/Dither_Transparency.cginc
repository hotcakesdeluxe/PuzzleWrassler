struct Input
{
	float2 uv_MainTex;
	float3 worldNormal;
	float3 worldPos;
	float4 screenPos;
};

void PerformDither(Input IN, fixed alpha)
{
	float4x4 thresholdMatrix =
	{
		1.0 / 17.0,  9.0 / 17.0,  3.0 / 17.0, 11.0 / 17.0,
		13.0 / 17.0,  5.0 / 17.0, 15.0 / 17.0,  7.0 / 17.0,
		4.0 / 17.0, 12.0 / 17.0,  2.0 / 17.0, 10.0 / 17.0,
		16.0 / 17.0,  8.0 / 17.0, 14.0 / 17.0,  6.0 / 17.0
	};

	float4x4 _RowAccess = { 1,0,0,0, 0,1,0,0, 0,0,1,0, 0,0,0,1 };
	float2 pos = IN.screenPos.xy / IN.screenPos.w;
	pos *= _ScreenParams.xy;

	clip(alpha - thresholdMatrix[fmod(pos.x, 4)] * _RowAccess[fmod(pos.y, 4)]);
}