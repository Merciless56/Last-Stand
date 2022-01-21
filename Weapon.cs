using UnityEngine;

public class Weapon : MonoBehaviour
{
    public int id; //идентификатор оружия
    public new string name; //название оружия
    public Sprite img; //иконка оружия
    public string type; //тип оружия
    public bool BigWeapon; // большое ли оружие
    public float Damage; //урон
    public float speed; //скорость пули
    public float timereload; //время перезарядки
    public Rigidbody2D bullet; //префаб пули
    public float fireRate; //скорострельность
    public int allAmmo; //количество патронов
    public int curAmmo; //количество патронов в магазине
    public int maxCurAmmo; //максимальное  количество патронов в магазине
    public Transform gunPoint; //точка выстрела
    public bool Reloaded;//процесс перезарядки
    public ParticleSystem effect; //эффект атаки
    public AudioClip reloadclip; //звук перезарядки
    public AudioClip fireclip;  //звук атаки
    public AudioSource Audio; //источник звука
}
