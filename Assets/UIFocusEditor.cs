using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;

public class UIFocusEditor : MonoBehaviour
{
    NFContainer focus;

    public Text Title;
    public Transform variablesList;
    public GameObject variableButtonPrefab;
    NFVariable selectedVariable;
    public InputField input;

    void Start()
    {
        
    }

    public void SelectVariable(NFVariable v)
    {
        selectedVariable = v;
        input.text = v.Value;
    }

    public void SaveChanges()
    {
        selectedVariable.Value = input.text;
        FindObjectOfType<NationalFocus>().RefreshFocusButtons();
    }

    public void SetFocus(FocusButton fb)
    {
        foreach (Transform child in variablesList)
        {
            GetComponent<Button>()?.onClick.RemoveAllListeners();
            Destroy(child.gameObject);
        }

        focus = fb.container;
        Title.text = focus.ID;

        foreach(NFVariable e in focus.Elements.FindAll(x=>x.GetType() == typeof(NFVariable)))
        {
            GameObject varBut = Instantiate(variableButtonPrefab,variablesList);
            //varBut.GetComponentInChildren<Text>().text = e.Name + "=" + e.Value;
            varBut.GetComponentInChildren<Text>().text = e.Name;
            varBut.GetComponent<Button>().onClick.AddListener(() => SelectVariable(e));
        }
    }
}
