//-----------------------------------------------------------------------------
// Fog settings
//-----------------------------------------------------------------------------

uniform const float		FogEnabled		: register(c0);
uniform const float		FogStart		: register(c1);
uniform const float		FogEnd			: register(c2);
uniform const float3	FogColor		: register(c3);

uniform const float3	EyePosition		: register(c4);		// in world space


//-----------------------------------------------------------------------------
// Material settings
//-----------------------------------------------------------------------------

uniform const float3	DiffuseColor	: register(c5) = 1;
uniform const float		Alpha			: register(c6) = 1;
uniform const float3	EmissiveColor	: register(c7) = 0;
uniform const float3	SpecularColor	: register(c8) = 1;
uniform const float		SpecularPower	: register(c9) = 16;


//-----------------------------------------------------------------------------
// Lights
// All directions and positions are in world space and must be unit vectors
//-----------------------------------------------------------------------------

uniform const float3	AmbientLightColor		: register(c10);

uniform const float3	LightDirection		: register(c11);
uniform const float3	LightDiffuseColor	: register(c12);
uniform const float3	LightSpecularColor	: register(c13);


//-----------------------------------------------------------------------------
// Matrices
//-----------------------------------------------------------------------------

uniform const float4x4	World		: register(vs, c20);	// 20 - 23
uniform const float4x4	View		: register(vs, c24);	// 24 - 27
uniform const float4x4	Projection	: register(vs, c28);	// 28 - 31



//-----------------------------------------------------------------------------
// Textures
//-----------------------------------------------------------------------------
float2 TextureScale;

texture Texture;
texture AlphaTexture;

sampler Sampler = sampler_state
{
    Texture = (Texture);

    MinFilter = Linear;
    MagFilter = Linear;
    MipFilter = Linear;
    
    AddressU = Wrap;
    AddressV = Wrap;
};
sampler AlphaSampler = sampler_state
{
    Texture = (AlphaTexture);

    MinFilter = Linear;
    MagFilter = Linear;
    MipFilter = Linear;
    
    AddressU = Wrap;
    AddressV = Wrap;
};

//-----------------------------------------------------------------------------
// Structure definitions
//-----------------------------------------------------------------------------

struct ColorPair
{
	float3 Diffuse;
	float3 Specular;
};


//-----------------------------------------------------------------------------
// Compute lighting
// E: Eye-Vector
// N: Unit vector normal in world space
//-----------------------------------------------------------------------------
ColorPair ComputeLights(float3 E, float3 N)
{
	ColorPair result;
	
	result.Diffuse = AmbientLightColor;
	result.Specular = 0;

	// Directional Light 0
	float3 L = -LightDirection;
	float3 H = normalize(E + L);
	float2 ret = lit(dot(N, L), dot(N, H), SpecularPower).yz;
	result.Diffuse += LightDiffuseColor * ret.x;
	result.Specular += LightSpecularColor * ret.y;
	
	
	result.Diffuse *= DiffuseColor;
	result.Diffuse	+= EmissiveColor;
	result.Specular	*= SpecularColor;
		
	return result;
}


// Vertex shader program.
void VertexShader(float4 position : POSITION0,
				  float3 normal : NORMAL0,
				  float2 uv : TEXCOORD0,
				  out float4 oPosition : POSITION0,
				  out float2 oUV : TEXCOORD0,
				  out float2 oUV1 : TEXCOORD1,
				  out float4 oDiffuse : COLOR0,
				  out float4 oSpecular : COLOR1)
{
	float4 pos_ws = mul(position, World);
	float4 pos_vs = mul(pos_ws, View);
	float4 pos_ps = mul(pos_vs, Projection);
	
	oPosition = pos_ps;
	oUV1 = uv;
	oUV = TextureScale * uv;
	
	float3 N = normalize(mul(normal, World));
	float3 posToEye = EyePosition - pos_ws;
	float3 E = normalize(posToEye);
	
	ColorPair lightResult = ComputeLights(E, N);
	
	oDiffuse	= float4(lightResult.Diffuse.rgb, Alpha);
	oSpecular	= float4(lightResult.Specular.rgb, 0);
}



// Pixel shader program.
float4 PixelShader(float4 diffuse : COLOR0, 
				   float4 specular : COLOR1, 
				   float2 uv : TEXCOORD0,
				   float2 uv1 : TEXCOORD1) : COLOR0
{	
    float4 color = tex2D(Sampler, uv);
    color.a *= tex2D(AlphaSampler, uv1).a;
    

	return diffuse * color + float4(specular.rgb, 0);
}



technique Default
{
    pass Default
    {
        VertexShader = compile vs_2_0 VertexShader();
        PixelShader = compile ps_2_0 PixelShader();
    }
}