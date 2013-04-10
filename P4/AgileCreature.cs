// AUTHOR: Kenneth Ordona 
//         Russell Asher  
// FILENAME: AgileCreature.cs
// DATE: 2/5/2012
// REVISION HISTORY: 1.0
// PLATFORM (Microsoft Visual Studio 10)

/* IMPLEMENTATION INVARIANTS
   The AgilerCreature class inherits all qualities, variables, and methods from the GameCreature class. The initial
 * values for mass, and energy levels is lower than predators due to design choices. Also, an int array is used to 
 * hold these intial values all throughout the AgileCreature's lifespan. The AgileCreature has an added feature of 
 * being able to escape from a lost fight(50% chance); however, the state of their energy and mass is restored 
 * to the initial values. 
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace P3
{
    class AgileCreature : GameCreature 
    {

        private const int maxEnergy = 150,            // constant int used to hold max initial energy possible
                          minEnergy = 50,             // constant int used to hold min initial energy possible
                          maxMass = 100,              // constant int used to hold max initial mass possible
                          minMass = 50,               // constant int used to hold min initial min possible
                          maxValues = 2;              // const int used to hold the max amount of values for initState
                                                      // array

        int[] initState = new int[maxValues];         // int array used to hold initial state for creature.
                                                      // energyLev = [0], mass = [1]
        private int escRoll;                          // int used to generate escape diceRoll

        // DATE: 2/5/12
        // Description: This is the constructor for the AgileCreature class. It initializes the energy
        // level and mass of the creature and sets the initialState array to these values.
        // It then calculates what the consumption and exhaust rate of the creature is.
        // Preconditions: None
        // Postconditions: None
        public AgileCreature()
        {
            energyLevel = data.Next(minEnergy, maxEnergy);
            energyRes = energyLevel;
            mass = data.Next(minMass, maxMass);
            initState[maxValues-2] = energyLevel;
            initState[maxValues-1] = mass;
            updEx();
            updCons();
        }

        // DATE: 2/5/12
        // Description: This overrided die function allows for the agile creature to escape if the dice roll is high
        // enough. 
        // Preconditions: None
        // Postconditions: If creature fails roll, alive state is set to false. If creature passes roll, the 
        // energy level and mass are reset to initial values.
        public override void die()
        {
            escRoll = data.Next(maxRoll);
            if(escRoll < (maxRoll/2))
                base.die();
            else
                restore();
        }



        // DATE: 2/5/12
        // Description: This function restores the energy and mass variables to their initial states.
        // Preconditions: None
        // Postconditions: Energy and Mass variables are reset.
        public void restore()
        {
            energyLevel = initState[maxValues-2];
            energyRes = energyLevel;
            mass = initState[maxValues - 1];
        }
            
        
    }
}
