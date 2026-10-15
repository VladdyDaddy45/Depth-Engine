using System.Numerics;

namespace Depth;

public class Transform
{
    public Vector3 Position;
    public Vector3 Rotation {get; set {field = new Vector3(fixRot(value.X),fixRot(value.Y),fixRot(value.Z)); UpdateVectors(this); } }
    public Vector3 Scale;

    public Vector3 Forward, Right, Up;
    public Matrix4x4 RotationMatrix;
    public Quaternion Orientation;

    public Transform()
    {
        Position = new Vector3(0f,0f,0f);
        Rotation = new Vector3(0f,0f,0f);
        Scale = new Vector3(1f,1f,1f);
    }

    private static void UpdateVectors(Transform t)
    {
        Matrix4x4 RotationMatrix = Matrix4x4.CreateFromYawPitchRoll(
            t.Rotation.X, 
            t.Rotation.Y, 
            t.Rotation.Z
        );

        t.Forward = Vector3.Transform(Vector3.UnitZ, RotationMatrix);
        t.Right = Vector3.Transform(Vector3.UnitX, RotationMatrix);
        t.Up = Vector3.Transform(Vector3.UnitY, RotationMatrix);
    }

    public static Matrix4x4 toMatrix4x4(Transform t)
    {
        t.Orientation = Quaternion.Identity * 
        Quaternion.CreateFromYawPitchRoll(t.Rotation.X, t.Rotation.Y, t.Rotation.Z);

        return
        Matrix4x4.Identity * 
        Matrix4x4.CreateScale(t.Scale) *
        Matrix4x4.CreateFromQuaternion(t.Orientation) *
        Matrix4x4.CreateTranslation(t.Position);
    }
    
    public static Vector3[] ToMatrix3x3(Transform t)
    {
        Matrix4x4 wrld = toMatrix4x4(t);
        Vector3[] mat =
        [
            new Vector3(wrld.M11, wrld.M12, wrld.M13),
            new Vector3(wrld.M21, wrld.M22, wrld.M23),
            new Vector3(wrld.M31, wrld.M32, wrld.M33),
        ];
        return mat;
    }

    public static float[] ToFloatArr(Transform transform)
    {
        return Mat4ToFloatArr(toMatrix4x4(transform));
    }

    public static float[] Mat4ToFloatArr(Matrix4x4 m)
    {
        var result = new float[16];
        // matrices and hotdogs
        result[0]   = m.M11;
        result[1]   = m.M12;
        result[2]   = m.M13;
        result[3]   = m.M14;
    
        result[4]   = m.M21;
        result[5]   = m.M22;
        result[6]   = m.M23;
        result[7]   = m.M24;
    
        result[8]   = m.M31;
        result[9]   = m.M32;
        result[10]  = m.M33;
        result[11]  = m.M34;
    
        result[12]  = m.M41;
        result[13]  = m.M42;
        result[14]  = m.M43;
        result[15]  = m.M44;

        return result;
    }


    private static float fixRot(float input)
    {
        if (input < -MathF.PI * 2)
            return input + (MathF.PI * 4);

        if (input > MathF.PI * 2)
            return input - MathF.PI * 4;

        return input;
    }
}