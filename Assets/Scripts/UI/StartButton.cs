using Unity.VectorGraphics;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StartButton : MonoBehaviour
{
    public void StartGame() 
    {
        // load the game scene
        SceneManager.LoadScene(1);
    }
}
