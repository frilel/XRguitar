using System.Text.RegularExpressions;
using System;
using UnityEngine;
using WebSocketSharp;
using UnityEngine.InputSystem;
using System.Collections.Concurrent;
using UnityEngine.UI;
public class WsClient : MonoBehaviour
{
    WebSocket ws;
    public string ipAddress;
    private DotsVisualization dotsVisualization;
    private GuitarManager guitarManager;
    public Text ipAdressText;
    public GameObject ipPanel;
    private void Awake()
    {
        dotsVisualization = FindObjectOfType<DotsVisualization>();
        guitarManager = FindObjectOfType<GuitarManager>();
    }
    private readonly ConcurrentQueue<Action> _actions = new ConcurrentQueue<Action>();
    private void Start()
    {
    }
    private void SendBendingData()
    {
        float value = guitarManager.BBendingValue;
        if (value > 0)
        {
            ws.Send(new MidiMessage("pitchbend", 1, "", 0f, value).CreateToJson());
        }
    }
    private void ConnectToWSS()
    {
        ws = new WebSocket("ws://" + ipAddress + ":4321");
        ws.Connect();
        ws.OnMessage += (sender, e) =>
        {
            if (ipPanel.activeSelf)
            {
                ipPanel.SetActive(false);
            }
            _actions.Enqueue(() => OnReceive(e));
        };
    }
    public void SetIpAddress()
    {
        ipAddress = ipAdressText.text;
        ConnectToWSS();
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
    private void FixedUpdate()
    {
        SendBendingData();
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
}
