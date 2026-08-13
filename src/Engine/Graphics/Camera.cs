using Silk.NET.OpenGL;
using Silk.NET.Maths;
using Utils.CMath;
using System.Numerics;
using Engine.Graphics;

namespace Engine.Graphics;

public class Camera
{
    private static GL gl = Video.gl;
    public Transform transform;
    public float fov
    {
        get;
        set
        {
            field = value;
            proj = Matrix4x4.CreatePerspectiveFieldOfView(CMath.rad(fov), (float)Application.MainApp.width / (float)Application.MainApp.height, 0.1f, 1000.0f);
        }
    }

    
    public Matrix4x4 view => Matrix4x4.CreateLookAt(transform.Position, transform.Position + transform.Forward, transform.Up);
    public Matrix4x4 proj;

    public Camera(Transform Trans, float FOV)
    {
        transform = Trans;
        fov = FOV;
    }
}