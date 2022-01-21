using UnityEngine;

public class WeaponHoldEnemy : MonoBehaviour
{
    public GameObject Left_Hand;
    public GameObject Right_Hand;
    public Transform leftHoldPoint;
    public Transform rightHoldPoint;
    private bool isBigWeapon;
    private string WeaponType;
    private Animator Enemy_Animator;
    private Enemy Enemy;
    public GameObject Weapon_Left;
    public GameObject Weapon_Right;
    private void Start()
    {
        Enemy = GetComponent<Enemy>();
        Enemy_Animator = GetComponent<Animator>();
        if (Weapon_Right != null)
        {
            GetWeapon(ref Right_Hand, ref Weapon_Right);
        }
        if (Weapon_Left != null)
        {
            GetWeapon(ref Left_Hand,  ref Weapon_Left);
        }
    }
    void Update()
    {
        if (Right_Hand != null)
        {
            GetWeaponParameters(Right_Hand);
            Right_Hand.transform.SetParent(rightHoldPoint.GetComponentInParent<Transform>().transform);

            if (!isBigWeapon)
            {
                Right_Hand.transform.position = rightHoldPoint.position;
                if (Enemy.isFacingRight)
                {
                    Right_Hand.GetComponent<SpriteRenderer>().sortingOrder = 13;
                    Right_Hand.transform.rotation = rightHoldPoint.rotation * Quaternion.Euler(0,0,-65);
                }
                else
                {
                    Right_Hand.GetComponent<SpriteRenderer>().sortingOrder = 1;
                    Right_Hand.transform.rotation = rightHoldPoint.rotation * Quaternion.Euler(0,0,40);
                }
            }
            else
            {
                if (Enemy.isFacingRight)
                {
                    Right_Hand.transform.position = rightHoldPoint.position;
                }
                else
                {
                    Right_Hand.transform.position = leftHoldPoint.position;
                }
                Right_Hand.transform.rotation = Quaternion.Euler(0, 0, 0);
                Right_Hand.GetComponent<SpriteRenderer>().sortingOrder = 13;
            }
            if (WeaponType == "Ammo Shooting")
            {
                if (Enemy.isFacingRight)
                {
                    Right_Hand.GetComponentInChildren<Transform>().GetChild(0).rotation = Quaternion.Euler(0, 0, 0);
                }
                else
                {
                    Right_Hand.GetComponentInChildren<Transform>().GetChild(0).rotation = Quaternion.Euler(0, 180, 0);
                }
            }
            else if (WeaponType == "Flamethrower")
            {
                if (Enemy.isFacingRight)
                {
                    Right_Hand.GetComponentInChildren<Transform>().GetChild(0).rotation = Quaternion.Euler(0, 90, 0);
                }
                else
                {
                    Right_Hand.GetComponentInChildren<Transform>().GetChild(0).rotation = Quaternion.Euler(0, -90, 0);
                }
            }
        }

        if (Left_Hand != null)
        {
            GetWeaponParameters(Left_Hand);
            Left_Hand.transform.SetParent(leftHoldPoint.GetComponent<Transform>().transform);
            Left_Hand.transform.position = leftHoldPoint.position;
            if (Enemy.isFacingRight)
            {
                Left_Hand.GetComponent<SpriteRenderer>().sortingOrder = 1;
                Left_Hand.transform.rotation = leftHoldPoint.rotation * Quaternion.Euler(0, 0, -40);
            }
            else
            {
                Left_Hand.GetComponent<SpriteRenderer>().sortingOrder = 13;
                Left_Hand.transform.rotation = leftHoldPoint.rotation * Quaternion.Euler(0, 0, 65);
            }
        }
    }
    public void GetWeapon(ref GameObject hold, ref GameObject Weapon) //получить оружие
    {
        //создание клона оружия
        GameObject clone = Instantiate(Weapon, transform.position, Quaternion.identity);
        clone.name = Weapon.name;
        hold = clone; //привязать оружие к руке
        GetWeaponParameters(hold); //gолучить параметры оружия
        DirectionWeapon(clone); //изменить направление оружия
        if (isBigWeapon)
        {
            //запрет вращения и движения по оси Z
            clone.GetComponent<Rigidbody2D>().constraints = 
                RigidbodyConstraints2D.FreezePositionY | RigidbodyConstraints2D.FreezeRotation;
            Enemy_Animator.SetBool("Big", true);
            //установить анимация захвата
            Enemy_Animator.SetFloat("HoldWeapon", int.Parse(clone.name));
        } else
            Enemy_Animator.SetBool("Big", false);
    }
    private void GetWeaponParameters(GameObject hold)
    {
        Weapon Weapon = hold.GetComponent<Weapon>();
        WeaponType = Weapon.type;
        isBigWeapon = Weapon.BigWeapon;
    }
    private void DirectionWeapon(GameObject weapon)
    {
        Vector3 theScale;
        theScale = weapon.transform.localScale;
        if (Enemy.isFacingRight)
        {
            theScale = new Vector2(theScale.x, theScale.y);
            if (WeaponType != "Sword" && WeaponType != "Flamethrower") weapon.GetComponentInChildren<Transform>().GetChild(0).rotation = Quaternion.Euler(0, 0, 0);
        }
        else
        {
            theScale = new Vector2(theScale.x * -1f, theScale.y);
            if (WeaponType != "Sword" && WeaponType != "Flamethrower") weapon.GetComponentInChildren<Transform>().GetChild(0).rotation = Quaternion.Euler(0, 180, 0);
        }
        weapon.transform.localScale = theScale;
    } 
    public void Swapp()
    {
        Transform T;
        T = rightHoldPoint;
        rightHoldPoint = leftHoldPoint;
        leftHoldPoint = T;
    }
}