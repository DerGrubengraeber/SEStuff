using System.Collections.Generic;
using VRage.Game;
using VRage.Game.Components;
using NerdRadar.Definitions;
using Sandbox.ModAPI;
using VRageMath;

namespace NerdRadar.ExampleMod
{
    [MySessionComponentDescriptor(MyUpdateOrder.NoUpdate)]
    public class Example_EWARStats : MySessionComponentBase
    {
        BlockConfig cfg => new BlockConfig()
        {
            // Feel free to change the Example_EWARStats to something else, along with the namespace (NerdRadar.ExampleMod)
            // Aside from those, do not change anything above this line

            // Stats for all the radar blocks. How stats interact can be found here http://nebfltcom.wikidot.com/mechanics:radar
            // RCS is calculated via (LG blockcount * 0.25)^2*2.5+(SG blockcount * 0.25)^2*0.01+(Generated power in MW) * 0.5
            RadarStats = new Dictionary<string, RadarStat>()
            {
                ["LG_dergrubengraeber_Huntress"] = new RadarStat()
                {
                    MaxRadiatedPower = 4000, // radiated power of the radar, in kilowatts
                    Gain = 40, // gain of the radar, in decibels. Must be above 0
                    Sensitivity = -38, // sensitivity of the radar, in decibels
                    MaxSearchRange = 9000, // maximum range radar will return targets, regardless of other settings, in meters

                    ApertureSize = 60, // aperture size of the radar, in meters^2
                    NoiseFilter = 0, // noise filter of the radar, in decibels
                    SignalToNoiseRatio = 1, // ratio of return signal to noise required for the radar to detect targets

                    PositionError = 45, // maximum error of position in any given direction the radar returns in meters
                    VelocityError = 5, // maximum error in the velocity vector in any given direction the radar returns (velocity indicator coming soonTM)

                    CanTargetLock = false, // determines whether or not the radar can lock. Locked targets have no velocity and position error, and will have the radar detected icon turn red from yellow.
                                          // more functionality coming soonTM
                    LOSCheckIncludesParentGrid = false,
                                          // determines whether the radar's LOS check will include the grid it is on. Useful for radar paneling and such. Subgrids attached to the main grid count as the main grid in this case.
                    StealthMultiplier = 1, // if the target is cloaked via the Stealth Drive mod (https://steamcommunity.com/sharedfiles/filedetails/?id=2805859069), then the target's RCS is multiplied by this when the radar scans.

                    CanDetectAllJumps = false, // Determines if the radar can detect and show any jumps caused by any track visible to the radar.
                    CanDetectLockedJumps = false, // Determines if the radar can detect and show any jumps caused by the tracked locked by the radar.
                },
                // other radar examples below, copied from Nerd's mod that uses this
                ["LG_dergrubengraeber_Ithaca"] = new RadarStat()
                {
                    MaxRadiatedPower = 3100,
                    Gain = 40,
                    Sensitivity = -34,
                    MaxSearchRange = 8500,

                    ApertureSize = 60,
                    NoiseFilter = 0,
                    SignalToNoiseRatio = 1,

                    PositionError = 20,
                    VelocityError = 5,

                    CanTargetLock = false,
                    LOSCheckIncludesParentGrid = true,

                    StealthMultiplier = 0,

                    CanDetectAllJumps = false,
                    CanDetectLockedJumps = false,
                },
				["SG_dergrubengraeber_Huntress"] = new RadarStat()
                {
                    MaxRadiatedPower = 4000, // radiated power of the radar, in kilowatts
                    Gain = 40, // gain of the radar, in decibels. Must be above 0
                    Sensitivity = -38, // sensitivity of the radar, in decibels
                    MaxSearchRange = 5000, // maximum range radar will return targets, regardless of other settings, in meters

                    ApertureSize = 60, // aperture size of the radar, in meters^2
                    NoiseFilter = 0, // noise filter of the radar, in decibels
                    SignalToNoiseRatio = 1, // ratio of return signal to noise required for the radar to detect targets

                    PositionError = 45, // maximum error of position in any given direction the radar returns in meters
                    VelocityError = 5, // maximum error in the velocity vector in any given direction the radar returns (velocity indicator coming soonTM)

                    CanTargetLock = false, // determines whether or not the radar can lock. Locked targets have no velocity and position error, and will have the radar detected icon turn red from yellow.
                                          // more functionality coming soonTM
                    LOSCheckIncludesParentGrid = false,
                                          // determines whether the radar's LOS check will include the grid it is on. Useful for radar paneling and such. Subgrids attached to the main grid count as the main grid in this case.
                    StealthMultiplier = 1, // if the target is cloaked via the Stealth Drive mod (https://steamcommunity.com/sharedfiles/filedetails/?id=2805859069), then the target's RCS is multiplied by this when the radar scans.

                    CanDetectAllJumps = false, // Determines if the radar can detect and show any jumps caused by any track visible to the radar.
                    CanDetectLockedJumps = false, // Determines if the radar can detect and show any jumps caused by the tracked locked by the radar.
                },
                // other radar examples below, copied from Nerd's mod that uses this
                ["SG_dergrubengraeber_Ithaca"] = new RadarStat()
                {
                    MaxRadiatedPower = 3100,
                    Gain = 40,
                    Sensitivity = -34,
                    MaxSearchRange = 4500,

                    ApertureSize = 60,
                    NoiseFilter = 0,
                    SignalToNoiseRatio = 1,

                    PositionError = 20,
                    VelocityError = 5,

                    CanTargetLock = false,
                    LOSCheckIncludesParentGrid = true,

                    StealthMultiplier = 0,

                    CanDetectAllJumps = false,
                    CanDetectLockedJumps = false,
                },
                ["dergrubengraeber_RadarStation"] = new RadarStat()
                {
                    MaxRadiatedPower = 4500,
                    Gain = 50,
                    Sensitivity = -36,
                    MaxSearchRange = 40000,

                    ApertureSize = 250,
                    NoiseFilter = 0,
                    SignalToNoiseRatio = 1,

                    PositionError = 150,
                    VelocityError = 1,

                    CanTargetLock = false,
                    LOSCheckIncludesParentGrid = true,

                    StealthMultiplier = 0,

                    CanDetectAllJumps = false,
                    CanDetectLockedJumps = false,
                },
                ["LG_dergrubengraeber_Parallax_Flat"] = new RadarStat() 
                {
                    MaxRadiatedPower = 4100,
                    Gain = 50,
                    Sensitivity = -38.5,
                    MaxSearchRange = 9000,

                    ApertureSize = 30,
                    NoiseFilter = 0,
                    SignalToNoiseRatio = 1,

                    PositionError = 30,
                    VelocityError = 1,

                    CanTargetLock = true,
                    LOSCheckIncludesParentGrid = true,

                    StealthMultiplier = 1,

                    CanDetectAllJumps = false,
                    CanDetectLockedJumps = true,
                },
				["LG_dergrubengraeber_Parallax_Sloped"] = new RadarStat() 
                {
                    MaxRadiatedPower = 4100,
                    Gain = 50,
                    Sensitivity = -38.5,
                    MaxSearchRange = 9000,

                    ApertureSize = 30,
                    NoiseFilter = 0,
                    SignalToNoiseRatio = 1,

                    PositionError = 30,
                    VelocityError = 1,

                    CanTargetLock = true,
                    LOSCheckIncludesParentGrid = true,

                    StealthMultiplier = 1,

                    CanDetectAllJumps = false,
                    CanDetectLockedJumps = true,
                },
				["SG_dergrubengraeber_Parallax_Flat"] = new RadarStat() 
                {
                    MaxRadiatedPower = 4100,
                    Gain = 50,
                    Sensitivity = -38.5,
                    MaxSearchRange = 5500,

                    ApertureSize = 30,
                    NoiseFilter = 0,
                    SignalToNoiseRatio = 1,

                    PositionError = 30,
                    VelocityError = 1,

                    CanTargetLock = true,
                    LOSCheckIncludesParentGrid = true,

                    StealthMultiplier = 1,

                    CanDetectAllJumps = false,
                    CanDetectLockedJumps = true,
                },
				["SG_dergrubengraeber_Parallax_Sloped"] = new RadarStat() 
                {
                    MaxRadiatedPower = 4100,
                    Gain = 50,
                    Sensitivity = -38.5,
                    MaxSearchRange = 5500,

                    ApertureSize = 30,
                    NoiseFilter = 0,
                    SignalToNoiseRatio = 1,

                    PositionError = 30,
                    VelocityError = 1,

                    CanTargetLock = true,
                    LOSCheckIncludesParentGrid = true,

                    StealthMultiplier = 1,

                    CanDetectAllJumps = false,
                    CanDetectLockedJumps = true,
                },
				["LG_dergrubengraeber_Frontline_Flat"] = new RadarStat() 
                {
                    MaxRadiatedPower = 3500,
                    Gain = 40,
                    Sensitivity = -38.5,
                    MaxSearchRange = 8000,

                    ApertureSize = 30,
                    NoiseFilter = 0,
                    SignalToNoiseRatio = 1,

                    PositionError = 30,
                    VelocityError = 1,

                    CanTargetLock = false,
                    LOSCheckIncludesParentGrid = true,

                    StealthMultiplier = 1,

                    CanDetectAllJumps = false,
                    CanDetectLockedJumps = true,
                },
				["LG_dergrubengraeber_Frontline_Sloped"] = new RadarStat() 
                {
                    MaxRadiatedPower = 3500,
                    Gain = 50,
                    Sensitivity = -38.5,
                    MaxSearchRange = 8000,

                    ApertureSize = 30,
                    NoiseFilter = 0,
                    SignalToNoiseRatio = 1,

                    PositionError = 30,
                    VelocityError = 1,

                    CanTargetLock = false,
                    LOSCheckIncludesParentGrid = true,

                    StealthMultiplier = 1,

                    CanDetectAllJumps = false,
                    CanDetectLockedJumps = true,
                },
				["SG_dergrubengraeber_Frontline_Flat"] = new RadarStat() 
                {
                    MaxRadiatedPower = 3500,
                    Gain = 40,
                    Sensitivity = -38.5,
                    MaxSearchRange = 4000,

                    ApertureSize = 30,
                    NoiseFilter = 0,
                    SignalToNoiseRatio = 1,

                    PositionError = 30,
                    VelocityError = 1,

                    CanTargetLock = false,
                    LOSCheckIncludesParentGrid = true,

                    StealthMultiplier = 1,

                    CanDetectAllJumps = false,
                    CanDetectLockedJumps = true,
                },
				["SG_dergrubengraeber_Frontline_Sloped"] = new RadarStat() 
                {
                    MaxRadiatedPower = 3500,
                    Gain = 50,
                    Sensitivity = -38.5,
                    MaxSearchRange = 4000,

                    ApertureSize = 30,
                    NoiseFilter = 0,
                    SignalToNoiseRatio = 1,

                    PositionError = 30,
                    VelocityError = 1,

                    CanTargetLock = false,
                    LOSCheckIncludesParentGrid = true,

                    StealthMultiplier = 1,

                    CanDetectAllJumps = false,
                    CanDetectLockedJumps = true,
                },
				["LG_dergrubengraeber_Spyglass_Sloped"] = new RadarStat() 
                {
                    MaxRadiatedPower = 4500,
                    Gain = 60,
                    Sensitivity = -38.5,
                    MaxSearchRange = 11500,

                    ApertureSize = 40,
                    NoiseFilter = 0,
                    SignalToNoiseRatio = 1,

                    PositionError = 60,
                    VelocityError = 1,

                    CanTargetLock = false,
                    LOSCheckIncludesParentGrid = true,

                    StealthMultiplier = 1,

                    CanDetectAllJumps = false,
                    CanDetectLockedJumps = false,
                },
				["LG_dergrubengraeber_Spyglass_Flat"] = new RadarStat() 
                {
                    MaxRadiatedPower = 4500,
                    Gain = 60,
                    Sensitivity = -38.5,
                    MaxSearchRange = 11500,

                    ApertureSize = 40,
                    NoiseFilter = 0,
                    SignalToNoiseRatio = 1,

                    PositionError = 60,
                    VelocityError = 1,

                    CanTargetLock = false,
                    LOSCheckIncludesParentGrid = true,

                    StealthMultiplier = 1,

                    CanDetectAllJumps = false,
                    CanDetectLockedJumps = false,
                },
				["SG_dergrubengraeber_Spyglass_Sloped"] = new RadarStat() 
                {
                    MaxRadiatedPower = 4500,
                    Gain = 60,
                    Sensitivity = -38.5,
                    MaxSearchRange = 8000,

                    ApertureSize = 40,
                    NoiseFilter = 0,
                    SignalToNoiseRatio = 1,

                    PositionError = 60,
                    VelocityError = 1,

                    CanTargetLock = false,
                    LOSCheckIncludesParentGrid = true,

                    StealthMultiplier = 1,

                    CanDetectAllJumps = false,
                    CanDetectLockedJumps = false,
                },
				["SG_dergrubengraeber_Spyglass_Flat"] = new RadarStat() 
                {
                    MaxRadiatedPower = 4500,
                    Gain = 60,
                    Sensitivity = -38.5,
                    MaxSearchRange = 8000,

                    ApertureSize = 40,
                    NoiseFilter = 0,
                    SignalToNoiseRatio = 1,

                    PositionError = 60,
                    VelocityError = 1,

                    CanTargetLock = false,
                    LOSCheckIncludesParentGrid = true,

                    StealthMultiplier = 1,

                    CanDetectAllJumps = false,
                    CanDetectLockedJumps = false,
                },
				["LG_dergrubengraeber_SphereRadar"] = new RadarStat() 
                {
                    MaxRadiatedPower = 5000,
                    Gain = 70,
                    Sensitivity = -34,
                    MaxSearchRange = 11500,

                    ApertureSize = 100,
                    NoiseFilter = 0,
                    SignalToNoiseRatio = 1,

                    PositionError = 30,
                    VelocityError = 1,

                    CanTargetLock = true,
                    LOSCheckIncludesParentGrid = true,

                    StealthMultiplier = 1,

                    CanDetectAllJumps = true,
                    CanDetectLockedJumps = true,
                },
				["SG_dergrubengraeber_SphereRadar"] = new RadarStat() 
                {
                    MaxRadiatedPower = 5000,
                    Gain = 70,
                    Sensitivity = -34,
                    MaxSearchRange = 8000,

                    ApertureSize = 100,
                    NoiseFilter = 0,
                    SignalToNoiseRatio = 1,

                    PositionError = 30,
                    VelocityError = 1,

                    CanTargetLock = true,
                    LOSCheckIncludesParentGrid = true,

                    StealthMultiplier = 1,

                    CanDetectAllJumps = false,
                    CanDetectLockedJumps = true,
                },
            },
            // stats for all the jammer blocks
            // stat interactions can be found here http://nebfltcom.wikidot.com/mechanics:electronic-warfare
            JammerStats = new Dictionary<string, JammerStat>()
            {
                
            },

            UpgradeBlockStats = new Dictionary<string, UpgradeBlockStat>()
            {
                ["Example_UpgradeBlockStats"] = new UpgradeBlockStat()
                {
                    // these two are incompatible with eachother.
                    ApplyOnlyWhenFiring = false, // WEAPON BLOCKS ONLY. Makes all of the addons/multipliers apply only when the weapon is firing.
                    ApplyOnlyWhenOn = false, // FUNCTIONAL BLOCKS ONLY. Makes all of the addons/multipliers apply only if the block is functional.

                    PositionalErrorMultiplier = 1, // Positional error multiplier for ALL radars on the grid this is mounted on. Multipliers are calculated BEFORE addons.
                    PositionalErrorAddon = 0, // Positional error addon for ALL radars on the grid this is mounted on. Addons are calculated AFTER multipliers.

                    VelocityErrorMultiplier = 1, // Velocity error multiplier for ALL radars on the grid this is mounted on. Multipliers are calculated BEFORE addons.
                    VelocityErrorAddon = 0, // Velocity error addon for ALL radars on the grid this is mounted on. Addons are calculated AFTER multipliers.

                    NoiseFilterMultiplier = 1, // Noise filter multiplier for ALL radars on the grid this is mounted on. Multipliers are calculated BEFORE addons.
                    NoiseFilterAddon = 0, // Noise filter addon for ALL radars on the grid this is mounted on. Addons are calculated AFTER multipliers.

                    RCSMultiplier = 1, // RCS multiplier for the grid this is mounted on. Multipliers are calculated BEFORE addons.
                    RCSAddon = 0, //  RCS addon for the grid this is mounted on. Addons are calculated AFTER multipliers.

                    SensitivityMultiplier = 1, // Sensitivity multiplier for ALL radars on the grid this is mounted on. Multipliers are calculated BEFORE addons.
                    SensitivityAddon = 0, // Sensitivity addon for ALL radars on the grid this is mounted on. Addons are calculated AFTER multipliers.
                }
            },

            // IFF beacons are vanilla beacons which when placed on a grid, will replace the ship name given on its radar track with whatever its HUD name is set to.
            IFFBlockStats = new Dictionary<string, IFFBlockStat>()
            {
                ["LG_dergrubengreaber_IFF"] = new IFFBlockStat()
                {
                    MaxCharacters = 30, // maximum characters the IFF beacon will use
                    ShowClass = true, // whether or not the IFF beacon name change will completely replace (false) or only add its name after the class name (true)
                },
				["SG_dergrubengreaber_IFF"] = new IFFBlockStat()
                {
                    MaxCharacters = 30, // maximum characters the IFF beacon will use
                    ShowClass = true, // whether or not the IFF beacon name change will completely replace (false) or only add its name after the class name (true)
                },
            }
        };
        // Do not touch below here
        public override void Init(MyObjectBuilder_SessionComponent sessionComponent)
        {
            byte[] data = MyAPIGateway.Utilities.SerializeToBinary(cfg);
            MyAPIGateway.Utilities.SendModMessage(DefConstants.MessageHandlerId, data);
        }
    }
}
