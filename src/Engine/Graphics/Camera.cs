using Silk.NET.OpenGL;
using Silk.NET.Maths;
using Utils.CMath;
using System.Numerics;
using Engine.Graphics;

namespace Engine.Graphics;

public class Camera
{
    private static float aspect_ratio => (float)Application.MainApp.width / (float)Application.MainApp.height;
    private static GL gl = Video.gl;
    public Transform transform;
    public float fov;

    //public Matrix4x4 view => Matrix4x4.CreateLookAt(transform.Position, transform.Rotation)
    public Matrix4x4 view => Matrix4x4.CreateLookAt(transform.Position, transform.Forward*2, transform.Up*2);
    public Matrix4x4 proj => Matrix4x4.CreatePerspectiveFieldOfView(CMath.rad(fov), aspect_ratio, 0.01f, 1000.0f);

    public Camera(Transform Trans, float FOV)
    {
        transform = Trans;
        fov = FOV;
    }
}