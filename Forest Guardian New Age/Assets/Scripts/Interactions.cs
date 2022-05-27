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

    public void ManagmentInteraction(GameObject interactions, bool isActive, float t)
    {
        if (end == false)
        {
            if (time >= t)
            {
                time = 0;
                tekstDescritpion.text = story.descritpionInteractions[id - 1].Substring(0, textIndex);
                textIndex++;
            }

            if (textIndex > story.descritpionInteractions[id - 1].Length)
            {
                time = 0;
                if (id == story.descritpionInteractions.Length)
                {
                    interactions.SetActive(isActive);
                    end = true;
                }
                textIndex = 0;
                id = story.descritpionInteractions.Length;
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
