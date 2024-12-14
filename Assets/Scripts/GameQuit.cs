using UnityEngine;


public class GameQuit : MonoBehaviour
{
    protected void Start()
    {
        
    }

    protected void Update()
    {
        // Escキーが押された場合、アプリケーションを終了する
        if (Input.GetKeyDown(KeyCode.Escape)) Application.Quit();
    }
}
