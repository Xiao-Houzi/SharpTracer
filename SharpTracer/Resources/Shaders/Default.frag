#version 440

precision mediump float;
in vec4 vColor;
in vec3 normal;
in mat4 Data;
out vec4 fColor;

void main()
{
    float light = dot(normal, vec3(-.5,.866,0)) + 1.0 * 0.4;
    vec4 color = vColor;
    color.rgb *= (0.2+light);
     fColor = color;
}