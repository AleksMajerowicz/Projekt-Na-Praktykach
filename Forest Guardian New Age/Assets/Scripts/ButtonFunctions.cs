using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class ButtonFunctions : MonoBehaviour
{
    [SerializeField] Player player;
    [SerializeField] Interactions interactions;
    [SerializeField] Story story;
    [SerializeField] GameObject [] buttons;

    int[] timeToRest = new int[3];//okreœla czas poszczególnych umiejetnosci,potrzebnych do zregenerowania siê
    int[] timneToReady = new int[3];//okreœla aktualny czas odpoczynku poszczególnych umiejêtnoœci
    bool[] skills = new bool[3];//Okreœla umiejetnoœci: I(index 0)- atak, II(index 1)- Obrone i III-Skok(index 2)

    private void Start()
    {
        CalibrationsParametrs();
    }

    //Funkja ta,okreœ³a,czy Golem sie porusza.Dziêki temu ¿e jest uniwersalna,moza jej uzyæ w innej funkji oraz zatrzymaæ ni¹ golema
    //Dzieki zasotoswaniu negacji fucnkji ismoving,bo domyœlnie,w ideksie zerowym,jest przycisk Stop,a w idneksie drugim,przyciski:"idŸ w lewo" i "idŸ w prawo"
    void managmentMoving(bool moving)
    {
        player.isMoving = moving;
        buttons[0].SetActive(moving);
        buttons[1].SetActive(!moving);
    }
    //Ta funkcja odpowiada sie za w³aczenie petli porusania siê gracza w lewo
    public void leftMoving()
    {
        managmentMoving(true);
        player.speedMoving *= -1;
    }
    //Ta funkcja odpowiada sie za w³aczenie petli porusania siê gracza w prawo
    public void RightMoving()
    {
        managmentMoving(true);
        player.speedMoving = Mathf.Abs(player.speedMoving);
    }

    public void Stop()
    {
        managmentMoving(false);
    }

    //Zrobiæ funckje osobne dla Taku, Obrony itp,które zwracaj¹wartoœc do,jako argument funckji PlayerDecision
    //Wywo¹³nie np: PlayerDecision(Atack()) gdzie Atack() { return 1} 
    public void PlayerDecision(Player player,int index)
    {
        if(timneToReady[index -1] >= timeToRest[index-1])
        {
            skills[index-1] = true;
            interactions.ManagmentInteraction(buttons[player.aktualnaForma - 1],buttons[player.aktualnaForma -1],story.descriptionInteractionPlayerToOpponent,false,0.5f,index-1);
        }
        else
        {
            interactions.ManagmentInteraction(buttons[player.aktualnaForma - 1], buttons[player.aktualnaForma - 1], story.descriptionInteractionPlayerToOpponent, false, 0.5f, index - 1/*Index odpoczynku*/);
        }
    }

    //Ta Funkcja Kalibruje Parametry z Gracza,to list,by mozna siê do nich uniwersalnie odo³aæ.|Aktualnie nei wiem,jak to Z dynamicnzym czasem zrobniæ|
    void CalibrationsParametrs()
    {
        skills[0] = player.atack;
        skills[1] = player.protectingOn;
        skills[2] = player.jumping;

        timeToRest[0] = player.timeToRestAtack;
        timeToRest[1] = player.timeToRestProtecting;
        timeToRest[2] = player.timeToRestJumping;
    }
}
