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
        dropdown.options.Add(new Dropdown.OptionData(index.ToString()));
    }
    public void RemoveOption(int index)
    {
        dropdown.options.Remove(new Dropdown.OptionData(index.ToString()));
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

    // Update is called once per frame
    void Update()
    {
        
    }
}
