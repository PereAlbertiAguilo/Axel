using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;
using System.Threading.Tasks;

public class CameraController : MonoBehaviour
{
    public static CameraController instance;

    public CinemachineVirtualCamera virtualCamera;
    public GameObject miniMapCam;

    private void Awake()
    {
        instance = this;
    }

    public void ChangeCameraPos(Transform newPos)
    {
        miniMapCam.transform.position = new Vector3(newPos.position.x, newPos.position.y, miniMapCam.transform.position.z);

        virtualCamera.Follow = newPos;
        virtualCamera.LookAt = newPos;
    }
}
