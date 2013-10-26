#version 400
in vec3 vp;
out vec3 pos;
out vec4 ray;
uniform float iGlobalTime;
uniform vec2 iResolution;

int fovX = 60;
int fovY = 40;
float depth = 1000.0;

void main () {
	gl_Position = vec4 (vp, 1.0);
	pos = vp;
	ray = vec4( cos(vp.x), cos(vp.y),1.0, 1.0)*vec4(0.0, 0.0, -1.0,  0.0);
};