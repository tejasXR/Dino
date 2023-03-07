using UnityEngine;
using UnityEngine.UI;

public class TakePhoto : MonoBehaviour
{
    [SerializeField] private Camera photoCamera;
    [SerializeField] private Image previewImage;
    
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            SavePhoto();
        }
    }

    private void SavePhoto()
    {
        var width = 300;
        var height = 300;
            
        var rt = new RenderTexture(width, height, 24);
        var snapshotTexture = new Texture2D(width, height, TextureFormat.RGB24, false);
        
        // Setup our render texture and assign this texture to our snapshot camera 
        photoCamera.targetTexture = rt;
        
        // Render our texture and read screen pixels
        RenderTexture.active = rt;
        photoCamera.Render();
        snapshotTexture.ReadPixels(new Rect(0, 0, width, height), 0, 0);
        snapshotTexture.Apply();
        
        // We've saved our texture, null out our camera render texture and destroy our cached render texture
        photoCamera.targetTexture = null;
        RenderTexture.active = null;
        Destroy(rt);

        Sprite tempSprite = Sprite.Create(snapshotTexture, new Rect(0, 0, width, height), new Vector2(0, 0));
        previewImage.sprite = tempSprite;
    }
}
