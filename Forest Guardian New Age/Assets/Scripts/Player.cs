using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    //[Header("Podstawowe parametry Golema")]
    //[SerializeField] bool isDamange;

    [Header("światło Golema")]
    [SerializeField] Slider[] listaPasków;//Lista gameobject'ów przechowująca paski
    public float[] iloscSwiatla;//Lista floatów,przehcowójąca daną ilosć światła,dla danego paska
    [SerializeField] float[] swiatlaDoNastepnejFormy;//Lista floatów przechowujaca maksymalną ilośc,przechowywanego swiatła,dla danego paska
    public int aktualnaForma;//Zmienna przehcowująca aliczbę określającą aktualną formę golema

    [Header("Statystyki Golema")]
    public bool isMoving;// Określa,cz golem sie porusza
    public float actuallyPosition;//Okreśła aktualną Pozycje golema
    public int speedMoving;//Określa szybkość poruszania się golema
    float speed;//określa predność golema.Ta zmienna w skrypcie,określa,po jakim czasie actuallyposition zmieni się
    public string seeOponentName;//określa,jakiego oponnenta widzi
    public bool inAtack;//bool sprawdzający,czy golem aktualnie atakuje?.Odpala sie w przyciskach ataku,a dezaktywuje w Opponencie
    public int damange;
    public bool jumping;
    public bool protectingOn;

    float time;

    // Start is called before the first frame update
    void Start()
    {
        inAtack = false;

        //isDamange = false;

        aktualnaForma = 1;

        for (int i = 0; i < listaPasków.Length; i++)
        {
            listaPasków[i].maxValue = swiatlaDoNastepnejFormy[i];
        }

        listaPasków[0].gameObject.SetActive(true);

        for (int i = 1; i < listaPasków.Length; i++)
        {
            listaPasków[i].gameObject.SetActive(false);
        }

    }

    // Update is called once per frame
    void Update()
    {
        listaPasków[aktualnaForma - 1].value = iloscSwiatla[aktualnaForma - 1];
        //Uniwersalny skrypt,zmiany formy golema
        //-------------------------------------------------------------------------------
        if (iloscSwiatla[aktualnaForma - 1] >= swiatlaDoNastepnejFormy[aktualnaForma - 1])
        {
            listaPasków[aktualnaForma].gameObject.SetActive(true);
            aktualnaForma += 1;
        }
        else if (inAtack == false && iloscSwiatla[aktualnaForma - 1] <= 1)//Ze względu na tą linijkę,trzeba ustawić mwarotści na min 1
        {
            //isDamange = false;
            aktualnaForma -= 1;
            listaPasków[aktualnaForma].gameObject.SetActive(false);
        }
        //--------------------------------------------------------------------------------

        if (seeOponentName != null)
        {
            if (isMoving)
            {
                isMoving = false;
            }

            //if(jumping)
            {
                //Ogarni�cie,�e po jakim� czasie,golem wyl�duje
            }
        }
        else
        {
            MovingManagment();
        }
    }

    void MovingManagment()
    {
        if (isMoving)
        {
            if (time >= speed)
            {
                actuallyPosition += speedMoving;
            }

            time += Time.deltaTime;
        }
    }
}
