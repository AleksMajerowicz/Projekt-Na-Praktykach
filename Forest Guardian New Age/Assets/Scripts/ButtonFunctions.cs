using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class ButtonFunctions : MonoBehaviour
{
    [SerializeField] Player player;
    [SerializeField] GameObject [] buttons;//Okre��a przyciski

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
        if(player.timeToRestAtack >= player.timeToRestAtack)
        {
            //player.isatack = true;
        }
        else
        {
            //odwo��nie sie do funkji wy�wietlajaca interakcje(lista interakcji gracza do opponenta[index])
        }
    }
}
