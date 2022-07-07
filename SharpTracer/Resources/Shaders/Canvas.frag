#version 440

precision mediump float;

uniform sampler2D texture1;

in vec2 uv;
out vec4 fColor;

void main()
{
     fColor = texture(texture1, uv);
}