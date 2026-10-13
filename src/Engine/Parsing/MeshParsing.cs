using Silk.NET.Assimp;
using System.Numerics;
using AssimpMesh = Silk.NET.Assimp.Mesh;
using Texture = Silk.NET.Assimp.Texture;
using Mesh = Depth.Graphics.Mesh;

namespace Parsing;

struct Vertex
{
    public Vector3 Pos;
    public Vector3 Norm;
    public Vector3 Tangent;
    public Vector2 TexCoords;
    public Vector3 Bitangent;

    public int[] BoneIds;
    public float[] Weights;

    public static readonly int MAX_BONE_INFLUENCE = 4;

    public override string ToString()
    {
        return Pos.X + ", " + Pos.Y + ", " + Pos.Z;
    }
}

public partial class Importer
{
    static Assimp assimp = Assimp.GetApi();
    public static List<Mesh> meshes = new List<Mesh>();

    public static unsafe Mesh LoadMesh(string path)
    {
        var scene = assimp.ImportFile(path, (uint)PostProcessSteps.Triangulate);

        if (scene == null || scene->MFlags == Assimp.SceneFlagsIncomplete || scene -> MRootNode == null)
            throw new Exception(assimp.GetErrorStringS());

        ProcessNode(scene->MRootNode, scene);
        Mesh result = meshes[0];
        meshes.Clear();

        return result;
    }

    static unsafe void ProcessNode(Node* node, Scene* scene)
    {
        for (int i = 0; i < node->MNumMeshes; i++)
        {
            var mesh = scene->MMeshes[node->MMeshes[i]];
            meshes.Add(ProcessMesh(mesh, scene));
        }

        for (int i = 0; i < node->MNumChildren; i++)
            ProcessNode(node->MChildren[i], scene);
    }

    static unsafe Mesh ProcessMesh(AssimpMesh* mesh, Scene* scene)
    {
        var vertices = new List<Vertex>();
        var indices = new List<uint>();
        var textures = new List<Texture>();

        for (uint i = 0; i < mesh->MNumVertices; i++)
        {
            Vertex vtx = new Vertex
            {
                BoneIds = new int[Vertex.MAX_BONE_INFLUENCE],
                Weights = new float[Vertex.MAX_BONE_INFLUENCE],
                Pos = mesh->MVertices[i]
            };

            if (mesh->MNormals != null)
                vtx.Norm = mesh->MNormals[i];

            if (mesh->MTangents != null)
                vtx.Tangent = mesh->MTangents[i];
            
            if (mesh->MBitangents != null)
                vtx.Bitangent = mesh->MBitangents[i];

            if (mesh->MTextureCoords[0] != null)
            {
                Vector3 texcoord3 = mesh->MTextureCoords[0][i];
                vtx.TexCoords = new Vector2(texcoord3.X, texcoord3.Y);
            }

            vertices.Add(vtx);
        }

        for (uint i = 0; i < mesh->MNumFaces; i++)
        {
            Face face = mesh->MFaces[i];

            for (uint j = 0; j < face.MNumIndices; j++)
                indices.Add(face.MIndices[j]);
        }

        // TODO: Add texture loading
        var VAO = new Depth.Graphics.VertexArray<float, uint>(
            new Depth.Graphics.BufferObject<float>(BuildVerts(vertices),BufferTargetARB.ArrayBuffer), // VBO
            new Depth.Graphics.BufferObject<uint>(indices.ToArray(),BufferTargetARB.ElementArrayBuffer)
        );
        
        VAO.AddAttribute(0, 3, VertAttribType.Float); // position
        VAO.AddAttribute(1, 3, VertAttribType.Float); // normal
        VAO.AddAttribute(2, 3, VertAttribType.Float); // color
        VAO.AddAttribute(3, 2, VertAttribType.Float); // texcoords
        VAO.CompileAttributes();
        return new Mesh(VAO);
    }

    static float[] BuildVerts(List<Vertex> vertices)
    {
        var result = new List<float>();

        for (int i = 0; i < vertices.Count; i++)
        {
            result.Add(vertices[i].Pos.X);
            result.Add(vertices[i].Pos.Y);
            result.Add(vertices[i].Pos.Z);
            result.Add(vertices[i].Norm.X);
            result.Add(vertices[i].Norm.Y);
            result.Add(vertices[i].Norm.Z);
            result.AddRange([.5f,.5f,.5f]);
            result.Add(vertices[i].TexCoords.X);
            result.Add(vertices[i].TexCoords.Y);
        }

        return result.ToArray();
    }
}