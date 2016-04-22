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
    }
}
