#version 400
in vec3 pos;
in vec4 ray;
out vec4 frag_colour;
uniform float iGlobalTime;
uniform vec2 iResolution;

float sdSphere( vec3 p, float s )
{
  return length(p)-s;
}
float rand(vec2 co){
    return fract(sin(dot(co.xy ,vec2(12.9898,78.233))) * 43758.5453);
}
bool passIhit()
{
	if(pos.x > 0.8 && pos.y > 0.8)
	{
		return true;
	}
	return false;
}
void main () {  
	if(passIhit())
		frag_colour = vec4 (0.5, 0.0, 0.5, 1.0);
	else
	{
		frag_colour = vec4(rand(pos.xy));
	}
}