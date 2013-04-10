// AUTHOR: Russell Asher 
//         Kenneth Ordona 
//          
// FILENAME: Driver.cs
// DATE: 2/5/2012
// REVISION HISTORY: 1.0
// PLATFORM (Microsoft Visual Studio 10)

//This Class Starts the application which simulates creatures battling on a 2d grid and outputs their energy
//stats to a file named WriteText.  Note you must change the path of the file (noted in code below)

//Each creature randomly moves during a cycle and then randomly interacts with other creatures in its area
//This is all done within GameWorld.cs class

//Descriptions of creatures special abilities or how the simulation works can be found in their respected classes


using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace P3
{
    class Driver
    {
        //This is the main method which sets up the game world and adds creatures to it

        static void Main(string[] args)
        {
            //Simulation Variables
            int gameBoardSize = 5; //The size of the world grid (EX: 5 would give you a 5 by 5 grid world)
            CreatureDataTracker tracker;  //Initilizes a tracker object to use for reporting after the world simulation
            CreatureWorld world = new CreatureWorld(gameBoardSize);  //Sets up a world  for a max
            int predNumber = 3;    //Number of preditor creatures to put into the world
            int agileNumber = 100; // Number of agile creatures to put into the world
            int cycles=25;          //Number of simulation cycles to run
            string output="";
            int cycleReport = 5;      //how often to report on the data tracker (dont in cycles) 


            //adds predetor creatures to the game world and ranking list
            for(int i = 0 ; i < predNumber; i++)
            world.addCreature(new PredatorCreature());
            //adds agile creatures to the game world
            for(int i = 0 ; i < agileNumber; i++)
            world.addCreature(new AgileCreature());
            //runs a for loop on the update funtction inside of world.  causing a creature simulation for as 
            //many cycles as you have set
            for (int i = 0; i < cycles; i++)
            {
                world.update();
                if (i % cycleReport == 0)
                {
                    //grabs the data tracker from world
                    tracker = world.giveTracker();

                    //formats it to a string
                    output = output + "Cycle Number " + i +": Median: " + tracker.getMedian() + " Mean: " + tracker.getMean() + " Min: " + tracker.getMin()
                        + " Max: " + tracker.getMax() + "\r\n";
                }
            }
            //Creates a file containing the above created string
            System.IO.File.WriteAllText(@"WriteText.txt", output); //OUTPUT PATH!!!!


        }
    }
}
