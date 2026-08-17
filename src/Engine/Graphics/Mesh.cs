using System.Numerics;
using Engine.Graphics;
using Silk.NET.OpenGL;

namespace Engine.Graphics;

public class Mesh
{
    public struct Instance
    {
        
    }

    private static List<object> objects = new List<object>();

    public VertexArray vao;
    public Program shader;


    public Mesh(VertexArray VAO)
    {
        vao = VAO;
        objects.Add(this);
    }

    public void PushDraw()
    {
        
    }

    public void Draw()
    {
        
    }
}