using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Interactions : MonoBehaviour
{
    [Header("Odwołania")]
    [SerializeField] TMP_Text tekstDescritpion;//Odwołuje się do tekst,który wyświetla opowieść
    [SerializeField] Player player;
    [SerializeField] Managment managment;
    [SerializeField] Story story;

    //[SerializeField] coś[] action;

    int indexS, textIndex,shortTextIndex;//Index Interakcji,Indeks Opowiści-Odwołuje się do Indexu opowiści,textIndex-Zmienna któa przechowuje idnex neizbedny do wyswietlania tekstu,aktualny rozdział-okresla aktualny rozdział,dzięki temu skrypt jest uniwersalny
    float time;

    public bool endStory;//Okreśła koniec pisanen opowieści,dzięki temu w Managment jest możliwe zablokowanie,powtarzanie się opowieści
    public bool endInteractions;

    public int id;//określa id opponenta.Dzięki temu,możliwe jest uniwersalne odwołąnie się,do poszczególnych indeksów Opisu interakcji

    // Start is called before the first frame update
    void Start()
    {
        endInteractions = true;
        indexS = 0;
        textIndex = 0;
        shortTextIndex = 0;
        //tekstInteraction.text = null;
        tekstDescritpion.text = null;
        time = 0;
    }
    
    /*Funkcja DisplayStory
     Wyświetla Historie
    Pobeira zmienną rozdział,książkę oraz czas w jakim będzie to wyswietlane
    jeżeli end nie jest równy false(zmienna end określa,czy wyświetlanie opowieści zostało zakończone) to w tedy sprawdza
    Czy w danym rozdziale,jest taki index,jeżeli nie to ustawia end na true
    jeżeli jest to wchodzimy w taką "unitową Pętle"
    któa sprawdza czy index tekstu jest więskzy od dłuiści "akapitu"
    jeżeli tak,to lecimy do następnego akapitu,zerujemy index tekstu
    potem jest klasyczna pętla spowalniająca*/
    //--------------------------------------------------------------------------------------------------
    public void DisplayStory(bool isActive,int chapter,List<List<string>> book, float t)
    {
        if (endStory == false)
        {
            managment.activeButton.SetActive(isActive);

            if (indexS < book[chapter].Count)
            {
                if (textIndex >= book[chapter][indexS].Length)
                {
                    indexS++;
                    textIndex = 0;
                }

                if (time >= t)
                {
                    time = 0;
                    tekstDescritpion.text = book[chapter][indexS].Substring(0, textIndex);
                    textIndex++;
                }
                time += Time.deltaTime;
            }
            else
            {
                endStory = true;
                textIndex = 0;
                indexS = 0;
                managment.nextChapter();
                managment.activeButton.SetActive(!isActive);
            }
        }
    }
    //--------------------------------------------------------------------------------------------------

    /*Funkcja ManagmentInteractions
    * Jest podobna w działaniu do Funkcji DisplayStory,bo też Wyświetla "Opowieść"
    * Różni się tym,idek opowieści,tym razem jest id oponenta.jest to niezbędny zabieg,bo to pozwala się odniesc do
    * Poszczeólnych akapitów Opowieści według id oponenta.Przt cyzm wazne ejst by w samej zmiennej descritpionInteraction były one ułożone według jakiegoś wzoru.
    * Kiedy tekst index będzie większy od danje wiadomości interakcji,to w tedy id = ostatni index tejze tablicy
    * który zawiera "Co robisz?".
    * Po wypisaniu tego,dzięki zastosowaniu kolejności wykonywania linijek,ze wzgledu że id == długości tablicy,to jest wyświetlany danego Panelu Interakcji.
    * Pozwoli to tez na jej wyłaczenie,keidy zasotsujemy Ucieczkę,jak i bez koniecnzości większej ilości funckji,kontynuwanie gry
    * Argumenty interactionsToActive i interactnionToDeactive to obiekty które będą aktywowanie i dezaktywowane,
    * lista stringów jako argument pozwala na odowałanai się do zmiennej opis interacji i opis interakcji gracza do opponenta,w jednej funckji
    * boll jest aktywny określa prawdepowiedziazy,od czego zaczynamy,czy od wyłączania panelu czy właczania?,
    * t okreśła czas wyswietlania się napisów
    * a id określa id oponenta,jak i umiejętności
    * |Naprawić tak,by można się było odołąć do koncowego indexu,mając ten argument funckji: id|
    * zmienna nazwa skilla określa skill,który będzie sprawdzany.Domyślnie ma Wartosc null,bo to bedzie potrzebne,keidy funckaj jest wużywana do wyswietlania interakcji z Opoonentem i na odwót
    */
    //--------------------------------------------------------------------------------------------------------------------------------------------------------
    public void ManagmentInteraction(GameObject interactionsToActive, GameObject interactionsToDeactive, string [] story,bool isActive, float t,string skillName = null)
    {
        if (endInteractions == false && endStory)
        {
            interactionsToDeactive.SetActive(!isActive);

            if (time >= t)
            {
                time = 0;
                tekstDescritpion.text = story[id - 1].Substring(0, textIndex);
                textIndex++;
            }

            //Write(story[id - 1], t);

            if (textIndex > story[id - 1].Length)
            {
                time = 0;
                if (id == story.Length)
                {
                    interactionsToActive.SetActive(isActive);
                    endInteractions = true;
                }
                textIndex = 0;
                id = story.Length;
            }

            time += Time.deltaTime;
        }
        else
        {
            endInteractions = true;
            //Do spojrzena na nowo!
            //------------------------------------------------------------------------------------------------
            if (player.inConfrontation && managment.opponentDoing && managment.oponentName == null)
            {
                player.inConfrontation = false;
            }
            else if(player.inConfrontation && managment.opponentDoing && managment.oponentName != null)
            {
                managment.opponentDoing = true;
            }

            if (player.skills["Jumping"])
            {
                player.onGround = false;
            }
            else if (player.skills["Jumping"] == false)
            {
                player.onGround = true;
            }
            //-------------------------------------------------------------------------------------------------

            if(managment.opponentDoing)
            {
                managment.opponentDoing = false;
            }
            else if(managment.playerDoing)
            {
                managment.playerDoing = false;
            }
            else if(player.ranAway)
            {
                player.ranAway = false;
            }
        }
    }
    //--------------------------------------------------------------------------------------------------------------------------------------------------------

    /*Funkcja Write
     Pisze,to co podamy jako Argument typu stirng,w danym czasie.*/
    //------------------------------------------------------------------------
    public void Write(string text,float t)
    {
        if (time >= t)
        {
            time = 0;
            tekstDescritpion.text = text.Substring(0, shortTextIndex);
            shortTextIndex++;
        }
        time += Time.deltaTime;
    }
    //------------------------------------------------------------------------

    /*Funckja Repeat
     Resetuje,po to,by na nowo powtarzac napis,ale nie zawsze*/
    //----------------------------------------------------------
    public void Repeat(string text,bool now)
    {
        if (shortTextIndex >= text.Length || now)
        {
            tekstDescritpion.text = "";
            shortTextIndex = 0;
        }
    }
    //----------------------------------------------------------
    /*public void DisplayInteraction(string[] dA)
    {
        if (end == false)
        {
            if (indexI < dA.Length)
            {
                tekstInteraction.text += dA[indexI];
                indexI++;
                tekstInteraction.text += " \n";
            }
            else
            {
                end = true;
            }
        }
    }*/
}
