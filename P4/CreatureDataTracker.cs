// AUTHOR: Kenneth Ordona 
//         Russell Asher  
// FILENAME: CreatureDataTracker.cs
// DATE: 2/5/2012
// REVISION HISTORY: 1.0
// PLATFORM (Microsoft Visual Studio 10)

/* IMPLEMENTATION INVARIANTS
 * The CreatureDataTracker class inherits all qualities, variables, and methods from the DataTracker class. This class was
 * modified so that the minimum value of all those tracked would also be included(cannot modify DataTracker). 
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace P3
{
    class CreatureDataTracker : DataTracker
    {
        private int min = 0;            // int used to hold the min value tracked in the dataTracker

        // DATE: 2/5/12
        // Description: Constructor function for CreatureDataTracker. Increments the unique count of dataTrackers and 
        // assigns that value to the trackerID.
        // Preconditions: None
        // Postconditions: None
        public CreatureDataTracker()
        {
            uniqueCount++;
            trackerID = uniqueCount;
        }

        // DATE: 2/5/12
        // Description: Get function for Min.
        // Preconditions: None
        // Postconditions: None
        public int getMin()
        {
            return min;
        }

        // DATE: 2/5/12
        // Description: Get function for Mean.
        // Preconditions: None
        // Postconditions: None
        public double getMean()
        {
            return mean;
        }

        // DATE: 2/5/12
        // Description: Get function for Median.
        // Preconditions: None
        // Postconditions: None
        public int getMedian()
        {
            return median;
        }

        // DATE: 2/5/12
        // Description: Get function for Max.
        // Preconditions: None
        // Postconditions: None
        public int getMax()
        {
            return max;
        }

        // DATE: 2/5/12
        // Description: The addData function adds the specified int into the dataAl list(using base.addData) 
        // and then figures out the minimum value as well.
        // Preconditions: None
        // Postconditions: If dataAl was null, it now has valid values within it.
        public override void addData(int number)
        {
            base.addData(number);
            min = dataAL.Min();
        }

    }
}
