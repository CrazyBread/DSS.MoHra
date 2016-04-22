using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace DSS.MoHra.Resolver.Tests
{
    [TestClass]
    public class DirectResolverTests
    {
        [TestMethod]
        public void DRT_AddFacts()
        {
            var resolver = new DirectResolver();
            resolver.AddFact(new ResolverFact("A"));
            Assert.AreEqual(1, resolver.Facts.Count);
        }

        [TestMethod]
        public void DRT_AddRules()
        {
            var resolver = new DirectResolver();
            resolver.AddRule(new ResolverRule("A", new ResolverFact("B")));
            Assert.AreEqual(1, resolver.Rules.Count);
        }

        [TestMethod]
        public void DRT_Resolve()
        {
            var resolver = new DirectResolver();

            var factA = new ResolverFact("A");
            var factB = new ResolverFact("B");
            var factC = new ResolverFact("C");

            var rule1 = new ResolverRule("A+B", factC);
            var rule2 = new ResolverRule("B", factA);

            resolver.AddFact(factA);
            resolver.AddFact(factB);
            resolver.AddFact(factC);
            resolver.AddRule(rule1);
            resolver.AddRule(rule2);
            resolver.AddKnownFact(factB);

            var result = resolver.Resolve();

            Assert.AreNotEqual(null, result);
        }
    }
}
