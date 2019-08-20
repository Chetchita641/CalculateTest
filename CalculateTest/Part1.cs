using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CalculateTest {
    public partial class Part1 : Form {
        Calculator calculator = new Calculator();

        public Part1()
        {
            InitializeComponent();
        }

        void PrintKeypadOutput ()
        {
            txtOutput.Text = calculator.sOutput;
            txtPreviousResults.Text = calculator.getAllPreviousResults();
        }

        void PrintSimpleOutput ()
        {
            txtSimpleResult.Text = calculator.sOutput;
            txtPreviousResults.Text = calculator.getAllPreviousResults();
        }

        private void btnClearEntry_Click(object sender, EventArgs e)
        {
            calculator.clearInput();
            txtOutput.Text = calculator.sOutput;
        }

        private void btnClearAll_Click(object sender, EventArgs e)
        {
            calculator.clearAll();
            txtOutput.Text = calculator.sOutput;
        }

        private void btn0_Click(object sender, EventArgs e)
        {
            calculator.setInput("0");
            PrintKeypadOutput();
        }

        private void btn1_Click(object sender, EventArgs e)
        {
            calculator.setInput("1");
            PrintKeypadOutput();
        }

        private void btn2_Click(object sender, EventArgs e)
        {
            calculator.setInput("2");
            PrintKeypadOutput();
        }

        private void btn3_Click(object sender, EventArgs e)
        {
            calculator.setInput("3");
            PrintKeypadOutput();
        }

        private void btn4_Click(object sender, EventArgs e)
        {
            calculator.setInput("4");
            PrintKeypadOutput();
        }

        private void btn5_Click(object sender, EventArgs e)
        {
            calculator.setInput("5");
            PrintKeypadOutput();
        }

        private void btn6_Click(object sender, EventArgs e)
        {
            calculator.setInput("6");
            PrintKeypadOutput();
        }

        private void btn7_Click(object sender, EventArgs e)
        {
            calculator.setInput("7");
            PrintKeypadOutput();
        }

        private void btn8_Click(object sender, EventArgs e)
        {
            calculator.setInput("8");
            PrintKeypadOutput();
        }

        private void btn9_Click(object sender, EventArgs e)
        {
            calculator.setInput("9");
            PrintKeypadOutput();
        }

        private void btnPoint_Click(object sender, EventArgs e)
        {
            calculator.setInput(".");
            PrintKeypadOutput();
        }

        private void btnDivide_Click(object sender, EventArgs e)
        {
            calculator.setInput("/");
            PrintKeypadOutput();
        }

        private void btnMultiply_Click(object sender, EventArgs e)
        {
            calculator.setInput("*");
            PrintKeypadOutput();
        }

        private void btnSubtract_Click(object sender, EventArgs e)
        {
            calculator.setInput("-");
            PrintKeypadOutput();
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            calculator.setInput("+");
            PrintKeypadOutput();
        }

        private void btnEqual_Click(object sender, EventArgs e)
        {
            calculator.getResult();
            PrintKeypadOutput();
        }

        private void btnSimpleCalculate_Click(object sender, EventArgs e)
        {
            calculator.setLeft(txtLeftValue.Text);
            calculator.setRight(txtRightValue.Text);
            calculator.setOperator(txtOperator.Text);

            calculator.getResult();
            PrintSimpleOutput();
        }

        private void btnSimpleClearForm_Click(object sender, EventArgs e)
        {
            calculator.clearAll();
            txtLeftValue.Text = "";
            txtOperator.Text = "";
            txtRightValue.Text = "";
        }

    }

    public class Calculator {
        public string sOutput { get; set; }
        public List<string> liPreviousResults = new List<string>();

        string sInput = "";

        string sLeftValue = "";
        string sRightValue = "";
        string sOperator = "";

        string[] asValidOperators = { "+", "-", "*", "/" };
        string[] asValidDigits = { "0", "1", "2", "3", "4", "5", "6", "7", "8", "9", "." };
        bool bHasPoint = false;

        int top = 0;
        int previousResultLimit = 10;

        public Calculator()
        {

        }

        public Calculator (string left, string right, string op)
        {
            sLeftValue = left;
            sRightValue = right;
            sOperator = op; 
        }

        public void setInput (string value)
        {
            // If the input is neither a digit or an operator, error
            if (!asValidDigits.Contains(value) && !asValidOperators.Contains(value))
                sOutput = "Error: Entry is invalid";

            // If the minus sign is being used as a negative sigil
            else if (value == "-" && (String.IsNullOrEmpty(sInput) || sInput == "-"))
            {
                // If good, use the sign as a toggle
                sInput = sInput != "-" ? "-" : "";
                sOutput = sInput;
            }

            // If an operator
            else if (asValidOperators.Contains(value))
            {
                // if an operator has been hit two or more times in row, replace it
                if (!String.IsNullOrEmpty(sOperator) && String.IsNullOrEmpty(sInput))
                {
                    setOperator(value);
                    sInput = "";
                }

                // if there's nothing to operate on, check if there's a previous result that can be used. Otherwise, error
                else if (String.IsNullOrEmpty(sInput))
                {
                    string previousResult = getPreviousResult(1);
                    if (String.IsNullOrEmpty(previousResult))
                    {
                        sOutput = "Error: No value provided yet";
                    }
                    else
                    {
                        setLeft(previousResult);
                        setOperator(value);
                        bHasPoint = false;
                    }
                }

                // if there's already an operator, calculate and proceed if there's a left and right value provided
                else if (!String.IsNullOrEmpty(sOperator) && !String.IsNullOrEmpty(sLeftValue))
                {
                    setRight(sInput);
                    setLeft(getResult());
                    setOperator(value);
                    setRight("");
                    bHasPoint = false;
                }
                else
                {
                    setLeft(sInput);
                    setOperator(value);
                    bHasPoint = false;
                }
                sOutput = sLeftValue;
                sInput = "";
            }

            // If a digit
            else if (asValidDigits.Contains(value))
            {
                // ensure that only one point is used per number
                if (value == ".")
                    if (bHasPoint)
                        return;
                    else
                    {
                        // If there is no leading number, add a zero
                        if (String.IsNullOrEmpty(sInput))
                            sInput += "0";

                        bHasPoint = true;
                    }

                // only one leading zero is allowed before a double point. If a zero then another digit, replace with other digit
                else if (sInput == "0")
                {
                    if (value == "0")
                        return;

                    sInput = "";
                }

                sInput += value;
                sOutput = sInput;
            }

            // Something weird
            else
                sOutput = "Error: An unknown error has occured";
        }

        public void clearInput ()
        {
            sInput = "";
            sOutput = getPreviousResult(1);
        }

        public void clearAll ()
        {
            sLeftValue = sRightValue = sOperator = sInput = sOutput = "";
        }

        public void setLeft (string value)
        {
            double d;
            if (Double.TryParse(value, out d))
                sLeftValue = value;
            else
                sLeftValue = "";
        }

        public void setRight (string value)
        {
            double d;
            if (Double.TryParse(value, out d))
                sRightValue = value;
            else
                sRightValue = "";
        }

        public void setOperator (string value)
        {
            if (asValidOperators.Contains(value))
                sOperator = value;
            else
                sOperator = "";
        }

        public double? calculate ()
        {
            double dLeft = !String.IsNullOrEmpty(sLeftValue) ? Double.Parse(sLeftValue) : 0;
            double dRight = !String.IsNullOrEmpty(sRightValue) ? Double.Parse(sRightValue) : 0;
            double result;
            switch (sOperator)
            {
                case "+":
                    result = dLeft + dRight;
                    break;
                case "-": 
                    result = dLeft - dRight;
                    break;
                case "*":
                    result = dLeft * dRight;
                    break;
                case "/":
                    if (dRight == 0)
                        return null;
                    result = dLeft / dRight;
                    break;
                default:
                    result = dLeft;
                    break;
            }

            return result;
        }

        public string getResult ()
        {
            string sResult = "";
            if (String.IsNullOrEmpty(sLeftValue))
            {
                if (!String.IsNullOrEmpty(sInput))
                {
                    setLeft(sInput);
                    sInput = "";
                }
                else if (!String.IsNullOrEmpty(getPreviousResult(1)))
                {
                    sResult = sOutput = getPreviousResult(1);
                    sInput = sLeftValue = sRightValue = sOperator = "";
                    return sResult;
                }
                else
                {
                    sResult = sOutput = "Error: Missing intial values";
                    sInput = sLeftValue = sRightValue = sOperator = "";
                    return sResult;
                }
            }
            else if (String.IsNullOrEmpty(sOperator))
            {
                sResult = sOutput = "Error: Invalid Operator";
                sInput = sLeftValue = sRightValue = sOperator = "";
                return sResult;
            }
            else if (String.IsNullOrEmpty(sRightValue))
            {
                if (!String.IsNullOrEmpty(sInput))
                    setRight(sInput);
                else
                {
                    sResult = sOutput = "Error: Invalid Right value";
                    sInput = sLeftValue = sRightValue = sOperator = "";
                    return sResult;
                }
            }

            double? dResult = calculate();
            if (dResult.Equals(null))
            {
                // Are there more errors other than divide by zero?
                sResult = sOutput = "Error: Cannot divide by zero";
            }
            else
            {
                sResult = sOutput = dResult.ToString();
                setPreviousResult(sResult);
            }

            sInput = sLeftValue = sRightValue = sOperator = "";
            return sResult;
        }


        // I'm using a dropout stack for this, which is the most memory efficient method and protects against memory overruns. 
        // Normally this would be included in a kind of helper class or file
        public string getPreviousResult (int index)
        {
            if (index > top)
                return null;

            int adjustedIndex = (top % previousResultLimit) - index;
            if (adjustedIndex < 0)
                adjustedIndex += previousResultLimit;

            return liPreviousResults[adjustedIndex];
        }

        public string getAllPreviousResults ()
        {
            string output = "";
            for (int i = 1; i <= liPreviousResults.Count; i++)
            {
                output += i + ") " + getPreviousResult(i) + "\r\n";
            }

            return output;
        }

        void setPreviousResult (string result)
        {
            if (liPreviousResults.Count >= previousResultLimit)
                liPreviousResults[top % previousResultLimit] = result;
            else
                liPreviousResults.Add(result);

            top++;
        }
    };
}

