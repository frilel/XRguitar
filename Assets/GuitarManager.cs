using System;
using UnityEngine;
using UnityEngine.UI;

public class GuitarManager : MonoBehaviour
{
    public Transform OVRControllerVisual;
    public Transform MinBendingTransform;
    public Transform MaxBendingTransform;
    public LineRenderer BbendingLineRenderer;
    public TMPro.TMP_Text BendingInfoText;

    public float BBendingValue { get; private set; } = 0f;

    private Vector3 MinBendingPos;
    private Vector3 MaxBendingPos;

    private bool HasBendingRange = false;
    private int[] bendingSet = new int[2];

    private void Start()
    {

    }

    private void Update()
    {
        UpdateBendingTransforms();

        CheckBendingSet();

        if (HasBendingRange)
        {
            if (OVRControllerVisual.position.y >= MinBendingTransform.position.y)
                BBendingValue = 0f;
            else if (OVRControllerVisual.position.y <= MinBendingTransform.position.y)
                BBendingValue = 1f;
            else
            {
                float maxDist = Vector3.Distance(MinBendingTransform.position, MaxBendingTransform.position);
                float currDist = Vector3.Distance(MinBendingTransform.position, OVRControllerVisual.position);

                if (currDist == 0f)
                    BBendingValue = 0f;
                else
                    BBendingValue = currDist / maxDist;


            }

            BbendingLineRenderer.SetPosition(0, MinBendingTransform.position);
            BbendingLineRenderer.SetPosition(1, MaxBendingTransform.position);

            BendingInfoText.text = $"Bending value: {BBendingValue}";
        }
    }

    private void UpdateBendingTransforms()
    {
        MinBendingTransform.position = new Vector3(OVRControllerVisual.position.x, MinBendingPos.y, OVRControllerVisual.position.z);
        MaxBendingTransform.position = new Vector3(OVRControllerVisual.position.x, MaxBendingPos.y, OVRControllerVisual.position.z);
    }

    private void CheckBendingSet()
    {
        if (bendingSet[0] == 1 && bendingSet[1] == 1)
            HasBendingRange = true;
    }

    public void OnMaxBendingReset()
    {
        MaxBendingPos = OVRControllerVisual.position;

        bendingSet[1] = 1;
    }

    public void OnMinBendingReset()
    {
        MinBendingPos = OVRControllerVisual.position;

        bendingSet[0] = 1;
    }

}
