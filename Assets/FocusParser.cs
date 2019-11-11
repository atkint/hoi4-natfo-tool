using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;

public class FocusParser : MonoBehaviour
{
    StreamReader sr;
    
    public Text textDisplay;
    int bracketDepth = 0;
    string text;


    public NationalFocus NationalFocus;

    void Start()
    {
        sr = new StreamReader(@"C:\Users\Tim\Desktop\hoi4-natfo-tool\Assets\Example\generic.txt");
        text = cleanText(sr.ReadToEnd());
        sr.Close();
        readLoop();
    }

    string cleanText(string textToClean)
    {
        List<string> textLines = new List<string>(textToClean.Split('\n'));
        
        for (int i = 0; i < textLines.Count; i++)
        {
            textLines[i] = cleanCommentsFromLine(textLines[i]);
        }
        Debug.Log(textLines.Count.ToString("0 lines"));
        return string.Join("\n", textLines);
    }

    public void readLoop ()
    {
        string[] lines = text.Split('\n');
        int characterIndex = 0;
        List<FocusContainer> containerDepth = new List<FocusContainer>();

        //string focusTreeContents = GetContentsOfBrackets(text.Substring(text.IndexOf("focus_tree")), text.IndexOf("focus_tree"));
        string focusTreeContents = GetContentsOfBrackets(text, text.IndexOf("focus_tree"));
        textDisplay.text = focusTreeContents;
        string debugString = "";
        for (int i = 0; i< lines.Length; i++)//i < 4;i++)//
        {
            
            var line = lines[i];
            int openBracketCount = line.Count(x => x == '{');
            int closeBracketCount = line.Count(x => x == '}');

            if (isVariable(line))
            {
                containerDepth.Last().AddVariable(line.Split('=')[0].Trim(), line.Trim().Split('=')[1]);
            }
            else if (isContainer(line) && (closeBracketCount == 0)) // Ignore lines with { and } for now
            {
                //Debug.Log(openBracketCount.ToString("0 open brackets")+"\n"+ closeBracketCount.ToString("0 close brackets"));
                var name = line.Split('=')[0].Trim();
                var contents = GetContentsOfBrackets(text, characterIndex);
                var container = new FocusContainer { Name = name, Contents = contents };
                if (containerDepth.Count> 0)
                {
                    containerDepth.Last().AddContainer(container.Name, container);
                }
                containerDepth.Add(container);
            }
            else if (line.Contains("}") && openBracketCount - closeBracketCount != 0)
            {
                containerDepth.RemoveAt(containerDepth.Count - 1);
            }
            try
            {
                //Debug.Log(line+"\n"+text.Substring(characterIndex, line.Length+1));
            }
            catch
            {
                //Debug.Log(line + "\n" + text.Substring(characterIndex, line.Length));
            }
            characterIndex += line.Length+1;
        }
    }

    bool isVariable(string line)
    {
        return line.Contains("=") && !line.Contains("{") && line.Split('=').Length >= 2;
    }

    bool isContainer(string line)
    {
        return line.Contains("=") && line.Contains("{");
    }


    public static string GetContentsOfBrackets(string totalText, int headingIndex)
    {
        int openBracketIndex = totalText.IndexOf('{', headingIndex) + 1;
        int closeBracketIndex = openBracketIndex;

        // counter used to track where which character is being read
        int counter = openBracketIndex;

        // bracketDepth is how nested the script is. If it is 0, it is the last bracket.
        int bracketDepth = 0;

        while (counter <= totalText.Length - 1)
        {
            if (totalText[counter] == '{')
            {
                bracketDepth++;
            }
            if (totalText[counter] == '}')
            {
                if (bracketDepth == 0)
                {
                    // Found close bracket
                    closeBracketIndex = counter;
                    break;
                }
                else
                {
                    bracketDepth--;
                }
            }
            counter++;
        }

        // Return the contents between open and close bracket indexes
        string contents = totalText.Substring(openBracketIndex, closeBracketIndex - openBracketIndex);
        return contents;
    }

    public void showText()
    {
        textDisplay.text = text;
    }

    string cleanCommentsFromLine(string line)
    {
        return line.Split('#')[0];
    }
}
