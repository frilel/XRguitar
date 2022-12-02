using System.Text.RegularExpressions;
using System;
using UnityEngine;
using WebSocketSharp;
using UnityEngine.InputSystem;
using System.Collections.Concurrent;
public class WsClient : MonoBehaviour
{
    WebSocket ws;
    public string ipAddress;
    private DotsVisualization dotsVisualization;
    public Transform channelParent;
    private void Awake()
    {
        dotsVisualization = FindObjectOfType<DotsVisualization>();
    }
    private readonly ConcurrentQueue<Action> _actions = new ConcurrentQueue<Action>();
    private void Start()
    {

        ws = new WebSocket("ws://" + ipAddress + ":4321");
        ws.Connect();
        ws.OnMessage += (sender, e) =>
        {
            _actions.Enqueue(() => OnReceive(e));
        };
        /*MidiMessage temp = MidiMessage.CreateFromJSON(new MidiMessage("pitchbend", 2, "A#4", 0f, 0.4f).CreateToJson());
        MidiMessage message = temp;
        string noteAlphabet = Regex.Replace(message.note, "[0-9]", "");
        string noteOctave = Regex.Replace(message.note, "[^0-9]", "");
        string dotName = message.channel + "-" + noteAlphabet + "-" + noteOctave;
        //Debug.Log(dotName);
        dotsVisualization.Visualize(message.channel, dotName);*/
    }
    private void Update()
    {
        if (ws == null)
        {
            return;
        }
        if (Keyboard.current.spaceKey.wasPressedThisFrame)
        {
            ws.Send(new MidiMessage("pitchbend", 2, "", 0f, 0.4f).CreateToJson());
            //Debug.Log(new MidiMessage("pitchbend",2,"",0f,0.4f).CreateToJson());
        }
        while (_actions.Count > 0)
        {
            if (_actions.TryDequeue(out var action))
            {
                action?.Invoke();
            }
        }
        //ReceiveMessage();
    }
    private static void OnReceive(MessageEventArgs e)
    {
        //Debug.Log("Message Received from " + ((WebSocket)sender).Url + ", Data : " + e.Data);
        //Debug.Log("Message Received from " + ((WebSocket)sender).Url + ", Data : " + e.Data);
        MidiMessage message = MidiMessage.CreateFromJSON(e.Data);
        string noteAlphabet = Regex.Replace(message.note, "[0-9]", "");
        string noteOctave = Regex.Replace(message.note, "[^0-9]", "");
        string dotName = message.channel + "-" + noteAlphabet + "-" + noteOctave;
        //Debug.Log(dotName);
        DotsVisualization.Instance.Visualize(message.channel, dotName);
    }
    /*private void ReceiveMessage()
    {
        //ws.OnMessage
        ws.OnMessage += (sender, e) =>
        {
            Debug.Log("Message Received from " + ((WebSocket)sender).Url + ", Data : " + e.Data);
            MidiMessage message = MidiMessage.CreateFromJSON(e.Data);
            string noteAlphabet = Regex.Replace(message.note, "[0-9]", "");
            string noteOctave = Regex.Replace(message.note, "[^0-9]", "");
            string dotName = message.channel + "-" + noteAlphabet + "-" + noteOctave;
            //Debug.Log(dotName);
            dotsVisualization.Visualize(message.channel, dotName);

        };

    }*/
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
        return $"{{\"type\":\"{type}\",\"channel\":{channel},\"note\":\"{note}\",\"attack\":{attack},\"pitchValue\":{pitchValue}}}";
        //return "hi";
    }
    public MidiMessage(string t, int c, string n, float a, float p)
    {
        type = t;
        channel = c;
        note = n;
        attack = a;
        pitchValue = p;
    }

    // Given JSON input:
    // {"name":"Dr Charles","lives":3,"health":0.8}
    // this example will return a PlayerInfo object with
    // name == "Dr Charles", lives == 3, and health == 0.8f.
}
