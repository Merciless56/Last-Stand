using UnityEngine;
using UnityEngine.UI;

public class WeaponHold : MonoBehaviour
{
    public float weaponCheckRadius; //радиус поиска оружия
    public static GameObject Left_Hand_UI; //ячейка оружия левой руки
    public static GameObject Right_Hand_UI; //ячейка оружия правой руки
    private Text WeaponName; //название оружия
    private Image WeaponImage; //иконка оружия
    public static GameObject Left_Hand; //левая рука
    public static GameObject Right_Hand; //правая рука
    public Transform leftHandPoint; //кисть левой руки
    public Transform rightHandPoint; // кисть правой руки
    private bool isWeaponFound; //найден ли оружие
    public Transform weaponCheck; //точка поиска оружия
    private Collider2D Weapon_Collider; //коллайдер оружия
    public float throwingSpeed; //сила выбрасывания оружия
    public static bool isBigWeapon; //большое ли оружие
    public static string WeaponType; //тип оружия
    private Animator Player_Animator; //аниматор
    public LayerMask whatIsWeapon; //слой оружия
    public GameObject Inventory; //инвентарь
    public static bool inventoryisOpen; //открыт ли инвентарь
    private Button tempLeftButton; //левая кнопка инвентаря
    private Button tempRightButton; //правая кнопка инвентаря
    public Sprite UIMaskSprite; // спрайт инвентаря по умолчанию 
    public Text Help; //текст подсказки
    private void Start()
    {
        //инициализация объектов
        Player_Animator = GetComponent<Animator>();
        Left_Hand_UI = Inventory.GetComponentInChildren<Transform>().GetChild(0).gameObject;
        Right_Hand_UI = Inventory.GetComponentInChildren<Transform>().GetChild(1).gameObject;
        tempLeftButton = Left_Hand_UI.GetComponentInChildren<Button>();
        tempRightButton = Right_Hand_UI.GetComponentInChildren<Button>();
    }
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.X))
        {
            if (Right_Hand != null)
            {
                Weapon Weapon = Right_Hand.GetComponent<Weapon>();
                if (!Weapon.BigWeapon)
                {
                    Swap();
                }
            }
            else if (Left_Hand != null)
            {
                Swap();
            }
        }
        //поиск оружия
        isWeaponFound = Physics2D.OverlapCircle(weaponCheck.position, weaponCheckRadius, whatIsWeapon);
        if (isWeaponFound) //если оружие найдено
        {
            //отобразить подсказку
            Help.gameObject.SetActive(true);
        } else
            Help.gameObject.SetActive(false);

        if (Input.GetKeyDown(KeyCode.E))//если нажата клавиша E
        {
            //поиск оружия
            isWeaponFound = Physics2D.OverlapCircle(weaponCheck.position, weaponCheckRadius, whatIsWeapon);   
            if (isWeaponFound)//если оружие найдено
            {
                //получить коллайдер оружия
                Weapon_Collider = Physics2D.OverlapCircle(weaponCheck.position, weaponCheckRadius, whatIsWeapon); 
                GetParametersWeapon(Weapon_Collider.gameObject);//получить параметры оружия
                if (!inventoryisOpen)//если инвентарь закрыт
                {
                    //активировать кнопки инвентаря
                    tempLeftButton.interactable = true;
                    tempRightButton.interactable = true;
                    inventoryisOpen = true;

                    if (isBigWeapon) //если оружие большое
                    {
                        //отключить левую кнопку
                        tempLeftButton.interactable = false;
                    }
                }
                else
                {
                    //закрыть инвентарь
                    CloseInventory();
                }
            }
        }

        if (Right_Hand != null)//если в парвой руке имеется оружие
        {
            GetParametersWeapon(Right_Hand);
            //удочерить оружие к руке
            Right_Hand.transform.SetParent(rightHandPoint.transform);

            if (!isBigWeapon)//если оружие небольшое
            {
                //перемещать оружие к правой руке
                Right_Hand.transform.position = rightHandPoint.position;
                if (Player.isFacingRight)
                {
                    Right_Hand.GetComponent<SpriteRenderer>().sortingOrder = 13;
                    Right_Hand.transform.rotation = rightHandPoint.rotation * Quaternion.Euler(0, 0, -65);
                }
                else
                {
                    Right_Hand.GetComponent<SpriteRenderer>().sortingOrder = 1;
                    Right_Hand.transform.rotation = rightHandPoint.rotation * Quaternion.Euler(0, 0, 40);
                }
            }
            else
            {
                if (Player.isFacingRight)
                {
                    //перемещать оружие к правой руке
                    Right_Hand.transform.position = rightHandPoint.position;
                }
                else
                {
                    //перемещать оружие к левой
                    Right_Hand.transform.position = leftHandPoint.position;
                }
                //направление оружия
                Right_Hand.transform.rotation = Quaternion.Euler(0, 0, 0);
                Right_Hand.GetComponent<SpriteRenderer>().sortingOrder = 13;
            }
            if (WeaponType == "Ammo Shooting")//если тип оружия Ammo Shooting
            {
                if (Player.isFacingRight)
                {
                    //напрваление точки выстрела
                    Right_Hand.GetComponentInChildren<Transform>().GetChild(0).rotation = Quaternion.Euler(0, 0, 0);
                }
                else
                {
                    //напрваление точки выстрела
                    Right_Hand.GetComponentInChildren<Transform>().GetChild(0).rotation = Quaternion.Euler(0, 180, 0);
                }
            }
            //если тип оружия Flamethrower или Blaster
            else if (WeaponType == "Flamethrower" || WeaponType == "Blaster")
            {
                if (Player.isFacingRight)
                {
                    //напрваление точки выстрела
                    Right_Hand.GetComponentInChildren<Transform>().GetChild(0).rotation = Quaternion.Euler(0, 90, 0);
                }
                else
                {
                    //напрваление точки выстрела
                    Right_Hand.GetComponentInChildren<Transform>().GetChild(0).rotation = Quaternion.Euler(0, -90, 0);
                }
            }
        }

        if (Left_Hand != null)//если в левой руке имеется оружие
        {
            GetParametersWeapon(Left_Hand);
            //удочерить оружие к левой руке
            Left_Hand.transform.SetParent(leftHandPoint.transform);
            //перемещать оружие к левой руке
            Left_Hand.transform.position = leftHandPoint.position;
            if (Player.isFacingRight)
            {
                //изменить порядок слоя
                Left_Hand.GetComponent<SpriteRenderer>().sortingOrder = 1;
                //вращение оружия
                Left_Hand.transform.rotation = leftHandPoint.rotation * Quaternion.Euler(0, 0, -40);
            }
            else
            {
                //изменить порядок слоя
                Left_Hand.GetComponent<SpriteRenderer>().sortingOrder = 13;
                //вращение оружия
                Left_Hand.transform.rotation = leftHandPoint.rotation * Quaternion.Euler(0, 0, 65);
            }
        }
    }

    public void Take_Left_Hand()
    {
        if (Left_Hand == null) //если левой рукой ничего не держим
        {
            TakeWeapon(ref Left_Hand, Weapon_Collider); //взять предмет
            SetWeaponCell(Left_Hand_UI, Weapon_Collider); //установить иконку
        }
        else //если левой рукой держим
        {
            GetParametersWeapon(Left_Hand);
            ThrowWeapon(ref Left_Hand); //выбрасить оружие
            TakeWeapon(ref Left_Hand, Weapon_Collider); //берем новое оружие
            SetWeaponCell(Left_Hand_UI, Weapon_Collider); //устанавливаем иконку
        }

        if (Right_Hand != null)//если левой рукой держим
        {
            GetParametersWeapon(Right_Hand);//получить параметры оружия
            if (isBigWeapon)//если большое оружие
            {
                ThrowWeapon(ref Right_Hand);//выбрасить оружие
                ClearWeaponCell(ref Right_Hand_UI);//удалить иконку
            }
        }
        CloseInventory();
    }
    public void Take_Right_Hand()
    {
        if (Right_Hand == null) //если правой рукой ничего не держим
        {
            TakeWeapon(ref Right_Hand, Weapon_Collider);//взять оружие
            SetWeaponCell(Right_Hand_UI, Weapon_Collider);
        }
        else //если правой рукой держим
        {
            GetParametersWeapon(Right_Hand); //получить параметры оружия
            ThrowWeapon(ref Right_Hand); //выбрасить оружие
            TakeWeapon(ref Right_Hand, Weapon_Collider); //берем новое оружие
            SetWeaponCell(Right_Hand_UI, Weapon_Collider); //устанавливаем иконку
        }
        if (Left_Hand != null) //если левой рукой держим
        {
            GetParametersWeapon(Right_Hand);//получить параметры оружия
            if (isBigWeapon)//если большое оружие
            {
                ThrowWeapon(ref Left_Hand);//выбрасить оружие
                ClearWeaponCell(ref Left_Hand_UI);//удалить иконку
            }
        }
        CloseInventory();//закрыть инвентарь
    }
    public void ThrowWeapon(ref GameObject hand) //выбрасить оружие
    {
        //отсоединить объект от родителя
        hand.transform.SetParent(null);
        hand.layer = 12;
        Vector3 theScale;
        theScale = hand.transform.localScale;
        hand.transform.rotation = Quaternion.Euler(0, 0, 0);
        if (!Player.isFacingRight)
        {
            theScale = new Vector2(theScale.x * -1f, theScale.y);
        }
        hand.transform.localScale = theScale;
        hand.GetComponent<Rigidbody2D>().velocity = new Vector2(1, 1) * throwingSpeed;
        hand.gameObject.GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezeRotation;
        hand = null; //убрать оружие с руки
    }
    public void TakeWeapon(ref GameObject hand, Collider2D weapon) //взять оружие
    {
        //присвоить к руке оружие
        hand = weapon.gameObject;
        hand.layer = 13;
        //получить параметры оружия
        GetParametersWeapon(hand);
        //вращать оружие
        RotateWeapon(weapon.gameObject);
        if (isBigWeapon)
        {
            Player.runSpeed = 6f;//уменьшить скорость бега
            //запрет вращение и перемещение по оси Y
            weapon.gameObject.GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezePositionY | RigidbodyConstraints2D.FreezeRotation;
            Player_Animator.SetBool("Big", true);//передаать значение аниматору
            Player_Animator.SetFloat("HoldWeapon", int.Parse(weapon.gameObject.name));//передаать значение аниматору
        }
        else
        {
            Player.runSpeed = 8f;
            Player_Animator.SetBool("Big", false);
        }
    }
    private void SetWeaponCell(GameObject UI, Collider2D weapon)//установить ячейку оружия
    {
        GetParametrsCell(UI);
        Weapon Weapon = weapon.gameObject.GetComponent<Weapon>();
        WeaponName.text = Weapon.name;
        WeaponImage.sprite = Weapon.img;
        if (WeaponType == "Ammo Shooting" || WeaponType == "Blaster" || WeaponType == "Flamethrower")
        {
            UI.GetComponentInChildren<Transform>().GetChild(2).gameObject.SetActive(true);
            UI.GetComponentInChildren<Transform>().GetChild(2).GetComponentInChildren<Text>().text = 
                Weapon.curAmmo + "/" + Weapon.allAmmo;
        }
        else
        {
            UI.GetComponentInChildren<Transform>().GetChild(2).gameObject.SetActive(false);
        }
    }
    private void ClearWeaponCell(ref GameObject handUI)//очистить ячейку оружия
    {
        handUI.GetComponentInChildren<Text>().text = "";
        handUI.GetComponentInChildren<Image>().sprite = UIMaskSprite;
        handUI.GetComponentInChildren<Transform>().GetChild(2).gameObject.SetActive(false);
    }
    private void CloseInventory()//закрыть инвентарь
    {
        tempLeftButton.interactable = false;
        tempRightButton.interactable = false;
        inventoryisOpen = false;
        //установить размер правой ячейки инвентаря 
        if (Right_Hand != null)
        {
            GetParametersWeapon(Right_Hand);
           
            if (isBigWeapon) //если оружия большое
            {
                //увеличиваем размер правой ячейки
                Right_Hand_UI.GetComponent<RectTransform>().sizeDelta = new Vector2(100, 50);
                Inventory.GetComponent<RectTransform>().sizeDelta = new Vector2(165, 78);
            }
            else
            {
                //уменьшаем размер правой ячейки
                Right_Hand_UI.GetComponent<RectTransform>().sizeDelta = new Vector2(50, 50);
                Inventory.GetComponent<RectTransform>().sizeDelta = new Vector2(115, 78);
            }
        }
        else
        {
            //уменьшаем размер правой ячейки
            Right_Hand_UI.GetComponent<RectTransform>().sizeDelta = new Vector2(50, 50);
            Inventory.GetComponent<RectTransform>().sizeDelta = new Vector2(115, 78);
        }
    }
    private void GetParametersWeapon(GameObject hand)
    {
        Weapon Weapon = hand.GetComponent<Weapon>();
        WeaponType = Weapon.type;
        isBigWeapon = Weapon.BigWeapon;
    }
    private void RotateWeapon(GameObject weapon)//сменить направление оружия
    {
        Vector3 theScale;
        theScale = weapon.transform.localScale;
        if (Player.isFacingRight)
        {
            theScale = new Vector2(theScale.x, theScale.y);
            if (WeaponType != "Sword" && WeaponType != "Flamethrower")
            {
                //напрваление точки выстрела
                weapon.GetComponentInChildren<Transform>().GetChild(0).rotation = Quaternion.Euler(0, 0, 0);
            }
        }
        else
        {
            theScale = new Vector2(theScale.x * -1f, theScale.y);
            if (WeaponType != "Sword" && WeaponType != "Flamethrower")
            {
                //напрваление точки выстрела
                weapon.GetComponentInChildren<Transform>().GetChild(0).rotation = Quaternion.Euler(0, 180, 0);
            }
        }
        weapon.transform.localScale = theScale;
    }
    private void Swap()//поменять местами оружия 
    {
        GameObject Swap = Right_Hand;
        Right_Hand = Left_Hand;
        Left_Hand = Swap;
        GetParametrsCell(Right_Hand_UI);
        string name = WeaponName.text;
        Sprite img = WeaponImage.sprite;
        WeaponName.text = Left_Hand_UI.GetComponentInChildren<Transform>().GetChild(0).GetComponentInChildren<Text>().text;
        WeaponImage.sprite = Left_Hand_UI.GetComponentInChildren<Image>().sprite;
        GetParametrsCell(Left_Hand_UI);
        WeaponName.text = name;
        WeaponImage.sprite = img;
    }
    public void SwapPoint()//поменять местами точки захвата   
    {
        Transform Point;
        Point = rightHandPoint;
        rightHandPoint = leftHandPoint;
        leftHandPoint = Point;
    }
    public void GetParametrsCell(GameObject UI)//получить параметры оружия
    {
        WeaponName = UI.GetComponentInChildren<Transform>().GetChild(0).GetComponentInChildren<Text>();
        WeaponImage = UI.GetComponentInChildren<Image>();
    }
}