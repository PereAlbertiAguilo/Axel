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

    AstarPath astar;
    Pathfinding.AstarData data;
    Pathfinding.GridGraph gridGraph;

    PlayerController playerController;

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        astar = AstarPath.active;
        Pathfinding.AstarData data = astar.data;
        gridGraph = data.gridGraph;

        playerController = FindObjectOfType<PlayerController>();
    }

    public async void ChangeCameraPos(Transform newPos)
    {
        playerController.canMove = false;

        //FadeBlack.instance.StartCoroutine(FadeBlack.instance.FadeInFadeOut(.1f));
        FadeBlack.instance.FadeFromBlack();

        gridGraph.center = newPos.position;
        gridGraph.Scan();

        await Task.Delay(150);

        playerController.canMove = true;

        virtualCamera.Follow = newPos;
        virtualCamera.LookAt = newPos;
    }
}
