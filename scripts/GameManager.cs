using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    private void Awake()
    {
        if(GameManager.instance != null)
        {
            Destroy(gameObject);
            return;
        }    
        
        instance = this;
        SceneManager.sceneLoaded += LoadState;
        DontDestroyOnLoad(gameObject); 
        
    }

    //Ressourcen
    public List<Sprite> playerSprites;
    public List<Sprite> weaponSprites;
    public List<int> weaponPrices;
    public List<int> xpTable; 

   
    public Player player;
    public Weapon weapon;
    public FloatingTextManager floatingTextManager;

    //Logik
    
    public int pesos;
    public int experience;

    //floating text
    public void ShowText(string msg, int fontSize, Color color,Vector3 position, Vector3 motion, float duration)
    {
        floatingTextManager.Show(msg, fontSize, color, position, motion, duration);


    }

    //waffe ugraden
    public bool TryUpgradeWeapon()
    {
        // waffe auf max level?
        if(weaponPrices.Count <= weapon.weaponLevel)
            return false;
            

        if(pesos >= weaponPrices[weapon.weaponLevel])
        {
            pesos -= weaponPrices[weapon.weaponLevel];
            weapon.UpgradeWeapon();
            return true;
        }

        return false;

    }

  


    //xp system
    public int GetCurrentLevel()
    {
        int r = 0;
        int add = 0;

        while (experience >= add)
        {
            add += xpTable[r];
            r++;

            if (r == xpTable.Count) // max level?
                return r;
        }

        return r;
    }
   

    public int GetXpToLevel(int level)
    {
        int r = 0;
        int xp = 0; 

        while(r < level)
        {
            xp += xpTable[r];
            r++;
        }

        return xp;
    }
    
    public void GrantXp(int xp)
    {
        int currLevel = GetCurrentLevel();
        experience += xp;
        if (currLevel < GetCurrentLevel())
            OnLevelUp();
    }

    public void GrantPesos(int pes)
    {
        pesos += pes;

        
    }


    public void OnLevelUp()
    {
        Debug.Log("Level up!");
        player.OnLevelUp();
    }



  //Spielstand speichern

    public void SaveState()
    {
        string s = "";

        s += "0" + "|";
        s += pesos.ToString() + "|"; 
        s += experience.ToString() + "|";
        s += weapon.weaponLevel.ToString();

        PlayerPrefs.SetString("SaveState", s);
    }

    //spielstand laden

    public void LoadState(Scene s, LoadSceneMode mode)
    {
        if (!PlayerPrefs.HasKey("SaveState"))
            return;

        string[] data = PlayerPrefs.GetString("SaveState").Split('|');

        // skin ändern
        pesos = int.Parse(data[1]);

        //Xp
        experience = int.Parse(data[2]);
        if(GetCurrentLevel() != 1)
            player.SetLevel(GetCurrentLevel());
        // waffen level ändern
        weapon.SetWeaponLevel(int.Parse(data[3]));

        Debug.Log("LoadState");

        
    }





}
