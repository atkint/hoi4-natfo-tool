using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FocusContainer : MonoBehaviour
{
    public List<FocusVariable> Variables = new List<FocusVariable>();
    public List<FocusContainer> Containers = new List<FocusContainer>();

    public string Name { get; set; }
    public string Contents { get; set; }

    public virtual void AddVariable(string name, string value)
    {
        //Debug.Log("Added " + name + " = " + value);
        Variables.Add(new FocusVariable { Name = name, Value = value });
    }

    public virtual void AddContainer(string name, FocusContainer value)
    {
        //Debug.Log("Added " + name + " = " + value);
        Containers.Add(value);
    }

    public override string ToString()
    {
        return Name;
    }
}
