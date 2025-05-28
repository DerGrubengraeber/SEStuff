using ProtoBuf;
using Sandbox.Common.ObjectBuilders;
using Sandbox.Definitions;
using Sandbox.Engine.Multiplayer;
using Sandbox.Game;
using Sandbox.Game.Entities;
using Sandbox.Game.Entities.Planet;
using Sandbox.Game.GameSystems;
using Sandbox.Game.Lights;
using Sandbox.ModAPI;
using SpaceEngineers.Game.Entities.Blocks;
using SpaceEngineers.Game.ModAPI;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using VRage.Game;
using VRage.Game.Components;
using VRage.Game.Entity;
using VRage.Game.ModAPI;
using VRage.Game.ModAPI.Interfaces;
using VRage.Library.Utils;
using VRage.ModAPI;
using VRage.Utils;
using VRageMath;
using VRageRender.Lights;

namespace AsteroidBelt
{
    [MySessionComponentDescriptor(MyUpdateOrder.NoUpdate)]
    public class AsteroidFilterTorusCore : MySessionComponentBase
    {
        double _r;
        BeltConfig _config = new BeltConfig();

        public static AsteroidFilterTorusCore Instance;
        public AsteroidFilterApi AsteroidFilterApi = new AsteroidFilterApi();

        public override void LoadData()
        {
            _config.Load();
            _r = _config.radius;

            base.LoadData();
            AsteroidFilterApi.Load();
        }
        protected override void UnloadData()
        {
            base.UnloadData();

            AsteroidFilterApi.Unload();
        }

        public override void BeforeStart()
        {
            Instance = this;
            if (MyAPIGateway.Session.IsServer)
            {
                //This Blocks every asteroid spawn except those in the Torus
                AsteroidFilterApi.SetAsteroidFilter("TorusFilter", 100, (asteroid) =>
                {
                    var pos = asteroid.PositionComp.GetPosition();
                    return !Torus.IsPointInTorus(pos.X, pos.Y, pos.Z, 60000, _r);
                }, (asteroid) => { return false; });

            }
        }
    }

}

