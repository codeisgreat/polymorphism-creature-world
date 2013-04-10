// AUTHOR: Kenneth Ordona 
//         Russell Asher  
// FILENAME: PredatorCreature.cs
// DATE: 2/5/2012
// REVISION HISTORY: 1.0
// PLATFORM (Microsoft Visual Studio 10)

/* IMPLEMENTATION INVARIANTS
   The PredatorCreature class inherits all qualities, variables, and methods from the GameCreature class. The max
 * and min initial energy and mass of a predator is larger than an agile creature due to design choices. The predator 
 * can also consume conquered creatures to restore its energy levels as well as increase its mass. In order to not
 * overpower predators, they can only consume a third of a killed creatures mass.
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace P3
{
    class PredatorCreature : GameCreature               
    {

        private const int maxEnergy = 350,               // const int used to hold max initial energy
                          minEnergy = 150,               // const int used to hold min initial energy
                          maxMass = 200,                 // const int used to hold max initial mass
                          minMass = 100;                 // const int used to hold min initial mass


        // DATE: 2/5/12
        // Description: This is the constructor for the PredatorCreature class. It initializes the energy
        // level and mass of the creature and then calculates what the consumption rate of the creature
        // is.
        // Preconditions: None
        // Postconditions: None
        public PredatorCreature()
        {
            energyLevel = data.Next(minEnergy, maxEnergy);
            energyRes = energyLevel;
            mass = data.Next(minMass, maxMass);
            consRate = (int)energyLevel / mass;
        }


        // DATE: 2/5/12
        // Description: This is the consume function. It calculates the newly assumed mass and energy reserves
        // of the predatory creature. Because the opposing creature is dead, the predator will consume a third
        // of the creature's meat and then continue along. The energy of the predator will go up by the 
        // amount in the dead creatures reserves.
        // Preconditions: None
        // Postconditions: None
        public void consume(GameCreature deadCreat)
        {
            mass += (deadCreat.getMass()/3);
            energyRes += ((deadCreat.getMass() + energyLevel)/consProp);
            if (energyRes > energyLevel)
                energyRes = energyLevel;
        }
    }
}
