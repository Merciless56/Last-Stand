using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float Damage;
    void Start()
    {
        Destroy(gameObject, 0.3f);//удалить пулю через 0.3 секунды
    }

    void OnTriggerEnter2D(Collider2D coll)
    {
        if (!coll.isTrigger)
        {
            switch (coll.tag)
            {
                case "Enemy":
                    var Enemy = coll.GetComponent<Enemy>();//получение доступа к классу Enemy
                    if (Enemy != null && Enemy.Live > 0)
                    {
                        Enemy.TakeDamage(Damage);//вызвать метод TakeDamage
                    }
                    break;
                case "Player":
                    var Player = coll.GetComponent<Player>();//получение доступа к классу Player
                    if (Player != null && Player.Live > 0)
                    {
                        Player.TakeDamage(Damage);//вызвать метод TakeDamage
                    }
                    break;
            }
            Destroy(gameObject);//удалить пулю
        }
    }
}