using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Managment : MonoBehaviour
{
    [Header("Odwołania")]
    [SerializeField] Player player;
    [SerializeField] Interactions interactions;
    [SerializeField] Story story;
    [SerializeField] GameObject[] buttons;//Zawiera WSZYSTKIE przyciska,i te do chodzenia,funkjami I,II,III i IV formy Golema itd
    public GameObject opponent;
    public GameObject activeButton;

    [Header("Do Zarzadzani Fabułą")]
    [SerializeField] List<List<string>> storyBook = new List<List<string>>();//Tworzy "Książkę"
    int chapter;//Określa aktualny rozdział
    [SerializeField]int[] needValuesToStory, curretValuesToStorry;// Potrzebne wartości do Opowieści-określa wartości,które są neizbędne by opowieśc rusyzła dalej, aktualne Wartosci do Opowieści-Określa aktualne nparametry do opowieści

    [Header("Globalne Parametry do zarządzanie Fabuły")]
    //Do przyszłęgo skryptu na zarządzanie umiejętnościami
    public int diededMolochs;//Okreśła,ile zabiłęś Molochów
    public bool opponentDoing;//Określa,czy Opponentcoś robi
    public bool playerDoing;//Określa czy gracz cos robi

    [Header("Parametry Opponenta")]
    [SerializeField] GameObject opponentAvatars;
    [SerializeField] TMP_Text textOpponentName;
    [SerializeField] TMP_Text timer;
    [SerializeField] GameObject[] opponentsLive;
    public string oponentName;
    int basicOpponentsLive;
    int oldBaicOpponentLive;
    bool isSet;

    // Start is called before the first frame update
    void Start()
    {
        SetOpponentsParamets();
        isSet = false;

        chapter = 0;
        opponentDoing = false;
        timer.text = "";
        playerDoing = false;

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
            interactions.DisplayStory(false, chapter, storyBook, 0.1f);
        }

        //To okreśła,ze keidy się nic niedzije,to włącza mozliwosc poruszania si
        //------------------------------------------------------------------
        if (player.isMoving == false && player.inConfrontation == false && interactions.endStory && interactions.endInteractions)
        {
            DeActiveButtons();
            buttons[1].SetActive(true);
        }
        //-------------------------------------------------------------------

        if (player.inConfrontation)
        {
            if (playerDoing && interactions.endStory && opponent != false)
            {
                interactions.ManagmentInteraction(buttons[player.aktualnaForma - 1], buttons[buttons.Length - 1], story.descriptionInteractionPlayerToOpponent, true, 0.1f);
            }

            //Ten if,wyswietla Panel ataku Golema w Danje formie RAZ,a dzieki zastosowaniu drugiego argumentu,to w tedy,jakby...resetujemy,co pozwala ponownie wrócić z opowieści
            //----------------------------------------------------------------------------------------------------------------------------------------------------------
            if (interactions.endStory && opponentDoing)
            {
                Debug.Log("YEY :D");
                interactions.ManagmentInteraction(buttons[player.aktualnaForma - 1], buttons[buttons.Length - 1], story.descritpionInteractions, true, 0.1f);
            }
            //----------------------------------------------------------------------------------------------------------------------------------------------------------
        }
        else if (player.ranAway)
        {
            interactions.ManagmentInteraction(buttons[1], buttons[buttons.Length - 1], story.descriptionInteractionPlayerToOpponent, true, 0.1f);
        }
        else if(player.inConfrontation == false)
        {
            interactions.ManagmentInteraction(buttons[1], buttons[buttons.Length - 1], story.descritpionInteractions, true, 0.1f);
        }
        //---------------------------------------------------------------------------------------------------------------------------------------------------------

        ManegmentPlayerComunitations();
        if(opponent != null )
        {
            if (isSet == false)
            {
                ManagmentOpponentsParamets(true);
            }

            if(opponentDoing)
            {
                ManegmentOpponentComunitations();
            }
        }
        else if(opponent == null && isSet)
        {
            ManagmentOpponentsParamets(false);
        }
    }

    /*Funckja Create Book
    "Tworzy książke",łącyz zmeine Rozdizał i opowieść na taką książkę wirtualną
   Dzięki temu w Skrycpe DisplayStory możliwe jest uniwersalne wyswietlanie chistoi
   to znaczy,zmeiniasz tylko zmienną "Aktrualny Rozdział",wywołujesz funckej,i gotowe :3*/
    void CreateBook()
    {
        storyBook.Add(new List<string>());
        storyBook[0].Add(story.story[0]);
        storyBook[0].Add(story.story[1]);
        storyBook[0].Add(story.story[2]);
        storyBook[0].Add(story.story[3]);
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

    void SetOpponentsParamets()
    {
        oponentName = null;
        opponentAvatars.SetActive(false);
        for (int i = 0; i < opponentsLive.Length; i++)
        {
            opponentsLive[i].SetActive(false);
        }
    }

    //Ta fucnka istnieje po to,by zmeinna ozdizał, nie była publiczna
    public void nextChapter()
    {
        chapter++;
    }

    void ManegmentPlayerComunitations()
    {
        if (player.isMoving)
        {
            //Określić funckej w Interactions,do zarządzania chodzeniem,która będzie wyświetlałą też informacje,o tym,że np: Golem wyszedł z lasu.Ta funckja ma sie wyświetlić raz
        }

        if (player.skills["Jumping"] && opponentDoing == false && player.onGround)
        {
            interactions.id = 3;
            interactions.endInteractions = false;
            interactions.ManagmentInteraction(buttons[player.aktualnaForma - 1], buttons[player.aktualnaForma - 1], story.descriptionInteractionPlayerToOpponent, true, 0.1f);
        }
        else if (player.skills["Jumping"] == false && opponentDoing == false && player.onGround == false)
        {
            interactions.id = 4;
            interactions.endInteractions = false;
            interactions.ManagmentInteraction(buttons[player.aktualnaForma - 1], buttons[player.aktualnaForma - 1], story.descriptionInteractionPlayerToOpponent, true, 0.1f);
        }
    }

    void ManagmentOpponentsParamets(bool isAvtive)
    {
        textOpponentName.text = oponentName;
        opponentAvatars.SetActive(isAvtive);
        basicOpponentsLive = opponent.GetComponent<Oponent>().live;
        if (opponent.GetComponent<Oponent>().live != oldBaicOpponentLive)
        {
            opponentsLive[basicOpponentsLive].SetActive(false);
        }
        else
        {
            for (int i = 0; i < basicOpponentsLive; i++)
            {
                opponentsLive[i].SetActive(isAvtive);
            }
            isSet = isAvtive;
        }
        oldBaicOpponentLive = basicOpponentsLive;
    }

    void ManegmentOpponentComunitations()
    {
        if (player.skills["Atack"] && opponent.GetComponent<Oponent>().isDefense)
        {
            Debug.Log("5");
            interactions.id = 3;
            interactions.endInteractions = false;
            interactions.ManagmentInteraction(buttons[player.aktualnaForma - 1], buttons[buttons.Length - 1], story.descritpionInteractions, true, 0.1f);
        }
        else if (player.skills["Atack"] && opponent.GetComponent<Oponent>().isDefense && interactions.endInteractions)
        {
            Debug.Log("6");
            opponent.GetComponent<Oponent>().isDefense = false;
        }
    }

    public void DeActiveButtons()
    {
        FindActiveButton();
        activeButton.SetActive(false);
    }

}
