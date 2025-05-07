using System;
using System.Text;

namespace StringCompressionApp
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Введите строку из маленьких латинских букв:");
            string input = Console.ReadLine();

            string compressed = Compress(input);
            Console.WriteLine($"Сжатая строка: {compressed}");

            string decompressed = Decompress(compressed);
            Console.WriteLine($"Восстановленная исходная строка: {decompressed}");

            string comparisonInputAndDecompressed = input == decompressed ? "да": "нет";          

            Console.WriteLine($"\nИсходная и восстановленная строки совпадают: {comparisonInputAndDecompressed}");
        }

        // Метод для сжатия строки
        public static string Compress(string input)
        {
            if (string.IsNullOrEmpty(input)) return input;

            StringBuilder result = new StringBuilder();
            int count = 1;
            char currentChar = input[0];

            for (int i = 1; i < input.Length; i++)
            {
                if (input[i] == currentChar)
                {
                    count++;
                }
                else
                {
                    result.Append(currentChar);
                    if (count > 1)
                        result.Append(count);

                    currentChar = input[i];
                    count = 1;
                }
            }

            result.Append(currentChar);
            if (count > 1)
                result.Append(count);

            return result.ToString();
        }

        // Метод для восстановления исходной строки
        public static string Decompress(string input)
        {
            if (string.IsNullOrEmpty(input)) return input;

            StringBuilder result = new StringBuilder();
            int i = 0;

            while (i < input.Length)
            {
                char letter = input[i++];
                StringBuilder countBuilder = new StringBuilder();

                while (i < input.Length && char.IsDigit(input[i]))
                {
                    countBuilder.Append(input[i]);
                    i++;
                }

                int count = countBuilder.Length > 0 ? int.Parse(countBuilder.ToString()) : 1;
                result.Append(new string(letter, count));
            }

            return result.ToString();
        }
    }
}