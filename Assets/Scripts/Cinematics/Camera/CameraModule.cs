using UnityEngine;

public interface ICameraModule
{
    void Init(CameraContext ctx);
}

public sealed class CameraContext
{
    public readonly Unity.Cinemachine.CinemachineCamera Cam;
    public readonly Transform Helper;

    public CameraContext(Unity.Cinemachine.CinemachineCamera cam, Transform helper)
    {
        Cam = cam; Helper = helper;
    }
}

public abstract class CameraModuleBase : MonoBehaviour, ICameraModule
{
    protected CameraContext Ctx { get; private set; }
    public void Init(CameraContext ctx) => Ctx = ctx;
}
