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
    public partial class Part2 : Form {
        Calculator calculator = new Calculator();
        bool hasOutput = false;

        public Part2()
        {
            InitializeComponent();
        }

        void PrintKeypadOutput ()
        {
            txtOutput.Text = hasOutput ? calculator.sOutput : calculator.sInput;
            txtPreviousResults.Text = calculator.getAllPreviousResults();
        }

        void PrintSimpleOutput ()
        {
            txtSimpleResult.Text = calculator.sOutput;
            txtPreviousResults.Text = calculator.getAllPreviousResults();
        }

        private void btnBackspace_Click(object sender, EventArgs e)
        {
            calculator.backspace();
            txtOutput.Text = calculator.sInput;
        }

        private void btnClearAll_Click(object sender, EventArgs e)
        {
            calculator.clearInput();
            txtOutput.Text = calculator.sInput;
        }

        private void btn0_Click(object sender, EventArgs e)
        {
            hasOutput = false;
            calculator.sInput += "0";
            PrintKeypadOutput();
        }

        private void btn1_Click(object sender, EventArgs e)
        {
            hasOutput = false;
            calculator.sInput += "1";
            PrintKeypadOutput();
        }

        private void btn2_Click(object sender, EventArgs e)
        {
            hasOutput = false;
            calculator.sInput += "2";
            PrintKeypadOutput();
        }

        private void btn3_Click(object sender, EventArgs e)
        {
            hasOutput = false;
            calculator.sInput += "3";
            PrintKeypadOutput();
        }

        private void btn4_Click(object sender, EventArgs e)
        {
            hasOutput = false;
            calculator.sInput += "4";
            PrintKeypadOutput();
        }

        private void btn5_Click(object sender, EventArgs e)
        {
            hasOutput = false;
            calculator.sInput += "5";
            PrintKeypadOutput();
        }

        private void btn6_Click(object sender, EventArgs e)
        {
            hasOutput = false;
            calculator.sInput += "6";
            PrintKeypadOutput();
        }

        private void btn7_Click(object sender, EventArgs e)
        {
            hasOutput = false;
            calculator.sInput += "7";
            PrintKeypadOutput();
        }

        private void btn8_Click(object sender, EventArgs e)
        {
            hasOutput = false;
            calculator.sInput += "8";
            PrintKeypadOutput();
        }

        private void btn9_Click(object sender, EventArgs e)
        {
            hasOutput = false;
            calculator.sInput += "9";
            PrintKeypadOutput();
        }

        private void btnPoint_Click(object sender, EventArgs e)
        {
            hasOutput = false;
            calculator.sInput += ".";
            PrintKeypadOutput();
        }

        private void btnDivide_Click(object sender, EventArgs e)
        {
            hasOutput = false;
            calculator.sInput += "/";
            PrintKeypadOutput();
        }

        private void btnMultiply_Click(object sender, EventArgs e)
        {
            hasOutput = false;
            calculator.sInput += "*";
            PrintKeypadOutput();
        }

        private void btnSubtract_Click(object sender, EventArgs e)
        {
            hasOutput = false;
            calculator.sInput += "-";
            PrintKeypadOutput();
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            hasOutput = false;
            calculator.sInput += "+";
            PrintKeypadOutput();
        }

        private void btnEqual_Click(object sender, EventArgs e)
        {
            calculator.getResult(txtOutput.Text);
            hasOutput = true;
            PrintKeypadOutput();
        }

        private void btnSimpleCalculate_Click(object sender, EventArgs e)
        {
            calculator.getResult(txtEquation.Text);
            hasOutput = true;
            PrintSimpleOutput();
        }

        private void btnSimpleClearForm_Click(object sender, EventArgs e)
        {
            calculator.clearInput();
            txtEquation.Text = "";
            PrintSimpleOutput();
        }
    }

    public class Calculator {
        public string sInput { get; set; }
        public string sOutput { get; set; }
        string sError = "";
        public List<string> liPreviousResults = new List<string>();

        List<string> asParsedEquation = new List<string>();

        char[] achValidOperators = { '+', '-', '*', '/' };
        char[] achValidDigits = { '0', '1', '2', '3', '4', '5', '6', '7', '8', '9', '.', '-' };

        int top = 0;
        int previousResultLimit = 10;

        public Calculator()
        {
            sInput = "";
        }

        public void clearInput ()
        {
            sInput = sOutput = "";
            sOutput += getPreviousResult(1);
        }

        public void backspace ()
        {
            if (sInput.Length > 0)
                sInput = sInput.Substring(0, sInput.Length - 1);
        }

        public void getResult (string equation)
        {
            sInput = "";
            sOutput = calculate(equation);
        }

        public string calculate (string equation)
        {
            // Parse the Equation first. Any invalid characters, numbers, or any invalid sequence is errored here
            // Results are saved in asParsedEquation if successful
            if (!ParseEquation(equation))
            {
                asParsedEquation.Clear();
                return sError;
            }

            // Now that equation has been parsed, calculate the result
            // First, simplify the equation in order to follow order of operations
            List<string> asSimplifiedEquation = new List<string>();
            int b = 0, w = 0;
            int e = asParsedEquation.Count - 1;
            string sLeftValue, sOp;
            double dLeftValue, dRightValue;
            bool lastNumber = false;

            while (b <= e)
            {
                sLeftValue = asParsedEquation[b];
                if (b == e)
                    lastNumber = true;
                sOp = !lastNumber ? asParsedEquation[b + 1] : "";
                
                // if the number has a multiply or divide succeeding it, simplify it first 
                if (sOp == "*" || sOp == "/")
                {
                    dLeftValue = Double.Parse(sLeftValue);
                    while (sOp == "*" || sOp == "/")
                    {
                        w += 2;
                        dRightValue = Double.Parse(asParsedEquation[w]);
                        switch (sOp)
                        {
                            case "*": dLeftValue = dLeftValue * dRightValue; break;
                            case "/":
                                // If divide by zero, error
                                if (dRightValue == 0)
                                {
                                    sError = "Error: Cannot divide by zero";
                                    asParsedEquation.Clear();
                                    asSimplifiedEquation.Clear();
                                    return sError;
                                }
                                dLeftValue = dLeftValue / dRightValue; break;
                        }

                        if (w == e)
                            lastNumber = true;
                        sOp = !lastNumber ? asParsedEquation[w + 1] : "";
                    }

                    sLeftValue = dLeftValue.ToString();
                }

                asSimplifiedEquation.Add(sLeftValue);
                if (lastNumber)
                {
                    w++;
                }
                else
                {
                    asSimplifiedEquation.Add(sOp);
                    w += 2;
                }

                b = w;
            }

            // Next, calculate the simplified equation
            b = 0;
            e = asSimplifiedEquation.Count - 1;
            double dResult = Double.Parse(asSimplifiedEquation[0]);

            while (b < e)
            {
                sOp = asSimplifiedEquation[b + 1];
                dRightValue = Double.Parse(asSimplifiedEquation[b + 2]);
                switch (sOp)
                {
                    case "+": dResult += dRightValue; break;
                    case "-": dResult -= dRightValue; break;
                }

                b += 2;
            }

            asParsedEquation.Clear();
            asSimplifiedEquation.Clear();

            string sResult = dResult.ToString();
            setPreviousResult(sResult);
            return sResult;
        }

        bool ParseEquation (string equation)
        {
            // First, separate the equation into an ordered List ([0]: number, [1]: operator, [2]: number...)
            // Invalid if:
            // - Elements not in <number><operator><number> sequence
            // - Invalid number / invalid operator
            // - Two decimal points / Two negative signs

            string el = "";
            bool hasPoint = false;
            bool hasNegativeSign = false;
            foreach (char ch in equation)
            {
                // Invalid character
                if (!achValidDigits.Contains(ch) && !achValidOperators.Contains(ch))
                {
                    sError = "Error: Invalid character in equation";
                    return false;
                }

                // if digit
                if (achValidDigits.Contains(ch))
                {
                    if (ch == '.')
                    {
                        // if two double points, error
                        if (hasPoint)
                        {
                            sError = "Error: Number has two decimal points";
                            return false;
                        }
                        else
                        {
                            hasPoint = true;
                            el += ch;
                        }
                    }

                    else if (ch == '-')
                    {
                        // if two negative signs, error
                        if (hasNegativeSign)
                        {
                            sError = "Error: Number has two negative signs";
                            return false;
                        }
                        // it's only a negative sign if it preceeds a number. Otherwise, it's an operator
                        else if (String.IsNullOrEmpty(el))
                        {
                            hasNegativeSign = true;
                            el += ch;
                        }
                    }
                    else
                        el += ch;
                }
                
                // if operator
                if (achValidOperators.Contains(ch) && el != "-")
                {
                    // Commit the previous number to array
                    if (!String.IsNullOrEmpty(el))
                    {
                        asParsedEquation.Add(el);
                        el = "";
                        hasPoint = false;
                        hasNegativeSign = false;
                    }

                    // if in an even numbered element, error
                    if (asParsedEquation.Count % 2 == 0)
                    {
                        sError = "Error: Invalid use of an operator";
                        return false;
                    }
                    else
                        asParsedEquation.Add(ch.ToString());
                }
            }

            // if there is no number at the end of the equation, error
            if (String.IsNullOrEmpty(el))
            {
                sError = "Error: Missing number at end of equation";
                return false;
            }

            asParsedEquation.Add(el);
            return true;
        }

        // I'm using a dropout stack for this, which is the most memory efficient method and protects against memory overruns. 
        // Normally this would be included in a kind of helper class or file
        public string getPreviousResult (int index)
        {
            if (index > top)
                return null;
            else if (index > previousResultLimit)
            {
                sOutput = "Error: Previous result index out of range";
                return null;
            }

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

