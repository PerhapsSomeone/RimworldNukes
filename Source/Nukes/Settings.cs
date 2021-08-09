using HugsLib;
using HugsLib.Settings;
using Verse;

namespace Nukes
{
    public class Settings : ModBase
    {
        public bool radiationEnabled;
        public int  radiationLevel;
        public bool radiationAirburst;
        public bool customSounds;

        public override string ModIdentifier
        {
            get { return "RimworldNukes"; }
        }

        private SettingHandle<bool> radiationSystemToggle;
        private SettingHandle<int> radiationAmountLevel;
        private SettingHandle<bool> radiationAirburstToggle;
        private SettingHandle<bool> nukeSoundToggle;

        public Settings()
        {
            this.DefsLoaded();
        }

        public override void DefsLoaded()
        {
            radiationSystemToggle = Settings.GetHandle<bool>("radiationToggle", "Radiation System", "Simulate radiation poisoning.", true);
            radiationAmountLevel = Settings.GetHandle<int>("radiationLevelSetter", "Radiation Amount", "Adjust the global modifier for radiation output. (1 to 10)", 3, Validators.IntRangeValidator(1, 10));
            radiationAirburstToggle = Settings.GetHandle<bool>("radiationAirburstToggle", "Radiation Burst", "Larger nukes will give everyone one the map a slight radiation dosage. Recommended for balance.", true);
            nukeSoundToggle = Settings.GetHandle<bool>("nukeSoundToggle", "Nuke Sounds", "Play custom nuke sounds.", true);

            updateSettingValues();
        }

        public override void SettingsChanged()
        {
            base.SettingsChanged();
            updateSettingValues();
        }

        private void updateSettingValues()
        {
            this.radiationEnabled = radiationSystemToggle.Value;
            this.radiationLevel = radiationAmountLevel.Value;
            this.radiationAirburst = radiationAirburstToggle.Value;
            this.customSounds = nukeSoundToggle.Value;
        }
    }
}
