using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Managment : MonoBehaviour
{
    public string oponentName;

    [SerializeField] Player player;
    [SerializeField] Interactions interactions;
    [SerializeField] Story story;
    [SerializeField] GameObject[] buttons;//Zawiera WSZYSTKIE przyciska,i te do chodzenia,funkjami I,II,III i IV formy Golema itd

    // Start is called before the first frame update
    void Start()
    {
        for(int i = 0; i < buttons.Length; i++)
        {
            buttons[i].SetActive(false);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if(player.isMoving && interactions.end == false)
        {
            //Określić funckej w Interactions,do zarządzania chodzeniem,która będzie wyświetlałą też informacje,o tym,że np: Golem wyszedł z lasu.Ta funckja ma sie wyświetlić raz
        }

        //Ten if,wyswietla Panel ataku Golema w Danje formie RAZ
        if (player.inConfrontation && interactions.end == false)
        {
            interactions.ManagmentInteraction(buttons[player.aktualnaForma - 1],buttons[buttons.Length -1],story.descritpionInteractions ,true, 0.1f,interactions.id);
        }
    }

}
