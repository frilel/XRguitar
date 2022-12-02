using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DotControl : MonoBehaviour
{
    private Transform dotVisual;
    // Start is called before the first frame update
    void Start()
    {
        dotVisual = transform.GetChild(0);
    }

    // Update is called once per frame
    void Update()
    {

    }
    public IEnumerator Flicker()
    {
        /*if (dotVisual.gameObject.activeSelf)
        {
            yield return null;
        }*/
        Debug.LogWarning("worked!");
        dotVisual.gameObject.SetActive(true);
        yield return new WaitForSeconds(0.5f);
        dotVisual.gameObject.SetActive(false);
    }
}
