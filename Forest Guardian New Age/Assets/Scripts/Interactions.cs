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


    [SerializeField] string[,] storyBook;//Tworzy "Książkę"
    //[SerializeField] coś[] action;

    int indexS, textIndex,actuallyChapter;//Index Interakcji,Indeks Opowiści-Odwołuje się do Indexu opowiści,textIndex-Zmienna któa przechowuje idnex neizbedny do wyswietlania tekstu,aktualny rozdział-okresla aktualny rozdział,dzięki temu skrypt jest uniwersalny
    float time;

    public bool end;//Okreśła koniec pisanen opowieści,dzięki temu w Managment jest możliwe zablokowanie,powtarzanie się opowieści

    public int id;//określa id opponenta.Dzięki temu,możliwe jest uniwersalne odwołąnie się,do poszczególnych indeksów Opisu interakcji

    // Start is called before the first frame update
    void Start()
    {
        //WARRING! Ogarnąć skrypt na wczytywanie,funckja ta BAARDZo spowalnia ładowanie
        CreateBook();
        end = false;
        indexS = 0;
        textIndex = 0;
        actuallyChapter = 1;
        //tekstInteraction.text = null;
        tekstDescritpion.text = null;
        time = 0;
    }

    private void Update()
    {
        //DisplayStory(actuallyChapter - 1, storyBook, 0.1f);
    }
    /*Funckja Create Book
     "Tworzy książke",łącyz zmeine Rozdizał i opowieść na taką książkę wirtualną
    Dzięki temu w Skrycpe DisplayStory możliwe jest uniwersalne wyswietlanie chistoi
    to znaczy,zmeiniasz tylko zmienną "Aktrualny Rozdział",wywołujesz funckej,i gotowe :3*/
    void CreateBook()
    {
        storyBook = new string[story.chapter.Length, story.story.Length];
        storyBook[0, 0] = story.story[0];
        storyBook[0, 1] = story.story[1];
        storyBook[1, 0] = story.story[2];
        storyBook[1, 1] = story.story[3];
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
    public void DisplayStory(int chapter,string[,] book, float t)
    {
        if (end == false)
        {
            if (book[chapter, indexS] != null)
            {
                if (textIndex > book[chapter,indexS].Length)
                {
                    indexS++;
                    textIndex = 0;
                }

                if (time >= t)
                {
                    time = 0;
                    tekstDescritpion.text = book[chapter,indexS].Substring(0, textIndex);
                    textIndex++;
                }
                time += Time.deltaTime;
            }
            else
            {
                actuallyChapter++;
                end = true;
            }
        }
    }

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
    */

    public void ManagmentInteraction(GameObject interactionsToActive, GameObject interactionsToDeactive, string [] story,bool isActive, float t, int id)
    {
        if (end == false)
        {
            interactionsToDeactive.SetActive(!isActive);
            if (time >= t)
            {
                time = 0;
                tekstDescritpion.text = story[id - 1].Substring(0, textIndex);
                textIndex++;
            }

            if (textIndex > story[id - 1].Length)
            {
                time = 0;
                if (id == story.Length)
                {
                    interactionsToActive.SetActive(isActive);
                    end = true;
                }
                textIndex = 0;
                id = story.Length;
            }

            time += Time.deltaTime;
        }
        else
        {
            end = true;
        }
    }
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
