using System.Collections.Generic;
using UnityEngine;

[RequireComponent (typeof(GUIText))]

public class ScreenPrinter : MonoBehaviour
{
    
    public TextAnchor anchorAt = TextAnchor.LowerLeft;
    public int numberOfLines = 8;
    public int pixelOffset = 5;
    public int chunkSize = 80;
    static ScreenPrinter defaultPrinter = null;
    static bool quitting = false;
    List<string> newMessages = new List<string>();
    TextAnchor _anchorAt;
    float _pixelOffset;
//    List<string> messageHistory = new List<string>();
    
    // static Print method: finds a ScreenPrinter in the project, 
    // or creates one if necessary, and prints to that.
    public static void Print(object message)
    {
        if (quitting)
            return;       // don't try to print while quitting
        if (!defaultPrinter)
        {
            GameObject gob = GameObject.Find("Screen Printer");
            if (!gob)
                gob = new GameObject("Screen Printer");
            defaultPrinter = gob.GetComponent<ScreenPrinter>();
            if (!defaultPrinter)
                defaultPrinter = gob.AddComponent<ScreenPrinter>();
        }
        if (null == message)
        {
            message = "null";
        }
        defaultPrinter.LocalPrint(message);
    }
    
    // member LocalPrint method: prints to this particular screen printer.
    // Called LocalPrint because C# won't let us use the same name for both
    // static and instance method.  Grr.  Argh.  >:(
    public void LocalPrint(object message)
    {
        if (quitting)
            return;       // don't try to print while quiting
        if (newMessages == null)
        {
            newMessages = new List<string>(numberOfLines);
        }
        newMessages.Add(message.ToString());
    }
    
    void Awake()
    {
        if (!guiText)
        {
            gameObject.AddComponent("GUIText");
            transform.position = Vector3.zero;
            transform.localScale = new Vector3(0, 0, 1);
        }
        _anchorAt = anchorAt;
        UpdatePosition();
    }
    
    void OnApplicationQuitting()
    {
        quitting = true;
    }

    private readonly object syncLock = new object();
    
    void Update()
    {
        // if anchorAt or pixelOffset has changed while running, update the text position
        if (_anchorAt != anchorAt || _pixelOffset != pixelOffset)
        {
            _anchorAt = anchorAt;
            _pixelOffset = pixelOffset;
            UpdatePosition();
        }

        lock (syncLock)
        {
            //  if the message has changed, update the display
            if (newMessages.Count > 0)
            {
                //  create the multi-line text to display
                foreach (string msg in newMessages.ToArray())
                {
                    for (int i = 0; i < msg.Length; i += chunkSize)
                    {
                        int len = chunkSize;
                        if (i + chunkSize > msg.Length)
                            len = msg.Length - i;
                        guiText.text += msg.Substring(i, len) + '\n';
                    }
                }
                newMessages.Clear();

                string[] strings = guiText.text.Split('\n');
                if (strings.Length > numberOfLines)
                {
                    int len = 0;
                    for (int i = 0; i < newMessages.Count; i++)
                    {
                        len += strings[i].Length;
                    }
                    guiText.text = guiText.text.Remove(0, len);
                }
                /*
                if (null == messageHistory)
                {
                    messageHistory = new List<string>(numberOfLines);
                }
                for (int messageIndex = 0; messageIndex < newMessages.Count; messageIndex++)
                {
                    messageHistory.Add(newMessages [messageIndex]);
                }
                if (messageHistory.Count > numberOfLines)
                {
                    guiText.text = "";
                    messageHistory.RemoveRange(0, messageHistory.Count - numberOfLines);
                }
                messageHistory.Clear();
                */
            }
        }
    }
    
    void UpdatePosition()
    {
        switch (anchorAt)
        {
            case TextAnchor.UpperLeft:
                transform.position = new Vector3(0.0f, 1.0f, 0.0f);
                guiText.anchor = anchorAt;
                guiText.alignment = TextAlignment.Left;
                guiText.pixelOffset = new Vector2(pixelOffset, -pixelOffset);
                break;
            case TextAnchor.UpperCenter:
                transform.position = new Vector3(0.5f, 1.0f, 0.0f);
                guiText.anchor = anchorAt;
                guiText.alignment = TextAlignment.Center;
                guiText.pixelOffset = new Vector2(0, -pixelOffset);
                break;
            case TextAnchor.UpperRight:
                transform.position = new Vector3(1.0f, 1.0f, 0.0f);
                guiText.anchor = anchorAt;
                guiText.alignment = TextAlignment.Right;
                guiText.pixelOffset = new Vector2(-pixelOffset, -pixelOffset);
                break;
            case TextAnchor.MiddleLeft:
                transform.position = new Vector3(0.0f, 0.5f, 0.0f);
                guiText.anchor = anchorAt;
                guiText.alignment = TextAlignment.Left;
                guiText.pixelOffset = new Vector2(pixelOffset, 0.0f);
                break;
            case TextAnchor.MiddleCenter:
                transform.position = new Vector3(0.5f, 0.5f, 0.0f);
                guiText.anchor = anchorAt;
                guiText.alignment = TextAlignment.Center;
                guiText.pixelOffset = new Vector2(0, 0);
                break;
            case TextAnchor.MiddleRight:
                transform.position = new Vector3(1.0f, 0.5f, 0.0f);            
                guiText.anchor = anchorAt;
                guiText.alignment = TextAlignment.Right;
                guiText.pixelOffset = new Vector2(-pixelOffset, 0.0f);
                break;
            case TextAnchor.LowerLeft:
                transform.position = new Vector3(0.0f, 0.0f, 0.0f);
                guiText.anchor = anchorAt;
                guiText.alignment = TextAlignment.Left;
                guiText.pixelOffset = new Vector2(pixelOffset, pixelOffset);
                break;
            case TextAnchor.LowerCenter:
                transform.position = new Vector3(0.5f, 0.0f, 0.0f);
                guiText.anchor = anchorAt;
                guiText.alignment = TextAlignment.Center;
                guiText.pixelOffset = new Vector2(0, pixelOffset);
                break;
            case TextAnchor.LowerRight:
                transform.position = new Vector3(1.0f, 0.0f, 0.0f);
                guiText.anchor = anchorAt;
                guiText.alignment = TextAlignment.Right;
                guiText.pixelOffset = new Vector2(-pixelOffset, pixelOffset);
                break;
        }
    }
}
