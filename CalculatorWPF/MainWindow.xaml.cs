using System;
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
using System.Data;
using System.Text.RegularExpressions;

namespace CalculatorWPF
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            foreach (UIElement button in CalculatorPanel.Children)
            {
                //проверка на соответствие типов
                if (button is Button)
                    ((Button)button).Click += ButtonHandler;
            }
            window.KeyDown += KeyboardHandler;
        }

        private void ButtonHandler(object sender, RoutedEventArgs e)
        {
            //считываем надпись на кнопке
            string content = ((Button)e.OriginalSource).Content.ToString();
            OutputSymbol(content);
        }

        private void KeyboardHandler(object sender, KeyEventArgs e)
        {
            string EnteredKey;
            switch (e.Key)
            {
                case Key.Divide:
                    EnteredKey = "/";
                    break;
                case Key.Multiply:
                    EnteredKey = "*";
                    break;
                case Key.Subtract:
                    EnteredKey = "-";
                    break;
                case Key.Add:
                    EnteredKey = "+";
                    break;
                case Key.Return:
                    EnteredKey = "=";
                    break;
                case Key.Delete:
                    EnteredKey = "C";
                    break;
                //либо цифра, либо пустая строка
                default:
                    EnteredKey = NumberOrNot(e.Key.ToString());
                    break;
            }

            //ищем цифру во введенной клавише
            string NumberOrNot(string keyState)
            {
                //this.input.Text = keyState.ToString();
                Regex regex = new Regex(@"(D|NumPad)\d{1}");
                Match match = regex.Match(keyState);
                //при вводе клавиши с цифрой возвращаем введенную цифру
                if (match.Success)
                    return match.Value[match.Value.Length - 1].ToString();
                //иначе возврат - пустая строка
                else
                    return string.Empty;
            }

            //не имеет смысла передавать пустую строку
            if (EnteredKey != string.Empty)
                OutputSymbol(EnteredKey);

        }

        //метод, выводящий выбранный символ на экран
        private void OutputSymbol(string symbol)
        {
            //массив операторов
            string[] operators = { "-", "+", "/", "*" };

            //если была нажата кнопка с числом
            if (int.TryParse(symbol, out int value))
            {
                input.Text += value;
            }
            //кнопка с оператором
            else if (input.Text.Length > 0 && operators.Contains(symbol))
            {
                //если последний символ в строке уже является оператором
                if (operators.Contains(input.Text[input.Text.Length - 1].ToString()))
                {
                    //если производится попытка заменить оператор, то меняем
                    if (!input.Text.EndsWith(symbol))
                        input.Text = input.Text.Substring(0, input.Text.Length - 1) + symbol;
                }
                //если последний символ в строке != оператору, то спокойно его добавляем
                else
                {
                    input.Text += symbol;
                }

            }
            //если была нажата кнопка, с очисткой окна или с оператором '='
            else
            {
                switch (symbol)
                {
                    case "=":
                        result.Text = ComputeExpression(input.Text, operators);
                        break;
                    case "C":
                        input.Text = "";
                        break;
                }
            }
        }

        //вычисляет значение выражения
        private string ComputeExpression(string expression, string[] operators)
        {
            foreach (string op in operators)
            {
                //оканчивается ли строка оператором
                while (expression.EndsWith(op))
                {
                    //удаляем последний символ в строке (который является оператором)
                    expression = expression.Remove(expression.Length - 1);
                }
            }
            string solution = new DataTable().Compute(expression, null).ToString();

            if (solution != string.Empty)
                return solution;
            else
                return "0";
        }
    }
}
