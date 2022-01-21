using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class Items : MonoBehaviour
{
    public string type;
    public int Ammunition;
    public int Health;
    public Text addHP;
    public Text addAmmo;
    public Text ammo;
    private AudioSource Audio;
    private void Start()
    {
        Audio = GetComponent<AudioSource>();
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            if (WeaponHold.Right_Hand != null)
            {
                //получение доступа к классу Weapon
                Weapon Weapon = WeaponHold.Right_Hand.GetComponent<Weapon>(); 
                if (Weapon.type == type)
                {
                    //прибавление патронов
                    Weapon.allAmmo += Ammunition;
                    //обновление интерфейса отображения патронов 
                    ammo.text = Weapon.curAmmo + "/" + Weapon.allAmmo;
                    addAmmo.text = "+" + Ammunition.ToString();
                    //запуск функции Clear в качестве сопрограммы
                    StartCoroutine(Clear());
                }
            }
            if (Player.Live < 100 && type == "First aid kit")
            {
                if (Player.Live + Health > 100)
                {
                    //установка добавленных очков здоровья
                    addHP.text = "+" + (100 - Player.Live).ToString();
                    Player.Live = 100;
                }
                else
                {
                    //прибавление очков здоровья
                    Player.Live += Health;
                    addHP.text = "+" + Health.ToString();
                }
                //запуск функции Clear в качестве сопрограммы
                StartCoroutine(Clear());
            }
        }
    }
    IEnumerator Clear()
    {
        Audio.Play();//воспроизводить звук подбора
        //выключить визуализизацию спрайта
        gameObject.GetComponent<SpriteRenderer>().enabled = false;
        //выключить коллайдер
        gameObject.GetComponent<BoxCollider2D>().enabled = false;
        //приостановление выполнение сопрограммы на 3 секунды
        yield return new WaitForSeconds(3f);
        addAmmo.text = "";
        addHP.text = "";
        Destroy(gameObject); //удаление объекта со сцены
    }
}