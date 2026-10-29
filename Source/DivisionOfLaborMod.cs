using System.Reflection;
using HarmonyLib;
using UnityEngine;
using Verse;

namespace DivisionOfLabor
{
    /// <summary>
    /// Loads the mod settings and applies the Work tab patches.
    /// </summary>
    public class DivisionOfLaborMod : Mod
    {
        public DivisionOfLaborMod(ModContentPack content) : base(content)
        {
            GetSettings<DivisionOfLaborSettings>();
            new Harmony(content.PackageId).PatchAll(Assembly.GetExecutingAssembly());
        }

        public override string SettingsCategory()
        {
            return Content.Name;
        }

        public override void DoSettingsWindowContents(Rect inRect)
        {
            DivisionOfLaborSettings.DoWindowContents(inRect);
        }
    }
}
