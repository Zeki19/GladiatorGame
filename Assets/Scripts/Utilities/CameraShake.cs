using UnityEngine;
using Unity.Cinemachine;
using UnityEngine.Serialization;

public class CameraShake : MonoBehaviour
{
    [SerializeField] private CinemachineImpulseSource impulseSource;

    void Start()
    {
        ServiceLocator.Instance.RegisterService(this);
    }

    public void Shake(float force = 1f)
    {
        if (impulseSource != null)
        {
            impulseSource.GenerateImpulse(force);
        }
    }
}