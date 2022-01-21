using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class AttackPlayer: MonoBehaviour
{
    private Animator Player_Animator;//аниматор
    private float curTime;//скорострельность
    public LayerMask whatIsEnemy;//слой врага
    private float score;//очки
    void Start()
    {
        Player_Animator = GetComponent<Animator>();
    }

    void Update()
    {
        if (!WeaponHold.inventoryisOpen && !Player.IsGameOver)
        {
            if (Input.GetMouseButton(0)) //нажатие ЛКМ
            {
                if (WeaponHold.Left_Hand != null)
                {
                    Attack(WeaponHold.Left_Hand, WeaponHold.Left_Hand_UI, true); //атака левой рукой
                }
            }
            if (Input.GetMouseButton(1)) //нажатие ПКМ
            {
                if (WeaponHold.Right_Hand != null)
                {
                    Attack(WeaponHold.Right_Hand, WeaponHold.Right_Hand_UI, false); //атака правой рукой
                }
            }

            if (Input.GetMouseButtonUp(0)) // отпускание ЛКМ
            {
                Player_Animator.SetBool("Attacked", false);
            }

            if (Input.GetMouseButtonUp(1)) // отпускание ПКМ
            {
                Player_Animator.SetBool("Attacked", false);
                if (WeaponHold.Right_Hand != null) // если левой рукой руков держим оружие
                {
                    Weapon Weapon = WeaponHold.Right_Hand.GetComponent<Weapon>();
                    if (WeaponHold.Right_Hand.name == "3" || WeaponHold.Right_Hand.name == "4") Weapon.effect.Stop();
                    if (!Weapon.Reloaded)
                    {
                        curTime = 20;//значение таймера
                    }
                }
            }
        }
    }
    public void Attack(GameObject hold, GameObject UI, bool hand)
    {
        Weapon Weapon = hold.GetComponent<Weapon>();
        switch (Weapon.type)
        {
            case "Ammo Shooting":
                Weapon.bullet.gameObject.layer = 15;
                if (Weapon.curAmmo > 0) //если есть патроны в магазине
                {
                    curTime += Time.deltaTime;//Запуск таймера
                    if (curTime > Weapon.fireRate)
                    {
                        curTime = 0;//обнулить таймер
                        Weapon.curAmmo--;//вычесть один патрон
                        UI.GetComponentInChildren<Transform>().GetChild(2).GetComponentInChildren<Text>
                            ().text = Weapon.curAmmo + "/" + Weapon.allAmmo;//обновить отабражение патронов

                        Rigidbody2D clone = Instantiate(Weapon.bullet, Weapon.gunPoint.position, 
                            Quaternion.identity) as Rigidbody2D;//клонирование пули
                        //передать скорость и направлени пули
                        clone.velocity = transform.TransformDirection(Weapon.gunPoint.right * Weapon.speed);
                        clone.transform.right = Weapon.gunPoint.right;
                        //передать значения урона классу Bullet
                        clone.GetComponent<Bullet>().Damage = Weapon.Damage;
                        Weapon.Audio.clip = Weapon.fireclip;//получить звук выстрела
                        Weapon.Audio.Play();//воспроизводить звук выстрела
                    }
                }
                else
                {
                    if (!Weapon.Reloaded && Weapon.allAmmo > 0)//если в запасе есть потроны
                    {
                        StartCoroutine(Reload(UI, hold));//запустить сопрограмму перезорядки
                    }
                }           
                break;
            case "Flamethrower":
                if (Weapon.curAmmo > 0 && !Weapon.Reloaded)
                {
                    curTime += Time.deltaTime;
                    if (curTime > Weapon.fireRate)
                    {
                        DetectingEnemy(6f, Weapon);
                        curTime = 0;
                        Weapon.curAmmo--;
                        UI.GetComponentInChildren<Transform>().GetChild(2).GetComponentInChildren<Text>().text = Weapon.curAmmo + "/" + Weapon.allAmmo;
                        Weapon.effect.Play();
                        Weapon.Audio.clip = Weapon.fireclip;
                        Weapon.Audio.Play();
                    }
                }
                else
                {
                    Weapon.effect.Stop();
                    if (!Weapon.Reloaded)
                    {
                        StartCoroutine(Reload(UI, hold));
                    }
                }
                break;
            case "Blaster":
                if (Weapon.curAmmo > 0)
                {
                    curTime += Time.deltaTime;
                    if (curTime > Weapon.fireRate)
                    {
                        DetectingEnemy(8f, Weapon);
                        curTime = 0;
                        Weapon.curAmmo--;
                        UI.GetComponentInChildren<Transform>().GetChild(2).GetComponentInChildren<Text>().text = Weapon.curAmmo + "/" + Weapon.allAmmo;
                        Weapon.effect.Play();
                        Weapon.Audio.clip = Weapon.fireclip;
                        Weapon.Audio.Play();
                    }
                }
                else
                {
                    Weapon.effect.Stop();
                    if (!Weapon.Reloaded)
                    {
                        StartCoroutine(Reload(UI, hold));
                    }
                }
                break;
            case "Sword":
                Player_Animator.SetBool("Attacked", true);
                DetectingEnemy(1f, Weapon);
                if (hand)
                {
                    if (Player.isFacingRight)
                    {
                        Player_Animator.SetFloat("Attack", 1);
                    }
                    else
                    {
                        Player_Animator.SetFloat("Attack", 0);
                    }
                }
                else
                {
                    if (Player.isFacingRight)
                    {
                        Player_Animator.SetFloat("Attack", 0);
                    }
                    else
                    {
                        Player_Animator.SetFloat("Attack", 1);
                    }
                }
                break;
        }
    }
    private void DetectingEnemy(float distance,Weapon weapon)
    {
        RaycastHit2D hit;
        if (Player.isFacingRight)
        {
            Ray2D ray = new Ray2D(transform.position, transform.right);
            hit = Physics2D.Raycast(ray.origin, ray.direction, distance, whatIsEnemy);
        }
        else
        {
            Ray2D ray = new Ray2D(transform.position, transform.right);
            hit = Physics2D.Raycast(ray.origin, ray.direction, -distance, whatIsEnemy);
        }
        if (hit.collider != null && hit.collider.tag == "Enemy")
        {
            var Enemy = hit.collider.GetComponent<Enemy>();
            score += weapon.Damage;
            if (score>1)
            {
                Enemy.TakeDamage(1f);
                score--;
            }
        }
    }
    IEnumerator Reload (GameObject UI, GameObject hold)
    {
        Weapon Weapon = hold.GetComponent<Weapon>(); //получить доступ к оружию
        Weapon.Audio.clip = Weapon.reloadclip;//получить звук выстрела
        Weapon.Audio.Play();//воспроизводить звук перезарядки
        Weapon.Reloaded = true;
        yield return new WaitForSeconds(Weapon.timereload);//время перезорядки
        if (Weapon.allAmmo >= Weapon.maxCurAmmo)
        {
            Weapon.allAmmo -= Weapon.maxCurAmmo;
            Weapon.curAmmo += Weapon.maxCurAmmo;
        }
        else
        {
            Weapon.allAmmo -= Weapon.allAmmo;
            Weapon.curAmmo += Weapon.allAmmo;
        }
        Weapon.Reloaded = false;
        //обновить информацию о патронах оружия
        UI.GetComponentInChildren<Transform>().GetChild(2).GetComponentInChildren<Text>().text = 
            Weapon.curAmmo + "/" + Weapon.allAmmo;
    }
}