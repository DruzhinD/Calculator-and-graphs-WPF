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

        private Calculator calculator;

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            calculator = new Calculator();
            foreach (UIElement button in CalculatorPanel.Children)
            {
                //проверка на соответствие типов
                if (button is Button)
                    ((Button)button).Click += ButtonHandler;
            }
            //переводим фокус на панель калькулятора, чтобы активировалась клавиатура
            CalculatorTab.Focus();
            CalculatorTab.KeyDown += KeyboardHandler;
        }

        private void ButtonHandler(object sender, RoutedEventArgs e)
        {
            //считываем надпись на кнопке
            string content = ((Button)e.OriginalSource).Content.ToString();
            if (content == "=")
                result.Text = calculator.ComputeExpression();
            input.Text = calculator.CurrentExpression(content);
            //content = ((Button)sender).Content.ToString();
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
                    result.Text = calculator.ComputeExpression();
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
                input.Text = calculator.CurrentExpression(EnteredKey);
        }
    }
}
