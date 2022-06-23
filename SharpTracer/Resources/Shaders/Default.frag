#version 440

precision mediump float;
in vec4 vColor;
in mat4 Data;
out vec4 fColor;

void main()
{
     fColor = vColor * Data[0];
}