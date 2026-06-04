// Program.cs
// Практическая работа №1. Алгоритмы и структуры данных.
// Вариант 6: Для заданной точки и треугольника на плоскости определить,
//            принадлежит ли точка треугольнику.
// Язык: C#
// Автор: Студент группы XXXX, Фамилия Имя Отчество. 2024 год.

using System;
using System.Globalization;
using System.IO;
using rps1.core;

namespace rps1
{
    /// <summary>
    /// Точка входа в программу. Содержит главный цикл, меню и ввод/вывод.
    /// </summary>
    class Program
    {
        // Количество координат вершины треугольника в файле (xA yA xB yB xC yC)
        private const int TriangleCoordCount = 6;

        // Количество координат точки в файле (xP yP)
        private const int PointCoordCount = 2;

        // Минимальное количество строк во входном файле
        private const int MinFileLineCount = 2;

        // Индекс строки с данными треугольника во входном файле
        private const int TriangleLineIndex = 0;

        // Индекс строки с данными точки во входном файле
        private const int PointLineIndex = 1;

        // Номер строки треугольника для сообщений об ошибках (1-based)
        private const int TriangleLineNumber = 1;

        // Номер строки точки для сообщений об ошибках (1-based)
        private const int PointLineNumber = 2;

        // Индексы координат вершины A в строке файла
        private const int AxIndex = 0;
        private const int AyIndex = 1;

        // Индексы координат вершины B в строке файла
        private const int BxIndex = 2;
        private const int ByIndex = 3;

        // Индексы координат вершины C в строке файла
        private const int CxIndex = 4;
        private const int CyIndex = 5;

        // Индексы координат точки P в строке файла
        private const int PxIndex = 0;
        private const int PyIndex = 1;
        static void Main(string[] args)
        {
            PrintWelcome();

            bool running = true;
            while (running)
            {
                PrintMenu();
                string choice = Console.ReadLine()?.Trim() ?? string.Empty;

                switch (choice)
                {
                    case "1":
                        RunConsoleInput();
                        break;
                    case "2":
                        RunFileInput();
                        break;
                    case "3":
                        PrintWelcome();
                        break;
                    case "0":
                        running = false;
                        Console.WriteLine("Программа завершена.");
                        break;
                    default:
                        Console.WriteLine("Ошибка: неверный пункт меню. Введите 0, 1, 2 или 3.");
                        break;
                }
            }
        }

        // ─────────────────────────── UI ───────────────────────────

        /// <summary>
        /// Выводит приветственное сообщение с описанием программы.
        /// </summary>
        static void PrintWelcome()
        {
            Console.WriteLine();
            Console.WriteLine("╔══════════════════════════════════════════════════════════╗");
            Console.WriteLine("║     Практическая работа №1. Алгоритмы и структуры данных ║");
            Console.WriteLine("║  Вариант 6: Принадлежность точки треугольнику.           ║");
            Console.WriteLine("║  Автор: Студент группы XXXX, Фамилия Имя Отчество.       ║");
            Console.WriteLine("║  Задача: по заданным координатам треугольника и точки    ║");
            Console.WriteLine("║  определить, лежит ли точка внутри треугольника,         ║");
            Console.WriteLine("║  на его границе или снаружи.                             ║");
            Console.WriteLine("╚══════════════════════════════════════════════════════════╝");
        }

        /// <summary>
        /// Выводит главное меню.
        /// </summary>
        static void PrintMenu()
        {
            Console.WriteLine();
            Console.WriteLine("Меню:");
            Console.WriteLine("  1 - Ввод данных с клавиатуры");
            Console.WriteLine("  2 - Ввод данных из файла");
            Console.WriteLine("  3 - О программе");
            Console.WriteLine("  0 - Выход");
            Console.Write("Ваш выбор: ");
        }

        // ─────────────────────────── Режимы работы ───────────────────────────

        /// <summary>
        /// Режим ввода данных с клавиатуры.
        /// </summary>
        static void RunConsoleInput()
        {
            Console.WriteLine();
            Console.WriteLine("=== Ввод с клавиатуры ===");

            Triangle triangle = InputTriangle();
            Point2D point = InputPoint("проверяемой");

            ProcessAndPrint(triangle, point, outputPath: null);
        }

        /// <summary>
        /// Режим ввода данных из файла с сохранением результата.
        /// Формат файла:
        ///   Строка 1: x_A y_A x_B y_B x_C y_C  (вершины треугольника)
        ///   Строка 2: x_P y_P                   (проверяемая точка)
        /// </summary>
        static void RunFileInput()
        {
            Console.WriteLine();
            Console.WriteLine("=== Ввод из файла ===");

            string inputPath = InputFilePath("Путь к входному файлу: ", forWriting: false);

            try
            {
                string[] lines = File.ReadAllLines(inputPath);

                if (lines.Length < MinFileLineCount)
                    throw new FormatException(
                        "Файл должен содержать не менее 2 строк:\n" +
                        "  Строка 1: x_A y_A x_B y_B x_C y_C\n" +
                        "  Строка 2: x_P y_P");

                Triangle triangle = ParseTriangleFromLine(lines[TriangleLineIndex], lineNumber: TriangleLineNumber);
                Point2D point = ParsePointFromLine(lines[PointLineIndex], lineNumber: PointLineNumber);

                Console.WriteLine($"Треугольник: {triangle}");
                Console.WriteLine($"Точка      : {point}");

                string outputPath = InputFilePath("Путь к файлу для сохранения результата: ", forWriting: true);

                ProcessAndPrint(triangle, point, outputPath);
            }
            catch (FileNotFoundException)
            {
                Console.WriteLine($"Ошибка: файл '{inputPath}' не найден.");
            }
            catch (FormatException ex)
            {
                Console.WriteLine($"Ошибка формата данных: {ex.Message}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка при чтении файла: {ex.Message}");
            }
        }

        // ─────────────────────────── Обработка ───────────────────────────

        /// <summary>
        /// Выполняет вычисление и вывод результата на экран и опционально в файл.
        /// </summary>
        /// <param name="triangle">Треугольник.</param>
        /// <param name="point">Проверяемая точка.</param>
        /// <param name="outputPath">Путь к файлу вывода или null.</param>
        static void ProcessAndPrint(Triangle triangle, Point2D point, string outputPath)
        {
            PointPosition position = PointInTriangleChecker.Check(point, triangle);
            string resultText = FormatResult(point, triangle, position);

            Console.WriteLine();
            Console.WriteLine(resultText);

            if (!string.IsNullOrEmpty(outputPath))
            {
                try
                {
                    string fileContent =
                        $"Треугольник: {triangle}\n" +
                        $"Точка      : {point}\n\n" +
                        resultText;

                    File.WriteAllText(outputPath, fileContent);
                    Console.WriteLine($"Результат сохранён в файл: {outputPath}");
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Ошибка при сохранении файла: {ex.Message}");
                }
            }
        }

        /// <summary>
        /// Формирует текст результата по положению точки.
        /// </summary>
        static string FormatResult(Point2D point, Triangle triangle, PointPosition position)
        {
            switch (position)
            {
                case PointPosition.Inside:
                    return $"Точка {point} лежит ВНУТРИ треугольника {triangle}.";
                case PointPosition.OnBoundary:
                    return $"Точка {point} лежит НА ГРАНИЦЕ (стороне) треугольника {triangle}.";
                case PointPosition.Outside:
                    return $"Точка {point} лежит СНАРУЖИ треугольника {triangle}.";
                case PointPosition.DegenerateTriangle:
                    return "Ошибка: треугольник вырожден (все три вершины коллинеарны). " +
                           "Введите вершины, не лежащие на одной прямой.";
                default:
                    return "Неизвестный результат.";
            }
        }

        // ─────────────────────────── Ввод с клавиатуры ───────────────────────────

        /// <summary>
        /// Запрашивает у пользователя координаты трёх вершин треугольника.
        /// </summary>
        static Triangle InputTriangle()
        {
            Console.WriteLine("Введите координаты вершин треугольника:");
            Point2D a = InputPoint("вершины A");
            Point2D b = InputPoint("вершины B");
            Point2D c = InputPoint("вершины C");

            Triangle triangle = new Triangle(a, b, c);

            if (Math.Abs(triangle.SignedArea()) < PointInTriangleChecker.DegeneracyEpsilon)
            {
                Console.WriteLine("Предупреждение: вершины коллинеарны — треугольник вырожден.");
            }

            return triangle;
        }

        /// <summary>
        /// Запрашивает у пользователя координаты одной точки.
        /// </summary>
        /// <param name="label">Метка точки для подсказки пользователю.</param>
        static Point2D InputPoint(string label)
        {
            Console.WriteLine($"Координаты {label}:");
            double x = InputDouble("  x: ");
            double y = InputDouble("  y: ");
            return new Point2D(x, y);
        }

        /// <summary>
        /// Читает одно вещественное число с консоли.
        /// Повторяет запрос при некорректном вводе или переполнении типа double.
        /// </summary>
        /// <param name="prompt">Строка-подсказка.</param>
        static double InputDouble(string prompt)
        {
            while (true)
            {
                Console.Write(prompt);
                string raw = Console.ReadLine()?.Trim() ?? string.Empty;

                if (string.IsNullOrEmpty(raw))
                {
                    Console.WriteLine("  Ошибка: введите вещественное число (например: 3, -1.5, 0.75).");
                    continue;
                }

                if (!double.TryParse(raw,
                    NumberStyles.Float | NumberStyles.AllowLeadingSign,
                    CultureInfo.InvariantCulture,
                    out double value))
                {
                    Console.WriteLine("  Ошибка: введите вещественное число (например: 3, -1.5, 0.75).");
                    continue;
                }

                if (double.IsInfinity(value) || double.IsNaN(value))
                {
                    Console.WriteLine("  Ошибка: число слишком велико для типа double. Введите другое значение.");
                    continue;
                }

                return value;
            }
        }

        /// <summary>
        /// Читает непустую строку пути к файлу с консоли и проверяет её корректность.
        /// Повторяет запрос при пустом вводе, запрещённых символах или если путь — директория.
        /// </summary>
        /// <param name="prompt">Строка-подсказка.</param>
        /// <param name="forWriting">Если true — также проверяет, что файл не read-only.</param>
        static string InputFilePath(string prompt, bool forWriting = false)
        {
            while (true)
            {
                Console.Write(prompt);
                string path = Console.ReadLine()?.Trim() ?? string.Empty;

                if (string.IsNullOrEmpty(path))
                {
                    Console.WriteLine("  Ошибка: путь не может быть пустым.");
                    continue;
                }

                // Проверка на запрещённые символы в пути
                try
                {
                    // GetFullPath бросает исключение при недопустимых символах
                    path = Path.GetFullPath(path);
                }
                catch
                {
                    Console.WriteLine("  Ошибка: путь содержит недопустимые символы.");
                    continue;
                }

                // Проверка: не является ли путь директорией
                if (Directory.Exists(path))
                {
                    Console.WriteLine("  Ошибка: указан путь к папке, а не к файлу.");
                    continue;
                }

                // Для записи — проверить read-only (если файл уже существует)
                if (forWriting && File.Exists(path))
                {
                    FileInfo fi = new FileInfo(path);
                    if (fi.IsReadOnly)
                    {
                        Console.WriteLine("  Ошибка: файл доступен только для чтения.");
                        continue;
                    }
                }

                return path;
            }
        }

        // ─────────────────────────── Разбор файла ───────────────────────────

        /// <summary>
        /// Разбирает строку файла в треугольник.
        /// Ожидаемый формат строки: "x_A y_A x_B y_B x_C y_C"
        /// </summary>
        /// <param name="line">Строка файла.</param>
        /// <param name="lineNumber">Номер строки (для сообщений об ошибках).</param>
        static Triangle ParseTriangleFromLine(string line, int lineNumber)
        {
            double[] nums = ParseDoubles(line, lineNumber, expected: TriangleCoordCount);
            return new Triangle(
                new Point2D(nums[AxIndex], nums[AyIndex]),
                new Point2D(nums[BxIndex], nums[ByIndex]),
                new Point2D(nums[CxIndex], nums[CyIndex]));
        }

        /// <summary>
        /// Разбирает строку файла в точку.
        /// Ожидаемый формат строки: "x_P y_P"
        /// </summary>
        /// <param name="line">Строка файла.</param>
        /// <param name="lineNumber">Номер строки (для сообщений об ошибках).</param>
        static Point2D ParsePointFromLine(string line, int lineNumber)
        {
            double[] nums = ParseDoubles(line, lineNumber, expected: PointCoordCount);
            return new Point2D(nums[PxIndex], nums[PyIndex]);
        }

        /// <summary>
        /// Разбирает строку в массив вещественных чисел заданной длины.
        /// </summary>
        /// <param name="line">Входная строка.</param>
        /// <param name="lineNumber">Номер строки (для сообщений об ошибках).</param>
        /// <param name="expected">Ожидаемое количество чисел.</param>
        static double[] ParseDoubles(string line, int lineNumber, int expected)
        {
            string[] parts = line.Trim().Split(
                new[] { ' ', '\t' }, StringSplitOptions.RemoveEmptyEntries);

            if (parts.Length < expected)
                throw new FormatException(
                    $"Строка {lineNumber}: ожидается {expected} чисел, получено {parts.Length}.");

            double[] result = new double[expected];
            for (int i = 0; i < expected; i++)
            {
                if (!double.TryParse(parts[i],
                    NumberStyles.Float | NumberStyles.AllowLeadingSign,
                    CultureInfo.InvariantCulture,
                    out result[i]))
                {
                    throw new FormatException(
                        $"Строка {lineNumber}, значение {i + 1}: '{parts[i]}' не является числом.");
                }

                if (double.IsInfinity(result[i]) || double.IsNaN(result[i]))
                {
                    throw new FormatException(
                        $"Строка {lineNumber}, значение {i + 1}: '{parts[i]}' выходит за пределы типа double.");
                }
            }

            return result;
        }
    }
}