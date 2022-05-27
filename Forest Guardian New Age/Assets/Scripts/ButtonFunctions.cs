using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class ButtonFunctions : MonoBehaviour
{
    [SerializeField] Player player;
    //Funkja ta,okreœ³a,czy Golem sie porusza.Dziêki temu ¿e jest uniwersalna,moza jej uzyæ w innej funkji oraz zatrzymaæ ni¹ golema
    void managmentMoving(bool moving)
    {
        player.isMoving = moving;
    }
    //Ta funkcja odpowiada sie za w³aczenie petli porusania siê gracza w lewo
    void leftMoving()
    {
        managmentMoving(true);
        player.speedMoving *= -1;
    }
    //Ta funkcja odpowiada sie za w³aczenie petli porusania siê gracza w prawo
    void RightMoving()
    {
        managmentMoving(true);
        Mathf.Abs(player.speedMoving);
    }

    void PlayerDecision(/*argument odnoszacy siê do nazwy zmiennej,*/ int index)
    {
        //if(player.timeToReady >= timeToRest)
        {
            //player.isatack = true;
        }
        //else
        {
            //odwo³¹nie sie do funkji wyœwietlajaca interakcje(lista interakcji gracza do opponenta[index])
        }
    }
}
