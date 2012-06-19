﻿using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace StronglyConnectedComponents.Tests
{
    [TestClass]
    public class GraphTests
    {
        [TestMethod]
        public void EmptyGraph()
        {
            var graph = new List<Vertex<int>>();
            var detector = new StronglyConnectedComponentFinder<int>();
            var cycles = detector.DetectCycle(graph);
            Assert.AreEqual(0, cycles.Count);
        }

        [TestMethod]
        public void SingleVertex()
        {
            var graph = new List<Vertex<int>>();
            graph.Add(new Vertex<int>(1));
            var detector = new StronglyConnectedComponentFinder<int>();
            var components = detector.DetectCycle(graph);
            Assert.AreEqual(1, components.Count);
            Assert.AreEqual(1, components.IndependentComponents().Count());
            Assert.AreEqual(0, components.Cycles().Count());
        }

        [TestMethod]
        public void Linear2()
        {
            var graph = new List<Vertex<int>>();
            var vA = new Vertex<int>(1);
            var vB = new Vertex<int>(2);
            vA.Dependencies.Add(vB);
            graph.Add(vA);
            graph.Add(vB);
            var detector = new StronglyConnectedComponentFinder<int>();
            var components = detector.DetectCycle(graph);
            Assert.AreEqual(2, components.Count);
            Assert.AreEqual(2, components.IndependentComponents().Count());
            Assert.AreEqual(0, components.Cycles().Count());
        }

        [TestMethod]
        public void Linear3()
        {
            var graph = new List<Vertex<int>>();
            var vA = new Vertex<int>(1);
            var vB = new Vertex<int>(2);
            var vC = new Vertex<int>(3);
            vA.Dependencies.Add(vB);
            vB.Dependencies.Add(vC);
            graph.Add(vA);
            graph.Add(vB);
            graph.Add(vC);
            var detector = new StronglyConnectedComponentFinder<int>();
            var components = detector.DetectCycle(graph);
            Assert.AreEqual(3, components.Count);
            Assert.AreEqual(3, components.IndependentComponents().Count());
            Assert.AreEqual(0, components.Cycles().Count());
        }

        [TestMethod]
        public void Cycle2()
        {
            var graph = new List<Vertex<int>>();
            var vA = new Vertex<int>(1);
            var vB = new Vertex<int>(2);
            vA.Dependencies.Add(vB);
            vB.Dependencies.Add(vA);
            graph.Add(vA);
            graph.Add(vB);
            var detector = new StronglyConnectedComponentFinder<int>();
            var components = detector.DetectCycle(graph);
            Assert.AreEqual(1, components.Count);
            Assert.AreEqual(0, components.IndependentComponents().Count());
            Assert.AreEqual(1, components.Cycles().Count());
            Assert.AreEqual(2, components.First().Count);
        }

        [TestMethod]
        public void Cycle3()
        {
            var graph = new List<Vertex<int>>();
            var vA = new Vertex<int>(1);
            var vB = new Vertex<int>(2);
            var vC = new Vertex<int>(3);
            vA.Dependencies.Add(vB);
            vB.Dependencies.Add(vC);
            vC.Dependencies.Add(vA);
            graph.Add(vA);
            graph.Add(vB);
            graph.Add(vC);
            var detector = new StronglyConnectedComponentFinder<int>();
            var components = detector.DetectCycle(graph);
            Assert.AreEqual(1, components.Count);
            Assert.AreEqual(0, components.IndependentComponents().Count());
            Assert.AreEqual(1, components.Cycles().Count());
            Assert.AreEqual(3, components.Single().Count);
        }

        [TestMethod]
        public void TwoIsolated3Cycles()
        {
            var graph = new List<Vertex<int>>();
            var vA1 = new Vertex<int>(1);
            var vB1 = new Vertex<int>(2);
            var vC1 = new Vertex<int>(3);
            vA1.Dependencies.Add(vB1);
            vB1.Dependencies.Add(vC1);
            vC1.Dependencies.Add(vA1);
            graph.Add(vA1);
            graph.Add(vB1);
            graph.Add(vC1);

            var vA2 = new Vertex<int>(4);
            var vB2 = new Vertex<int>(5);
            var vC2 = new Vertex<int>(6);
            vA2.Dependencies.Add(vB2);
            vB2.Dependencies.Add(vC2);
            vC2.Dependencies.Add(vA2);
            graph.Add(vA2);
            graph.Add(vB2);
            graph.Add(vC2);

            var detector = new StronglyConnectedComponentFinder<int>();
            var components = detector.DetectCycle(graph);
            Assert.AreEqual(2, components.Count);
            Assert.AreEqual(0, components.IndependentComponents().Count());
            Assert.AreEqual(2, components.Cycles().Count());
            Assert.IsTrue(components.All(c => c.Count == 3));
        }

        [TestMethod]
        public void Cycle3WithStub()
        {
            var graph = new List<Vertex<int>>();
            var vA = new Vertex<int>(1);
            var vB = new Vertex<int>(2);
            var vC = new Vertex<int>(3);
            var vD = new Vertex<int>(4);
            vA.Dependencies.Add(vB);
            vB.Dependencies.Add(vC);
            vC.Dependencies.Add(vA);
            vC.Dependencies.Add(vD);
            graph.Add(vA);
            graph.Add(vB);
            graph.Add(vC);
            graph.Add(vD);
            var detector = new StronglyConnectedComponentFinder<int>();
            var components = detector.DetectCycle(graph);
            Assert.AreEqual(2, components.Count);
            Assert.AreEqual(1, components.IndependentComponents().Count());
            Assert.AreEqual(1, components.Cycles().Count());
            Assert.AreEqual(1, components.Count(c => c.Count == 3));
            Assert.AreEqual(1, components.Count(c => c.Count == 1));
            Assert.IsTrue(components.Single(c => c.Count == 1).Single() == vD);
        }
    }
}