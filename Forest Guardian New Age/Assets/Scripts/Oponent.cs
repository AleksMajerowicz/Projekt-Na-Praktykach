using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Oponent : MonoBehaviour
{
    [Header("Podstawowe parametry Opponenta")]
    [SerializeField]string oponentName;//Określa nazwę Oponentu(do wyświetlania w Opowieści)
    [SerializeField] int id;//określa id,co pozwala na odołanie się do odpowiadająceog mu Indexu w story
    public int live;//Określa ilość życia
    [SerializeField]int damage;//Określa zadawane Obrażenia Graczowi
    public float location;//Okresla pozycje Oponentu
    [SerializeField] bool distanceAtack;//Okresla czy oponent atakuje z odległości
    [SerializeField] bool closeAtack;//określa czy oponent atakuje z Bliska

    [Header("Stan Oponenta")]
    bool wantAtack;//Określa,czy Przeciwnik chce zaatakować,Bedzie true kiedy czas bedzie równy losowemu czasowi
    public bool defense;//Określa czy Opponent postanowił bronić się
    bool isSet;//określa,czy Skrypt Managment dostał potrzebne informacje.Dizęki temu,możliwe jest zmienienie w Funkcji "Zarządzaniem Interakcjami" id na ostatni index Opisu Opowieści,czyli na "Co robisz?"

    float timeToChangeDecision,timeToGiveDamange,time;//Czas do wykonania-określa czas do wykonania danej czynności,Czas Do Zadania obrażeń-określa czas do zadania obrażeń graczowi,czas-zawiera aktualnyczas

    [Header("odwołania")]
    [SerializeField] Player player;
    [SerializeField] Interactions interactions;
    [SerializeField] Managment managment;

    private void Start()
    {
        wantAtack = false;
        isSet = false;
        Draws(Random.Range(1f, 20f),Random.Range(30f,300f));
    }

    // Update is called once per frame
    void Update()
    {
        if (player.actuallyPosition >= location)
        {
            if (isSet == false)
            {
                managment.oponentName = oponentName;
                player.seeOponentName = oponentName;//Tymczasowo!
                interactions.id = id;
                player.skills[0] = true;
                isSet = true;
            }

            Atack();
            Defense();

            if (time >= timeToChangeDecision && wantAtack == false)
            {
                Decision();
            }

            if(live == 0)
            {
                Destroy(gameObject);
            }

            time += Time.deltaTime;
        }

    }

    void Draws(float from,float to)
    {
        timeToGiveDamange = Random.Range(5f, 10f);
        timeToChangeDecision = Random.Range(from, to);
        time = 0;
    }

    void Decision()
    {
        int decison = Random.Range(1, 2);
        wantAtack = false;
        defense = false;

        if (decison == 1)
        {
            wantAtack = true;
        }
        else if (decison == 2)
        {
            defense = true;
        }

        Draws(Random.Range(1f, 20f), Random.Range(30f, 300f));
    }

    void Atack()
    {
        if (wantAtack)
        {
            time = 0;
            if (time >= timeToGiveDamange)
            {
                //Zrobienie w chuj skomplikowanego,ale uniwersalnego skryptu,że to,bedize w kilku linijakch jako ciąg wywoływanych funckji(distance atack i close atack dac jako lista,co pozwloli na zredukowanie ilości ifów)
                //----------------------------------
                if (distanceAtack)
                {
                    if (player.skills[2])
                    {
                        //Zrobienie coś,że pojawi się informacja,że misną
                        Decision();
                    }
                    else
                    {
                        //Zrobienie coś,że pojawi się informacja,że gracz dostał
                        player.iloscSwiatla[player.aktualnaForma - 1] -= damage;
                        Decision();
                    }
                }
                else if (closeAtack)
                {
                    if (player.skills[1])
                    {
                        //Zrobienie coś,że pojawi się informacja,że misną
                        Decision();
                    }
                    else
                    {
                        //Zrobienie coś,że pojawi się informacja,że gracz dostał
                        player.iloscSwiatla[player.aktualnaForma - 1] -= damage;
                        Decision();
                    }
                }
                //---------------------------------------
            }
        }
    }

    void Defense()
    {
        if (player.skills[0] && defense == false)
        {
            //odwołanie się do Funkji piszaca interackje(lista interakcji gracza na opponenta[index])
            live -= player.damange;
            player.skills[0] = false;
        }
        else if (player.skills[0] && defense)
        {
            //odwołanie się do Funkcji piszaca Interakcje(lista interakcji gracza na opponenta[index])
            player.skills[0] = false;
        }
    }
}
