using System.Collections;

// Не, ну мы же выебистые пацаны, сходу пишем свою имплементацию кругового массива через IReadOnlyList

/// <summary>
/// Круговой массив (read-only)
/// </summary>
public class CircularArray : IReadOnlyList<int>
{
    /// <summary>
    /// N
    /// </summary>
    private readonly int _n;

    /// <summary>
    /// Создаёт круговой массив
    /// </summary>
    /// <param name="n">N кругового массива</param>
    public CircularArray(int n)
    {
        _n = n;
    }

    /// <summary>
    /// Получает элемент кругового массива
    /// </summary>
    /// <param name="index">Индекс элемента</param>
    /// <returns>Элемент массива</returns>
    public int this[int index] => (index % _n) + 1;

    /// <summary>
    /// Безполезная хуйня
    /// </summary>
    public int Count => int.MaxValue; // Вообще массив как бы бесконечный, но int.PositiveInfinity не завезли

    /// <summary>
    /// Получает Enumerator кругового массива
    /// </summary>
    /// <returns>Enumerator кругового массива</returns>
    public IEnumerator<int> GetEnumerator()
    {
        // возвращаем Enumerator
        return new CircularArrayEnumerator(_n);
    }

    /// <summary>
    /// Получает Enumerator кругового массива
    /// </summary>
    /// <returns>Enumerator кругового массива</returns>
    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }
}

// А потом ещё выебисто дополняем всё IEnumerator'ом (хотя он тут нахуй не нужен)

/// <summary>
/// Enumerator кругового массива
/// </summary>
public class CircularArrayEnumerator : IEnumerator<int>
{
    /// <summary>
    /// N
    /// </summary>
    private readonly int _n;

    /// <summary>
    /// Текущий элемент
    /// </summary>
    private int _current;

    /// <summary>
    /// Создаёт Enumerator кругового массива
    /// </summary>
    /// <param name="n">N кругового массива</param>
    public CircularArrayEnumerator(int n)
    {
        _n = n;
        _current = 1; // если подозрение, что этого тут быть не должно, но мне похуй
    }

    /// <summary>
    /// Текущий элемент
    /// </summary>
    public int Current => _current; // Возвращаем текущий элемент

    /// <summary>
    /// Текущий элемент
    /// </summary>
    object IEnumerator.Current => _current;

    /// <summary>
    /// Безполезная хуйня
    /// </summary>
    public void Dispose()
    {
        // Ну и нахуя нужно было делать IEnumerator : IDisposable?
        // И чо мне тут писать сука?
    }

    /// <summary>
    /// Получает следующий элемент массива
    /// </summary>
    /// <returns>Всегда true</returns>
    public bool MoveNext()
    {
        if (_current == _n) // если мы в конце
            _current = 1; // возращаемся в начало
        else
            _current++; // иначе хуярим дальше

        // Массив у нас бесконечный, поэтому смело возвращаем труъ
        return true;
    }

    public void Reset()
    {
        // ресетим значение
        _current = 1;
    }
}

public class Program
{
    public static void Main(string[] args)
    {
        if (args.Length < 2) // если аргументов меньше 2
        {
            // кидаем исключение пользователю о том что он ебучий дегенерат и дифиченто
            throw new ArgumentException("Ну ахуеть, а ничо что нужно написать в аргументах 2 числа?", nameof(args));
        }

        // парсим числа
        int n, m;
        if (!(int.TryParse(args[0], out n) && int.TryParse(args[1], out m)))
        {
            // и если они не спарсились, кидаем пользователю исключение что он ебучий дегенерат и дифиченто (х2)
            throw new ArgumentException("Ну ахуеть, а ничо что нужно написать в аргументах ЦЕЛЫЕ ЧИСЛА?", nameof(args));
        }

        CircularArray ca = new CircularArray(n); // создаём круговой массив
        m--; // оптимизация от бога
        Console.Write("1"); // начинаем сходу хуярить очевидный ответ в консоль
        for (int i = m; ; i += m) // похуй, использую for как хочу
        {
            if (ca[i] == 1) // проверяем что число не является первым элементом (ca[0] всегда равен 1)
            {
                break; // выходим
            }
            Console.Write(ca[i]); // продолжаем хуярить ответ в консоль
        }
        Console.WriteLine(); // переходим на новую строку

        // радуемся что завтра вас точно не уволят, потому что никто кроме вас не ебёт как это работает
    }
}