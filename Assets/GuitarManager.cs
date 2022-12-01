using System;
using UnityEngine;
using UnityEngine.UI;

public class GuitarManager : MonoBehaviour
{
    public Transform LeftHandAnchor;
    public Transform TopBendingTransform;
    public Transform BottomBendingTransform;
    public LineRenderer BbendingLineRenderer;
    public TMPro.TMP_Text BendingInfoText;

    public float BBendingValue { get; private set; } = 0f;

    private Vector3 SavedTopBendingPos;
    private Vector3 SavedBottomBendingPos;

    private bool HasBendingSet = false;
    private bool[] bendingSet = { false, false };

    private void Update()
    {
        UpdateBendingTransforms();

        CheckBendingIsSet();

        if (HasBendingSet)
        {
            if (LeftHandAnchor.position.y >= TopBendingTransform.position.y)
                BBendingValue = 0f;
            else if (LeftHandAnchor.position.y <= BottomBendingTransform.position.y)
                BBendingValue = 1f;
            else
            {
                float maxDist = Vector3.Distance(TopBendingTransform.position, BottomBendingTransform.position);
                float currDist = Vector3.Distance(TopBendingTransform.position, LeftHandAnchor.position);

                if (currDist == 0f)
                    BBendingValue = 0f;
                else
                    BBendingValue = currDist / maxDist;
            }

            BbendingLineRenderer.SetPosition((int)BendingSet.Top, TopBendingTransform.position);
            BbendingLineRenderer.SetPosition((int)BendingSet.Bottom, BottomBendingTransform.position);

            BendingInfoText.text = $"Bending value: {BBendingValue}";
        }
    }

    private void UpdateBendingTransforms()
    {
        TopBendingTransform.position = new Vector3(LeftHandAnchor.position.x, SavedTopBendingPos.y, LeftHandAnchor.position.z);
        BottomBendingTransform.position = new Vector3(LeftHandAnchor.position.x, SavedBottomBendingPos.y, LeftHandAnchor.position.z);
    }

    private void CheckBendingIsSet()
    {
        if (bendingSet[(int)BendingSet.Top] && bendingSet[(int)BendingSet.Bottom])
            HasBendingSet = true;
    }

    public void OnMaxBendingReset()
    {
        SavedBottomBendingPos = LeftHandAnchor.position;

        bendingSet[(int)BendingSet.Bottom] = true;
    }

    public void OnMinBendingReset()
    {
        SavedTopBendingPos = LeftHandAnchor.position;

        bendingSet[(int)BendingSet.Top] = true;
    }
    private enum BendingSet
    {
        Top,
        Bottom
    }
}
