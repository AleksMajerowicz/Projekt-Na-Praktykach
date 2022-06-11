using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    [Header("Odwołąnia")]
    [SerializeField] Managment managment;
    [SerializeField] Interactions interactions;

    [Header("światło Golema")]
    [SerializeField] Slider[] listaPasków;//Lista gameobject'ów przechowująca paski
    public float[] iloscSwiatla;//Lista floatów,przehcowójąca daną ilosć światła,dla danego paska
    [SerializeField] float[] swiatlaDoNastepnejFormy;//Lista floatów przechowujaca maksymalną ilośc,przechowywanego swiatła,dla danego paska
    public int aktualnaForma;//Zmienna przehcowująca aliczbę określającą aktualną formę golema

    [Header("Statystyki Golema")]
    public bool isMoving;// Określa,cz golem sie porusza
    public float actuallyPosition;//Okreśła aktualną Pozycje golema
    public int speedMoving;//Określa szybkość poruszania się golema
    [SerializeField]float speed;//określa predność golema.Ta zmienna w skrypcie,określa,po jakim czasie actuallyposition zmieni się
    public string seeOponentName;//określa,jakiego oponnenta widzi
    public bool inConfrontation;//Okresla,czy Golem jest w konfrontacji z Opponentem

    [SerializeField]public Dictionary<string, bool> skills= new Dictionary<string, bool>();//Określa Skille.Jest to w Dictonary w celu uporzadkowania kodu,żeby był bardzije czytelny :3
    public int damange;//Okreśła obraqżenia Golema

    public int timeToRestAtack;//Okresla czas odpoczyn potrzebnego do ponownego zaatakowania.W skrycpie okrełsa czas potrzebny do możliwości ponownego ataku
    public int timeToRestProtecting;//Okrełśa czas odpoczynku potrzebnego do ponownej obron.W skrypcie okreśła czas potrzebny do możliwości ponownej obrony
    public int timeToRestJumping;//Określa czas odpoczynku do ponownego skoku.Określa czas potrzebny do możliwości wybrania skoku

    float timeMoving;//Czas chodzenia-okresala aktalny czas chodzenia gracza
    public Dictionary<string, float> timneToReady = new Dictionary<string, float>();

    public bool isHit, onGround;
    float gravity,timeGravity;//*
    // Start is called before the first frame update
    void Start()
    {
        skills.Add("Atack", false);//bool sprawdzający,czy golem aktualnie atakuje?.Odpala sie w przyciskach ataku,a dezaktywuje w Opponencie
        skills.Add("ProtectingOn", false);//Okreśła czy Gracz broni sie
        skills.Add("Jumping", false);//Określa,czy gracz skoczył
        
        timneToReady.Add("TimeToReadyAtack", 0);//Czas gotowości do ataku-okresla aktualny czas
        timneToReady.Add("TimeToReadyProtecting", 0);//Czas goowości do Obrony-Okresla aktualny czas
        timneToReady.Add("TimeToReadyJumping", 0);//Czas gotowości do Skoku-Określa aktualny czas

        isMoving = false;
        isHit = false;
        onGround = true;

        aktualnaForma = 1;
        gravity = 2.5f;

        for (int i = 0; i < listaPasków.Length; i++)
        {
            listaPasków[i].maxValue = swiatlaDoNastepnejFormy[i];
        }

        listaPasków[0].gameObject.SetActive(true);

        for (int i = 1; i < listaPasków.Length; i++)
        {
            listaPasków[i].gameObject.SetActive(false);
        }

        ManagmentHPBarsPlayer();
    }

    // Update is called once per frame
    void Update()
    {

        /*Kiedy golem jest w konfrontacji,to w tedy jest otwarty na zmieniane parametrów takich jak jumping czy ProtectingOn
        Kiedy nie,to włączany jest Panel Poruszania sie gracza
         */
        if (inConfrontation)
        {
            ManagmentHPBarsPlayer();

            if (isMoving)
            {
                isMoving = false;
            }

            if(skills["Jumping"])
            {
                timeGravity += Time.deltaTime;
                if (timeGravity > gravity && interactions.endInteractions == false)
                {
                    skills["Jumping"] = false;
                    timeGravity = 0;
                }
            }
            
            CalculatingSkillsTime();
        }
        else
        {
            MovingManagment();
        }
    }

    void ManagmentHPBarsPlayer()
    {
        listaPasków[aktualnaForma - 1].value = iloscSwiatla[aktualnaForma - 1];
        //Uniwersalny skrypt,zmiany formy golema
        //-------------------------------------------------------------------------------
        if (iloscSwiatla[aktualnaForma - 1] >= swiatlaDoNastepnejFormy[aktualnaForma - 1])
        {
            //Zrobić tak,by możliwość przejscia do następenj formy,była możlwia w tedy,keidy jest się na rozdziale 2
            listaPasków[aktualnaForma].gameObject.SetActive(true);
            aktualnaForma += 1;

            managment.ToStory(2, Mathf.RoundToInt(iloscSwiatla[0]));
            iloscSwiatla[aktualnaForma - 1] = swiatlaDoNastepnejFormy[aktualnaForma - 1];
        }
        else if (skills["Atack"] == false && iloscSwiatla[aktualnaForma - 1] <= 1)//Ze względu na tą linijkę,trzeba ustawić mwarotści na min 1
        {
            aktualnaForma -= 1;
            listaPasków[aktualnaForma].gameObject.SetActive(false);
        }
        //--------------------------------------------------------------------------------
    }

    /*Funckaj MovingManagment
     Zarzadza Poruszaniem się gracza*/
    void MovingManagment()
    {
        if (isMoving)
        {
            interactions.Write("Idziesz.....", 0.25f);
            interactions.Repeat("Idziesz.....",false);

            //Okresla po jakim czasie(speed) gracz stawi krok(speedMoving)
            //-----------------------------------
            if (timeMoving >= speed)
            {
                actuallyPosition += speedMoving;
                timeMoving = 0;
            }
            //------------------------------------

            timeMoving += Time.deltaTime;
        }
    }

    /*Funkcja CalculatingSkillsTime
     Oblicza aktualny czas skilli.Jest to w Funkcji,w Celu uporzadkowania Kodu*/
    void CalculatingSkillsTime()
    {
        timneToReady["TimeToReadyAtack"] += Time.deltaTime;
        timneToReady["TimeToReadyProtecting"] += Time.deltaTime;
        timneToReady["TimeToReadyJumping"] += Time.deltaTime;
    }
}
