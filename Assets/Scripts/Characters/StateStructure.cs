using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct StateStructure
{
    public int StateValue { get; set; }
    public string StateName { get; set; }
    public int? NextState { get; set; }
    public int? PreviousState { get; set; }
}
