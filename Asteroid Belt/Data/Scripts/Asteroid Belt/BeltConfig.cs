using ProtoBuf;
using Sandbox.ModAPI;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VRage.Game.ModAPI.Ingame.Utilities;
using VRageMath;


namespace AsteroidBelt
{
    //[ProtoContract(UseProtoMembersOnly = true)]
    public class BeltConfig
    {
        //came back to this project. What is this? Redoing with Digi's example.
        /*
        [ProtoMember(1)]
        public int MinorRadius = 3000000;
        [ProtoMember(2)]
        public int MajorRadius = 1000;
        [ProtoMember(3)]
        public Vector3D Center = new Vector3D(0,0,0);
        [ProtoMember(4)]
        public int ParticleScale = 150;
        [ProtoMember(5)]
        public String ParticleName = "pr_test";
        [ProtoMember(6)]
        public int MinimumParticleViewDistance = 10000;
        [ProtoMember(7)]
        public int NumberOfParticles = 200;
        */

        //source https://github.com/THDigi/SE-ModScript-Examples/blob/master/Data/Scripts/Examples/Example_ServerConfig_Basic.cs
        const string VariableId = nameof(BeltConfig);
        const string FileName = "Config.ini";
        const string IniSection = "Config";

        //values
        public bool enableParticles = true;
        public bool enableAsteroids = true;
        //public Vector3D center = new Vector3D(0,0,0);
        public float radius = 3000000;
        //public Vector3D normal = new Vector3D(0,1,0);
        public bool enableCullingNearParticles = true;
        public float cullingRange = 20000;
        public int particlePointNumber = 500;
        public string particleName = "pr_test";
        public string secondaryParticleName = "pr_test2";
        public bool enableAtmosphericDaytimeCulling = true;


        void LoadConfig(MyIni iniParser)
        {
            enableParticles = iniParser.Get(IniSection, nameof(enableParticles)).ToBoolean(enableParticles);
            enableAsteroids = iniParser.Get(IniSection, nameof(enableAsteroids)).ToBoolean(enableAsteroids);
            radius = iniParser.Get(IniSection, nameof(radius)).ToSingle(radius);
            enableCullingNearParticles = iniParser.Get(IniSection, nameof(enableCullingNearParticles)).ToBoolean(enableCullingNearParticles);
            cullingRange = iniParser.Get(IniSection, nameof(cullingRange)).ToSingle(cullingRange);
            particlePointNumber = iniParser.Get(IniSection, nameof(particlePointNumber)).ToInt32(particlePointNumber);
            particleName = iniParser.Get(IniSection, nameof(particleName)).ToString(particleName);
            secondaryParticleName = iniParser.Get(IniSection, nameof(secondaryParticleName)).ToString(secondaryParticleName);
            enableAtmosphericDaytimeCulling = iniParser.Get(IniSection, nameof(enableAtmosphericDaytimeCulling)).ToBoolean(enableAtmosphericDaytimeCulling);
        }

        void SaveConfig(MyIni iniParser)
        {
            iniParser.Set(IniSection, nameof(enableParticles), enableParticles);
            iniParser.SetComment(IniSection, nameof(enableParticles), "Enables/Disables the particles");

            iniParser.Set(IniSection, nameof(enableAsteroids), enableAsteroids);
            iniParser.SetComment(IniSection, nameof(enableAsteroids), "Enables/Disables Asteroid Filtering");

            iniParser.Set(IniSection, nameof(radius), radius);
            iniParser.SetComment(IniSection, nameof(radius), "Sets the radius of the ring");

            iniParser.Set(IniSection, nameof(enableCullingNearParticles), enableCullingNearParticles);
            iniParser.SetComment(IniSection, nameof(enableCullingNearParticles), "Enables/Disables particles being culled when players are close to them");

            iniParser.Set(IniSection, nameof(cullingRange), cullingRange);
            iniParser.SetComment(IniSection, nameof(cullingRange), "Sets the culling range");

            iniParser.Set(IniSection, nameof(particlePointNumber), particlePointNumber);
            iniParser.SetComment(IniSection, nameof(particlePointNumber), "Amount of particles");

            iniParser.Set(IniSection, nameof(particleName), particleName);
            iniParser.SetComment(IniSection, nameof(particleName), "Filename (without extension) of the primary particle");

            iniParser.Set(IniSection, nameof(secondaryParticleName), secondaryParticleName);
            iniParser.SetComment(IniSection, nameof(secondaryParticleName), "Filename (without extension) of the secondary particle");

            iniParser.Set(IniSection, nameof(enableAtmosphericDaytimeCulling), enableAtmosphericDaytimeCulling);
            iniParser.SetComment(IniSection, nameof(enableAtmosphericDaytimeCulling), "Enables/Disables particles being culled when players are inside of a planets atmosphere and it's daytime (prevents black streaks in the sky)");
        }


        public BeltConfig()
        {
        }

        public void Load()
        {
            if (MyAPIGateway.Session.IsServer)
                LoadOnHost();
            else
                LoadOnClient();
        }

        void LoadOnHost()
        {
            MyIni iniParser = new MyIni();

            // load file if exists then save it regardless so that it can be sanitized and updated

            if (MyAPIGateway.Utilities.FileExistsInWorldStorage(FileName, typeof(BeltConfig)))
            {
                using (TextReader file = MyAPIGateway.Utilities.ReadFileInWorldStorage(FileName, typeof(BeltConfig)))
                {
                    string text = file.ReadToEnd();

                    MyIniParseResult result;
                    if (!iniParser.TryParse(text, out result))
                        throw new Exception($"Config error: {result.ToString()}");

                    LoadConfig(iniParser);
                }
            }

            iniParser.Clear(); // remove any existing settings that might no longer exist

            SaveConfig(iniParser);

            string saveText = iniParser.ToString();

            MyAPIGateway.Utilities.SetVariable<string>(VariableId, saveText);

            using (TextWriter file = MyAPIGateway.Utilities.WriteFileInWorldStorage(FileName, typeof(BeltConfig)))
            {
                file.Write(saveText);
            }
        }

        void LoadOnClient()
        {
            string text;
            if (!MyAPIGateway.Utilities.GetVariable<string>(VariableId, out text))
                throw new Exception("No config found in sandbox.sbc!");

            MyIni iniParser = new MyIni();
            MyIniParseResult result;
            if (!iniParser.TryParse(text, out result))
                throw new Exception($"Config error: {result.ToString()}");

            LoadConfig(iniParser);
        }

    }
}
