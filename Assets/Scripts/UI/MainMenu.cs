using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    [SerializeField] Transform cameraFollow;

    [SerializeField] Button continueButon;

    private void Start()
    {
        if(PlayerPrefs.HasKey("Continue"))
        {
            continueButon.interactable = PlayerPrefs.GetInt("Continue") > 0;
        }
    }

    public void Play(string sceneName)
    {
        for (int i = 0; i < SceneManager.sceneCountInBuildSettings; i++)
        {
            string key = System.IO.Path.GetFileNameWithoutExtension(SceneUtility.GetScenePathByBuildIndex(i));

            if (PlayerPrefs.HasKey(key)) PlayerPrefs.DeleteKey(key);
        }

        PlayerPrefs.SetInt("Continue", 1);
        DataPersistenceManager.instance.NewGame();
        MenusManager.instance.ChangeScene(sceneName);
    }

    public void Continue()
    {
        MenusManager.instance.ChangeScene(DataPersistenceManager.instance.gameData.currentFloor);
    }

    public void Exit()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
        Application.Quit();
    }

    public void MovePanel(float offset)
    {
        cameraFollow.position = Vector3.up * offset;
    }
}
