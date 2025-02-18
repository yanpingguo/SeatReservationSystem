using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.StartPanel;

namespace SeatResevationSystem
{
    internal class VenueBook
    {
        string[,] venueArray = new string[3, 4];
        List<string> waitList = new List<string>();
        public const int TOTALCOUNT = 12;

        private int countAvailable = TOTALCOUNT;
        public int CountAvailable
        {
            get { return countAvailable; }
            set { countAvailable = value; }
        }
        public string userName {  get; set; }
        public int waitListCount {  get; set; }
        private string capacity = "0.0%";
        public string Capacity
        {
            get { return capacity; }
            set {  capacity = value; }
        }


        public VenueBook()
        {

        }

        /**
         * Add booking 
         */
        public string AddBook(string row, int column, string userName)
        {
            //get row and determine if there is a available
            int indexRow = returnRowIndex(row);
            string seatAvailable = venueArray[indexRow, column - 1];
            if (string.IsNullOrEmpty(seatAvailable))
            {
                venueArray[indexRow, column - 1] = userName;

                //get available count and capacity percent
                countAvailable--;
                capacity = $"{((double)(TOTALCOUNT - countAvailable) / TOTALCOUNT):P}";
            }
            else
            {
                // Calculation  wait list username 
                AddToWaitList(userName);
            }
           
            return seatAvailable;

        }

        /**
        * fill all the vacant seats
        */
        public string FillAllSeats(string userName)
        {
            string combinIndex="";
            string row = "A";
            for(int i = 0;i< venueArray.GetLength(0); i++)
            {
                for (int j = 0;j< venueArray.GetLength(1); j++)
                {
                    if (string.IsNullOrEmpty(venueArray[i, j]))
                    {
                        venueArray[i, j]= userName;
                        if (i==1)
                        {
                            row="B";
                        }else if (i==2)
                        {
                            row = "C";
                        }
                        combinIndex += row+(j+1)+",";
                    }
                }
            }
            //set capacity count
            countAvailable = 0;
            capacity = $"{((double)(TOTALCOUNT - countAvailable) / TOTALCOUNT):P}";

            return combinIndex.Trim();
        }

        /**
         * add to wait list
         */
        public string AddToWaitList(string userName)
        {
            string message = "Seats are available";
           
            for (int i = 0; i < venueArray.GetLength(0); i++)
            {
                for (int j = 0; j < venueArray.GetLength(1); j++)
                {
                    if (string.IsNullOrEmpty(venueArray[i, j]))
                    {
                        return message;
                    }
                }
            }
            waitList.Add(userName);
            waitListCount++;
            message = "Add to wait list successful";
            
            return message;
        }

        /**
         * cancel one booking
         */
        public string Cancel(string row, int column,string userName)
        {
            int indexRow = returnRowIndex(row);
            string seatAvailable = venueArray[indexRow, column - 1];
            string message = "Cancel successful";
            if (!string.IsNullOrEmpty(seatAvailable))
            {
                if (!userName.Equals(seatAvailable))
                {
                    return "your userName is not exist";
                }
                if (waitList.Count != 0)
                {
                    venueArray[indexRow, column - 1] = waitList[0];
                    waitList.RemoveAt(0);
                    waitListCount--;
                    message = "Cancel userName:"+ userName + " successful! waitlist userName:" +
                        venueArray[indexRow, column - 1] + " add this seat successful";
                }
                else
                {
                    venueArray[indexRow, column - 1] = null;
                    //get available count and capacity percent
                    countAvailable++;
                    capacity = $"{((double)(TOTALCOUNT - countAvailable) / TOTALCOUNT):P}";
                }
            }
            else
            {
                message = "This seat no book, no need cancel";
            }

            return message;

        }
        /**
         * cancel All booking and wait list
         */
        public void CancelAll()
        {
            venueArray = new string[3, 4];

            waitListCount = 0;

            //get available count and capacity percent
            countAvailable = TOTALCOUNT;
            capacity = $"{((double)(TOTALCOUNT - countAvailable) / TOTALCOUNT):P}";

        }

        /**
         * get mousehover show current represent username
         */
        public string getFouceName(string text)
        {
            string row = text.Substring(0, 1);
            int column = int.Parse(text.Substring(1, 1));
            string userName = venueArray[returnRowIndex(row), column-1] ;
            if (!string.IsNullOrEmpty(userName))
            {
                return userName;
            }
            else
            {
                return "available";
            }
        }

        public int returnRowIndex(string row)
        {
            int indexRow = 0;
            if (row.Equals("B"))
            {
                indexRow = 1;
            }
            else if (row.Equals("C"))
            {
                indexRow = 2;
            }
            return indexRow;

        }
    }
}
