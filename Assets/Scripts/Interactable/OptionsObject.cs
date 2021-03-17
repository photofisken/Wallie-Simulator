using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OptionsObject : InteractObject
{
    [SerializeField]
    private List<OptionsObject> options;
    [SerializeField]
    private string optionSpecifics;
    [SerializeField]
    private List<string> decision;

    public override void Interact()
    {
        //base.Interact(); = do old interract AND add new to interract
        Debug.Log("Jag är ett option");
    }

    public int SelectedOption(List<OptionsObject> option)
    {
        return 0;
    }
}
