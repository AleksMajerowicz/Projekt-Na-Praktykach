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
    [SerializeField] int consumeLight;
    public float location;//Okresla pozycje Oponentu
    [SerializeField] bool distanceAtack;//Okresla czy oponent atakuje z odległości
    [SerializeField] bool closeAtack;//określa czy oponent atakuje z Bliska

    [Header("Stan Oponenta")]
    bool wantAtack;//Określa,czy Przeciwnik chce zaatakować,Bedzie true kiedy czas bedzie równy losowemu czasowi
    public bool defense;//Określa czy Opponent postanowił bronić się
    bool isSet;//określa,czy Skrypt Managment dostał potrzebne informacje.Dizęki temu,możliwe jest zmienienie w Funkcji "Zarządzaniem Interakcjami" id na ostatni index Opisu Opowieści,czyli na "Co robisz?"
    public bool isHit;
    public bool isDefense;

    [SerializeField]float timeToChangeDecision,timeToGiveDamange,time;//Czas do wykonania-określa czas do wykonania danej czynności,Czas Do Zadania obrażeń-określa czas do zadania obrażeń graczowi,czas-zawiera aktualnyczas

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
        isHit = false;
        isDefense = false;
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
                for(int i = 0; i < textOpponentInteractions.Length; i++)
                {
                    story.descritpionInteractions[i] = textOpponentInteractions[i];
                }
                player.inConfrontation = true;
                managment.opponentDoing = true;
                SetParamets(oponentName,gameObject,1, false, true);
                Decision();
            }
            //---------------------------------------------------

            if (player.skills["Jumping"] == false)
            {
                Atack();
            }
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
                player.iloscSwiatla[player.aktualnaForma - 1] += consumeLight;
                managment.diededMolochs += 1;
                SetParamets(null,null,5, false, false);
                managment.opponentDoing = true;
                player.inConfrontation = false;
                managment.DeActiveButtons();
                Destroy(gameObject);
            }

            if (interactions.endInteractions)
            {
                time += Time.deltaTime;
            }
        }
        else if(player.inConfrontation && player.ranAway)
        {
            time = 0;
            SetParamets(null,null, 5, false, false);
            player.inConfrontation = false;
            managment.DeActiveButtons();
        }

    }

    void SetParamets(string nameOpponent,GameObject opponent, int id,bool whatEnd,bool set)
    {
        managment.oponentName = nameOpponent;
        managment.opponent= opponent;
        interactions.id = id;
        interactions.endInteractions = whatEnd;
        isSet = set;

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
        int decison = Random.Range(2, 2);
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
                if (distanceAtack && player.skills["Jumping"] || distanceAtack && player.skills["ProtectingOn"])
                {

                    //Zrobienie coś,że pojawi się informacja,że misną   
                    Decision();
                }
                else if (distanceAtack && player.skills["Jumping"] == false || distanceAtack && player.skills["ProtectingOn"] == false)
                {
                    SetPlayerParamets();
                    Decision();
                }

                else if (closeAtack && player.skills["ProtectingOn"])
                {
                    //Zrobienie coś,że pojawi się informacja,że misną
                    Decision();

                }
                else if (closeAtack && player.skills["ProtectingOn"] == false)
                {
                    SetPlayerParamets();
                    Decision();
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
            interactions.id = 3;
            isDefense = true;
            player.skills["Atack"] = false;
        }
    }

    /*Funckaj Hit
     Określa obrażenia zadawane Opponentowi.Funckja pwostała glownie po toz, koniecozści uyzycia podówje tego samego kodu*/
    void Hit()
    {
        live -= player.damange;
        player.skills["Atack"] = false;
    }

    void SetPlayerParamets()
    {
        interactions.id = 3;
        interactions.endInteractions = false;
        player.isHit = true;
        managment.opponentDoing = true;
        player.iloscSwiatla[player.aktualnaForma - 1] -= damage;
        time = 0;
    }
}
