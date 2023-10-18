﻿using Kitchen;
using KitchenData;
using KitchenLib;
using KitchenLib.Event;
using KitchenLib.References;
using KitchenMods;
using System.IO;
using System.Linq;
using System.Reflection;
using static KitchenLib.Utils.GDOUtils;
using UnityEngine;
using KitchenLib.Customs;
using KitchenLib.Utils;
using MethMod.Mains;
using KitchenLib.Logging.Exceptions;
using static UnityEngine.Rendering.VirtualTexturing.Debugging;
using UnityEngine.Networking.Types;


// Namespace should have "Kitchen" in the beginning - no
namespace MethMod
{
    public class Mod : BaseMod, IModSystem
    {
        
        public const string MOD_GUID = "com.icecreamsandwch.breakingkitchen";
        public const string MOD_NAME = "Cook Meth";
        public const string MOD_VERSION = "0.1.0";
        public const string MOD_AUTHOR = "IceCreamSandwch";
        public const string MOD_GAMEVERSION = ">=1.1.7";

        // Boolean constant whose value depends on whether you built with DEBUG or RELEASE mode, useful for testing
#if DEBUG
        public const bool DEBUG_MODE = true;
#else
        public const bool DEBUG_MODE = false;
#endif

        public static AssetBundle Bundle;

        //Game Data Objects already in the game
        public static Item Cheese => (Item)GDOUtils.GetExistingGDO(ItemReferences.Cheese);
        //GDO from the helpful "IngredientLib"
        //public static Item Garlic => (Item)GDOUtils.GetExistingGDO(IngredientLib.References.GetIngredient("garlic"));

        //processes, like how the character interacts
        public static Process Cook => (Process)GDOUtils.GetExistingGDO(ProcessReferences.Cook);
        public static Process Chop => (Process)GDOUtils.GetExistingGDO(ProcessReferences.Chop);
        public static Process Knead => (Process)GDOUtils.GetExistingGDO(ProcessReferences.Knead);

        //item, can be combined and carried around
        public static Item CookedMeth => (Item)GDOUtils.GetCustomGameDataObject<CookedMeth>().GameDataObject;
        //dish
        public static Dish MethDish => (Dish)GDOUtils.GetCustomGameDataObject<MethDish>().GameDataObject;

        public Mod() : base(MOD_GUID, MOD_NAME, MOD_AUTHOR, MOD_VERSION, $"{MOD_GAMEVERSION}", Assembly.GetExecutingAssembly())
        {
            /*
            string bundlePath = Path.Combine(new string[] { Directory.GetParent(Application.dataPath).FullName, "Mods", ModID });

            Debug.Log($"{MOD_NAME} {MOD_VERSION} {MOD_AUTHOR}: Loaded");
            Debug.Log($"Assets Loaded From {bundlePath}");*/
        }

        protected override void OnInitialise()
        {
            LogWarning($"{MOD_GUID} v{MOD_VERSION} in use!");
        }

        private void AddGameData()
        {
            LogInfo("Attempting to register game data...");

            //add the GDOs you need 
            AddGameDataObject<CookedMeth>();
            AddGameDataObject<MethDish>();

            LogInfo("Done loading game data.");
        }

        protected override void OnUpdate()
        {

        }

        protected override void OnPostActivate(KitchenMods.Mod mod)
        {
            // TODO: Uncomment the following if you have an asset bundle.
            // TODO: Also, make sure to set EnableAssetBundleDeploy to 'true' in your ModName.csproj
            LogInfo("Attempting to load asset bundle...");
            Bundle = mod.GetPacks<AssetBundleModPack>().SelectMany(e => e.AssetBundles).FirstOrDefault() ?? throw new MissingAssetBundleException(MOD_GUID);
            LogInfo("Done loading asset bundle.");

            // Register custom GDOs
            AddGameData();
        }

        #region Logging
        public static void LogInfo(string _log) { Debug.Log($"[{MOD_NAME}] " + _log); }
        public static void LogWarning(string _log) { Debug.LogWarning($"[{MOD_NAME}] " + _log); }
        public static void LogError(string _log) { Debug.LogError($"[{MOD_NAME}] " + _log); }
        public static void LogInfo(object _log) { LogInfo(_log.ToString()); }
        public static void LogWarning(object _log) { LogWarning(_log.ToString()); }
        public static void LogError(object _log) { LogError(_log.ToString()); }
        #endregion
    }
}