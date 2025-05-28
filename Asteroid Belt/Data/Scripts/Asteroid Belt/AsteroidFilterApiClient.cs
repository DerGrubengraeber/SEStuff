using Sandbox.ModAPI;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using VRage.ModAPI;
using VRageMath;

namespace AsteroidBelt
{
    public class AsteroidFilterApi
    {
        private bool _apiInit;

        private Func<string, int, Func<IMyVoxelBase, bool>, Func<IMyVoxelBase, bool>, bool?> _setAsteroidFilter;

        private const long Channel = 991750096348;

        public bool IsReady { get; private set; }
        public bool Compromised { get; private set; }
        private void HandleMessage(object o)
        {
            if (_apiInit) return;
            var dict = o as IReadOnlyDictionary<string, Delegate>;
            var message = o as string;

            if (message != null && message == "Compromised")
                Compromised = true;

            if (dict == null || dict is ImmutableDictionary<string, Delegate>)
                return;

            var builder = ImmutableDictionary.CreateBuilder<string, Delegate>();
            foreach (var pair in dict)
                builder.Add(pair.Key, pair.Value);

            MyAPIGateway.Utilities.SendModMessage(Channel, builder.ToImmutable());

            ApiLoad(dict);
            IsReady = true;
        }

        private bool _isRegistered;

        public bool Load()
        {
            if (!_isRegistered)
            {
                _isRegistered = true;
                MyAPIGateway.Utilities.RegisterMessageHandler(Channel, HandleMessage);
            }
            if (!IsReady)
                MyAPIGateway.Utilities.SendModMessage(Channel, "ApiEndpointRequest");
            return IsReady;
        }

        public void Unload()
        {
            if (_isRegistered)
            {
                _isRegistered = false;
                MyAPIGateway.Utilities.UnregisterMessageHandler(Channel, HandleMessage);
            }
            IsReady = false;
        }

        public void ApiLoad(IReadOnlyDictionary<string, Delegate> delegates)
        {
            _apiInit = true;

            _setAsteroidFilter = (Func<string, int, Func<IMyVoxelBase, bool>, Func<IMyVoxelBase, bool>, bool?>)delegates["SetAsteroidFilter"];
        }


        public bool? SetAsteroidFilter(string name, int priority, Func<IMyVoxelBase, bool> isZoneActive, Func<IMyVoxelBase, bool> isSpawnValid) => _setAsteroidFilter?.Invoke(name, priority, isZoneActive, isSpawnValid) ?? null;
    }
}
