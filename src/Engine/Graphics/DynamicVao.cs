using System.Collections.Immutable;
using Depth.Graphics.Shaders;
using Silk.NET.OpenGL;

namespace Depth.Graphics;

public class DynamicVao<VertexData> where VertexData : struct
{
    static GL gl = Video.gl;

    uint empty;
    uint vertDataBuffer;
    List<VertexData> vertices;
    
    public unsafe DynamicVao(List<VertexData> verts)
    {
        vertices = verts;
    }

    public unsafe void Compile()
    {
        var verts = vertices.ToArray();
        gl.GenVertexArrays(empty);
        gl.CreateBuffers(vertDataBuffer);
        
        fixed (VertexData* arr = verts)
        gl.NamedBufferStorage(vertDataBuffer,
            (nuint)(sizeof(VertexData) * verts.Length),
            (void*)arr,
            (uint)GLEnum.DynamicStorageBit
        );
    }

    public void Draw(ShaderProgram program)
    {
        program.Use();
        
        gl.DrawArrays(GLEnum.Triangles, 0, (uint)vertices.Count);

        gl.UseProgram(0);
    }
}