uniform sampler3D baseTexture;
uniform float SampleDensityValue;
uniform float TransparencyValue;
uniform float AlphaFuncValue;

varying vec4 cameraPos;
varying vec4 vertexPos;
varying mat4 texgen;

void main(void)
{ 
    vec3 t0 = (texgen * vertexPos).xyz;
    vec3 te = (texgen * cameraPos).xyz;

    if (te.x>=0.0 && te.x<=1.0 &&
        te.y>=0.0 && te.y<=1.0 &&
        te.z>=0.0 && te.z<=1.0)
    {
        // do nothing... te inside volume
    }
    else
    {
        if (te.x<0.0)
        {
            float r = -te.x / (t0.x-te.x);
            te = te + (t0-te)*r;
        }

        if (te.x>1.0)
        {
            float r = (1.0-te.x) / (t0.x-te.x);
            te = te + (t0-te)*r;
        }

        if (te.y<0.0)
        {
            float r = -te.y / (t0.y-te.y);
            te = te + (t0-te)*r;
        }

        if (te.y>1.0)
        {
            float r = (1.0-te.y) / (t0.y-te.y);
            te = te + (t0-te)*r;
        }

        if (te.z<0.0)
        {
            float r = -te.z / (t0.z-te.z);
            te = te + (t0-te)*r;
        }

        if (te.z>1.0)
        {
            float r = (1.0-te.z) / (t0.z-te.z);
            te = te + (t0-te)*r;
        }
    }

    const float max_iteratrions = 2048.0;
    float num_iterations = ceil(length(te-t0)/SampleDensityValue);
    if (num_iterations<2.0) num_iterations = 2.0;

    if (num_iterations>max_iteratrions) 
    {
        num_iterations = max_iteratrions;
    }

    vec3 deltaTexCoord=(te-t0)/float(num_iterations-1.0);
    vec3 texcoord = t0;

    vec4 fragColor = vec4(0.0, 0.0, 0.0, 0.0); 
    while(num_iterations>0.0)
    {
        vec4 color = texture3D( baseTexture, texcoord);
        float r = color[3]*TransparencyValue;
        if (r>AlphaFuncValue)
        {
            fragColor.xyz = fragColor.xyz*(1.0-r)+color.xyz*r;
            fragColor.w += r;
        }

        if (fragColor.w<color.w)
        {
            fragColor = color;
        }
        texcoord += deltaTexCoord; 

        --num_iterations;
    }

    fragColor.w *= TransparencyValue;

    if (fragColor.w>1.0) fragColor.w = 1.0; 
    if (fragColor.w<AlphaFuncValue) discard;
    
    gl_FragColor = fragColor;
}
