// AUTHOR: Russell Asher 
//         Kenneth Ordona 
//          
// FILENAME: CreatureWorld.cs
// DATE: 2/5/2012
// REVISION HISTORY: 1.0
// PLATFORM (Microsoft Visual Studio 10)

/* IMPLEMENTATION INVARIANTS

 * We created a 2d array which holds Creature Lists
 * when update is called all alive creatures are called to update themselves
 * creatures in the same creaturelist (grid area)  interact with each other on a really basic fighting sim
 * agile creatures also randomly are given food
 * 
 * creatures are added into the game world and their positions are updated in a wraping mannor to fit
 * the grid
 * 
 * What really needs tweaking is our battling system but I believe we've set up an interesting framework that
 * would allow us to make some interesting simulations if we spent some more time on making the creatures interactions more
 * ballanced
 * 
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace P3
{
    class CreatureWorld
    {
        static int daysPassed = 0;
        List<GameCreature>[][] world;  //Initilizes a 3d array representing a game world for creatures to be in 
        List<GameCreature> rankingList = new List<GameCreature>();   //Used to keep track of energy rankings
        Random randGen = new Random(); //Initilizes a new random number generator
        int diceRoll;  //integer used to produce random numbers with the random number generator above
        CreatureDataTracker tracker = new CreatureDataTracker();
        int amountToFeed = 50;  //amount to feed agile creatures if they find food.  amount represents energry levels
        int probFeed = 25;  //probability out of 100 that a agile creature will find food
        string output = "";

        // AUTHOR: Russell Asher 
        //         Kenneth Ordona 
        // DATE: Feb 1st 2012
        // Description: Creates a game world using a series of nested arrays which mimic a coordinate grid of "size"
        // preconditions: NONE
        // postconditions: NONE

        public CreatureWorld(int size)
        {

            world = new List<GameCreature>[size][];  //sets up an array of arrays which hold a List of Game Creatures
            for (int x = 0; x < world.Length; x++)
            {
                world[x] = new List<GameCreature>[size];
                for (int y = 0; y < world[x].Length; y++)
                {
                    world[x][y] = new List<GameCreature>();
                }
            }

        }


        // AUTHOR: Russell Asher 
        //         Kenneth Ordona 
        // DATE: Feb 1st 2012
        // Description: Adds a creature to the 2D array gameworld
        // preconditions: NONE
        // postconditions: The creatures positions will be modified to fit inside the game world

        public void addCreature(GameCreature critter)
        {

            //Checks x and y positional values for the game creature and updates them to fit the confines of the game world
            //Wrapping occurs  if positional values are negitive or exccede the game world

            //The x coordinate is checked and updated if needed
            if (critter.getX() >= world.Length || critter.getX() < 0)
            {
                int newX = critter.getX() % world.Length;
                if (newX < 0)
                    newX = (world.Length - 2) - newX;

                critter.setX(newX);
            }

            //The Y coodinate is checked and updated if needed
            if (critter.getY() >= world.Length || critter.getY() < 0)
            {
                int newY = critter.getX() % world.Length;
                if (newY < 0)
                    newY = (world.Length - 2) - newY;
                critter.setY(newY);
            }

            //The creatures information is added to the CreatureTracker

            tracker.addData(critter.getEnergy());

            //The creature is then put onto the board
            world[critter.getX()][critter.getY()].Add(critter);

        }


        // AUTHOR: Russell Asher 
        //         Kenneth Ordona 
        // DATE: Feb 1st 2012
        // Description:Travels through every GameCreature list on every grid location of the 2D array "world" 
        // and calls every creatures update() function which updates their position and energy levels
        // preconditions: NONE
        // postconditions: Position checking and wrapping happen inside of this function for all creatures ppositions 

        public void update()
        {
            //visits each container on the world "grid" and calls the dayPass function
            for (int x = 0; x < world.Length; x++)
            {
                for (int y = 0; y < world.Length; y++)
                {
                    for (int i = 0; i < world[x][y].Count; i++)
                    {
                        GameCreature critter;
                        critter = world[x][y][i];
                        world[x][y].Remove(critter);
                        if ((critter is AgileCreature && !critter.getAlive()))   //This check insures that dead creatures do not update their position
                        { }
                        else
                        {
                            critter.dayPass();

                        }
                        // Testing to see if critter should be culled
                            addCreature(critter);


                    }
                }
            }

            daysPassed++;

            //Looks at each container on the grid and runs the areaSim function
            for (int x = 0; x < world.Length; x++)
            {
                for (int y = 0; y < world.Length; y++)
                {
                    for (int i = 0; i < world[x][y].Count; i++)
                        areaSim(world[x][y]);
                }
            }


            for (int x = 0; x < world.Length; x++)
            {
                for (int y = 0; y < world.Length; y++)
                {
                    for (int i = 0; i < world[x][y].Count; i++)
                    {
                        GameCreature tempCritter;
                        if (world[x][y][i] != null)
                        {
                            tempCritter = world[x][y][i];
                            rankingList.Add(tempCritter);
                        }
                    }
                }
            }

            sortEnrg(rankingList);
            outputRankings(rankingList);
            rankingList.Clear();
        }

        // AUTHOR: Russell Asher 
        //         Kenneth Ordona 
        // DATE: Feb 1st 2012
        // Description:Takes a List of game creatures and has one randomly interact with anotherone on the list
        // preconditions: NONE
        // postconditions: NONE

        private void areaSim(List<GameCreature> gridArea)
        {

            if (gridArea.Count() > 1)
            {

                foreach (GameCreature critter in gridArea)
                {
                    if (critter.getAlive())
                    {    //If the creature is alive then preform the following tasks

                        diceRoll = randGen.Next(gridArea.Count);  //random number is generated between 0 and the size of the list
                        if (critter == gridArea[diceRoll]) { }
                        else
                        {
                            if (critter.getMass() + critter.getEnergy() > gridArea[diceRoll].getMass() + gridArea[diceRoll].getEnergy())
                            {
                                gridArea[diceRoll].die();
                                if (critter is PredatorCreature)
                                {

                                    //The program ran without doing this conversion but visual studio kept 
                                    //reporting it as an error when it should be more apporpreatly be seen as a warning
                                    PredatorCreature predator = (PredatorCreature)critter;
                                    predator.consume(gridArea[diceRoll]);

                                }

                            }
                            else
                            {
                                critter.die();

                            }
                        }
                        if (critter is AgileCreature)
                        {
                            diceRoll = randGen.Next(100);
                            if (diceRoll <= probFeed)
                                critter.addEnergy(amountToFeed);
                        }
                    }
                }
            }

        }

        // AUTHOR: Russell Asher 
        //         Kenneth Ordona 
        // DATE: Feb 1st 2012
        // Description: Gives a copy of the energy data Tracker 
        // preconditions: NONE
        // postconditions: NONE

        public CreatureDataTracker giveTracker()
        {

            return tracker;
        }

        public List<GameCreature> sortEnrg(List<GameCreature> critterList)
        {

            GameCreature tempCritter;
            
                for (int i = 0; i < critterList.Count(); i++)
                {
                    for (int j = i+1; j < critterList.Count(); j++)
                    {
                        if (critterList[j] > critterList[i])
                        {
                            tempCritter = critterList[i];
                            critterList[i] = critterList[j];
                            critterList[j] = tempCritter;
                        }
                        critterList[i].setRanking(i + 1);
                        critterList[j].setRanking(j + 1);
                    }

                }
            
                return critterList;
        }
        public void outputRankings(List<GameCreature> critterList)
        {
            output = output + "Day: " + (daysPassed) + "\r\n";
            foreach (GameCreature critter in critterList)
            {
                output = output + "CreatureID: " + critter.getID() + " Ranking: " + critter.getRanking() +
                    " Cons. Rate: " + critter.getCons() + "\r\n";

            }
            output = output + "\r\n";

            System.IO.File.WriteAllText(@"C:\Users\kenneth\Desktop\RankingList.txt", output); //OUTPUT PATH!!!!
        }

    }


}
