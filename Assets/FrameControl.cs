using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class FrameControl : MonoBehaviour
{
    public Transform content;
    public Material contentMaterial;
    public bool isPortrait;
    public Material[] portraitMaterial;
    public Material[] landscapeMaterial;

    [ContextMenu("ChangeImage")]
    public void ChangeImage()
    {
        content.GetComponent<MeshRenderer>().material = contentMaterial;
    }
    public void RandomizeImage()
    {
        if (isPortrait)
        {
            
            if (portraitMaterial.Length < 6)
            {
                Debug.Log(transform);
            }
            contentMaterial = portraitMaterial[Random.Range(0, portraitMaterial.Length)];
        }
        else
        {
            
            if (landscapeMaterial.Length < 9)
            {
                Debug.Log(transform);
            }
            contentMaterial = landscapeMaterial[Random.Range(0, landscapeMaterial.Length)];
        }
        content.GetComponent<MeshRenderer>().material = contentMaterial;
    }
    [ContextMenu("RandomizeAllImages")]
    public void RandomizeAllImages()
    {
        foreach (FrameControl frameControl in FindObjectsOfType<FrameControl>())
        {
            frameControl.RandomizeImage();
        }
    }
}
