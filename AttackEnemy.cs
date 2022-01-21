using UnityEngine;
using System.Collections;

public class AttackEnemy : MonoBehaviour
{
    private Enemy Enemy;
    private WeaponHoldEnemy Hold;
    private Animator Enemy_Animator;
    private float curTime;
    private bool Attacked = false;
    public LayerMask whatIsPlayer;

    void Start()
    {
        Enemy_Animator = GetComponent<Animator>();
        Enemy = GetComponent<Enemy>();
        Hold = GetComponent<WeaponHoldEnemy>();
    }
    void Update()
    {
        //если враг может стрелять и игра не окончена
        if (Enemy.shouldShoot && !Player.IsGameOver)
        {
            StartCoroutine(Rate()); //Запуск коротины для атаки 
            if (Attacked)
            {
                if (Hold.Left_Hand != null)
                {
                    Attack(Hold.Left_Hand, true); //атака левой рукой
                }

                if (Hold.Right_Hand != null)
                {
                    Attack(Hold.Right_Hand, false); //атака правой рукой
                }
                Attacked = false;
            }
            else
            {
                Enemy_Animator.SetBool("Attacked", false);
                Attacked = false;
            }
        }
    }
    public void Attack(GameObject hold, bool hand)
    {
        Weapon Weapon = hold.GetComponent<Weapon>();
        switch (Weapon.type)
        {
            case "Ammo Shooting":
                Weapon.bullet.gameObject.layer = 14;
                Weapon.fireRate = Random.Range(0.1f, 0.5f);
                curTime += Time.deltaTime;
                if (curTime > Weapon.fireRate)
                {
                    curTime = 0;
                    Rigidbody2D clone = Instantiate(Weapon.bullet, Weapon.gunPoint.position, Quaternion.identity) as Rigidbody2D;
                    clone.velocity = transform.TransformDirection(Weapon.gunPoint.right * Weapon.speed);
                    clone.transform.right = Weapon.gunPoint.right;
                    clone.GetComponent<Bullet>().Damage = Weapon.Damage;
                    Weapon.Audio.clip = Weapon.fireclip;
                    Weapon.Audio.Play();
                }
                break;
            case "Flamethrower":
                DetectingPlayer(6f, Weapon);
                curTime += Time.deltaTime;
                if (curTime > Weapon.fireRate)
                {
                    curTime = 0;
                    Weapon.effect.Play();
                }
                break;
            case "Blaster":
                DetectingPlayer(6f, Weapon);
                curTime += Time.deltaTime;
                if (curTime > Weapon.fireRate)
                {
                    curTime = 0;
                    Weapon.effect.Play();
                    Weapon.Audio.clip = Weapon.fireclip;
                    Weapon.Audio.Play();
                }
                break;
            case "Sword":
                Enemy_Animator.SetBool("Attacked", true);
                DetectingPlayer(1.5f, Weapon);
                if (hand)
                {
                    if (Enemy.isFacingRight)
                    {
                        Enemy_Animator.SetFloat("Attack", 1);
                    }
                    else
                    {
                        Enemy_Animator.SetFloat("Attack", 0);
                    }
                }
                else
                {
                    if (Enemy.isFacingRight)
                    {
                        Enemy_Animator.SetFloat("Attack", 0);
                    }
                    else
                    {
                        Enemy_Animator.SetFloat("Attack", 1);
                    }
                }
                break;
        }
    }
    private void DetectingPlayer(float distance, Weapon weapon)
    {
        RaycastHit2D hit;
        Ray2D ray;
        Vector2 offset = new Vector2(transform.position.x, transform.position.y + 1f);
        ray = new Ray2D(offset, transform.right);
        if (Enemy.isFacingRight)
        {
            hit = Physics2D.Raycast(ray.origin, ray.direction, distance, whatIsPlayer);
        }
        else
        {
            hit = Physics2D.Raycast(ray.origin, ray.direction, -distance, whatIsPlayer);
        }
        if (hit.collider != null && hit.collider.tag == "Player")
        {
            var Player = hit.collider.GetComponent<Player>();

            Player.TakeDamage(weapon.Damage);
        }
    }
    IEnumerator Rate() 
    {
        //случайный промежуток времени
        yield return new WaitForSeconds(Random.Range(0f, 2f));
        Attacked = true; //разрешить атаку
    }
}