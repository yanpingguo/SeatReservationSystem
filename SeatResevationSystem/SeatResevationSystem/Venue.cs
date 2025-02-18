/* 
 * Name of Project: ChiayinYanping_Assignment1
 * Purpose: Understaing how to create a booking system using C#
 * Revision History: 
 * - Chiayin Yang and Yanping Guo, May 31th 2024, Create basic design and functions.
 */
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SeatResevationSystem
{
    public partial class Venue : Form
    {
        string row;
        int column =-1;
        VenueBook book = new VenueBook();
        public Venue()
        {
            InitializeComponent();
            // adding items to listboxs

            listBoxRows.Items.Add("A");
            listBoxRows.Items.Add("B");
            listBoxRows.Items.Add("C");

            listBoxColums.Items.Add("1");
            listBoxColums.Items.Add("2");
            listBoxColums.Items.Add("3");
            listBoxColums.Items.Add("4");

            /// Initialize seat buttons color
            ResetAllSeatButtonColors();

            //Initialize capacity total lable
            setTotalCapacityLable();
        }

        private void Venue_Load(object sender, EventArgs e)
        {


        }

        //Add booking seat
        private void btnBook_Click(object sender, EventArgs e)
        {
            string txtCustomerNames = txtCustomerName.Text;
            if(!string.IsNullOrEmpty(txtCustomerNames) && !string.IsNullOrEmpty(row) && column>=0)
            {
                string returnValue = book.AddBook(row, column, txtCustomerNames);
                if (string.IsNullOrEmpty(returnValue))
                {
                    lblMessage.Text = "This seat book sucessfull";

                    /// Change button color to red when booked
                    UpdateSeatButtonColor(row, column, Color.Red);
                }
                else
                {
                    lblMessage.Text = "This seat is already booked";
                }
            }
            else
            {
                lblMessage.Text = "Please enter your username OR choose row column!!!";
            }

            //set capcity Lable 
            setTotalCapacityLable();
        }

        /**
         * add to wait list
         */
        private void btnAddToWaitList_Click(object sender, EventArgs e)
        {
            lblMessage.Text = book.AddToWaitList(txtCustomerName.Text);

            //set capcity Lable 
            setTotalCapacityLable();
        }


        //cancel one booking
        private void btnCancel_Click(object sender, EventArgs e)
        {
            string userName = txtCustomerName.Text.ToString();
            if (!string.IsNullOrEmpty(userName) && !string.IsNullOrEmpty(row) && column >= 0)
            {
                /// messagebox
                DialogResult result = MessageBox.Show("Are you sure you want to delete this userName?",
                "Confirmation", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);

                /// Change button color to green when cancel
                if (result == DialogResult.Yes)
                {
                    lblMessage.Text = book.Cancel(row, column, userName);

                    if (!lblMessage.Text.Contains("userName"))
                    {
                        UpdateSeatButtonColor(row, column, Color.LightGreen);
                    }
                }
            }
            else
            {
                lblMessage.Text = "Please enter your userName OR choose row and column";
            }

            //set capcity Lable 
            setTotalCapacityLable();

        }

        /**
         * cancel all booking
         */
        private void btnCancelAllBookings_Click(object sender, EventArgs e)
        {
            book.CancelAll();

            //Change button color to green 
            ResetAllSeatButtonColors();

            lblMessage.Text = "Cancel all successfull";

            //set capcity Lable 
            setTotalCapacityLable();

        }

        // get listBoxRow  selected value
        private void listBoxRows_SelectedIndexChanged(object sender, EventArgs e)
        {
            row = listBoxRows.SelectedItem.ToString();
        }

        // get  listBoxColumn value
        private void listBoxColums_SelectedIndexChanged(object sender, EventArgs e)
        {
            column = int.Parse(listBoxColums.SelectedItem.ToString());
        }

        /// Method to update seat button color
        private void UpdateSeatButtonColor(string row, int column, Color color)
        {
            string buttonName = $"btn{row}{column}";
            Button seatButton = this.Controls.Find(buttonName, true).FirstOrDefault() as Button;
            if (seatButton != null)
            {
                seatButton.BackColor = color;
            }
        }

        /// Method to reset all seat button colors
        private void ResetAllSeatButtonColors()
        {
            Console.WriteLine("laile ResetAllSeatButtonColors");
            foreach (var row in new[] { "A", "B", "C" })
            {
                for (int col = 1; col <= 4; col++)
                {
                    UpdateSeatButtonColor(row, col, Color.LightGreen);
                }
            }
        }

        /**
         * get mousehover show current represent username
         */
        private void button_focus(object sender, EventArgs e)
        {
            Button button = (Button)sender;
            string buttonText = button.Text;
            string userName = book.getFouceName(buttonText);
            toolTip1.SetToolTip(button, userName);
        }

        /**
         * fill all the vacant seats
         */
        private void btnFillAllSeats_Click(object sender, EventArgs e)
        {
            string txtcustomerName = txtCustomerName.Text;
            if(!string.IsNullOrEmpty(txtcustomerName))
            {
                string result = book.FillAllSeats(txtcustomerName);
                if (!string.IsNullOrEmpty(result))
                {
                    string[] resultArray = result.Split(',');
                    for(int i = 0; i < resultArray.Length; i++)
                    {
                        if (!string.IsNullOrEmpty(resultArray[i]))
                        {
                            Button seatButton = this.Controls.Find($"btn{resultArray[i]}", true).FirstOrDefault() as Button;
                            if (seatButton != null)
                            {
                                seatButton.BackColor = Color.Red;
                            }
                        }
                    }
                    lblMessage.Text = "Fill all seats successful";
                }
                else
                {
                    lblMessage.Text = "The seats are fill";
                }
               
            }
            else
            {
                lblMessage.Text = "Please enter userName!!!";
            }

            //set capcity Lable 
            setTotalCapacityLable();
        }
        /**
         * set total capacity lable
         */
        public void setTotalCapacityLable()
        {
            string waitListValue = "no one is on the wait list";
            int waitListCount = book.waitListCount;
            if (waitListCount > 0)
            {
                 waitListValue = waitListCount + " is on the wait list";

            }

            lblStatus.Text = "Seat available: " +
                book.CountAvailable + "(i.e.at " + book.Capacity + ")."+ waitListValue;
        }
    }
}
