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
    string[] timneToReady = new string[3];//okre�la nazwy akt�lanych czas�w odpoczynku poszczeg�lnych umiej�tno�ci
    string[] playerSkills = new string[3];//Okre�la nazwy

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
    public void PlayerDecision(int index)
    {
        CalibrationsParametrs();

        interactions.endInteractions = false;

        if (player.timneToReady[timneToReady[index - 1]] >= timeToRest[index-1])
        {
            player.skills[playerSkills[index - 1]] = true;
            player.timneToReady[timneToReady[index - 1]] = 0;
            interactions.id = index;
            interactions.ManagmentInteraction(buttons[player.aktualnaForma - 1],buttons[player.aktualnaForma -1],story.descriptionInteractionPlayerToOpponent, false,0.5f);
        }
        else
        {
            interactions.id = index;//Indek odpoczynku
            interactions.ManagmentInteraction(buttons[player.aktualnaForma - 1], buttons[player.aktualnaForma - 1], story.descriptionInteractionPlayerToOpponent, false, 0.5f);
        }

    }

    public void Atack()
    {
        PlayerDecision(1);
    }
    public void ProtectingOn()
    {
        PlayerDecision(2);
    }
    public void Jumping()
    {
        PlayerDecision(3);
    }
    public void RunAway()
    {
        player.actuallyPosition -= 5;
        interactions.ManagmentInteraction(buttons[player.aktualnaForma - 1], buttons[buttons.Length-2], story.descriptionInteractionPlayerToOpponent, false, 0.5f);
    }

    //Ta Funkcja Kalibruje Parametry z Gracza,to list,by mozna si� do nich uniwersalnie odo�a�.|Aktualnie nei wiem,jak to Z dynamicnzym czasem zrobni�|
    void CalibrationsParametrs()
    {
        playerSkills = new string[3] { "Atack", "ProtectingOn", "Jumping" };

        timeToRest = new int[3] { player.timeToRestAtack, player.timeToRestProtecting, player.timeToRestJumping };

        timneToReady = new string[3] { "TimeToReadyAtack", "TimeToReadyProtecting", "TimeToReadyJumping" };
    }
}
