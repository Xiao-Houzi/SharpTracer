#version 440
precision mediump float;

in vec4 vColor;
in mat4 Data;
in vec2 vUV;

out vec4 fColor;

vec4 Grid(vec2 point, float num)
{
    vec2 r = vec2(1, 1.732);
    vec2 h = r*.5;
    vec2 uv = point*num;
    
    vec2 a = mod(uv, r) - h;
    vec2 b = mod(uv-h, r) - h;
    
    uv = length(a)<length(b)? a:b;
    vec2 id = point*num-uv;
    
    return vec4(uv, id); 
}

vec2 Hex(vec2 uv)
{
    uv=abs(uv);
    float c = dot(uv, normalize(vec2(1, 1)));
    c=max(c, uv.x);
    return vec2(c, 0);
}


void main()
{
    vec2 uv = vUV;
    uv.y*=Data[3][3];
	vec4 coord = Grid(uv, 48.);
    uv = coord.xy;
    
    float c = length(Hex(uv));
    c = smoothstep(c, .48, .5);
    
    vec3 col = vec3(c*.1);

    // Output to screen
    fColor = vec4(col * vec3(.5, .7, 1), 1.0);
}