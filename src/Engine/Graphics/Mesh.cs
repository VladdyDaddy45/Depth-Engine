using System.Numerics;
using Engine.Graphics;
using Silk.NET.GLFW;
using Silk.NET.Maths;
using Silk.NET.OpenGL;
using Silk.NET.Vulkan;

namespace Engine.Graphics;

public class Mesh
{
    private static GL gl = Video.gl;

    public struct Instance
    {
        public Matrix4x4 WorldMatrix;
        public VertexArray VAO;
        public Mesh mesh;
        
    }

    private static List<object> objects = new List<object>();

    public VertexArray vao;
    public Program shader;
    public uint buffer;

    private List<Instance> instances = new List<Instance>();


    public Mesh(VertexArray VAO)
    {
        vao = VAO;
        objects.Add(this);

        
    }

    
    public unsafe void ProcessBuffer()
    {
        buffer = gl.GenBuffers(1);
        gl.BindBuffer(GLEnum.ArrayBuffer, buffer);
        Matrix4x4[] matrices = new Matrix4x4[instances.Count];

        for (int i = 0; i < instances.Count; i++)
        {
            matrices[i] = instances[i].WorldMatrix;
        }
        fixed (Matrix4x4* transforms = matrices) {
            gl.BufferData(GLEnum.ArrayBuffer, (uint)instances.Count * (uint)sizeof(Matrix4x4), &transforms[0], GLEnum.DynamicDraw);
        }

        for (int i = 0; i < instances.Count; i++)
        {
            instances[i].VAO.Bind();

            gl.EnableVertexAttribArray(2);
            for (uint j = 0; j < 4; j++)
            {
                gl.EnableVertexAttribArray(j+2);
                gl.VertexAttribPointer(j+2, 4, GLEnum.Float, false, (uint)sizeof(Matrix4x4), (void*) (j * sizeof(Vector4)));
            }

            gl.VertexAttribDivisor(2, 1);
            gl.VertexAttribDivisor(3, 1);
            gl.VertexAttribDivisor(4, 1);
            gl.VertexAttribDivisor(5, 1);

            gl.BindVertexArray(0);
        }
    }
    

    /*
    public unsafe void ProcessBuffer()
    {
        buffer = gl.GenBuffers(1);
        gl.BindBuffer(GLEnum.ArrayBuffer, buffer);
        Matrix3x3[] matrices = new Matrix3x3[instances.Count];

        for (int i = 0; i < instances.Count; i++)
        {
            matrices[i] = instances[i].WorldMatrix;
        }
        fixed (Matrix3x3* transforms = matrices) {
            gl.BufferData(GLEnum.ArrayBuffer, (uint)instances.Count * (uint)sizeof(Matrix3x3), &transforms[0], GLEnum.DynamicDraw);
        }

        for (int i = 0; i < instances.Count; i++)
        {
            instances[i].VAO.Bind();

            gl.EnableVertexAttribArray(2);
            for (uint j = 0; j < 4; j++)
            {
                gl.EnableVertexAttribArray(j+2);
                gl.VertexAttribPointer(j+2, 4, GLEnum.Float, false, (uint)sizeof(Matrix3x3), (void*) (j * sizeof(Vector3)));
            }

            gl.VertexAttribDivisor(2, 1);
            gl.VertexAttribDivisor(3, 1);
            gl.VertexAttribDivisor(4, 1);

            gl.BindVertexArray(0);
        }
    }
    */
    public Instance NewInstance(Transform transform)
    {
        Instance instance;
        
        instance.WorldMatrix = transform.World;
        instance.VAO = vao;
        instance.mesh = this;

        instance.VAO.Bind();
        
        instances.Add(instance);

        return instance;
    }
    
    public void RemoveInstance(Instance instance)
    { instances.Remove(instance); }

    public unsafe void Draw()
    {
        instances[0].VAO.Bind();

        gl.DrawElementsInstanced(
            GLEnum.Triangles, 
            (uint)instances[0].VAO.indices.Length, 
            GLEnum.UnsignedInt, 
            (void*)0, 
            (uint)instances.Count
        );

        gl.BindVertexArray(0);
    }
}