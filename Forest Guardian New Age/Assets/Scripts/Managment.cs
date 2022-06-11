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
    public GameObject activeButton;

    [SerializeField] string[,] storyBook;//Tworzy "Książkę"
    int chapter;//Określa aktualny rozdział
    [SerializeField]int[] needValuesToStory, curretValuesToStorry;// Potrzebne wartości do Opowieści-określa wartości,które są neizbędne by opowieśc rusyzła dalej, aktualne Wartosci do Opowieści-Określa aktualne nparametry do opowieści

    //Do przyszłęgo skryptu na zarządzanie umiejętnościami
    public int diededMolochs;
    public bool isOpponentDoSomething;//Określa,czy fucnkaj ogarnia,ze widza się i że nietrzeba o tym jeszcze raz informować
    // Start is called before the first frame update
    void Start()
    {
        chapter = 0;
        isOpponentDoSomething = false;

        CreateBook();
        ToStory(0,1);

        for(int i = 0; i < buttons.Length; i++)
        {
            if(i != buttons.Length - 2)
            {
                buttons[i].SetActive(false);
            }
            else
            {
                buttons[i].SetActive(true);
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (interactions.endStory == false)
        {

            FindActiveButton();
            interactions.DisplayStory(activeButton, false, chapter, storyBook, 0.1f);
            chapter++;
        }

        //To okreśła,ze keidy się nic niedzije,to włącza mozliwosc poruszania si
        //------------------------------------------------------------------
        if (player.isMoving == false && player.inConfrontation == false)
        {
            FindActiveButton();//*
            activeButton.SetActive(false);//*
            buttons[1].SetActive(true);
        }
        //-------------------------------------------------------------------

        if (player.isMoving)
        {
            //Określić funckej w Interactions,do zarządzania chodzeniem,która będzie wyświetlałą też informacje,o tym,że np: Golem wyszedł z lasu.Ta funckja ma sie wyświetlić raz
        }

        //Ten if,wyswietla Panel ataku Golema w Danje formie RAZ,a dzieki zastosowaniu drugiego argumentu,to w tedy,jakby...resetujemy,co pozwala ponownie wrócić z opowieści
        //----------------------------------------------------------------------------------------------------------------------------------------------------------
        if (player.inConfrontation && interactions.endStory && isOpponentDoSomething == false)
        {
            interactions.ManagmentInteraction(buttons[player.aktualnaForma - 1], buttons[buttons.Length - 1], story.descritpionInteractions, true, 0.1f);
        }
        //---------------------------------------------------------------------------------------------------------------------------------------------------------

        //---------------------------------------------------------------------------------------------------------------------------------------------------------
        if(player.isHit)
        {
            interactions.ManagmentInteraction(buttons[player.aktualnaForma - 1], buttons[buttons.Length - 1], story.descritpionInteractions, true, 0.1f);
        }
        //--------------------------------------------------------------------------------------------------------------------------------------------------------

        if(player.skills["Jumping"] && isOpponentDoSomething && player.onGround)
        {
            interactions.id = 3;
            interactions.ManagmentInteraction(buttons[player.aktualnaForma - 1], buttons[player.aktualnaForma - 1], story.descriptionInteractionPlayerToOpponent, true, 0.1f);
        }
        else if(player.skills["Jumping"] == false && isOpponentDoSomething && player.onGround == false)
        {
            interactions.id = 4;
            interactions.ManagmentInteraction(buttons[player.aktualnaForma - 1], buttons[player.aktualnaForma - 1], story.descriptionInteractionPlayerToOpponent, true, 0.1f);
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
            curretValuesToStorry[chapter] = values;

            if(curretValuesToStorry[chapter] == needValuesToStory[chapter])
            {
                interactions.endStory = false;
            }
        }
    }

    /*Funckaj FindActiveButton
     Szuka aktyuwnego przycisku,po to,by go zapisać w zmiennej w celu
    ponownego jego uruchomienia po zakończeniu wyswietlania opowieści,bo...nie zawsze bedą te same przyciski*/
    void FindActiveButton()
    {
        for(int i = 0; i < buttons.Length; i++)
        {
            if(buttons[i].activeSelf)
            {
                activeButton = buttons[i];
            }
        }
    }

}
