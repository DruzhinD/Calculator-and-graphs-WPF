using System.Data;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace CalculatorWPF
{
    internal class Calculator
    {
        //массив операторов
        private string[] operators = { "-", "+", "/", "*" };

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
                CurrentExpression(EnteredKey);

        }

        private void 

        //метод, выводящий выбранный символ на экран
        private void CurrentExpression(string expression, string symbol)
        {
            //если была нажата кнопка с числом
            if (int.TryParse(symbol, out int value))
            {
                expression += value;
            }
            //кнопка с оператором
            else if (expression.Length > 0 && operators.Contains(symbol))
            {
                //если последний символ в строке уже является оператором
                if (operators.Contains(expression[expression.Length - 1].ToString()))
                {
                    //если производится попытка заменить оператор, то меняем
                    if (!expression.EndsWith(symbol))
                        expression = expression.Substring(0, expression.Length - 1) + symbol;
                }
                //если последний символ в строке != оператору, то спокойно его добавляем
                else
                {
                    expression += symbol;
                }

            }
            //если была нажата кнопка, с очисткой окна или с оператором '='
            else
            {
                switch (symbol)
                {
                    case "=":
                        result.Text = ComputeExpression(expression, operators);
                        break;
                    case "C":
                        expression = "";
                        break;
                }
            }
        }

        /// <summary>
        /// Вычисление значения выражения
        /// </summary>
        /// <param name="expression">выражение</param>
        /// <returns>вычисленное значение</returns>
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
