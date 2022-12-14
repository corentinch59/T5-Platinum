using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class ScreenShot : MonoBehaviour
{
    public Texture2D text;
    /*private bool takeScreenShot;
  
   private void OnEnable()
   {
       RenderPipelineManager.endCameraRendering += RenderPipelineManager_endCameraRendering;
       
   }
   
   private void OnDisable()
   {
       RenderPipelineManager.endCameraRendering -= RenderPipelineManager_endCameraRendering;
   }

   private void RenderPipelineManager_endCameraRendering(ScriptableRenderContext arg1, Camera arg2)
   {
       
       if (takeScreenShot)
       {
           takeScreenShot = false;
           int width = Screen.width;
           int height = Screen.height;
           Texture2D screenShotTexture = new Texture2D(width, height, TextureFormat.ARGB32, false);
           Rect rect = new Rect(0, 0, width, height); 
           screenShotTexture.ReadPixels(rect, 0, 0);
           screenShotTexture.Apply();
           Debug.Log("lol");
           byte[] byteArray = screenShotTexture.EncodeToPNG();
           System.IO.File.WriteAllBytes(Application.dataPath + "/CameraScreenshot.png", byteArray);
       }
       
   }
   */

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.A))
        { 
            //ScreenCapture.CaptureScreenshot("ScreenShot.png");
            //StartCoroutine(TakeScreenShot());
            //takeScreenShot = true;
           
        }
    }

    private IEnumerator TakeScreenShot()
    {
        //Wait so EveryThing is rendered
        yield return new WaitForEndOfFrame();

        int width = Screen.width;
        int height = Screen.height;
        Texture2D screenShotTexture = new Texture2D(width, height, TextureFormat.ARGB32, false);
        Rect rect = new Rect(0, 0, width, height); 
        //Permet de capturer tous les pixels dans le rectangle donné (ici tout l'écran)
        screenShotTexture.ReadPixels(rect, 0, 0);
        screenShotTexture.Apply();
        
        //save directement la texture in game pour la reutiliser pour l'écran de fin
        text = screenShotTexture;
        /*
        //Permet de convertir une texture en png puis de la save dans les assets
        byte[] byteArray = screenShotTexture.EncodeToPNG();
        System.IO.File.WriteAllBytes(Application.dataPath + "/CameraScreenshot.png", byteArray);
        */
    }
    
    
}
