using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.UI;

public class FocusButton : MonoBehaviour
{
    public NFContainer container;
    [TextArea(3,16)]
    public string contents;

    void Start()
    {
        name = container.ID;
        contents = container.Contents;
        GetComponentInChildren<Text>().text = name;
        //Debug.Log(container.Name + "\nX: "+container.X.ToString()+"\nY: "+container.Y.ToString());
    }

    private void OnDestroy()
    {
        GetComponentInChildren<Button>().onClick.RemoveAllListeners();
    }
}
