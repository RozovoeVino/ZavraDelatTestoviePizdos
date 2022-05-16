// Хуярим интерфейс для точки

public interface IPoint
{
    /// <summary>
    /// Позиция X
    /// </summary>
    float X { get; set; }

    /// <summary>
    /// Позиция Y
    /// </summary>
    float Y { get; set; }
}

// Хуярим саму точку

/// <summary>
/// Точка
/// </summary>
public struct Point : IPoint
{
    /// <summary>
    /// Создает точку
    /// </summary>
    /// <param name="x">Позиция X</param>
    /// <param name="y">Позиция Y</param>
    public Point(float x, float y)
    {
        this.x = x;
        this.y = y;
    }

    /// <summary>
    /// Позиция X
    /// </summary>
    private float x;

    /// <summary>
    /// Позиция Y
    /// </summary>
    private float y;

    /// <summary>
    /// Позиция X
    /// </summary>
    public float X { get => x; set => x = value; }

    /// <summary>
    /// Позиция Y
    /// </summary>
    public float Y { get => y; set => y = value; }

    /// <summary>
    /// Получает расстояние между двумя точками
    /// </summary>
    /// <param name="point1">Первая точка</param>
    /// <param name="point2">Вторая точка</param>
    /// <returns>Расстояние между точками</returns>
    public static float Distance(IPoint point1, IPoint point2)
    {
        float x = point2.X - point1.X;
        float y = point2.Y - point1.Y;
        return (float)Math.Sqrt(x * x + y * y);
    }
}

// ...и тут же хуярим структуру для круга

/// <summary>
/// Окружность
/// </summary>
public struct Circle : IPoint
{
    /// <summary>
    /// Создает окружность
    /// </summary>
    /// <param name="x">Позиция X</param>
    /// <param name="y">Позиция Y</param>
    /// <param name="radius">Радиус</param>
    public Circle(float x, float y, float radius)
    {
        this.x = x;
        this.y = y;
        this.radius = radius;
    }

    /// <summary>
    /// Позиция X
    /// </summary>
    private float x;

    /// <summary>
    /// Позиция Y
    /// </summary>
    private float y;

    /// <summary>
    /// Радиус
    /// </summary>
    private float radius;

    /// <summary>
    /// Позиция X
    /// </summary>
    public float X { get => x; set => x = value; }

    /// <summary>
    /// Позиция Y
    /// </summary>
    public float Y { get => y; set => y = value; }

    /// <summary>
    /// Радиус
    /// </summary>
    public float Radius { get => radius; set => radius = value; }
}

// Хуярим ридеры для файлов (напоминаю, мы выебистые)

/// <summary>
/// Ридер для файла с координатами круга
/// </summary>
public class CircleFileReader : IDisposable
{
    private readonly TextReader _reader;

    /// <summary>
    /// Создаёт ридер
    /// </summary>
    /// <param name="reader">TextReader файла</param>
    public CircleFileReader(TextReader reader)
    {
        _reader = reader;
    }

    /// <summary>
    /// Читает координаты круга из файла
    /// </summary>
    /// <returns>Круг</returns>
    /// <exception cref="FormatException">Вылетает если файл хуйня</exception>
    public Circle ReadCircle()
    {
        var coords = _reader.ReadLine()!.Split(' ');
        var radius = _reader.ReadLine();
        try
        {
            return new Circle(float.Parse(coords[0]), float.Parse(coords[1]), float.Parse(radius));
        } 
        catch (FormatException e)
        {
            throw new FormatException("Пиздос, файл хуйня", e);
        }
    }

    public void Dispose()
    {
        _reader.Dispose(); // уничтожаем TextReader вместе с нашим ридером
    }
}

/// <summary>
/// Ридер для файла с точками
/// </summary>
public class PointsFileReader : IDisposable
{
    private readonly TextReader _reader;

    /// <summary>
    /// Создаёт ридер
    /// </summary>
    /// <param name="reader">TextReader файла</param>
    public PointsFileReader(TextReader reader)
    {
        _reader = reader;
    }

    /// <summary>
    /// Читает координаты точек из файла
    /// </summary>
    /// <returns>Массив точек</returns>
    /// <exception cref="FormatException">Вылетает если файл хуйня</exception>
    public Point[] ReadPoints()
    {
        var lines = _reader.ReadToEnd().Split('\n');
        var points = new Point[lines.Length];
        try
        {
            for (int i = 0; i < lines.Length; i++)
            {
                var coords = lines[i].Split(' ');
                points[i] = new Point(float.Parse(coords[0]), float.Parse(coords[1]));
            }
        }
        catch (FormatException e)
        {
            throw new FormatException("Пиздос, файл хуйня", e);
        }
        return points;
    }

    public void Dispose()
    {
        _reader.Dispose(); // уничтожаем TextReader вместе с нашим ридером
    }
}

public static class Program
{
    public static void Main(string[] args)
    {
        if (args.Length < 2) // если аргументов меньше 2
        {
            // кидаем исключение пользователю о том что он ебучий дегенерат и дифиченто
            throw new ArgumentException("Ну ахуеть, а ничо что нужно написать название двух файлов?", nameof(args));
        }

        if (!(File.Exists(args[0]) && File.Exists(args[1]))) // проверяем есть ли файлы
        {
            // и если нету, кидаем исключение пользователю о том что он ебучий дегенерат и дифиченто (х2)
            throw new ArgumentException("Ну ахуеть, а ничо таких файлов нет?", nameof(args));
        }

        // Выебисто читаем файл с кругом
        Circle circle;
        using (var reader = new CircleFileReader(File.OpenText(args[0])))
        {
            circle = reader.ReadCircle();
        }

        // ...и продолжаем выебисто читать файл с точками
        Point[] points;
        using (var reader = new PointsFileReader(File.OpenText(args[1])))
        {
            points = reader.ReadPoints();
        }

        // А теперь тупа чекаем каждую точку
        foreach (var point in points)
        {
            var distance = Point.Distance(circle, point);
            if (distance > circle.Radius)
            {
                Console.WriteLine("2");
            }
            else if (distance == circle.Radius)
            {
                Console.WriteLine("0");
            }
            else
            {
                Console.WriteLine("1");
            }
        }
    }
}