using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class TestGame : MonoBehaviour
{

    public Stickman[] warrior;
    public Enemy[] Ewarrior;
    void Start()
    {
        //List<WeaponDataConfig> list = new List<WeaponDataConfig>();
        //WeaponDataConfig weaponDataConfig = new WeaponDataConfig();
        //weaponDataConfig.bulletPrefabName = "Bullet_Warrior";
        //weaponDataConfig.damage = 5;
        //weaponDataConfig.spreadAmount = 1;
        //StickmanDataConfig stickmanDataConfig = new StickmanDataConfig();
        //list.Add(weaponDataConfig);
        //for (int i = 0; i < warrior.Length; i++) {
        //    warrior[i].InitStickman(stickmanDataConfig);
        //    warrior[i].RegisterInScene();
        //    warrior[i].InitWeapon(list);
        //    warrior[i].Fight();
        //}

        for (int i = 0; i < Ewarrior.Length; i++) {
            Ewarrior[i].BattleCry();
            Ewarrior[i].RegisterInScene();
            Ewarrior[i].SetTeam(IngameTeam.Team2);
            Ewarrior[i].InitEnemy();
            Ewarrior[i].Fight();
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
