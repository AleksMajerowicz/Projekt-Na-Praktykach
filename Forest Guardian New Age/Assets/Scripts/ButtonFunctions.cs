using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class ButtonFunctions : MonoBehaviour
{
    [SerializeField] Player player;
    [SerializeField] GameObject [] buttons;//Okreœ³a przyciski

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
        if(player.timeToRestAtack >= player.timeToRestAtack)
        {
            //player.isatack = true;
        }
        else
        {
            //odwo³¹nie sie do funkji wyœwietlajaca interakcje(lista interakcji gracza do opponenta[index])
        }
    }
}
