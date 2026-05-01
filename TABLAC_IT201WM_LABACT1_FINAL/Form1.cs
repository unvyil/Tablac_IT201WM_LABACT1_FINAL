using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TABLAC_IT201WM_LABACT1_FINAL
{ 

    public partial class Form1 : Form
    {
            private Receipt currentTransaction;

            public Form1()
        {
            InitializeComponent();

        }


        private void BtnRegister_Click(object sender, EventArgs e)
        {
            if (!int.TryParse(hrsParked.Text, out int hours))
            {
                MessageBox.Show("Please enter a valid number for Hours Parked.", "Input Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            string plate = plateNum.Text;
            string type = cbVehType.SelectedItem?.ToString() ?? "Unknown";
            string slot = assSlot.Text.ToUpper();

            string discountType = cbDisc.SelectedItem?.ToString() ?? "None";

            currentTransaction = new Receipt(plate, type, hours, slot, discountType);

            trPlateNum.Text = currentTransaction.PlateNumber;
            trVehType.Text = currentTransaction.VehicleType;
            trDuration.Text = currentTransaction.HoursParked.ToString();
            trAssSlot.Text = currentTransaction.SlotNumber;

            decimal baseFee = currentTransaction.ComputeBaseFee();
            decimal totalFee = currentTransaction.ComputeTotalFee();

            feeCalc.Text = $"₱{baseFee:F2}";
            servCharge.Text = $"₱{Receipt.ServiceCharge:F2}";
            total.Text = $"₱{totalFee:F2}";

            Control[] slotButtons = this.Controls.Find(slot, true);
            if (slotButtons.Length > 0 && slotButtons[0] is Button btnSlot)
            {
                btnSlot.BackColor = Color.IndianRed;

                btnSlot.Text = currentTransaction.PlateNumber;
            }
            else
            {
                MessageBox.Show($"Slot '{slot}' not found. Please ensure it is a valid slot (A1-G5).", "Slot Notice");
            }
        }

        private void BtnUpdate_Click(object sender, EventArgs e)
        {
            if (currentTransaction == null)
            {
                MessageBox.Show("No active transaction to update.", "Notice");
                return;
            }

            DialogResult result = MessageBox.Show($"Has vehicle {currentTransaction.PlateNumber} exited?",
                                                  "Checkout", MessageBoxButtons.YesNo);

            if (result == DialogResult.Yes)
            {
                Control[] slotButtons = this.Controls.Find(currentTransaction.SlotNumber, true);
                if (slotButtons.Length > 0 && slotButtons[0] is Button btnSlot)
                {
                    btnSlot.BackColor = Color.DarkSeaGreen;
                    btnSlot.Text = currentTransaction.SlotNumber;
                }

                BtnClear_Click(sender, e);
                MessageBox.Show("Parking slot successfully updated to Available.", "Success");
            }
        }

        private void BtnProcess_Click(object sender, EventArgs e)
        {
            if (currentTransaction == null)
            {
                MessageBox.Show("Please register a vehicle first.", "Error");
                return;
            }

            currentTransaction.DiscountType = cbDisc.SelectedItem?.ToString() ?? "None";

            if (decimal.TryParse(payAmt.Text, out decimal paymentAmount))
            {
                decimal totalDue = currentTransaction.ComputeTotalFee();

                    if (paymentAmount >= totalDue)
                    {
                        decimal changeAmt = paymentAmount - totalDue;
                        change.Text = $"₱{changeAmt:F2}";
                        MessageBox.Show("Payment Successful!", "Success");
                    }
                    else
                    {
                        MessageBox.Show("Insufficient payment amount.", "Payment Error");
                    }
                }
                else
                {
                    MessageBox.Show("Please enter a valid payment amount.", "Input Error");
                }
            }

        private void BtnReceipt_Click(object sender, EventArgs e)
        {
            if (currentTransaction == null) return;

            decimal hourlyTotal = currentTransaction.ComputeHourlyRate();
            decimal overtime = currentTransaction.ComputeOvertimeFee();
            decimal totalFee = currentTransaction.ComputeTotalFee();

            string discountDisplay = currentTransaction.DiscountType == "None"
                ? "None"
                : $"{currentTransaction.DiscountType} (20%)";

            listBox1.Items.Clear();
            listBox1.Items.Add("----- PARKING RECEIPT -----");
            listBox1.Items.Add($"Plate No: {currentTransaction.PlateNumber}");
            listBox1.Items.Add($"Type: {currentTransaction.VehicleType}");
            listBox1.Items.Add($"Slot: {currentTransaction.SlotNumber}");
            listBox1.Items.Add($"Hours: {currentTransaction.HoursParked}");
            listBox1.Items.Add("-----------------------");

            listBox1.Items.Add($"Rate Fee: ₱{hourlyTotal:F2}");

            if (overtime > 0)
            {
                listBox1.Items.Add($"OVERTIME FEE: ₱{overtime:F2}");
            }

            listBox1.Items.Add($"Discount: {discountDisplay}");
            listBox1.Items.Add($"Service Charge: ₱{Receipt.ServiceCharge:F2}");
            listBox1.Items.Add("-----------------------");
            listBox1.Items.Add($"TOTAL PAID: ₱{totalFee:F2}");
        }

        private void BtnClear_Click(object sender, EventArgs e)
        {
            plateNum.Clear();
            cbVehType.SelectedIndex = -1;
            hrsParked.Clear();
            cbDisc.SelectedIndex = -1;
            payAmt.Clear();
            assSlot.Clear();

            trPlateNum.Text = "N/A";
            trVehType.Text = "N/A";
            trDuration.Text = "N/A";
            trAssSlot.Text = "N/A";
            feeCalc.Text = "₱0.00";
            servCharge.Text = "₱0.00";
            total.Text = "₱0.00";
            change.Text = "₱0.00";

            foreach (Control c in this.Controls)
            {
                ResetParkingButtons(c);
            }

            listBox1.Items.Clear();
            currentTransaction = null;
        }


            private void ResetParkingButtons(Control parent)
        {
            foreach (Control c in parent.Controls)
            {
                if (c is Button btn)
                {
                    string name = btn.Name.ToLower();
                    if (btn.Name.Length <= 3 && !name.Contains("btn") && !name.Contains("clear"))
                    {
                        btn.BackColor = Color.DarkSeaGreen;
                        btn.Text = btn.Name; 
                    }
                }

                if (c.HasChildren)
                {
                    ResetParkingButtons(c);
                }
            }
        }

        private void trAssSlot_Click(object sender, EventArgs e)
        {

        }

        private void label5_Click(object sender, EventArgs e)
        {

        }
    }
    }
