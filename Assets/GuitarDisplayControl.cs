using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GuitarDisplayControl : MonoBehaviour
{
    private int index;
    
    [ContextMenu("RandomizeAllGuitars")]
    void RandomizeAllGuitars()
    {
        foreach (GuitarDisplayControl guitarDisplayControl in FindObjectsOfType<GuitarDisplayControl>())
        {
            guitarDisplayControl.RandomizeGuitar();
        }
    }
    public void RandomizeGuitar()
    {
        index = Random.Range(0, transform.childCount);
        for (int i = 0; i < transform.childCount; i++)
        {
            if (i == index)
            {
                transform.GetChild(i).gameObject.SetActive(true);
            }
            else
            {
                transform.GetChild(i).gameObject.SetActive(false);
            }
        }
    }
}
