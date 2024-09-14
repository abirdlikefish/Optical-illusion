using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor.UI;
using UnityEngine;
using UnityEngine.UI;

public class LevelSelection : MonoBehaviour
{
    Dropdown dropdown;
    void Awake()
    {
        dropdown = transform.GetComponent<Dropdown>();
    }

    public void AddOption(int index)
    {
        dropdown.AddOptions(new List<string>{index.ToString()});
        SetOption(index);
    }
    public void RemoveOption(int index)
    {
        dropdown.options.Remove(new Dropdown.OptionData(index.ToString()));
    }

    public bool SetOption(int index)
    {
        for(int i = 0 ; i < dropdown.options.Count ; i++)
        {
            if(int.Parse(dropdown.options[i].text) == index)
            {
                dropdown.SetValueWithoutNotify(i);
                return true;
            }
        }
        return false;
    }

    public void Select(int index)
    {
        int selectedOption = int.Parse(dropdown.options[index].text);
        Command_normal.UseLevelData(selectedOption);
    }
    public void Init()
    {
        dropdown.onValueChanged.AddListener(Select);
    }
}
