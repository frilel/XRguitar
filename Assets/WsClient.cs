using UnityEngine;
using WebSocketSharp;
using UnityEngine.InputSystem;

public class WsClient : MonoBehaviour
{
    WebSocket ws;
    private void Start()
    {
        ws = new WebSocket("ws://130.229.129.180:4321");
        ws.Connect();
        ws.OnMessage += (sender, e) =>
        {
            Debug.Log("Message Received from " + ((WebSocket)sender).Url + ", Data : " + e.Data);
            //Debug.Log("Converted to " + PlayerInfo.CreateFromJSON(e.Data).note+","+PlayerInfo.CreateFromJSON(e.Data).channel+","+PlayerInfo.CreateFromJSON(e.Data).attack);
        };
    }
    private void Update()
    {
        if (ws == null)
        {
            return;
        }
        if (Keyboard.current.spaceKey.wasPressedThisFrame)
        {
            ws.Send(new MidiMessage("noteon",5,"A4",5f,0.5f).CreateToJson());
        }
        //ReceiveMessage();
        
    }
    private void ReceiveMessage()
    {
        //ws.OnMessage
        ws.OnMessage += (sender, e) =>
        {
            Debug.Log("Message Received from " + ((WebSocket)sender).Url + ", Data : " + e.Data);
            //Debug.Log("Converted to " + PlayerInfo.CreateFromJSON(e.Data).note+","+PlayerInfo.CreateFromJSON(e.Data).channel+","+PlayerInfo.CreateFromJSON(e.Data).attack);
        };
    }
}
[System.Serializable]
public class MidiMessage
{
    public string type;
    public int channel;
    public string note;
    public float attack;
    public float pitchValue;

    public static MidiMessage CreateFromJSON(string jsonString)
    {
        return JsonUtility.FromJson<MidiMessage>(jsonString);
    }
    public string CreateToJson()
    {
        //return "{\"type\":\""+type+"\",\"channel\":"+channel+",\"attack\":"+attack+"}";
        return "hi";
    }
    public MidiMessage(string t, int c, string n,float a,float p)
    {
        type=t;
        channel=c;
        note=n;
        attack=a;
        pitchValue=p;
    }

    // Given JSON input:
    // {"name":"Dr Charles","lives":3,"health":0.8}
    // this example will return a PlayerInfo object with
    // name == "Dr Charles", lives == 3, and health == 0.8f.
}
