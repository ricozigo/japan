using System;
using System.Collections.Generic;
using System.Linq;

namespace JapanCross

{
    class Program
    {
        // Размер игрового поля
        const int rows = 5;
        const int cols = 5;

        // Фигня, который нужно отгадать
        // 0 - пустая клетка, 1 - закрашенная клетка
        static int[,] picture = new int[rows, cols]
        {
{1, 1, 0, 1, 1},
{1, 1, 0, 1, 1},
{0, 0, 1, 0, 0},
{1, 0, 0, 0, 1},
{0, 1, 1, 1, 0}
        };

        // Подсказки по строкам и столбцам
        static List<int>[] rowHints = new List<int>[rows];
        static List<int>[] colHints = new List<int>[cols];

        // Игровое поле пользователя
        static char[,] board = new char[rows, cols];

        // Символы для отображения клеток
        const char empty = '.';
        const char filled = '#';
        const char crossed = '.';

        static void Main(string[] args)
        {
            //подсказки и игрового поля
            GenerateHints();
            ClearBoard();

            // Основной цикл
            bool isSolved = false;
            while (!isSolved)
            {
                // Вывод игрового поля и подсказок
                PrintBoard();

                // Ввод действия от пользователя
                Console.WriteLine("Введите координаты клетки (строка и столбец) и действие (закрасить F или очистить D):");
                Console.WriteLine("Пример: 2 4 F");
                Console.WriteLine("Для выхода из игры введите Q");
                string input = Console.ReadLine();
                if (input == "Q" || input == "q")
                {
                    break;
                }

                // Проверка корректности ввода
                if (!IsValidInput(input))
                {
                    Console.WriteLine("Неверный формат ввода. ");
                    continue;
                }

                // Обработка ввода и обновление игрового поля
                ProcessInput(input);

                // Проверка условия победы
                isSolved = IsSolved();
                if (isSolved)
                {
                    Console.WriteLine("Поздравляем! Вы отгадали рисунок!");
                    PrintBoard();
                }
            }
        }

        // Метод для генерации подсказок по рисунку
        static void GenerateHints()
        {
            for (int i = 0; i < rows; i++)
            {
                rowHints[i] = new List<int>();
                int count = 0;
                for (int j = 0; j < cols; j++)
                {
                    if (picture[i, j] == 1)
                    {
                        count++;
                    }
                    else
                    {
                        if (count > 0)
                        {
                            rowHints[i].Add(count);
                            count = 0;
                        }
                    }
                }
                if (count > 0)
                {
                    rowHints[i].Add(count);
                }
            }

            for (int j = 0; j < cols; j++)
            {
                colHints[j] = new List<int>();
                int count = 0;
                for (int i = 0; i < rows; i++)
                {
                    if (picture[i, j] == 1)
                    {
                        count++;
                    }
                    else
                    {
                        if (count > 0)
                        {
                            colHints[j].Add(count);
                            count = 0;
                        }
                    }
                }
                if (count > 0)
                {
                    colHints[j].Add(count);
                }
            }
        }

        // Метод для очистки игрового поля
        static void ClearBoard()
        {
            for (int i = 0; i < rows; i++)
            {
                for (int j = 0; j < cols; j++)
                {
                    board[i, j] = empty;
                }
            }
        }

        // Метод для вывода игрового поля и подсказок
        static void PrintBoard()
        {
            // Вывод подсказок по столбцам
            int maxColHints = colHints.Max(h => h.Count);
            for (int i = 0; i < maxColHints; i++)
            {
                Console.Write(" ");
                for (int j = 0; j < cols; j++)
                {
                    if (i < maxColHints - colHints[j].Count)
                    {
                        Console.Write(" ");
                    }
                    else
                    {
                        Console.Write(colHints[j][i - (maxColHints - colHints[j].Count)] + " ");
                    }
                }
                Console.WriteLine();
            }

            // Вывод горизонтальной линии
            Console.Write(" ");
            for (int j = 0; j < cols; j++)
            {
                Console.Write("--");
            }
            Console.WriteLine();

            // Вывод игрового поля и подсказок по строкам
            for (int i = 0; i < rows; i++)
            {
                Console.Write("| ");
                for (int j = 0; j < cols; j++)
                {
                    Console.Write(board[i, j] + " ");
                }
                Console.Write("| ");
                foreach (var hint in rowHints[i])
                {
                    Console.Write(hint + " ");
                }
                Console.WriteLine();
            }

            // Вывод горизонтальной линии
            Console.Write(" ");
            for (int j = 0; j < cols; j++)
            {
                Console.Write("--");
            }
            Console.WriteLine();
        }

        // Метод для проверки корректности ввода пользователя
        static bool IsValidInput(string input)
        {
            // Ввод должен состоять из трех частей, разделенных пробелами
            string[] parts = input.Split();
            if (parts.Length != 3)
            {
                return false;
            }

            // Первая и вторая часть должны быть целыми числами от 1 до размера поля
            int row, col;
            if (!int.TryParse(parts[0], out row) || !int.TryParse(parts[1], out col))
            {
                return false;
            }
            if (row < 1 || row > rows || col < 1 || col > cols)
            {
                return false;
            }

            // Третья часть должна быть либо D (очистить), либо F (закрасить)
            char action = parts[2][0];
            if (action != 'D' && action != 'd' && action != 'F' && action != 'f')
            {
                return false;
            }

            // Ввод корректен
            return true;
        }

        // Метод для обработки ввода пользователя и обновления игрового поля
        static void ProcessInput(string input)
        {
            // Разбиваем ввод на части
            string[] parts = input.Split();

            // Получаем координаты клетки и действие
            int row = int.Parse(parts[0]) - 1;
            int col = int.Parse(parts[1]) - 1;
            char action = parts[2][0];

            // Обновляем игровое поле в соответствии с действием
            if (action == 'D' || action == 'd')
            {
                board[row, col] = crossed;
            }
            else if (action == 'F' || action == 'f')
            {
                board[row, col] = filled;
            }
        }

        // Метод для проверки условия победы
        static bool IsSolved()
        {
            for (int i = 0; i < rows; i++)
            {
                for (int j = 0; j < cols; j++)
                {
                    // Если клетка закрашена, но не соответствует рисунку, то игра не решена
                    if (board[i, j] == filled && picture[i, j] != 1)
                    {
                        return false;
                    }

                    // Если клетка не закрашена, но соответствует рисунку, то игра не решена
                    if (board[i, j] != filled && picture[i, j] == 1)
                    {
                        return false;
                    }
                }
            }

            // Игра решена
            return true;
        }
    }
}




