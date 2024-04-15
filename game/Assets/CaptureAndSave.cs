using System.Collections;
using UnityEngine;

public class CaptureAndSave : MonoBehaviour
{
    public Camera captureCamera; // Assign your camera in the inspector
    public string folderName = "CapturedPictures";
    public string fileNamePrefix = "Picture";

    private int pictureIndex = 0;

    void Start()
    {
        InvokeRepeating("CapturePicture", 0f, .8f); 
    }

    void CapturePicture()
    {
        StartCoroutine(TakeAndSavePicture());
    }

    IEnumerator TakeAndSavePicture()
    {
        yield return new WaitForEndOfFrame();

        // Get the screen resolution
        int width = Screen.width;
        int height = Screen.height;

        // Create a texture the size of the screen, RGB24 format
        Texture2D tex = new Texture2D(width, height, TextureFormat.RGB24, false);

        // Capture the screen
        Rect rect = new Rect(0, 0, width, height);
        captureCamera.targetTexture = RenderTexture.GetTemporary(width, height, 24);
        captureCamera.Render();
        RenderTexture.active = captureCamera.targetTexture;
        tex.ReadPixels(rect, 0, 0);
        tex.Apply();

        // Reset the camera's target texture
        captureCamera.targetTexture = null;
        RenderTexture.active = null;
        RenderTexture.ReleaseTemporary(captureCamera.targetTexture);

        // Encode texture into PNG
        byte[] bytes = tex.EncodeToPNG();
        Destroy(tex);

        // Determine the file path
        string folderPath = Application.persistentDataPath + "/" + folderName;
        if (!System.IO.Directory.Exists(folderPath))
            System.IO.Directory.CreateDirectory(folderPath);

        string filePath = folderPath + "/" + fileNamePrefix + "_" + pictureIndex + ".png";

        // Save the PNG file to disk
        System.IO.File.WriteAllBytes(filePath, bytes);

        Debug.Log("Picture saved at: " + filePath);

        pictureIndex++;
    }
}
