using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Managment : MonoBehaviour
{
    public string oponentName;

    [SerializeField] Player player;
    [SerializeField] Interactions interactions;
    [SerializeField] Story story;
    [SerializeField] GameObject[] buttons;//Zawiera WSZYSTKIE przyciska,i te do chodzenia,funkjami I,II,III i IV formy Golema itd

    [SerializeField] string[,] storyBook;//Tworzy "Książkę"
    int chapter;//Określa aktualny rozdział
    [SerializeField]int[] needValuesToStory, curretValuesToStorry;// Potrzebne wartości do Opowieści-określa wartości,które są neizbędne by opowieśc rusyzła dalej, aktualne Wartosci do Opowieści-Określa aktualne nparametry do opowieści

    // Start is called before the first frame update
    void Start()
    {
        CreateBook();
        ToStory(0,1);

        chapter = 0;

        for(int i = 0; i < buttons.Length; i++)
        {
            buttons[i].SetActive(false);
        }
    }

    // Update is called once per frame
    void Update()
    {
            interactions.DisplayStory(chapter,storyBook,0.1f);

        
            if (player.isMoving)
            {
                //Określić funckej w Interactions,do zarządzania chodzeniem,która będzie wyświetlałą też informacje,o tym,że np: Golem wyszedł z lasu.Ta funckja ma sie wyświetlić raz
            }

            //Ten if,wyswietla Panel ataku Golema w Danje formie RAZ
            if (player.inConfrontation)
            {
                interactions.ManagmentInteraction(buttons[player.aktualnaForma - 1], buttons[buttons.Length - 1], story.descritpionInteractions, true, 0.1f);
            }
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

    /*Funckja do powieści
     Odpwoaida za...jakby,zapisywanie danych parametrów bezposrednio w źródłach
    ideks opwości orkesla indek opwieści ptrorzebny by źródło zapisywało zmienną
    wartości oikreśła wartość jaka zostanie dana do aktualnych warotści do opwieści
    sprawdzane ejst czy rozdizał jest równuy indeksowi opowiści,jeżlei tak,dodaje dana warotsć do aktualnych wartosci,a jeżeli są
    one równe z potrzebnymi,to w tedy koneic opwoeści ejst równy false,co,dzieki przestawieniu na rozdizął anstępony w Funckji Wyswieltanie Opowiści
    wyświetla dalsza część opowieści
    |Oganrąc,by ta funckaj inicjowała,zaczecie opwoeiści)żeby ie dawać chapter - 1)|
     */
    public void ToStory(int indexStory, int values)
    {
        if(chapter == indexStory)
        {
            curretValuesToStorry[chapter] += values;

            if(curretValuesToStorry[chapter] == needValuesToStory[chapter])
            {
                interactions.endStory = false;
            }
        }
    }

}
