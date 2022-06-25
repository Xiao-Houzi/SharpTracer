#version 440

layout (location = 0) in vec3 Position;
layout (location = 1) in vec4 Color;
layout (location = 2) in vec2 UV;

uniform mat4 uModelMatrix;
uniform mat4 uViewMatrix;
uniform mat4 uProjMatrix;

uniform mat4 uData;

out vec4 vColor;
out mat4 Data;
out vec2 vUV;

void main()
{
    vec4 pos =  uProjMatrix * uViewMatrix * uModelMatrix * vec4(Position, 1.0);

    gl_Position = pos;
    gl_PointSize = 50.0;
    vColor = Color;
    Data = uData;
    vUV = UV;
}
