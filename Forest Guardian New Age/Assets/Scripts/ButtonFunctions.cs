using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class ButtonFunctions : MonoBehaviour
{
    [SerializeField] Player player;
    //Funkja ta,okre��a,czy Golem sie porusza.Dzi�ki temu �e jest uniwersalna,moza jej uzy� w innej funkji oraz zatrzyma� ni� golema
    void managmentMoving(bool moving)
    {
        player.isMoving = moving;
    }
    //Ta funkcja odpowiada sie za w�aczenie petli porusania si� gracza w lewo
    void leftMoving()
    {
        managmentMoving(true);
        player.speedMoving *= -1;
    }
    //Ta funkcja odpowiada sie za w�aczenie petli porusania si� gracza w prawo
    void RightMoving()
    {
        managmentMoving(true);
        Mathf.Abs(player.speedMoving);
    }

    void PlayerDecision(/*argument odnoszacy si� do nazwy zmiennej,*/ int index)
    {
        //if(player.timeToReady >= timeToRest)
        {
            //player.isatack = true;
        }
        //else
        {
            //odwo��nie sie do funkji wy�wietlajaca interakcje(lista interakcji gracza do opponenta[index])
        }
    }
}
