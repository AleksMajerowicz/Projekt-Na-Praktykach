using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Story")]
public class Story : ScriptableObject
{
    [TextArea]
    public string[] story;
    public string[] chapter;
    public string[] descritpionInteractions;
    public string[] descriptionInteractionPlayerToOpponent;
}
