using UnityEngine;
using UnityEngine.UI;

public class IPtextfieldUpdater : MonoBehaviour
{
    public Text TextFieldToUpdate;

    private string textOnStart;

    private void Start()
    {
        if (!TextFieldToUpdate) 
            TextFieldToUpdate = this.GetComponentInChildren<Text>();

        textOnStart = TextFieldToUpdate.text;
    }

    public void UpdateTextField(string text)
    {
        // Making sure to remove the 'hint text'
        if (TextFieldToUpdate.text.Equals(textOnStart))
        {
            TextFieldToUpdate.text = text;
        }
        // Remove last char
        else if (text == "<")
        {
            TextFieldToUpdate.text = TextFieldToUpdate.text.Remove(TextFieldToUpdate.text.Length - 1, 1);
        }
        else
        {
            TextFieldToUpdate.text += text;
        }
    }
}
