﻿using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;


public class PictureTaker : MonoBehaviour
{
    [Header("Camera Settings")]
    public RawImage pictureHolder;

    //  Events
    public delegate void CameraEvents(Texture2D texture2d, RawImage rawImage);
    public static CameraEvents OnPictureTaken;

    // Start is called before the first frame update
    void Start()
    {
       
    }

    /// <summary>
    /// Opens the native camera of device and will cache picture to raw image
    /// </summary>
    /// <param name="maxSize"></param>
    public void TakePicture()
    {
        // Don't attempt to use the camera if it is already open
        if (NativeCamera.IsCameraBusy())
            return;

        NativeCamera.Permission permission = NativeCamera.TakePicture((path) =>
        {
            Debug.Log("Image path: " + path);
            if (path != null)
            {
                // Create a Texture2D from the captured image
                Texture2D texture = NativeCamera.LoadImageAtPath(path);
                if (texture == null)
                {
                    Debug.Log("Couldn't load texture from " + path);
                    return;
                }

                pictureHolder.texture = texture;

                //  Broadcast events
                OnPictureTaken?.Invoke(texture, pictureHolder);
            }
        });

        Debug.Log("Permission result: " + permission);
    }

    public void RecordVideo()
    {
        // Don't attempt to use the camera if it is already open
        if (NativeCamera.IsCameraBusy())
            return;

        NativeCamera.Permission permission = NativeCamera.RecordVideo((path) =>
        {
            Debug.Log("Video path: " + path);
            if (path != null)
            {
                // Play the recorded video
                Handheld.PlayFullScreenMovie("file://" + path);
            }
        });

        Debug.Log("Permission result: " + permission);
    }
}
