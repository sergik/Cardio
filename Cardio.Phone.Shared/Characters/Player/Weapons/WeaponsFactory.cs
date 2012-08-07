using System;
using System.Collections.Generic;
using Cardio.Phone.Shared.Characters.Player.Weapons.FrontWeapons;

namespace Cardio.Phone.Shared.Weapons
{
    public static class WeaponsFactory
    {
        private static Dictionary<String, Func<Weapon>> _weaponsMap = new Dictionary<string, Func<Weapon>>
                                                             {
                                                                 {"baseweapon", () =>new Weapon()},
                                                                 {"weapon1", () =>new MainWeaponLevel1()},
                                                                 {"weapon2", () =>new MainWeaponLevel2()},
                                                                 {"weapon3", () =>new MainWeaponLevel2()},
                                                                 {"weapon4", () =>new MainWeaponLevel2()},
                                                                 {"frontweapon1", () =>new FrontWeaponLevel1()},
                                                                 {"frontweapon2", () =>new FrontWeaponLevel2()}
                                                             };

        public static Weapon GetWeapon(String weaponName)
        {
            if(_weaponsMap.ContainsKey(weaponName.ToLower()))
            {
                return _weaponsMap[weaponName.ToLower()]();
            }
            return new Weapon();
        }
    }
}
