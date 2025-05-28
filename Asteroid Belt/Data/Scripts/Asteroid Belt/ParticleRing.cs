using System;
using Sandbox.Definitions;
using Sandbox.Game;
using Sandbox.Game.Entities;
using Sandbox.ModAPI;
using VRage.Game;
using VRage.Game.Components;
using VRage.Utils;
using System.Collections.Generic;
using VRageMath;

namespace AsteroidBelt{

    [MySessionComponentDescriptor(MyUpdateOrder.BeforeSimulation)]
    public class RingSession : MySessionComponentBase{
        public static RingSession Instance;
        
        private List<Particle> _particleList;
        private List<Particle> _2ndparticleList;
        Vector3D _center;
        float _radius;
        Vector3D _normal;
        int _numPoints;
        Vector3D _cameraPos;
        static float _distance;
        static string _particleName;
        static string _2ndparticleName;
        static bool _cullnearplanets;
        static bool _enableCulling;
        BeltConfig _config = new BeltConfig();

        public override void LoadData()
        {
            _config.Load();
            //load center, radius, normal, particle name, distance, and point number from config
            //EDIT or not lol
            _center = new Vector3D(0,0,0);
            _radius = _config.radius;
            _normal = new Vector3D(0,1,0);
            _distance = _config.cullingRange;
            _numPoints = _config.particlePointNumber;
            _particleName = _config.particleName;
            _2ndparticleName = _config.secondaryParticleName;
            _cullnearplanets = _config.enableAtmosphericDaytimeCulling;
            _enableCulling = _config.enableCullingNearParticles;
            //calculate points
            _particleList = GetPointsOnCircle(_center, _radius, _normal, _numPoints, _particleName);
            _2ndparticleList = GetPointsOnCircle(_center,_radius, _normal, _numPoints/2, _2ndparticleName);
            Instance = this;

        }

        public override void UpdateBeforeSimulation()
        {
            if (MyAPIGateway.Session?.Player?.Character != null)
            {
                //if(MyAPIGateway.Session.IsServer) return;
                _cameraPos = MyAPIGateway.Session.Player.Character.GetPosition();
                //iterate over list to see if player is close
                foreach(Particle particle in _particleList){
                    if(cullornah(_cameraPos, particle.Position)){
                        particle.IsEnabled = false;
                    }
                    else
                    {
                        particle.IsEnabled = true;
                    }
                    particle.Update();
                }
                foreach (Particle particle in _2ndparticleList)
                {
                    if (cullornah(_cameraPos, particle.Position))
                    {
                        particle.IsEnabled = false;
                    }
                    else
                    {
                        particle.IsEnabled = true;
                    }
                    particle.Update();
                }
            }

        }

        protected override void UnloadData()
        {
            Instance = null;
        }

        static bool cullornah(Vector3D cameraPos, Vector3D particlepos)
        {
            //This was bad, don't use it again
            //return ((VectorDistance(particlepos, cameraPos) < _distance)||(_cullnearplanets &&(VectorDistance(MyGamePruningStructure.GetClosestPlanet(cameraPos).GetClosestSurfacePointGlobal(cameraPos), cameraPos)< 8000 && !MyVisualScriptLogicProvider.IsOnDarkSide(MyGamePruningStructure.GetClosestPlanet(cameraPos),cameraPos))));
            Sandbox.Game.Entities.MyPlanet closestPlanet = MyGamePruningStructure.GetClosestPlanet(cameraPos);
            if (closestPlanet != null)
            {

                Vector3D closestSurfacePoint = closestPlanet.GetClosestSurfacePointGlobal(cameraPos);


                return (VectorDistance(particlepos, cameraPos) < _distance) ||
                       (_cullnearplanets &&
                        (VectorDistance(closestSurfacePoint, cameraPos) < closestPlanet.AtmosphereAltitude &&
                         !MyVisualScriptLogicProvider.IsOnDarkSide(closestPlanet, cameraPos)));
            }
            else
            {
                //should prolly not make this an else statement but who the fuck would put a planet into the ring anyways
                return VectorDistance(particlepos, cameraPos) < _distance && _enableCulling;
            }


        }


        static List<Particle> GetPointsOnCircle(Vector3D center, float radius, Vector3D normal, int numPoints, string name)
        {
            List<Particle> points = new List<Particle>();
            Vector3D u, v;
            if (Vector3D.Cross(normal, Vector3D.UnitX) != Vector3D.Zero)
                u = Vector3D.Normalize(Vector3D.Cross(normal, Vector3D.UnitX));
            else
                u = Vector3D.Normalize(Vector3D.Cross(normal, Vector3D.UnitY));

            v = Vector3D.Normalize(Vector3D.Cross(normal, u));

            for (int i = 0; i < numPoints; i++)
            {
                float theta = (float)(2 * Math.PI * i / numPoints);
                Vector3D point = center + radius * (u * Math.Cos(theta) + v * Math.Sin(theta));
                Particle particle = new Particle(point, name);
                points.Add(particle);

            }
            return points;

        }

        //implementing the little I remember from LinA 1 lol
        static double VectorDistance(Vector3D u, Vector3D v){
            double deltaX = u.X - v.X;
            double deltaY = u.Y - v.Y;
            double deltaZ = u.Z - v.Z;
            return Math.Sqrt(deltaX * deltaX + deltaY * deltaY + deltaZ * deltaZ);
        }

    }

    public class Particle
    {
        MyParticleEffect _effect;
        public bool IsEnabled;
        public Vector3D Position;
        readonly string _particleName;
        private MatrixD _effectMatrix;

        public Particle(Vector3D position, string particlename){
            this.Position = position;
            IsEnabled = true;
            this._particleName = particlename;
            _effectMatrix = MatrixD.CreateTranslation(Position);
        }

        public void Update(){
            if(IsEnabled && _effect == null || _effect != null && (!_effect.Loop && (_effect.DurationMax <= _effect.GetElapsedTime()))){
                if(MyParticlesManager.TryCreateParticleEffect(_particleName, ref _effectMatrix, ref Position, uint.MaxValue, out _effect) && _effect != null) _effect.UserScale = 7500;
            }else if (!IsEnabled && _effect != null)
            {
                MyParticlesManager.RemoveParticleEffect(_effect);
                _effect = null;
            }

        }


    }
}

