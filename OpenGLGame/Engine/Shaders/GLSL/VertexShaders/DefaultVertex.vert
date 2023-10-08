#version 430 core
 
layout(location = 0) in vec3 aPosition;
layout(location = 1) in vec2 aTexCoord;

struct MVPSMatrix{
    mat4 modelMatrix;
    mat4 viewMatrix;
    mat4 projMatrix;
};


layout(std430,binding=3) buffer MatrixBlock{
    MVPSMatrix mvps_matrixes[];
};

MVPSMatrix currentMatrixes = mvps_matrixes[gl_InstanceID];

out vec2 texCoord;

void main(void)
{
    gl_Position = currentMatrixes.modelMatrix * currentMatrixes.viewMatrix * currentMatrixes.projMatrix * vec4(aPosition,1.0);
    texCoord = aTexCoord;
}