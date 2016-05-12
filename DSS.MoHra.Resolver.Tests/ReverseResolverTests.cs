using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace DSS.MoHra.Resolver.Tests
{
    [TestClass]
    public class ReverseResolverTests
    {
        [TestMethod]
        public void testResolve()
        {
            var resolver = new ReverseResolver();

            var factA = new ResolverFact("A");
            var factB = new ResolverFact("B");
            var factC = new ResolverFact("C");
            var factD = new ResolverFact("D");

            var rule1 = new ResolverRule("A*B", factC);
            var rule2 = new ResolverRule("D", factA);

            var answer = new ResolverAnswer(factC);

            resolver.AddFact(factA);
            resolver.AddFact(factB);
            resolver.AddFact(factC);
            resolver.AddFact(factD);
            resolver.AddRule(rule1);
            resolver.AddRule(rule2);
            resolver.AddKnownFact(factB);
            resolver.AddKnownFact(factD);
            resolver.AddAnswer(answer);

            var result = resolver.Resolve();

            Assert.AreNotEqual(null, result);
            Assert.AreNotEqual(0, result.Log.Count);
        }
    }
}
