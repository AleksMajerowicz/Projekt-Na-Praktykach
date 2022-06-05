﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Story")]
public class Story : ScriptableObject
{
    [TextArea]
    public string[] story;//Określa Akapity history.
    public string[] chapter;//Określa roźdizały historii
    public string[] descritpionInteractions;//Okreśła opowieści interackji do oponenta z graczecz
    public string[] descriptionInteractionPlayerToOpponent;//Okreśła opowiesci do iterakcji gracza z Oponentem
}
