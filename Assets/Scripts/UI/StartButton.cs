using Unity.VectorGraphics;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StartButton : MonoBehaviour
{
    private void Awake()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }
    public void StartGame() 
    {
        // load the game scene
        SceneManager.LoadScene(1);
    }
}
