using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DotsVisualization : MonoBehaviour
{
    public GameObject channelParent;
    public static DotsVisualization Instance;
    private void Awake()
    {
        Instance = this;
    }
    // Start is called before the first frame update
    void Start()
    {
        Debug.Log(channelParent.transform.GetChild(0));
    }

    // Update is called once per frame
    void Update()
    {

    }
    public void Visualize(int channel, string dotName)
    {
        //Debug.Log("fffffff"+(channel - 1).ToString()+"name:"+dotName);
        //Debug.Log(channel);
        Transform channelObject;
        if (channelParent.transform.GetChild(channel - 1))
        {
            channelObject = channelParent.transform.GetChild(channel - 1);
        }
        else
        {
            return;
        }
        if (channelObject.Find(dotName) != null)
        {
            Transform dot = channelObject.Find(dotName);
            StartCoroutine(dot.GetComponent<DotControl>().Flicker());
        }
        else
        {
            return;
        }

    }
}
