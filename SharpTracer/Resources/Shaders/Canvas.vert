#version 440

layout (location = 0) in vec3 Position;
layout (location = 1) in vec4 Color;
layout (location = 2) in vec2 UV;
layout (location = 2) in vec3 Normal;

uniform mat4 uModelMatrix;

out vec2 uv;

void main()
{
    vec4 Pos = uModelMatrix * vec4(Position, 1.0);
    gl_Position = Pos;
}
