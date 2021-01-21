using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Camera))]
public class SnapshotCamera : MonoBehaviour
{
    Camera snapCam;

    int resWidth = 256;
    int resHeight = 256;

    public string filePath;

    void Awake()
    {
        snapCam = GetComponent<Camera>();
        if(snapCam.targetTexture == null)
        {
            snapCam.targetTexture = new RenderTexture(resWidth, resHeight, 24);
        }
        else
        {
            resWidth = snapCam.targetTexture.width;
            resHeight = snapCam.targetTexture.height;
        }

        filePath = Application.dataPath + "/../Snapshots/";

        if(!System.IO.Directory.Exists(filePath))
        {
            System.IO.Directory.CreateDirectory(filePath);
        }
    }

    void Update()
    {
            Texture2D snapshot = new Texture2D(resWidth, resHeight, TextureFormat.RGB24, false);
            snapCam.Render();
            RenderTexture.active = snapCam.targetTexture;
            snapshot.ReadPixels(new Rect(0, 0, resWidth, resHeight), 0, 0);
            byte[] bytes = snapshot.EncodeToJPG();
            string fileName = SnapShotName();
            System.IO.File.WriteAllBytes(fileName, bytes);
            Debug.Log("Snapshot captured - " + fileName);
    }

    private string SnapShotName()
    {
        return string.Format("{0}/snap_{1}x{2}_{3}.jpg",
            filePath,
            resWidth, resHeight,
            System.DateTime.Now.ToString("yyy-MM-dd-HH-mm-ss-SSS"));
    }
}
