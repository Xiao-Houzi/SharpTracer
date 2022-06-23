#version 440

layout (location = 0) in vec3 Position;
layout (location = 1) in vec4 Color;
layout (location = 2) in vec2 UV;

uniform mat4 uModelMatrix;
uniform mat4 uViewMatrix;
uniform mat4 uProjMatrix;

uniform mat4 uData;

out vec2 vUV;
out mat3 texMat;
out mat3 imgMat;

out vec2 v_SSP;

void main()
{
   imgMat = new mat3(0);
   texMat[0] = vec3( .5, 0., 0. );
   texMat[1] = vec3( 0., -.37, -.25 );
   texMat[2] = vec3( .5, .5 ,1. );

   imgMat[0] = vec3( .5, 0., 0. );
   imgMat[1] = vec3( 0., -.4, .0 );
   imgMat[2] = vec3( 0., 0.,1. );

   v_SSP = (gl_Position.xyz / gl_Position.w).xy;

   vec4 pos =  uProjMatrix * uViewMatrix * uModelMatrix * vec4(Position, 1.0);

   vUV = UV;
   gl_Position = pos;
}
