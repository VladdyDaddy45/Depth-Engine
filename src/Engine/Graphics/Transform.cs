using System.Numerics;
using Utils.CMath;
using Silk.NET.Maths;

namespace Engine.Graphics;

public struct Transform
{
    public Transform() {}

    public Vector3 Position {get; set;} = new Vector3(0f,0f,0f);
    public Vector3 Rotation {get; set;} = new Vector3(0f,0f,0f);

    public float Scale {get; set;} = 1f;

    // Allows for idk man im tired and dont wanna write this comment
    public Quaternion Orientation => Quaternion.Identity * 
        Quaternion.CreateFromYawPitchRoll(Rotation.X, Rotation.Y, Rotation.Z);
    
    // rotation matrix for calculating forward and up vectors.
    private Matrix3X3<float> rotmat => Matrix3X3.CreateFromYawPitchRoll<float>(
        Rotation.X, 
        Rotation.Y, 
        Rotation.Z
    );

    /*
     precomputation less expensive. 
     need less power. save on electricity. 
     take wife to dinner with money you saved. 
     life good.
    */

    public Vector3 Forward => Vector3.Normalize(new Vector3(
        MathF.Cos(Rotation.X) * MathF.Cos(Rotation.Y),
        MathF.Sin(Rotation.Y),
        MathF.Sin(Rotation.X) * MathF.Cos(Rotation.Y))
    );
    
    public Vector3 Right => Vector3.Normalize(new Vector3(
            MathF.Cos(Rotation.X),
            0f,
            -MathF.Sin(Rotation.X)
        )
    );

    public Matrix4x4 world => 
        Matrix4x4.Identity * 
        Matrix4x4.CreateFromQuaternion(Orientation) *
        Matrix4x4.CreateScale(Scale) *
        Matrix4x4.CreateTranslation(Position);
}