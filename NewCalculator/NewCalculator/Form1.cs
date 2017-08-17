using System;
using System.Windows.Forms;

namespace NewCalculator
{
    public partial class CalculatorForm : Form
    {
        // Define and initilize local variables.
        Double num1 = 0;
        Double num2 = 0;
        Boolean isOperatorClicked = false;
        String operatorClicked = "";
        public String keyboardOperatorClicked = "";
        public String keyboardNumberClicked = "";

        public CalculatorForm()
        {
            InitializeComponent();
            // Always applying Focus to a HiddenButton with no functionality allows 
            // us to control what happens when the Enter button is pressed on the Keyboard.
            hiddenButton.Focus();
        }    

        // Button Click Method
        private void Button_Click(object sender, EventArgs e)
        {
            // Clear the TextBox before adding the pressed number if it is 
            // currently 0 or an Operator had just been clicked.
            if (resultBox.Text == "0" || (isOperatorClicked))
            {
                resultBox.Clear();
            }

            // Handle Mouse or Keyboard input
            if (keyboardNumberClicked == "")
            {
                Button button = (Button)sender;
                resultBox.Text = resultBox.Text + button.Text;
            }
            else
                resultBox.Text = resultBox.Text + keyboardNumberClicked;

            isOperatorClicked = false;
            // Return Focus to the HiddenButton.
            hiddenButton.Focus();
        } // Button_Click


        // ButtonPoint_Click
        private void ButtonPoint_Click(object sender, EventArgs e)
        {
            // Clear the TextBox before adding the pressed number if it is 
            // currently 0 or an Operator had just been clicked.
            if (resultBox.Text == "0" || (isOperatorClicked))
            {
                resultBox.Clear();
            }
            isOperatorClicked = false;
            
            /// Only allow 1 point in the resultBox.
            if (!resultBox.Text.Contains("."))
            {
                resultBox.Text = resultBox.Text + ".";
            }

            // Return Focus to the HiddenButton.
            hiddenButton.Focus();
        } // ButtonPoint_Click

        // ButtonEquals_Click
        private void ButtonEquals_Click(object sender, EventArgs e)
        {
            // Return if the textbox is empty.
            if (resultBox.Text == "" || resultBox.Text == "0" || resultBox.Text == "." || resultBox.Text == "0.")
                return;

            // Assign num2 = current value in the textbox if it is 0.
            if (num2 == 0)
                num2 = Convert.ToDouble(resultBox.Text);

            // Switch case for Operator choosen.
            switch (operatorClicked)
            {
                case "+":
                    resultBox.Text = (num1 + num2).ToString();
                    break;
                case "-":
                    resultBox.Text = (num1 - num2).ToString();
                    break;
                case "×":
                    resultBox.Text = (num1 * num2).ToString();
                    break;
                case "÷":
                    resultBox.Text = (num1 / num2).ToString();
                    break;
                default:
                    break;
            }

            // Populate the top line with the calculation so far.
            previousTextBox.Text = num1.ToString() + " " + operatorClicked + " " + num2.ToString();

            // Reset variables.
            operatorClicked = "";
            isOperatorClicked = true;
            // num1 = Convert.ToDouble(resultBox.Text); ;
            num1 = 0;
            num2 = 0;

            // Return Focus to the HiddenButton.
            hiddenButton.Focus();
        } // ButtonEquals_Click

        // Clear Button
        private void ButtonClear_Click(object sender, EventArgs e)
        {
            // Reset variables.
            resultBox.Text = "0";
            num1 = 0;
            num2 = 0;
            operatorClicked = "";
            isOperatorClicked = false;
            previousTextBox.Text = "";

            // Return Focus to the HiddenButton.
            hiddenButton.Focus();
        }

        private void Operator_Click(object sender, EventArgs e)
        {
            // Set num2 and get result.
            if (operatorClicked != "" && resultBox.Text != "")
            {
                num2 = Convert.ToDouble(resultBox.Text);
                buttonEquals.PerformClick();
            }

            // Set Operator.
            if (sender.GetType().ToString() == "System.Windows.Forms.Button")
            {
                Button button = (Button)sender;
                operatorClicked = button.Text;
            }
            else
                operatorClicked = keyboardOperatorClicked;

            // Populate num1.
            if (num1 == 0 && resultBox.Text != "")
                num1 = Convert.ToDouble(resultBox.Text);
            
            isOperatorClicked = true;
            previousTextBox.Text = num1.ToString() + " " + operatorClicked;
            resultBox.Text = "";

            // Return Focus to the HiddenButton.
            hiddenButton.Focus();
        } // Operator_Click


        // CalculatorForm_KeyPress
        private void CalculatorForm_KeyPress(object sender, KeyPressEventArgs e)
        {
            // Do nothing when Tab is pressed.
            if (e.KeyChar == (char)Keys.Tab)
            {
                return;
            }
            
            // Handle Operator input from keyboard.
            if (e.KeyChar == '+' || e.KeyChar == '-' || e.KeyChar == '/' || e.KeyChar == '*')
            {
                if (e.KeyChar == '/')
                    keyboardOperatorClicked = "÷";
                else if (e.KeyChar == '*')
                    keyboardOperatorClicked = "×";
                else
                    keyboardOperatorClicked = e.KeyChar.ToString();

                Operator_Click(sender, e);
                return;
            }

            // Impliment Backspace.
            if (e.KeyChar == (char)Keys.Back)
            {
                if (resultBox.Text != "")
                {
                    resultBox.Text = resultBox.Text.Substring(0, (resultBox.Text.Length - 1));
                }
                return;
            }

            // Supress non-numeric keys, except the point.
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar) && (e.KeyChar != '.'))
            {
                e.Handled = true;                
            }
            else
            {
                if (e.KeyChar == '.')
                {
                    ButtonPoint_Click(sender,e);
                    return;
                }
                keyboardNumberClicked = e.KeyChar.ToString();
                Button_Click(sender, e);
                keyboardNumberClicked = "";
            }

        } // CalculatorForm_KeyPress

        // Since the HiddenButton should always be on Focused, hitting enter  
        // on it should do the same as hitting the Equals Button.
        private void HiddenButton_Click(object sender, EventArgs e)
        {
            ButtonEquals_Click(sender, e);            
        }

        // Keep focus on the HidenButton.
        private void HiddenButton_Leave(object sender, EventArgs e)
        {
            // Return Focus to the HiddenButton.
            hiddenButton.Focus();
        }
    }
}
