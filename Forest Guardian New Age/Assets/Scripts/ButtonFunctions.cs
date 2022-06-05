using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class ButtonFunctions : MonoBehaviour
{
    [SerializeField] Player player;
    [SerializeField] Interactions interactions;
    [SerializeField] Story story;
    [SerializeField] GameObject [] buttons;

    int[] timeToRest = new int[3];//okre�la czas poszczeg�lnych umiejetnosci,potrzebnych do zregenerowania si�
    int[] timneToReady = new int[3];//okre�la aktualny czas odpoczynku poszczeg�lnych umiej�tno�ci
    bool[] skills = new bool[3];//Okre�la umiejetno�ci: I(index 0)- atak, II(index 1)- Obrone i III-Skok(index 2)

    private void Start()
    {
        CalibrationsParametrs();
    }

    //Funkja ta,okre��a,czy Golem sie porusza.Dzi�ki temu �e jest uniwersalna,moza jej uzy� w innej funkji oraz zatrzyma� ni� golema
    //Dzieki zasotoswaniu negacji fucnkji ismoving,bo domy�lnie,w ideksie zerowym,jest przycisk Stop,a w idneksie drugim,przyciski:"id� w lewo" i "id� w prawo"
    void managmentMoving(bool moving)
    {
        player.isMoving = moving;
        buttons[0].SetActive(moving);
        buttons[1].SetActive(!moving);
    }
    //Ta funkcja odpowiada sie za w�aczenie petli porusania si� gracza w lewo
    public void leftMoving()
    {
        managmentMoving(true);
        player.speedMoving *= -1;
    }
    //Ta funkcja odpowiada sie za w�aczenie petli porusania si� gracza w prawo
    public void RightMoving()
    {
        managmentMoving(true);
        player.speedMoving = Mathf.Abs(player.speedMoving);
    }

    public void Stop()
    {
        managmentMoving(false);
    }

    //Zrobi� funckje osobne dla Taku, Obrony itp,kt�re zwracaj�warto�c do,jako argument funckji PlayerDecision
    //Wywo��nie np: PlayerDecision(Atack()) gdzie Atack() { return 1} 
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

    //Ta Funkcja Kalibruje Parametry z Gracza,to list,by mozna si� do nich uniwersalnie odo�a�.|Aktualnie nei wiem,jak to Z dynamicnzym czasem zrobni�|
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
