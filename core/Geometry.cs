// Geometry.cs
// Практическая работа №1. Алгоритмы и структуры данных.
// Вариант 6: Точка и треугольник на плоскости.
// Содержит типы данных Point2D и Triangle.
// Автор: Студент группы 445, Сафонова Елена Андреевна. 2026 год.

namespace rps1.core
{
    /// <summary>
    /// Точка на плоскости с вещественными координатами X и Y.
    /// </summary>
    public struct Point2D
    {
        /// <summary>Координата X.</summary>
        public double X { get; }

        /// <summary>Координата Y.</summary>
        public double Y { get; }

        /// <summary>
        /// Создаёт точку с заданными координатами.
        /// </summary>
        /// <param name="x">Координата X.</param>
        /// <param name="y">Координата Y.</param>
        public Point2D(double x, double y)
        {
            X = x;
            Y = y;
        }

        public override string ToString() => $"({X}, {Y})";
    }

    /// <summary>
    /// Треугольник на плоскости, заданный тремя вершинами.
    /// </summary>
    public struct Triangle
    {
        /// <summary>Первая вершина треугольника.</summary>
        public Point2D A { get; }

        /// <summary>Вторая вершина треугольника.</summary>
        public Point2D B { get; }

        /// <summary>Третья вершина треугольника.</summary>
        public Point2D C { get; }

        /// <summary>
        /// Создаёт треугольник по трём вершинам.
        /// Предусловие: вершины не должны быть коллинеарны (иначе треугольник вырождается).
        /// </summary>
        /// <param name="a">Первая вершина.</param>
        /// <param name="b">Вторая вершина.</param>
        /// <param name="c">Третья вершина.</param>
        public Triangle(Point2D a, Point2D b, Point2D c)
        {
            A = a;
            B = b;
            C = c;
        }

        /// <summary>
        /// Вычисляет площадь треугольника через формулу Герона (знаковая площадь).
        /// Возвращает 0, если вершины коллинеарны (вырожденный треугольник).
        /// </summary>
        public double SignedArea()
        {
            return (B.X - A.X) * (C.Y - A.Y) - (C.X - A.X) * (B.Y - A.Y);
        }

        public override string ToString() => $"A{A}, B{B}, C{C}";
    }
}