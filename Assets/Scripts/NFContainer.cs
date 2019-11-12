using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
public class NFContainer : NFElement
{
    public string ID {
        get
        {
            return ((NFVariable)Elements.Find((e => e.Name == "id")))?.Value;
        }
    }
    public string RelativePositionID {
        get
        {
            return ((NFVariable)Elements.Find((e => e.Name == "relative_position_id")))?.Value;
        }
    }
    public string Icon {
        get
        {
            return ((NFVariable)Elements.Find((e => e.Name == "icon")))?.Value;
        }
    }
    public int X {
        get
        {
            return int.Parse(((NFVariable)Elements.Find((e => e.Name == "x")))?.Value);
        }
    }
    public int Y {
        get
        {
            return int.Parse(((NFVariable)Elements.Find((e => e.Name == "y")))?.Value);
        }
    }
    public string Contents { get; set; }

    public List<NFElement> Elements = new List<NFElement>();

    public void AddVariable(string name, string value)
    {
        name = name.Trim();
        value = value.Trim();
        //Debug.Log("Added " + name + " = " + value);
        Elements.Add(new NFVariable { Name = name, Value = value });
    }

    public void AddContainer(NFContainer container)
    {
        //Debug.Log("Added " + name + " = " + value);
        Elements.Add(container);
    }

    public override string ToString()
    {
        return Name;
    }
}
