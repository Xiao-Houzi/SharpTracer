#version 440

precision mediump float;
in vec4 vColor;
in mat4 Data;
in vec2 vUV;
out vec4 fColor;

uniform sampler2D texture1;
uniform sampler2D texture2;
uniform sampler2D texture3;

in mat3 texMat;
in mat3 imgMat;
in vec2 v_SSP;

void main()
{
     vec2 uv = vUV;
     uv.y = 1.-uv.y;
     float d = texture(texture2, (vec3(vUV*2.,1)*texMat).xy).r;
     vec4 imageCol = texture(texture1,(vec3(vUV*2.,1)*imgMat).xy);
     vec4 depthmix = mix(vec4(1), vec4(1,0,0,1), d);
     fColor = depthmix * imageCol;
}