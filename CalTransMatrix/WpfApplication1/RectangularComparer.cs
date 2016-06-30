using System.Collections;

namespace WpfApplication1
{
    class RectangularComparer : IComparer
    {
        // maintain a reference to the 2-dimensional array being sorted
        private double[,] sortArray;

        // constructor initializes the sortArray reference
        public RectangularComparer(double[,] theArray)
        {
            sortArray = theArray;
        }

        public int Compare(object x, object y)
        {
            // x and y are integer row numbers into the sortArray
            int i1 = (int)x;
            int i2 = (int)y;

            // compare the items in the sortArray
            return sortArray[i1, 3].CompareTo(sortArray[i2, 3]);
        }
    }
}