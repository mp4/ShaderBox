#version 400
out vec4 frag_colour;
uniform float iGlobalTime;
uniform vec2 iResolution;
//uniform 
vec4 fpar00[16];
in vec3 raydir;
bool intSphere( in vec4 sp, in vec3 ro, in vec3 rd, in float tm, out float t )
{
    bool  r = false;
    vec3  d = ro - sp.xyz;
    float b = dot(rd,d);
    float c = dot(d,d) - sp.w*sp.w;
    t = b*b-c;
    if( t > 0.0 )
    {
        t = -b-sqrt(t);
        r = (t > 0.0) && (t < tm);
    }

    return r;
}

bool intCylinder( in vec4 sp, in vec3 ro, in vec3 rd, in float tm, out float t )
{
    bool  r = false;
    vec3  d = ro - sp.xyz;
    float a = dot(rd.xz,rd.xz);
    float b = dot(rd.xz,d.xz);
    float c = dot(d.xz,d.xz) - sp.w*sp.w;
    t = b*b - a*c;
    if( t > 0.0 )
    {
        t = (-b-sqrt(t)*sign(sp.w))/a;
        r = (t > 0.0) && (t < tm);
    }
    return r;
}

bool intPlane( in vec4 pl, in vec3 ro, in vec3 rd, in float tm, out float t )
{
    t = -(dot(pl.xyz,ro)+pl.w)/dot(pl.xyz,rd);
    return (t > 0.0) && (t < tm);
}

vec3 calcnor(in vec4 ob,in vec4 ot,in vec3 po,out vec2 uv )
{
    vec3 no;

    if(ot.w>2.5)
    {
        no.xz = po.xz-ob.xz;
        no.y = 0.0;
        no = no/ob.w;
        uv = vec2(no.x,po.y+fpar00[12].w);
    }
    else if(ot.w>1.5)
    {
        no = ob.xyz;
        uv = po.xz*.2;
    }
    else
    {
        no = po-ob.xyz;
        no = no/ob.w;
        uv = no.xy;
    }

    return no;
}


float calcinter(in vec3 ro,in vec3 rd,out vec4 ob,out vec4 co)
{
    float tm=10000.0;
    float t;

    if( intSphere(  fpar00[0],ro,rd,tm,t) ) { ob = fpar00[0]; co = fpar00[ 6]; tm = t; }
    if( intSphere(  fpar00[1],ro,rd,tm,t) ) { ob = fpar00[1]; co = fpar00[ 7]; tm = t; }
    if( intCylinder(fpar00[2],ro,rd,tm,t) ) { ob = fpar00[2]; co = fpar00[ 8]; tm = t; }
    if( intCylinder(fpar00[3],ro,rd,tm,t) ) { ob = fpar00[3]; co = fpar00[ 9]; tm = t; }
    if( intPlane(   fpar00[4],ro,rd,tm,t) ) { ob = fpar00[4]; co = fpar00[10]; tm = t; }
    if( intPlane(   fpar00[5],ro,rd,tm,t) ) { ob = fpar00[5]; co = fpar00[11]; tm = t; }

    return tm;
}


bool calcshadow(in vec3 ro,in vec3 rd,in float l)
{
    float t;

    bvec4 ss = bvec4(intSphere(  fpar00[0],ro,rd,l,t),
                     intSphere(  fpar00[1],ro,rd,l,t),
                     intCylinder(fpar00[2],ro,rd,l,t),
                     intCylinder(fpar00[3],ro,rd,l,t));
    return any(ss);
}


vec4 calcshade(in vec3 po,in vec4 ob,in vec4 co,in vec3 rd,out vec4 re)
{
    float di,sp;
    vec2 uv;
    vec3 no;
    vec4 lz;

    lz.xyz = vec3(0.0,1.5,-3.0) - po;
    lz.w = length(lz.xyz);
    lz.xyz = lz.xyz/lz.w;

    no = calcnor(ob,co,po,uv);

    di = dot(no,lz.xyz);
    re.xyz = reflect(rd,no);
    sp = dot(re.xyz,lz.xyz);
    sp = max(sp,0.0);
    sp = sp*sp;
    sp = sp*sp;

    if( calcshadow(po,lz.xyz,lz.w) )
        di = 0.0;

    di = max(di,0.0);
    co = co*(vec4(.21,.28,.3,1) + .5*vec4(1.0,.9,.65,1.0)*di) + sp;

    di = dot(no,-rd);
    re.w = di;
    di = 1.0-di*di;
    di = di*di;

    return(co+0.6*vec4(di));
}

void main( void )
{

	fpar00[0] = vec4(0.1, 0.1, 0.1, 0.1);
    float tm;
    vec4 ob,co,co2,re,re2;
    vec3 no,po;
    vec3 ro = fpar00[12].xyz;
    vec3 rd = normalize(raydir);

    tm = calcinter(ro,rd,ob,co);

    po = ro + rd*tm;
    co = calcshade(po,ob,co,rd,re);

    tm = calcinter(po,re.xyz,ob,co2);
    po += re.xyz*tm;
    co2 = calcshade(po,ob,co2,re.xyz,re2);
    gl_FragColor=mix(co,co2,.5-.5*re.w) + vec4(fpar00[13].w);
};