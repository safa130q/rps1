// PointInTriangleChecker.cs
// Практическая работа №1. Алгоритмы и структуры данных.
// Вариант 6: Алгоритм определения принадлежности точки треугольнику.
// Алгоритм: метод барицентрических координат (знаки векторных произведений).
// Автор: Студент группы 445, Сафонова Елена Андреевна. 2026 год.

using System;

namespace rps1.core
{
    /// <summary>
    /// Результат проверки принадлежности точки треугольнику.
    /// </summary>
    public enum PointPosition
    {
        /// <summary>Точка строго внутри треугольника.</summary>
        Inside,

        /// <summary>Точка лежит на одной из сторон треугольника.</summary>
        OnBoundary,

        /// <summary>Точка снаружи треугольника.</summary>
        Outside,

        /// <summary>Треугольник вырожден (вершины коллинеарны).</summary>
        DegenerateTriangle
    }

    /// <summary>
    /// Содержит метод для определения положения точки относительно треугольника.
    /// </summary>
    public static class PointInTriangleChecker
    {
        // Погрешность для сравнения вещественных чисел
        private const double Epsilon = 1e-10;

        /// <summary>
        /// Определяет положение точки P относительно треугольника ABC.
        ///
        /// Алгоритм (знаки косых произведений):
        /// Вычисляются знаки z-компонент векторных произведений:
        ///   d1 = sign(cross(AB, AP))
        ///   d2 = sign(cross(BC, BP))
        ///   d3 = sign(cross(CA, CP))
        /// Если все три знака совпадают — точка внутри.
        /// Если один из знаков равен нулю — точка на стороне.
        /// Иначе — точка снаружи.
        ///
        /// Предусловие: треугольник не вырожден.
        /// Постусловие: возвращает корректное значение PointPosition.
        /// </summary>
        /// <param name="point">Проверяемая точка.</param>
        /// <param name="triangle">Треугольник.</param>
        /// <returns>Положение точки относительно треугольника.</returns>
        public static PointPosition Check(Point2D point, Triangle triangle)
        {
            // Проверка вырожденности треугольника
            double area = triangle.SignedArea();
            if (Math.Abs(area) < Epsilon)
                return PointPosition.DegenerateTriangle;

            double d1 = CrossSign(point, triangle.A, triangle.B);
            double d2 = CrossSign(point, triangle.B, triangle.C);
            double d3 = CrossSign(point, triangle.C, triangle.A);

            bool hasNeg = (d1 < -Epsilon) || (d2 < -Epsilon) || (d3 < -Epsilon);
            bool hasPos = (d1 > Epsilon) || (d2 > Epsilon) || (d3 > Epsilon);
            bool hasZero = (Math.Abs(d1) <= Epsilon) ||
                           (Math.Abs(d2) <= Epsilon) ||
                           (Math.Abs(d3) <= Epsilon);

            // Если знаки d1, d2, d3 и положительные, и отрицательные — точка снаружи
            if (hasNeg && hasPos)
                return PointPosition.Outside;

            // Если хотя бы один знак равен нулю — точка на границе
            if (hasZero)
                return PointPosition.OnBoundary;

            return PointPosition.Inside;
        }

        /// <summary>
        /// Вычисляет z-компоненту векторного произведения (P - A) × (B - A).
        /// Используется для определения ориентации точки P относительно прямой AB.
        /// </summary>
        /// <param name="p">Проверяемая точка.</param>
        /// <param name="a">Начало стороны.</param>
        /// <param name="b">Конец стороны.</param>
        /// <returns>Значение косого произведения.</returns>
        private static double CrossSign(Point2D p, Point2D a, Point2D b)
        {
            return (p.X - b.X) * (a.Y - b.Y) - (a.X - b.X) * (p.Y - b.Y);
        }
    }
}