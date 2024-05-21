using System;
using System.Text.RegularExpressions;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Calc
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        string outputText ="";


        public MainWindow()
        {
            InitializeComponent();
        }

        private void clearButton_Click(object sender, RoutedEventArgs e)
        {
            outputTBox.Text = "";
        }

        private void inputNumbersButton_Click(object sender, RoutedEventArgs e)
        {
            var buttonContent = (e.Source as Button).Content;
            string symbol = buttonContent.ToString();
            outputTBox.Text += symbol;
        }

        private void inputMathSymbolsButton_Click(object sender, RoutedEventArgs e)
        {
            if (outputText == "") outputTBox.Text = "0";
            
            var buttonContent = (e.Source as Button).Content;
            string symbol = buttonContent.ToString();
            outputTBox.Text += symbol;
        }

        private void backSpaceButton_Click(object sender, RoutedEventArgs e)
        {
            if (outputText != "")
            {
                outputTBox.Text = outputText.Remove(outputText.Length - 1, 1);
            }
        }

        private void outputTBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            outputText = outputTBox.Text;
            Regex searchDoubleSpecialSymbol = new Regex(@"\W\W$");
            if (searchDoubleSpecialSymbol.Matches(outputText).Count != 0)
            {
                outputTBox.Text = outputText.Remove(outputText.Length - 1, 1);
            }
            Regex searchLastNumber = new Regex(@"\d{1,},{1}\d{1,},{1}$");
            if (searchLastNumber.Matches(outputText).Count != 0)
            {
                outputTBox.Text = outputText.Remove(outputText.Length - 1, 1);
            }
        }

        private void inputMathFunctionButton_Click(object sender, RoutedEventArgs e)
        {
            Regex searchLastNumber = new Regex(@"\d{1,},?\d*$");
            MatchCollection matches = searchLastNumber.Matches(outputText);
            string number = "";
            int numberLength = 0;
            double functionFormat;
            if (matches.Count != 0)
            {
                number = matches[matches.Count-1].Value.ToString();
                numberLength = number.Length;
                double calcNumber = Convert.ToDouble(number);

                switch ((e.Source as Button).Content.ToString())
                {
                    case "Cos":
                        functionFormat = Math.Cos(calcNumber);
                        outputText = outputText.Substring(0, outputText.Length - numberLength);
                        outputText += string.Format("{0:0.0000}", functionFormat);
                        outputTBox.Text = outputText;
                        break;
                    case "Sin":
                        functionFormat = Math.Sin(calcNumber);
                        outputText = outputText.Substring(0, outputText.Length - numberLength);
                        outputText += string.Format("{0:0.0000}", functionFormat);
                        outputTBox.Text = outputText;
                        break;
                    case "Tan":
                        functionFormat = Math.Tan(calcNumber);
                        outputText = outputText.Substring(0, outputText.Length - numberLength);
                        outputText += string.Format("{0:0.0000}", functionFormat);
                        outputTBox.Text = outputText;
                        break;
                    case "√":
                        functionFormat = Math.Sqrt(calcNumber);
                        outputText = outputText.Substring(0, outputText.Length - numberLength);
                        outputText += string.Format("{0:0.0000}", functionFormat);
                        outputTBox.Text = outputText;
                        break;
                    case "Х²":
                        functionFormat = Math.Pow(calcNumber, 2);
                        outputText = outputText.Substring(0, outputText.Length - numberLength);
                        outputText += functionFormat;
                        outputTBox.Text = outputText;
                        break;
                    case "X³":
                        functionFormat = Math.Pow(calcNumber, 3);
                        outputText = outputText.Substring(0, outputText.Length - numberLength);
                        outputText += functionFormat;
                        outputTBox.Text = outputText;
                        break;
                }
            }
        }

        private void calculateButton_Click(object sender, RoutedEventArgs e)
        {
            Regex regex = new Regex(@".{0,1}\d{1,},?\d*");
            MatchCollection matches = regex.Matches(outputText);
            double result = 0;
            char operand = '0';
            double number = 0;
            if (matches.Count > 0)
            {
                foreach (Match match in matches)
                {
                    if (match.Index == 0)
                    {
                        result += Convert.ToDouble(match.Value);
                    }
                    else
                    {
                        operand = match.Value[0];
                        number = Convert.ToDouble(match.Value.Substring(1));

                        switch (operand)
                        {
                            case '+':
                                result += number;
                                break;
                            case '-':
                                result -= number;
                                break;
                            case '*':
                                result *= number;
                                break;
                            case '/':
                                result /= number;
                                break;
                        }
                    }
                }
                outputTBox.Text = $"{result}";
            }
        }

        private void negativeNumberButton_Click(object sender, RoutedEventArgs e)
        {
            Regex searchLastNumber = new Regex(@".{0,1}\d{1,},?\d*$");
            MatchCollection matches = searchLastNumber.Matches(outputText);
            string number;
            if (matches.Count > 0)
            {
                number = matches[matches.Count - 1].Value.ToString();
                if (number[0] == '-')
                {
                    number = number.Replace("-", "+");
                }
                else number = number.Replace('+', '-');

                outputText = outputText.Substring(0, outputText.Length - number.Length);
                outputText += number;
                outputTBox.Text = outputText;
            }
        }

        private void outputTBox_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            
            if ((e.Key < Key.D0 || e.Key > Key.D9) && e.Key != Key.Back)
            {
                e.Handled = true;
            }
            
        }
    }
}
