using System;
using System.Collections.Generic;
using System.Text;
using ApprovalTests;
using ApprovalTests.Combinations;
using ApprovalTests.Reporters;
using NUnit.Framework;
using NUnit.Framework.Internal.Builders;

namespace Pairwise
{
	[TestFixture]
	[UseReporter(typeof(DiffReporter))]
	class Program
	{
		[Test]
		public void CombinationTest()
		{
			CombinationApprovals.VerifyAllCombinations(
				ParameterizedTest,
				new[]{ 1, 2, 3, 4 },
				new[]{ 2, 3, 4 },
				new[]{ 3, 4, 5 });
		}

		[Test]
		public void PairwiseTest()
		{
			PairwiseApprovals.VerifyAllPairs(
				ParameterizedTest,
				new[]{ 1, 2, 3, 4 },
				new[]{ 2, 3, 4 },
				new[]{ 3, 4, 5 });
		}

		object ParameterizedTest(int a, int b, int c)
		{
			return new ClassUnderTest().MethodUnderTest(a, b, c);
		}
	}

	static class PairwiseApprovals
	{
		public static void VerifyAllPairs<A,B,C>(
			Func<A,B,C,object> function,
			IEnumerable<A> aItems,
			IEnumerable<B> bItems,
			IEnumerable<C> cItems)
		{
			var testCases = new PairwiseStrategy().GetTestCases(new System.Collections.IEnumerable[]
			{
				aItems,
				bItems,
				cItems
			});
			StringBuilder output = new StringBuilder();
			foreach (NUnit.Framework.Interfaces.ITestCaseData testCase in testCases)
			{
				var result = function(
					(A)testCase.Arguments[0],
					(B)testCase.Arguments[1],
					(C)testCase.Arguments[2]);
				output.AppendLine(
					"[" + string.Join(",", testCase.Arguments) + "] => " +
					result);
			}
			Approvals.Verify(output);
		}
	}

	class ClassUnderTest
	{
		public int MethodUnderTest(int a, int b, int c)
		{
			return a * b + c;
		}
	}
}