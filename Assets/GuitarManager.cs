using UnityEngine;
using UnityEngine.UI;

public class GuitarManager : MonoBehaviour
{
    public Transform CenterEyeAnchor;
    public Transform LeftHandAnchor;
    public Transform TopBendingTransform;
    public Transform BottomBendingTransform;
    public GameObject HoverButtons;
    private Vector3 HoverButtonsStartPos;
    public TMPro.TMP_Text SecondaryInfoText;

    public GameObject BendingFeedback;
    private Vector3 BendingFeedbackStartPos;
    public TMPro.TMP_Text BendingAmountText;
    public Slider BendingAmountSlider;

    public LineRenderer BbendingLineRenderer;

    public float BBendingValue { get; private set; } = 0f;

    private Vector3 SavedTopBendingPos;
    private Vector3 SavedBottomBendingPos;

    private bool[] bendingSet = { false, false };

    private void Start()
    {
        HoverButtonsStartPos = HoverButtons.transform.position;
        BendingFeedbackStartPos = BendingFeedback.transform.position;
    }
    private void Update()
    {
        UpdateBendingTransforms();
        UpdateHoverButtonsAndFeedback();

        if (CheckBendingIsSet())
        {
            if (LeftHandAnchor.position.y >= TopBendingTransform.position.y) // above top
                BBendingValue = 0f;
            else if (LeftHandAnchor.position.y <= BottomBendingTransform.position.y) // below bottom
                BBendingValue = 1f;
            else
            {
                float maxDist = Vector3.Distance(TopBendingTransform.position, BottomBendingTransform.position);
                float currDist = Vector3.Distance(TopBendingTransform.position, LeftHandAnchor.position);

                BBendingValue = currDist / maxDist;
            }

            BbendingLineRenderer.SetPosition((int)BendingSet.Top, TopBendingTransform.position);
            BbendingLineRenderer.SetPosition((int)BendingSet.Bottom, BottomBendingTransform.position);

            SecondaryInfoText.text = $"Bending positions has been set";
            BendingAmountText.text = string.Format("B-bending amount: {0:#.0}", BBendingValue);
            BendingAmountSlider.value = BBendingValue;

            UpdateLineColor();
        }
    }

    private void UpdateHoverButtonsAndFeedback()
    {
        if (!HoverButtons || !BendingFeedback) return;

        if (OVRInput.GetDown(OVRInput.RawButton.B))
            HoverButtons.SetActive(!HoverButtons.activeSelf);

        if (HoverButtons.activeSelf)
        {
            HoverButtons.transform.position = new Vector3(
                CenterEyeAnchor.position.x + HoverButtonsStartPos.x,
                HoverButtonsStartPos.y,
                CenterEyeAnchor.position.z + HoverButtonsStartPos.z);
        }

        BendingFeedback.transform.position = new Vector3(
            CenterEyeAnchor.position.x + BendingFeedbackStartPos.x,
            BendingFeedbackStartPos.y,
            CenterEyeAnchor.position.z + BendingFeedbackStartPos.z);

    }

    private void UpdateLineColor()
    {
        BbendingLineRenderer.sharedMaterial.color = new Color(
            1f,
            (1f - BBendingValue),
            (1f - BBendingValue));
    }

    private void UpdateBendingTransforms()
    {
        TopBendingTransform.position = new Vector3(LeftHandAnchor.position.x, SavedTopBendingPos.y, LeftHandAnchor.position.z);
        BottomBendingTransform.position = new Vector3(LeftHandAnchor.position.x, SavedBottomBendingPos.y, LeftHandAnchor.position.z);
    }

    private bool CheckBendingIsSet()
    {
        return (bendingSet[(int)BendingSet.Top] && bendingSet[(int)BendingSet.Bottom]);
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
