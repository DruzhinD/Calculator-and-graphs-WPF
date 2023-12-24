using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CalculatorWPF
{
    internal class Calculator
    {
        private readonly string[] operators = { "+", "-", "*", "/" };

        private string expression = "";

        /// <summary>
        /// Составление строки, представляющей математическое выражение
        /// </summary>
        /// <param name="expression">текущее математическое выражение</param>
        /// <param name="symbol">новый символ</param>
        /// <returns></returns>
        public string CurrentExpression(string symbol)
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
                    //очищаем выражение при нажатии кнопки вычисления результата или очищения ввода
                    case "=":
                    case "C":
                        expression = "";
                        break;
                }
            }

            return expression;
        }

        /// <summary>
        /// Вычисление значения заданного выражения
        /// </summary>
        /// <param name="expression">полученное выражение</param>
        /// <returns>искомый ответ</returns>
        public string ComputeExpression()
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
