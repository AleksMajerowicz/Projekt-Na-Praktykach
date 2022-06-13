using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class ButtonFunctions : MonoBehaviour
{
    [SerializeField] Player player;
    [SerializeField] Interactions interactions;
    [SerializeField] Managment managment;
    [SerializeField] Story story;
    [SerializeField] GameObject [] buttons;

    int[] timeToRest = new int[3];//okreœla czas poszczególnych umiejetnosci,potrzebnych do zregenerowania siê
    string[] timneToReady = new string[3];//okreœla nazwy aktólanych czasów odpoczynku poszczególnych umiejêtnoœci
    string[] playerSkills = new string[3];//Okreœla nazwy

    private void Start()
    {
        CalibrationsParametrs();
    }

    //Funkja ta,okreœ³a,czy Golem sie porusza.Dziêki temu ¿e jest uniwersalna,moza jej uzyæ w innej funkji oraz zatrzymaæ ni¹ golema
    //Dzieki zasotoswaniu negacji fucnkji ismoving,bo domyœlnie,w ideksie zerowym,jest przycisk Stop,a w idneksie drugim,przyciski:"idŸ w lewo" i "idŸ w prawo"
    void managmentMoving(bool moving)
    {
        player.isMoving = moving;
        buttons[buttons.Length - 1].SetActive(moving);
        buttons[buttons.Length-2].SetActive(!moving);
        interactions.Repeat(" ", true);
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
    public void PlayerDecision(int index)
    {
        CalibrationsParametrs();

        interactions.endInteractions = false;

        if (player.timneToReady[timneToReady[index - 1]] >= timeToRest[index-1])
        {
            player.skills[playerSkills[index - 1]] = true;
            player.timneToReady[timneToReady[index - 1]] = 0;
            interactions.id = index;
            managment.playerDoing = true;
        }
        else
        {
            interactions.id = index;//Indek odpoczynku
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
        //PlayerDecision(3);Jesteb blisko naprawienia skoku,ale konieczna jest zmiana w skrycpie ManagmentInteractions
    }
    public void RunAway()
    {
        player.actuallyPosition -= 5;
        player.ranAway = true;
    }

    //Ta Funkcja Kalibruje Parametry z Gracza,to list,by mozna siê do nich uniwersalnie odo³aæ.|Aktualnie nei wiem,jak to Z dynamicnzym czasem zrobniæ|
    void CalibrationsParametrs()
    {
        playerSkills = new string[3] { "Atack", "ProtectingOn", "Jumping" };

        timeToRest = new int[3] { player.timeToRestAtack, player.timeToRestProtecting, player.timeToRestJumping };

        timneToReady = new string[3] { "TimeToReadyAtack", "TimeToReadyProtecting", "TimeToReadyJumping" };
    }
}
