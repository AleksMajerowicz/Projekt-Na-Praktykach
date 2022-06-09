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
    [SerializeField] Story story;

    [Header("Do Opowieści")]
    public string[] textOpponentInteractions;//Zawiera Interakcje Oponenta.Dizęki temu,można dynamicznie zmieniać zmienną descritpionInteractions

    private void Start()
    {
        wantAtack = false;
        defense = false;
        isSet = false;
    }

    // Update is called once per frame
    void Update()
    {
        //Dzięki zastosowaniu sprawdznaiu,czy jest koniec opowieści,unikamy sytacji gdzie gracz,czytajac dalsza częśc opwoieści,dosatej obrażenia
        if (player.actuallyPosition >= location && interactions.endStory)
        {
            /*Po zbliżeniu się gracza,oponnt wysyął swoje dane do story Managment(moze do innej funkcji,tóra zajmie się wyświetlenie się jego danych na Prawym Panelu)
           Wysyła swoje imie do managmentstory,swoje id do itneraction i ustawia że gracz jest w ataku(z niewyjaśnionych przycyzn,ustawienie to nie działa) oraz ustawien zmienną
           isSet,na tru by obciążyć Procesor.    
            */
            //---------------------------------------------------
            if (isSet == false)
            {
                managment.oponentName = oponentName;
                interactions.id = 1;
                for(int i = 0; i < textOpponentInteractions.Length; i++)
                {
                    story.descritpionInteractions[i] = textOpponentInteractions[i];
                }
                interactions.endInteractions = false;
                player.inConfrontation = true;
                interactions.endInteractions = false;
                isSet = true;
                Decision();
            }
            //---------------------------------------------------

            Atack();
            Defense();

            /*W przypadku Obrony,kiedy jest ustawiona,to sprawdza czy czas jest większy lub róny Czasowi do zmainy Decyzji;
             * Jeżeli tak,to ją zmienia.
             * tutaj nie ustawiamy nowej zmiennej,któa będzie okreslaął czas do zmiany decyzji,tylko po przez uwzgleninie parametru wantAtack
             * "Przekierowywujemy" zmienną czas,na obliczanie czasu do zmiany Decyzji.
             */
            //---------------------------------------------------------
            if (time >= timeToChangeDecision && wantAtack == false)
            {
                Decision();
            }
            //---------------------------------------------------------
            if(player.skills["Atack"])
            {
                Hit();
            }
            
            if (live == 0)
            {
                managment.diededMolochs += 1;
                interactions.id = 4;
                interactions.endInteractions = false;
                managment.isOpponentDoSomething = false;
                managment.oponentName = null;
                Destroy(gameObject);
            }

            time += Time.deltaTime;
        }
        else
        {
            //interactions.id = 5;
            //interactions.endInteractions = false;
            //managment.isOpponentDoSomething = false;
            //managment.oponentName = null; zrobić łądną funckje do tego :3
            time = 0;
            isSet = false;
        }

    }

    /*Funckja Draws
     Losuje czas który po którym Opponent zaatakuje
      Oraz dany czas do zmiany decyzji,i ustawia czas na 0,w celu kalibracji*/
    void Draws(float from,float to)
    {
        timeToGiveDamange = Random.Range(5f, 10f);
        timeToChangeDecision = Random.Range(from, to);
        time = 0;
    }

    /*Funckaj Decysion
     Losuje decyzje Opponenta w lokalnej zmiennej,i ustawia jego decyzjed,domysle na false
    i w zaleznopści od wyboru,taką decyzje podejmuje.Na początku wszystkie są na false bo...jest to krótsze i ąłtwijesze rozwiązanie
    miej obciążajace komputer
    Po podjhęciu decyzji,znów losuje czas,w celu dynamicnzości Przeciwnika(coś jak AI)*/
    void Decision()
    {
        int decison = 0;//Random.Range(2, 2);
        wantAtack = false;
        defense = false;

        if (decison%2 == 0)
        {
            wantAtack = true;
        }
        else
        {
            defense = true;
        }

        Draws(Random.Range(1f, 20f), Random.Range(30f, 45f));
    }

    /*Funckaj Atack
     Okresla atak gracza,keidy chce zaatakować,to sprawdza czy czasa jest równy czasowi do zadania obrażeń
    Jeżeli tak,to w zalezności od tego,ja opoonen zatakuje taskie parametry Obrony(czyli ProtecintOn i Jumpig) gracza sprawdza
    I na tej podawie,albo zabiera mu  Swiatło,albo nie i Wyswietla dany Komunikat */
    void Atack()
    {
        if (wantAtack)
        {
            if (time >= timeToGiveDamange)
            {
                //Zrobienie w chuj skomplikowanego,ale uniwersalnego skryptu,że to,bedize w kilku linijakch jako ciąg wywoływanych funckji(distance atack i close atack dac jako lista,co pozwloli na zredukowanie ilości ifów)
                //----------------------------------
                if (distanceAtack)
                {
                    if (player.skills["Jumping"])
                    {
                        //Zrobienie coś,że pojawi się informacja,że misną   
                        Decision();
                    }
                    else
                    {
                        interactions.id = 3;//*Zrobić funckej ustawiajacą poszczególne parametry
                        interactions.endInteractions = false;
                        player.isHit = true;
                        player.iloscSwiatla[player.aktualnaForma - 1] -= damage;
                        Decision();
                    }
                }
                else if (closeAtack)
                {
                    if (player.skills["ProtectingOn"])
                    {
                        //Zrobienie coś,że pojawi się informacja,że misną
                        Decision();
                    }
                    else
                    {
                        interactions.id = 3;
                        interactions.endInteractions = false;
                        player.isHit = true;
                        player.iloscSwiatla[player.aktualnaForma - 1] -= damage;
                        Decision();
                    }
                }
                //---------------------------------------
            }
        }
    }

    /*Funkcja Defense
     Okresla Obrone Opponenta*/
    void Defense()
    {
        if (player.skills["Atack"] && defense == false)
        {
            Hit();
        }
        else if (player.skills["Atack"] && defense)
        {
            //odwołanie się do Funkcji piszaca Interakcje(lista interakcji gracza na opponenta[index])
            player.skills["Atack"] = false;
        }
    }

    /*Funckaj Hit
     Określa obrażenia zadawane Opponentowi.Funckja pwostała glownie po toz, koniecozści uyzycia podówje tego samego kodu*/
    void Hit()
    {
        //odwołanie się do Funkji piszaca interackje(lista interakcji gracza na opponenta[index])
        live -= player.damange;
        player.skills["Atack"] = false;

        managment.ToStory(1, managment.diededMolochs);
    }
}
