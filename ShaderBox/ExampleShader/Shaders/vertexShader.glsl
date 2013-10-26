#version 400
in vec3 vp;
uniform float iGlobalTime;
uniform vec2 iResolution;
void main () {gl_Position = vec4 (vp, 1.0);};