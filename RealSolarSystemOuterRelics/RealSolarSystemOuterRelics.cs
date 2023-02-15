using OWML.Common;
using OWML.ModHelper;

namespace RealSolarSystemOuterRelics
{
    public class RealSolarSystemOuterRelics : ModBehaviour
    {
        // Here is one way to create a mod Singleton
        // First you create a static variable that is an instance of your main mod class
        private static RealSolarSystemOuterRelics instance;

        // Then you create a property that references it
        public static RealSolarSystemOuterRelics Instance
        {
            get
            {
                if (instance == null) instance = FindObjectOfType<RealSolarSystemOuterRelics>();
                return instance;
            }
        }
        // In other classes, you can reference the singleton using code like this:
        // RealSolarSystemOuterRelics Main = RealSolarSystemOuterRelics.Instance;

        // You'll then want to make a field to hold the API. It does not need to be public to work, but we make it public here to reference in another class.
        public IOuterRelicsAPI orAPI;

        // We're also going to create some fields to determine which settings we have enabled
        bool closePlanets;
        bool distantPlanets;
        bool useHints;

        // Since we handle pool management in a seperate class, we'll also want a reference to it
        PoolChanger pools;

        private void Awake()
        {
            // You won't be able to access OWML's mod helper in Awake.
            // So you probably don't want to do anything here.
            // Use Start() instead.
            // This includes anything having to do with Outer Relics
        }

        private void Start()
        {
            // Starting here, you'll have access to OWML's mod helper.

            // You'll want to get the Outer Relics API early
            orAPI = ModHelper.Interaction.TryGetModApi<IOuterRelicsAPI>("GameWyrm.OuterRelics");

            // The easiest way to register your mod is to register all the files in it at once, like this:

            // orAPI.RegisterMod(this);

            // RegisterMod adds all the files from your mod into the randomization pool, including hint placements.
            // You need to specify your mod, however, so Outer Relics can keep track of it.
            // If you register in your main mod class, the "this" keyword will suffice. If using a different class, you'll need to get your mod class' singleton.
            // However, doing this provides you no control over which files are used. If you use a custom config to let the player decide where items and hints can spawn, you'll need to register files manually
            // We do this in the Configure method below

            // But before we do that, let's load our settings from the config file and register files as needed
            closePlanets = ModHelper.Config.GetSettingsValue<bool>("ClosePlanets");
            distantPlanets = ModHelper.Config.GetSettingsValue<bool>("DistantPlanets");
            useHints = ModHelper.Config.GetSettingsValue<bool>("Hints");

            // Before we can manage pools, we'll need to instantiate our pool manager
            pools = new();

            // Now let's use our pool manager to change existing pools
            if (closePlanets) pools.SetClosePlanets(true);
            if (distantPlanets) pools.SetDistantPlanets(true);
            if (useHints) pools.SetCustomHints(true);

            // You should also register the proper names of any bodies in your mod
            // Any body with a standard name will work fine
            // For example, if you have a body named "MyCoolPlanet_Body" then Outer Relics will automatically give it the name "My Cool Planet"
            // However, you may want to define a custom name
            // For example, you may have called MyCoolPlanet_Body "Super Coolio"
            // In this case, you can set its name here, like this
            // orAPI.RegisterBody("MyCoolPlanet_Body", "Super Coolio");
            // Here we do it for bodies that have special characters
            orAPI.RegisterBody("BarnardsB_Body", "Barnard's B");
            orAPI.RegisterBody("HalleysComet_Body", "Halley's Comet");
        }

        // This method runs whenever the Mod Config is edited in-game
        // Here we use it to register and unregister files as needed
        public override void Configure(IModConfig config)
        {
            if (pools != null)
            {
                if (config.GetSettingsValue<bool>("ClosePlanets") != closePlanets)
                {
                    closePlanets = config.GetSettingsValue<bool>("ClosePlanets");
                    pools.SetClosePlanets(closePlanets);
                }

                if (config.GetSettingsValue<bool>("DistantPlanets") != distantPlanets)
                {
                    distantPlanets = config.GetSettingsValue<bool>("DistantPlanets");
                    pools.SetDistantPlanets(distantPlanets);
                }
                if (config.GetSettingsValue<bool>("Hints") != useHints)
                {
                    useHints = config.GetSettingsValue<bool>("Hints");
                    pools.SetCustomHints(useHints);
                }
            }
        }

    }
}