using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test : MonoBehaviour
{
    public delegate void OnMarkerDetectedDelegate(int markerId, Vector3 pos, Quaternion rot);
    public OnMarkerDetectedDelegate OnMarkerDetected;
    /// <summary>
    /// Process the image to figure out whether a marker has been detected
    /// If it has, it'll notify it
    /// </summary>
    /// <param name="imageData">The captured image</param>
    /// <param name="imageWidth">Width of the image</param>
    /// <param name="imageHeight">Height of the image</param>
    private void ProcessImage(List<byte> imageData, int imageWidth, int imageHeight)
    {
#if UNITY_WSA
       
                
                    //if(OnMarkerDetected != null)
                    //{
                    //    OnMarkerDetected( pos, rot);
                    //}
                
            
#endif
    }
}
