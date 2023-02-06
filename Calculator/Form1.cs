using System;
using System.Drawing;
using System.Windows.Forms;
using System.Collections.Generic;

namespace Calculator
{
    public partial class Calculator : Form
    {
        private const decimal MAX_VALUE = 100_00_00;
        private const decimal MIN_VALUE = -100_00_00;

        private decimal _first = 0;
        private decimal _second = 0;

        private string _chosenOperation = string.Empty;

        private bool _firstIsParsed = false;
        private bool _gottenResult = false;

        private Dictionary<string, Func<string>> operations;

        public Calculator()
        {
            InitializeComponent();

            operations = new Dictionary<string, Func<string>>();

            operations.Add("+", Sum);
            operations.Add("-", Subtract);
            operations.Add("*", Multiplate);
            operations.Add("/", Devide);
        }

        private string Sum() => CheckNumber(_first + _second).ToString();
        private string Subtract() => CheckNumber(_first - _second).ToString();
        private string Multiplate() => CheckNumber(_first * _second).ToString();

        private string Devide()
        {
            if (_second == 0)
            {
                errorInput.Text = "Error: division by zero";
                errorInput.BackColor = Color.Red;
                return string.Empty;
            }

            return CheckNumber(_first / _second).ToString();
        }

        private void ChangeNumber(string value)
        {
            if (_gottenResult)
            {
                numberInput.Text = string.Empty;
                _gottenResult = false;
            }

            numberInput.Text += value;
        }

        private void Calculator_Load(object sender, EventArgs e) { }
        private void operationLabel_Click(object sender, EventArgs e) { }
        private void numberInput_TextChanged(object sender, EventArgs e) { }
       
        private void clearButton_Click(object sender, EventArgs e) => numberInput.Text = string.Empty;
        private void errorInput_TextChanged(object sender, EventArgs e) => errorInput.BackColor = Color.White;

        private void comaButton_Click(object sender, EventArgs e)
        {
            ChangeNumber(",");
            comaButton.Enabled = false;
        }

        private decimal CheckNumber(decimal number)
        {
            if (number > MAX_VALUE)
            {
                number = MAX_VALUE;
                errorInput.Text = "Warning: Number is too big";
                errorInput.BackColor = Color.Yellow;
            }

            if (number < MIN_VALUE)
            {
                number = MIN_VALUE;
                errorInput.Text = "Warning: Number is too small";
                errorInput.BackColor = Color.Yellow;
            }


            return Math.Round(number, 3);
        }

        private decimal TryParse(string number, ref decimal parsed)
        {
            try
            {
                 number = number.Replace(',', '.');
                 parsed = decimal.Parse(number);
            }
            catch (FormatException)
            {
                parsed = 0;
                errorInput.Text = "Error: Input was invalid to parse. Assume it to be 0";
                errorInput.BackColor = Color.Red;
            }

            return CheckNumber(Math.Round(parsed, 3));
        }

        private void SetOperationButtonState(bool enabled)
        {
            operationButtonPlus.Enabled = enabled;
            operationButtonSubtract.Enabled = enabled;
            operationButtonDevide.Enabled = enabled;
            operationButtonMultiplate.Enabled = enabled;
        }

        private void ClickOperationButton(string operation)
        {
            TryParse(numberInput.Text, ref _first);
            DisableOperationButtons();

            numberInput.Text = string.Empty;
            getResultButton.Enabled = true;
            comaButton.Enabled = true;
            _firstIsParsed = true;
            _chosenOperation = operation;
        }

        private void getResultButton_Click(object sender, EventArgs e)
        {
            if (_firstIsParsed)
            {            
                TryParse(numberInput.Text, ref _second);

                numberInput.Text = operations[_chosenOperation]();
                numberInput.Text = numberInput.Text.Replace('.', ',');
                
                _first = 0;
                _second = 0;
            }

            EnableOperationButtons();

            getResultButton.Enabled = false;
            comaButton.Enabled = true;
            _firstIsParsed = false;
            _gottenResult = true;
        }

        private void ImplementTrigonometryFunction(Func<double, double> function)
        {
            decimal number = 0;
            number = TryParse(numberInput.Text, ref number);
            number = (decimal)function((double)number);
            numberInput.Text = CheckNumber(number).ToString();
            numberInput.Text = numberInput.Text.Replace('.', ',');
        }

        private void EnableOperationButtons() => SetOperationButtonState(true);
        private void DisableOperationButtons() => SetOperationButtonState(false);

        private void numberButton0_Click(object sender, EventArgs e) => ChangeNumber("0");
        private void numberButton1_Click(object sender, EventArgs e) => ChangeNumber("1");
        private void numberButton2_Click(object sender, EventArgs e) => ChangeNumber("2");
        private void numberButton3_Click(object sender, EventArgs e) => ChangeNumber("3");
        private void numberButton4_Click(object sender, EventArgs e) => ChangeNumber("4");
        private void numberButton5_Click(object sender, EventArgs e) => ChangeNumber("5");
        private void numberButton6_Click(object sender, EventArgs e) => ChangeNumber("6");
        private void numberButton7_Click(object sender, EventArgs e) => ChangeNumber("7");
        private void numberButton8_Click(object sender, EventArgs e) => ChangeNumber("8");
        private void numberButton9_Click(object sender, EventArgs e) => ChangeNumber("9");

        private void mathConstantE_Click(object sender, EventArgs e) => ChangeNumber(Math.E.ToString());
        private void mathConstantPI_Click(object sender, EventArgs e) => ChangeNumber(Math.PI.ToString());

        private void operationButtonPlus_Click(object sender, EventArgs e) => ClickOperationButton("+");
        private void operationButtonDevide_Click(object sender, EventArgs e) => ClickOperationButton("/");
        private void operationButtonSubtract_Click(object sender, EventArgs e) => ClickOperationButton("-");
        private void operationButtonMultiplate_Click(object sender, EventArgs e) => ClickOperationButton("*");

        private void mathFunctionCos_Click(object sender, EventArgs e) => ImplementTrigonometryFunction(Math.Cos);
        private void mathFunctionButtonSin_Click(object sender, EventArgs e) => ImplementTrigonometryFunction(Math.Sin);

        private void backspaceButton_Click(object sender, EventArgs e)
        {
            try
            {
                numberInput.Text = numberInput.Text.Remove(numberInput.Text.Length - 1, 1);
            }
            catch (ArgumentOutOfRangeException)
            {
                errorInput.Text = "Warning: Nothing to erase";
                errorInput.BackColor = Color.Yellow;
            }
        }
    }
}
