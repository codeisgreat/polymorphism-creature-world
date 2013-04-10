// AUTHOR: Kenneth Ordona 
//         Russell Asher  
// FILENAME: GameCreature.cs
// DATE: 2/5/2012
// REVISION HISTORY: 1.0
// PLATFORM (Microsoft Visual Studio 10)

/* IMPLEMENTATION INVARIANTS
   The GameCreature class is a base for other class-designers to create new creatures. The Creature has specific
 * game variables of energyLevel and reserves, a mass, an x and y coordinate(for positioning),
 * a unique rate of consumption and exhaustion as well as a bool that keeps track of the state of the creature's health
 * (dead or alive). Once the creature is dead, they cannot be brought back to life. The die function is virtual due 
 * to the belief that inherited creatures might not want to automatically die, but have a chance to escape. The dayPass 
 * algorithm is used to move the creatures around the game-board on a 1:1(x:y) basis. GameCreatures cannot have energy reserves
 * higher than energy level. However, the addEnergy function is virtual because the prospect of increasing their energy
 * levels(for inherited creatures), should be possible. Exhaustion rate is based on mass(bigger creatures exhaust quicker), and consumption rate is a 
 * dually based on mass and current reserves of the creature.
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace P3
{
    class GameCreature
    {
        private static int creatureCounter = 1;
        private int creatureID;

        protected const int exhProp = 10,         // const int used to hold a proportion for use in calculating exhaust rate
                            consProp = 3;         // const int used to hold a proportion for use in calculating consump. rate
        protected const int maxRoll = 100;        // const int used to hold the max diceRoll(for movement use)

        protected Random data = new Random();     // Random generated to stream randomized ints in

        protected int energyLevel,                // int used to hold the current energy level of the creature
                      energyRes,                  // int used to hold the current energy reserves of the creature
                      mass,                       // int used to hold the mass of the creature
                      xPos,                       // int used to hold the x Position of the creature(on gameBoard)
                      yPos,                       // int used to hold the y Position of the creature(on gameBoard)
                      consRate,                   // int used to hold the consumption rate of creature
                      exhaustRate,                // int used to hold the exhaustion rate of creature
                      xRoll,                      // int used to hold the dice roll for x-increments or dec.
                      yRoll,                      // int used to hold the dice roll for y-increments or dec.
                      ranking;
                     
        protected bool alive;                     // bool used to determine whether or not creature is alive

        // DATE: 2/5/12
        // Description: This is the constructor for the gameCreature class. Energy levels and mass are set to 10,
        // alive is set to true, and xPos/yPos are randomized between 0 and 10.
        // Preconditions: None
        // Postconditions: None
        public GameCreature()
        {
            creatureID = creatureCounter;
            energyLevel = 10;
            energyRes = 10;
            mass = 10;
            xPos = data.Next(10);
            yPos = data.Next(10);
            updCons();
            updEx();
            alive = true;
            creatureCounter++;
        }

        // DATE: 2/13/12
        // Description: Overloaded operator used to compare the energy consumption rate of two critters
        // Preconditions: None
        // Postconditions: None
        public static bool operator <(GameCreature critter1, GameCreature critter2)
        {
            return critter1.getCons() < critter2.getCons();
        }

        // DATE: 2/13/12
        // Description: Overloaded operator used to compare the energy consumption rate of two critters
        // Preconditions: None
        // Postconditions: None
        public static bool operator >(GameCreature critter1, GameCreature critter2)
        {
            return critter1.getCons() > critter2.getCons();
        }

        public int getID()
        {
            return creatureID;
        }

        // DATE: 2/5/12
        // Description: Get function for energy.
        // Preconditions: None
        // Postconditions: None
        public int getEnergy()
        {
            return energyRes;
        }

        // DATE: 2/5/12
        // Description: Get function for energyLevel. 
        // Preconditions: None
        // Postconditions: None
        public int getEnergyLevel()
        {
            return energyLevel;
        }

        // DATE: 2/5/12
        // Description: Get function for mass.
        // Preconditions: None
        // Postconditions: None
        public int getMass()
        {
            return mass;
        }

        public int getCons()
        {
            return consRate;
        }

        // DATE: 2/5/12
        // Description: Get function for X.
        // Preconditions: None
        // Postconditions: None
        public int getX()
        {
            return xPos;
        }

        // DATE: 2/5/12
        // Description: Get function for Y.
        // Preconditions: None
        // Postconditions: None
        public int getY()
        {
            return yPos;
        }

        // DATE: 2/5/12
        // Description: Get function for alive bool.
        // Preconditions: None
        // Postconditions: None
        public bool getAlive()
        {
            return alive;
        }

        // DATE: 2/5/12
        // Description: Set function for X
        // Preconditions: None
        // Postconditions: If xPos was null, it now has a value.
        public void setX(int x)
        {
            xPos = x;
        }

        // DATE: 2/5/12
        // Description: Set function for Y
        // Preconditions: None
        // Postconditions: If yPos was null, it now has a value.
        public void setY(int y)
        {
            yPos = y;
        }

        public void setRanking(int newRank)
        {
            ranking = newRank;
        }

        public int getRanking()
        {
            return ranking;
        }

        // DATE: 2/5/12
        // Description: This function adds in the allotted amount of energy to the creature's reserves.
        // If the creatures reserves exceed its level, it is set back to its max energy.
        // Preconditions: None
        // Postconditions: None
        virtual public void addEnergy(int addEn)
        {
            energyRes += addEn;
            if (energyRes > energyLevel)
                energyRes = energyLevel;
        }

        // DATE: 2/5/12
        // Description: SubEnergy function used to subtract the specified amount of energy from the
        // creature's reserves. 
        // Preconditions: None
        // Postconditions: None
        public void subEnergy(int subEn)
        {
            energyRes -= subEn;
        }

        // DATE: 2/5/12
        // Description: Virtual die function used to set the energy and mass to 0 and alive bool to false
        // Preconditions: None
        // Postconditions: State of alive is set to false.
        virtual public void die()
        {
            energyLevel = 0;
            mass = 0;
            energyRes = 0;
            alive = false;
        }

        // DATE: 2/5/12
        // Description: DayPass function is used to pass the turn, randomizing which area the creature will go to
        // (at most(X,Y) in increments of 1). It then subtracts the custom exhaustion rate from the energy-reserves. 
        // Then, it checks if the creature has died or not(energyRes<=0). If it has, then the die function is called.
        // Exhaustion rates and consumption rates are updated accordingly at the end.
        // Preconditions: None
        // Postconditions: If energy reserves lower below 0, then the creature's alive state will become false.
        public void dayPass()
        {
            
            xRoll = data.Next(maxRoll);
            yRoll = data.Next(maxRoll);

            if (xRoll < maxRoll / 2)
                xPos++;
            else
                xPos--;

            if (yRoll < maxRoll / 2)
                yPos++;
            else
                yPos--;

            subEnergy(exhaustRate);

            if (energyRes <= 0)
                die();
            
            updEx();
            updCons();

        }

        // DATE: 2/5/12
        // Description: updEx is a function that reconfigures the exhaust rate of the creature.
        // Preconditions: None
        // Postconditions: If exhaust rate was null, it now has a valid value.
        public void updEx()
        {
            exhaustRate = (int)(mass / 10);
        }

        // DATE: 2/5/12
        // Description: updCons is a function that reconfigures the consumption rate of the creature.
        // Preconditions: None
        // Postconditions: If consumption rate was null, it now has a valid value.
        public void updCons()
        {
            consRate = (int)((mass + energyRes)/consProp);
        }

 

    }
}
