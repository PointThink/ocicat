namespace ocicat.Graphics.Rendering;

public class Primitives
{
	public Mesh RectangleMesh;
	public Shader UntexturedMeshShader;
	public Shader TexturedMeshShader;
	public Shader SpritesheetShader;
	public Shader TextShader;
	public Texture WhiteTexture;

	public Shader BatchedQuadShader;

	public Primitives(Renderer renderer)
	{
		float[] rectVertices = {
			0, 0, 0, 1,
			1, 0, 1, 1,
			1, 1, 1, 0,
			0, 1, 0, 0
		};

		uint[] rectIndices =
		{
			0, 1, 2, 2, 3, 0
		};

		RectangleMesh = new Mesh(
			renderer, rectVertices, rectIndices,
			new BufferLayout([
				new BufferElement("position", ShaderDataType.Float2),
				new BufferElement("uv", ShaderDataType.Float2)
			])
		);

		string untexturedVertShader = @"#version 450 core
layout (location = 0) in vec2 aPosition;

uniform mat4 projection;
uniform mat4 transform;
uniform mat4 positionMat;

void main()
{
    gl_Position = vec4(aPosition, 0.0, 1.0) * transform * projection;
}";

		string untexturedFragShader = @"#version 450 core
out vec4 FragColor;
uniform vec4 color;

void main()
{
    FragColor = color;
}";

		string texturedVertShader = @"#version 450 core
layout (location = 0) in vec2 aPosition;
layout (location = 1) in vec2 texCoords;

uniform mat4 projection;
uniform mat4 transform;
uniform mat4 positionMat;

out vec2 vTexCoords;

void main()
{
    gl_Position = vec4(aPosition, 0.0, 1.0) * transform * projection;
	vTexCoords = texCoords;
}";

		string texturedFragShader = @"#version 450 core
out vec4 FragColor;

uniform sampler2D textureSampler;
uniform vec4 tint;

in vec2 vTexCoords;

void main()
{
    FragColor = texture(textureSampler, vTexCoords) * tint;
}";

		string spritesheetFragShader = @"#version 450 core
out vec4 FragColor;

uniform sampler2D textureSampler;
uniform vec4 tint;

in vec2 vTexCoords;

uniform vec2 sheetSize;
uniform vec2 spriteOffset;
uniform vec2 spriteSize;

void main()
{
	vec2 spriteBegin = spriteOffset / sheetSize;
	vec2 spriteTrueSize = spriteSize / sheetSize;
	vec2 trueOffset = spriteTrueSize * vTexCoords;
    FragColor = texture(textureSampler, spriteBegin + trueOffset) * tint;
}";

		string fontFragShader = @"#version 450 core
out vec4 FragColor;

uniform sampler2D textures[32];

in vec4 vColor;
in vec2 vTexCoords;
in float vTextureID;

void main()
{
	int index = int(vTextureID);
	float smpl = texture(textures[index], vec2(vTexCoords.x, 1 - vTexCoords.y)).r;
	float sd = smpl - 0.5;
	float smoothing = fwidth(sd);
	float alpha = smoothstep(-smoothing, +smoothing, sd);

    FragColor = vec4(1, 1, 1, alpha) * vColor;
}";


		string batchedQuadVert = @"#version 450 core
layout (location = 0) in vec2 position;
layout (location = 1) in vec4 color;
layout (location = 2) in vec2 uvs;
layout (location = 3) in float textureId;

uniform mat4 projection;
uniform mat4 transform;

out vec2 vTexCoords;
out vec4 vColor;
out float vTextureID;

void main()
{
    gl_Position = vec4(position, 0.0, 1.0) * transform * projection;
	vTexCoords = uvs;
	vColor = color;
	vTextureID = textureId;
}";

		string batchedQuadFrag = @"#version 450 core
out vec4 FragColor;

in vec4 vColor;
in vec2 vTexCoords;
in float vTextureID;

uniform sampler2D textures[32];

void main()
{
	int index = int(vTextureID);
    FragColor = texture(textures[index], vTexCoords) * vColor;
}";

		UntexturedMeshShader = Shader.Create(renderer, untexturedVertShader, untexturedFragShader);
		TexturedMeshShader = Shader.Create(renderer, texturedVertShader, texturedFragShader);
		SpritesheetShader = Shader.Create(renderer, texturedVertShader, spritesheetFragShader);
		TextShader = Shader.Create(renderer, batchedQuadVert, fontFragShader);
		BatchedQuadShader = Shader.Create(renderer, batchedQuadVert, batchedQuadFrag);

		Image white = Image.GenerateWhitePixel();
		WhiteTexture = Texture.Create(renderer, white.Data, white.Width, white.Height, TextureFilter.Nearest);
	}
}