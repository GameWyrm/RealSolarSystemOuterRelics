using System.Collections.Generic;

namespace RealSolarSystemOuterRelics
{
    /// <summary>
    /// This class handles changing the pools available to Outer Relics
    /// </summary>
    public class PoolChanger
    {
        // First we should grab our mod's singleton
        RealSolarSystemOuterRelics main => RealSolarSystemOuterRelics.Instance;
        // This is a delegate. It tells the code that whenever the main field is referenced, to instead look at our singleton
        // For convenience, let's also make a reference to the API here
        IOuterRelicsAPI orAPI => main.orAPI;

        // Let's also create the lists that will be used the options
        // We'll need a constructor for this
        // Lists are part of System.Collections.Generic. If you don't want to use a List for whatever reason, you can use an array instead.
        List<string> closePlanets;
        List<string> distantPlanets;
        public PoolChanger()
        {
            closePlanets = new List<string>
            {
                "Earth_Body",
                "Europa_Body",
                "HalleysComet_Body",
                "Mars_Body",
                "Mercury_Body",
                "Pluto_Body",
                "TheMoon_Body",
                "Titan_Body",
                "Titania_Body",
                "Triton_Body",
                "Venus_Body"
            };
            distantPlanets = new List<string>
            {
                "BarnardsB_Body",
                "Perdition_Body",
                "ProximaB_Body"
            };
        }
        
        /// <summary>
        /// Enables or disables planets orbitting Sol
        /// </summary>
        /// <param name="enable">If true, the files will be registerd, or unregistered otherwise.</param>
        public void SetClosePlanets(bool enable)
        {
            bool success;
            if (enable)
            {
                // Here we register all of the close planets. The false is unnecessary but helps makes this clear.
                success = orAPI.TryRegisterFiles(main, closePlanets, false);
            }
            else
            {
                success = orAPI.TryUnregisterFiles(main, closePlanets, false);
            }

            // You may have noticed that TryRegisterFiles and TryUnregisterFiles are bools. They will pass true if they succeed, or false otherwise.
            // This allows you to run code based on whether they succeed if you want. This is optional and you will see that we do not bother for the distant planets.
            // This is a bit of a complex output log that simply tells us whether the list was turned on or off, but only if it succeeds
            if (success) main.ModHelper.Console.WriteLine("Toggled close planets! They are now " + (enable ? "On" : "Off"));
        }

        /// <summary>
        /// Enables or disables planets in distant solar systems
        /// </summary>
        /// <param name="enable">If true, the files will be registered, or unregisterd otherwise.</param>
        public void SetDistantPlanets(bool enable)
        {
            if (enable)
            {
                orAPI.TryRegisterFiles(main, distantPlanets, false);
            }
            else
            {
                orAPI.TryUnregisterFiles(main, distantPlanets, false);
            }
        }

        /// <summary>
        /// Enables or disables hints spawning in Real Solar System
        /// </summary>
        /// <param name="enable">If true, the hints will be enabled, or disabled otherwise.</param>
        public void SetCustomHints(bool enable)
        {
            // This function is very similar to registering placement files,
            // but you set the "isHint" variable to true, which will load the file from your Hints folder
            // We only have one hint file, so we use the singular version of the function
            if (enable)
            {
                orAPI.TryRegisterFile(main, "HintPlacements", true);
            }
            else
            {
                orAPI.TryUnregisterFile(main, "HintPlacements", true);
            }
        }
    }
}