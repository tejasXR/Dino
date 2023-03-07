using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class PhotoMetaData
{
    public int Points;
}

[System.Serializable]
public struct PhotoData
{
    public Sprite PhotoSprite;
    public PhotoMetaData PhotoMetaData;

    public PhotoData(Sprite sprite, PhotoMetaData metaData)
    {
        PhotoSprite = sprite;
        PhotoMetaData = metaData;
    }
    
}

public class TakePhoto : MonoBehaviour
{
    [SerializeField] private Camera photoCamera;
    [SerializeField] private Image previewImage;

    private List<PhotoData> _photos = new();
    
    private const int PhotoWidth = 300;
    private const int PhotoHeight = 300;
    private const int PhotoZDepth = 10;
    
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            SavePhoto();
        }
    }

    private void SavePhoto()
    {
        var rt = new RenderTexture(PhotoWidth, PhotoHeight, 24);
        var snapshotTexture = new Texture2D(PhotoWidth, PhotoHeight, TextureFormat.RGB24, false);
        
        // Setup our render texture and assign this texture to our snapshot camera 
        photoCamera.targetTexture = rt;
        
        // Render our texture and read screen pixels
        RenderTexture.active = rt;
        photoCamera.Render();
        snapshotTexture.ReadPixels(new Rect(0, 0, PhotoWidth, PhotoHeight), 0, 0);
        snapshotTexture.Apply();
        
        var photoMetaData = GetMetaDataFromPhoto();

        // We've saved our texture, null out our camera render texture and destroy our cached render texture
        photoCamera.targetTexture = null;
        RenderTexture.active = null;
        Destroy(rt);

        Sprite tempSprite = Sprite.Create(snapshotTexture, new Rect(0, 0, PhotoWidth, PhotoHeight), new Vector2(0, 0));
        previewImage.sprite = tempSprite;

        var photoData = new PhotoData(tempSprite, photoMetaData);
        _photos.Add(photoData);
        
        Debug.Log($"Points earned from photo: {photoMetaData.Points}");
    }

    private PhotoMetaData GetMetaDataFromPhoto()
    {
        RaycastHit[] hitResults = new RaycastHit[100];
        Physics.BoxCastNonAlloc(photoCamera.transform.position,
            new Vector3(.75F, .75F, PhotoZDepth), photoCamera.transform.forward, hitResults);

        var points = 0;
        foreach (var raycastHit in hitResults)
        {
            if (raycastHit.collider == null) continue;
            if (raycastHit.collider.CompareTag("ObjectOfInterest")) points++;
        }

        var pMD = new PhotoMetaData
        {
            Points = points
        };

        return pMD;
    }
}
