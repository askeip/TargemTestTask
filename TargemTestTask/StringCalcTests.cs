using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentAssertions;

namespace TargemTestTask
{
    [TestFixture]
    class StringCalcTests
    {
        private RPN_Calc calc = new RPN_Calc();

        [TestCase]
        public void EmptyEntryTest()
        {
            Assert.AreEqual(calc.Calculate(""), 0);
        }

        [TestCase]
        public void OpenBracketOnlyTest()
        {
            Assert.Throws<UnexpectedOpeningBracketException>(() => calc.Calculate("("));
        }

        [TestCase]
        public void ClosedBracketOnlyTest()
        {
            Assert.Throws<UnexpectedClosingBracketException>(() => calc.Calculate(")"));
        }

        [TestCase]
        public void OperatorOnlyTest()
        {
            Assert.Throws<OperatorUnexpectedException>(() => calc.Calculate("/"));
        }

        [TestCase]
        public void EmptyBracketsTest()
        {
            Assert.AreEqual(calc.Calculate("()"), 0);
        }

        [TestCase]
        public void NoOperatorsBetweenBracketsTest()
        {
            Assert.Throws<UnexpectedOpeningBracketException>(() => calc.Calculate("(2)()"));
        }

        [TestCase]
        public void OperatorExpectedTest()
        {
            Assert.Throws<OperatorExpectedException>(() => calc.Calculate("2,2 3"));
        }

        [TestCase]
        public void NoBracketsExpressionTest()
        {
            Assert.AreEqual(-21, calc.Calculate("2+3/6*14-30"));
        }

        [TestCase]
        public void WrongDoubleTest()
        {
            Assert.Throws<ArgumentException>(() => calc.Calculate("2,2.1"));
        }

        [TestCase]
        public void DoubleParsingTest()
        {
            calc.Calculate("2,2 - 4.3 ").Should().BeApproximately(-2.1, 0.001);
        }

        [TestCase]
        public void HardExpressionTest()
        {
            calc.Calculate("((2*34-1834387/12)-13.43141)-5*123*(643/13/13)")
                .Should()
                .BeApproximately(-155150.9259859369, 0.0000000001);
        }

        //[TestCase]
    }
}