// AUTHOR: Kenneth Ordona 
//         Russell Asher  
// FILENAME: DataTracker.cs
// DATE: 2/5/2012
// REVISION HISTORY: 1.0
// PLATFORM (Microsoft Visual Studio 10)

/* IMPLEMENTATION INVARIANTS
 * The DataTracker class is used to monitor, track and measure data that is input into the class. The addData function 
 * is virtual due to forseeable modifications being added to it. 
 */

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace P3
{
    public class DataTracker
    {

        protected static int uniqueCount = 0;                 // Static counter used to count the # of unique DataTracker objects as well as make sure
                                                    // each object has a unique ID.
        protected List<int> dataAL = new List<int>();         // List used to hold the ints passed through the addData function
        
        protected int trackerID;                              // Used to keep track of unique IDs for each object.
        protected int sum = 0,                                // Used to keep track of the sum of added data
            max = 0,                                // Used to keep track of the max int within added data
            median = 0;                             // Used to keep track of the median int within added data
                                                    // ASSUMPTION: Median is equal to dataAL[(maxSize-1)/2], not the estimates between two numbers
                                                    // as would be done when there are an even amount of objects in the list

        protected double mean = 0;                            // Double used to show accurate estimation of mean

        protected bool state = true;                          // bool used to control the on/off state of the DataTracker

        // DATE: 1/10/12
        // Description: This is the constructor for the DataTracker class. It constructs a new DataTracker object and then
        // assigns a new unique ID, based on the static int above, to the tracker as well 
        // Preconditions: None
        // Postconditions: None
        public DataTracker()                        
        {
            uniqueCount++;
            trackerID = uniqueCount;
        }

        // DATE: 1/10/12
        // Description: This function returns the trackerID, which is used within the driver file
        // Preconditions: None
        // Postconditions: None
        public int getID()
        {
            return trackerID;
        }

        // DATE: 1/10/12
        // Description: This function switches the object to the opposite of the current state. 
        // Preconditions: None
        // Postconditions: State of data tracker is switched to opposite
        public void switchState()
        {
            state = !state;
        }

        // DATE: 1/10/12
        // Description: This function takes an int in and adds it to the int list within the object
        // if the object's state is currently enabled. After doing so, the sum, mean, max and median
        // variables are adjusted to include the new number. Then, these new values are displayed 
        // to the console. If the state is currently disabled, nothing happens.
        // Preconditions: Object must be constructed before attempting to use this function
        // Postconditions: None
        // Assumptions: addData can only process ints. Anything other than that is not allowed 
        // within the DataTracker objects
        public virtual void addData(int number)
        {
            if (state)
            {
                dataAL.Add(number);
                dataAL.Sort();                
                sum += number;
                mean = (double)sum / dataAL.Count;
                max = dataAL.Max();
                median = dataAL[(dataAL.Count - 1) / 2];

                // Displaying statistics of current enabled data tracker to the console
                Console.WriteLine("Max: " + max + " Mean: " + mean + " Median: " + median);
                Console.WriteLine();
               
            }
        }

    }
}
