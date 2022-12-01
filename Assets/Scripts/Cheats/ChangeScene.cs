using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;

public class ChangeScene : MonoBehaviour
{
    public List<string> scenes = new List<string>();
    private int sceneNber = 0;

    public void LD1(InputAction.CallbackContext ctx)
    {
        if (ctx.performed)
        {
            SceneManager.LoadScene(scenes[sceneNber]);
            sceneNber++;
            if(sceneNber >= scenes.Count)
            {
                sceneNber = 0;
            }
        }
    }
}
