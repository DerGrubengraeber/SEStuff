using System.Collections.Generic;
using VRage.Game;
using VRage.Game.Components;
using NerdRadar.Definitions;
using Sandbox.ModAPI;
using ShipClass = VRage.MyTuple<int, string>;

namespace NerdRadar.ExampleMod
{
    [MySessionComponentDescriptor(MyUpdateOrder.NoUpdate)]
    public class Example_ShipClassificationStats : MySessionComponentBase
    {
        ClassConfig cfg => new ClassConfig()
        {
            ReplaceClasses = true, // set to true to overwrite ALL previous classes from any other mod loaded before this one
                                    // set to false to add these classes to the existing list
                                    // Weird and unintended behavior may occur when false and adding a class with exactly the same max blockcount as another

            /* Class type priority is as follows:
             - If station, then use station classes
             - If the "largest grid" of the grid group is large grid, use the large grid classes
             - Else, use small grid classes
             where the "largest grid" is determined by the grid of highest value of the (expression blockcount * grid size)
             - this means large grid blockcount counts for 5x the small grid blockcount
             
            Anyways, did I mention subgrids are a pain to deal with code wise?
            */
            LargeGridClasses = new List<ShipClass>() // list of large grid ship classes
			{
				new ShipClass(10, "Debris"),                     
				new ShipClass(100, "Drone"),                     
				new ShipClass(1000, "Corvette"),                
				new ShipClass(1800, "Frigate"),                  
				new ShipClass(3000, "Destroyer"),                
				new ShipClass(10000, "Cruiser"),                
				new ShipClass(15000, "Heavy Cruiser"),  
				new ShipClass(20000, "Battleship"),              
				new ShipClass(50000, "Carrier"),                
				new ShipClass(100000, "Dreadnought"),          
				new ShipClass(int.MaxValue, "Why..."),     
			},

			SmallGridClasses = new List<ShipClass>() // list of small grid ship classes
			{
				new ShipClass(10, "Debris"),                     
				new ShipClass(250, "Drone/Missile"),                                
				new ShipClass(1500, "Light Spacecraft"),                  
				new ShipClass(3000, "Medium Spacecraft"),                 
				new ShipClass(5000, "Heavy Spacecraft"),                
				new ShipClass(10000, "Mobile Base"),            
				new ShipClass(int.MaxValue, "Please no..."),       
			},

			StationClasses = new List<ShipClass>() // list of station classes, whether large or small grid
			{
				new ShipClass(10, "Outpost"),                   
				new ShipClass(1000, "Listening Post"),           
				new ShipClass(5000, "Mining Station"),           
				new ShipClass(20000, "Industrial Complex"),      
				new ShipClass(50000, "Fortress"),                
				new ShipClass(100000, "Shipyard"),               
				new ShipClass(int.MaxValue, "Oh God..."),       
			},
		};
        // Do not modify below this line
        public override void Init(MyObjectBuilder_SessionComponent sessionComponent)
        {
            byte[] data = MyAPIGateway.Utilities.SerializeToBinary(cfg);
            MyAPIGateway.Utilities.SendModMessage(DefConstants.MessageHandlerId, data);
        }
    }
}
