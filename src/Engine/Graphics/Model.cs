using System.Numerics;
using Depth.Graphics.Shaders;

namespace Depth.Graphics;

public struct DetailLevel
{
    public Mesh mesh;
    public Texture texture;
    public float distance;
}

public class Model
{
    public Mesh mesh;
    public Texture texture;
    public List<DetailLevel> LODs = new List<DetailLevel>();

    public Model(Mesh standardMesh, Texture standardTexture)
    {
        mesh = standardMesh;
        texture = standardTexture;
    }

    public void Draw()
    {
        texture.PassToShader(ShaderProgram.ActiveProgram);
        mesh.Draw();
    }
    
}