using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Managment : MonoBehaviour
{
    public string oponentName;

    [SerializeField] Player player;
    [SerializeField] Interactions interactions;
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
        if(player.inAtack && interactions.end == false)
        {
            interactions.ManagmentInteraction(buttons[player.aktualnaForma - 1], true,0.1f);
        }
    }

}
