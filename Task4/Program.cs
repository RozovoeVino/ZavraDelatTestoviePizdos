﻿if (args.Length == 0) // если аргументов ноль
{
    // кидаем исключение пользователю о том что он ебучий дегенерат и дифиченто
    throw new ArgumentException("Ну ахуеть, а ничо что нужно написать название файла?", nameof(args));
}

if (!File.Exists(args[0])) // проверяем есть ли файл
{
    // и если нету, кидаем исключение пользователю о том что он ебучий дегенерат и дифиченто (х2)
    throw new ArgumentException("Ну ахуеть, а ничо такого файла нет?", nameof(args));
}

var nums = File.ReadAllLines(args[0]).Select(x => long.Parse(x)); // читаем числа из файла

// тут ожидается что мы будем дрочить массив с числами, но мы продолжаем выебываться
// поэтому высераем никому непонятные формулы используя МАТАН!!!1! ВО СЛАВУ САТАНЕ!
// (на самом деле мат. статистику, но всем похуй)

var avg = (long)Math.Round(Math.Pow((double)nums.Aggregate(1L, (x, y) => x * y), 1 / (double)nums.Count()));
var result = nums.Select(x => Math.Abs(x - avg)).Sum();
Console.WriteLine(result);