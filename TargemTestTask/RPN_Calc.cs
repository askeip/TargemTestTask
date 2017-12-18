using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TargemTestTask
{
    public class RPN_Calc
    {
        private Dictionary<char, int> priorities = new Dictionary<char, int>
        {
            {'(', 0},
            {')', 0},
            {'+', 1},
            {'-', 1},
            {'/', 2},
            {'*', 2}
        };

        private Dictionary<string, Func<double, double, double>> functions = new Dictionary<string, Func<double, double, double>>
        {
            {"+", (a, b) => a + b},
            {"-", (a, b) => a - b},
            {"*", (a, b) => a * b},
            {"/", (a, b) => a / b}
        };

        public double Calculate(string stringToCalc)
        {
            var rpnString = ToRPN(stringToCalc);

            return CountRPNString(rpnString);
        }

        private string ToRPN(string input)
        {
            var stack = new Stack<Char>();
            var sb = new StringBuilder();
            var typeOfLast = LastRead.Nothing;

            for (int i = 0; i < input.Length; i++)
            {
                var e = input[i];
                if (e == ' ')
                {
                    continue;
                }
                else if (priorities.ContainsKey(e))
                {
                    typeOfLast = HandleOperator(e, typeOfLast, stack, sb);
                }
                else if (Char.IsDigit(e) || IsPoint(e))
                {
                    if (typeOfLast == LastRead.Number)
                        throw new OperatorExpectedException();
                    var numberSb = new StringBuilder();
                    var pointsCount = 0;
                    while (Char.IsDigit(input[i]) || IsPoint(input[i]))
                    {
                        var c = input[i];
                        if (IsPoint(c))
                        {
                            c = ',';
                            pointsCount++;
                        }
                        if (pointsCount > 1)
                            throw new ArgumentException("Неправильный ввод");
                        numberSb.Append(c);
                        i++;
                        if (i == input.Length)
                            break;
                    }

                    sb.Append(numberSb);
                    sb.Append(' ');
                    i--;
                    typeOfLast = LastRead.Number;
                }
                else
                    throw new ArgumentException("Неправильный ввод");
            }
            while (stack.Count > 0)
            {
                char op = stack.Pop();
                if (op == '(')
                    throw new UnexpectedOpeningBracketException();
                sb.Append(op);
                sb.Append(' ');
            }
            if (sb.Length == 0)
                return "";
            sb.Length--;

            return sb.ToString();
        }

        private double CountRPNString(string rpnString)
        {
            if (rpnString.Length == 0)
                return 0;
            double result = 0;
            Stack<double> stack = new Stack<double>();
            var expression = rpnString.Split(' ');
            foreach (var e in expression)
            {
                if (e.Length == 1 && priorities.ContainsKey(e[0]))
                {
                    if (stack.Count < 2)
                        throw new OperatorUnexpectedException();

                    var secondNum = stack.Pop();
                    var firstNum = stack.Pop();

                    result = functions[e](firstNum, secondNum);

                    stack.Push(result);
                }
                else
                    stack.Push(double.Parse(e));
            }
            
            return stack.Pop();
        }

        private bool IsPoint(char c)
        {
            return c == '.' || c == ',';
        }

        private LastRead HandleOperator(char e, LastRead typeOfLast, Stack<char> stack, StringBuilder sb)
        {
            if (e == '(' || e == ')')
            {
                return HandleBrackets(e, typeOfLast, stack, sb);
            }
            if (typeOfLast == LastRead.Operator || typeOfLast == LastRead.OpenBracket || typeOfLast == LastRead.Nothing)
                throw new OperatorUnexpectedException();
            if (stack.Count > 0 && priorities[stack.Peek()] >= priorities[e])
            {
                sb.Append(stack.Pop());
                sb.Append(' ');
            }
            stack.Push(e);

            return LastRead.Operator;
        }

        private LastRead HandleBrackets(char e, LastRead typeOfLast, Stack<char> stack, StringBuilder sb)
        {
            if (e == '(')
            {
                if (typeOfLast == LastRead.ClosedBracket)
                    throw new UnexpectedOpeningBracketException();
                if (typeOfLast == LastRead.Number)
                    throw new OperatorExpectedException();
                stack.Push(e);
                typeOfLast = LastRead.OpenBracket;
            }
            else if (e == ')')
            {
                if (typeOfLast == LastRead.Operator || stack.Count == 0)
                    throw new UnexpectedClosingBracketException();

                char op = stack.Pop();

                while (stack.Count > 0 && op != '(')
                {
                    sb.Append(op);
                    sb.Append(' ');
                    op = stack.Pop();
                }

                if (op != '(')
                    throw new UnexpectedClosingBracketException();
                typeOfLast = LastRead.ClosedBracket;
            }
            else
                throw new Exception();

            return typeOfLast;
        }
    }

    class RPNCalculatorException : Exception
    { }

    class UnexpectedOpeningBracketException : RPNCalculatorException
    {
        public override string Message => "Открытая скобка не закрыта";
    }

    class UnexpectedClosingBracketException : RPNCalculatorException
    {
        public override string Message => "Отсутствует открытая скобка";
    }

    class OperatorExpectedException : RPNCalculatorException
    {
        public override string Message => "Отсутствует оператор между операндами";
    }

    class OperatorUnexpectedException : RPNCalculatorException
    {
        public override string Message => "Отсутствует операнд для оператора";
    }

    enum LastRead //rename
    {
        Nothing,
        Number,
        Operator,
        OpenBracket,
        ClosedBracket
    }
}
