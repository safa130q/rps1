// PointInTriangleTests.cs
// Модульные тесты для алгоритма PointInTriangleChecker.
// Покрывают: точку внутри, на каждой стороне, в вершине, снаружи,
//            вырожденный треугольник, граничные случаи.

using Microsoft.VisualStudio.TestTools.UnitTesting;
using rps1.core;

namespace rps1.tests
{
    [TestClass]
    public class PointInTriangleTests
    {
        // Стандартный прямоугольный треугольник с вершинами (0,0),(4,0),(0,4)
        private static readonly Triangle StandardTriangle =
            new Triangle(new Point2D(0, 0), new Point2D(4, 0), new Point2D(0, 4));

        // ───── Точка внутри ─────

        [TestMethod]
        public void Check_PointInsideTriangle_ReturnsInside()
        {
            Point2D point = new Point2D(1, 1);
            PointPosition result = PointInTriangleChecker.Check(point, StandardTriangle);
            Assert.AreEqual(PointPosition.Inside, result);
        }

        [TestMethod]
        public void Check_CentroidOfTriangle_ReturnsInside()
        {
            // Центроид всегда строго внутри
            Point2D centroid = new Point2D((0 + 4 + 0) / 3.0, (0 + 0 + 4) / 3.0);
            PointPosition result = PointInTriangleChecker.Check(centroid, StandardTriangle);
            Assert.AreEqual(PointPosition.Inside, result);
        }

        // ───── Точка снаружи ─────

        [TestMethod]
        public void Check_PointOutsideTriangle_ReturnsOutside()
        {
            Point2D point = new Point2D(5, 5);
            PointPosition result = PointInTriangleChecker.Check(point, StandardTriangle);
            Assert.AreEqual(PointPosition.Outside, result);
        }

        [TestMethod]
        public void Check_PointFarOutside_ReturnsOutside()
        {
            Point2D point = new Point2D(-10, -10);
            PointPosition result = PointInTriangleChecker.Check(point, StandardTriangle);
            Assert.AreEqual(PointPosition.Outside, result);
        }

        [TestMethod]
        public void Check_PointBeyondHypotenuse_ReturnsOutside()
        {
            // По другую сторону от гипотенузы x+y=4
            Point2D point = new Point2D(3, 3);
            PointPosition result = PointInTriangleChecker.Check(point, StandardTriangle);
            Assert.AreEqual(PointPosition.Outside, result);
        }

        // ───── Точка на границе ─────

        [TestMethod]
        public void Check_PointOnVertexA_ReturnsOnBoundary()
        {
            Point2D point = new Point2D(0, 0);
            PointPosition result = PointInTriangleChecker.Check(point, StandardTriangle);
            Assert.AreEqual(PointPosition.OnBoundary, result);
        }

        [TestMethod]
        public void Check_PointOnVertexB_ReturnsOnBoundary()
        {
            Point2D point = new Point2D(4, 0);
            PointPosition result = PointInTriangleChecker.Check(point, StandardTriangle);
            Assert.AreEqual(PointPosition.OnBoundary, result);
        }

        [TestMethod]
        public void Check_PointOnVertexC_ReturnsOnBoundary()
        {
            Point2D point = new Point2D(0, 4);
            PointPosition result = PointInTriangleChecker.Check(point, StandardTriangle);
            Assert.AreEqual(PointPosition.OnBoundary, result);
        }

        [TestMethod]
        public void Check_PointOnSideAB_ReturnsOnBoundary()
        {
            // Середина стороны AB
            Point2D point = new Point2D(2, 0);
            PointPosition result = PointInTriangleChecker.Check(point, StandardTriangle);
            Assert.AreEqual(PointPosition.OnBoundary, result);
        }

        [TestMethod]
        public void Check_PointOnSideAC_ReturnsOnBoundary()
        {
            // Середина стороны AC
            Point2D point = new Point2D(0, 2);
            PointPosition result = PointInTriangleChecker.Check(point, StandardTriangle);
            Assert.AreEqual(PointPosition.OnBoundary, result);
        }

        [TestMethod]
        public void Check_PointOnHypotenuse_ReturnsOnBoundary()
        {
            // Середина гипотенузы: x+y=4, (2,2)
            Point2D point = new Point2D(2, 2);
            PointPosition result = PointInTriangleChecker.Check(point, StandardTriangle);
            Assert.AreEqual(PointPosition.OnBoundary, result);
        }

        // ───── Вырожденный треугольник ─────

        [TestMethod]
        public void Check_DegenerateTriangle_ReturnsDegenerateTriangle()
        {
            // Все три вершины на одной прямой
            Triangle degenerate = new Triangle(
                new Point2D(0, 0), new Point2D(1, 1), new Point2D(2, 2));
            Point2D point = new Point2D(0.5, 0.5);
            PointPosition result = PointInTriangleChecker.Check(point, degenerate);
            Assert.AreEqual(PointPosition.DegenerateTriangle, result);
        }

        // ───── Отрицательные координаты ─────

        [TestMethod]
        public void Check_TriangleWithNegativeCoords_InsideCorrect()
        {
            Triangle t = new Triangle(
                new Point2D(-3, -3), new Point2D(3, -3), new Point2D(0, 3));
            Point2D point = new Point2D(0, 0);
            PointPosition result = PointInTriangleChecker.Check(point, t);
            Assert.AreEqual(PointPosition.Inside, result);
        }
    }
}