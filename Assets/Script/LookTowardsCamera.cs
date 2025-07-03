using UnityEngine;

public class LookTowardsCamera : MonoBehaviour
{
    private enum CameraFacingType
    {
        LookAtCamera,
        CameraForward
    }

    [SerializeField] private CameraFacingType facingType;

    private Camera _mainCam;

    private void Awake()
    {
        _mainCam = Camera.main;
    }

    private void LateUpdate()
    {
        switch (facingType)
        {
            case CameraFacingType.LookAtCamera:
                transform.LookAt(_mainCam.transform.position, Vector3.up);
                break;
            case CameraFacingType.CameraForward:
                transform.forward = _mainCam.transform.forward;
                break;
        }
    }
}
