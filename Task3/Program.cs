using System.Text.Json;
using System.Text.Json.Serialization;

/// <summary>
/// Тест
/// </summary>
public class Test
{
    /// <summary>
    /// Id
    /// </summary>
    [JsonPropertyName("id")]
    public int Id { get; set; }

    /// <summary>
    /// Название
    /// </summary>
    [JsonPropertyName("title")]
    public string Title { get; set; }

    /// <summary>
    /// Значение теста
    /// </summary>
    [JsonPropertyName("value")]
    public string Value { get; set; }

    /// <summary>
    /// Вложенные тесты
    /// </summary>
    [JsonPropertyName("values")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)] // игнорим null при сериализации
    public Test[]? Tests { get; set; }

    /// <summary>
    /// Рекурсивно устанавливает значения из TestResults
    /// </summary>
    /// <param name="values">TestValues</param>
    public void SetValues(TestResults values)
    {
        // аки труъ челы преобразуем массив в словарь и начинаем ебашить
        SetValues(values.GetValuesAsDictionary());
    }

    /// <summary>
    /// Рекурсивно устанавливает значения из TestResults
    /// </summary>
    /// <param name="values">Словарь со значениями</param>
    private void SetValues(IDictionary<int, string> values)
    {
        // если ключ есть в словаре, то ставим значение (если нет, то похуй)
        if (values.ContainsKey(Id))
            Value = values[Id];

        if (Tests != null) // если есть вложенные элементы
            foreach (var value in Tests) // то проходимся по ним
                value.SetValues(values);
    }
}

/// <summary>
/// Результаты теста
/// </summary>
public class TestResults
{
    /// <summary>
    /// Значения тестов
    /// </summary>
    [JsonPropertyName("values")]
    public TestValue[] Values { get; set; }

    /// <summary>
    /// Преобразует результаты тестов в словарь
    /// </summary>
    /// <returns>Словарь с результами тестов</returns>
    public Dictionary<int, string> GetValuesAsDictionary() =>
        new Dictionary<int, string>(Values.Select(x => new KeyValuePair<int, string>(x.Id, x.Value)));

    /// <summary>
    /// Значение теста
    /// </summary>
    public class TestValue
    {
        /// <summary>
        /// Id
        /// </summary>
        [JsonPropertyName("id")]
        public int Id { get; set; }

        /// <summary>
        /// Значение
        /// </summary>
        [JsonPropertyName("value")]
        public string Value { get; set; }
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

        var tests = JsonSerializer.Deserialize<Test>(File.ReadAllText(args[0])); // читаем тесты
        var testResults = JsonSerializer.Deserialize<TestResults>(File.ReadAllText(args[1])); // читаем результаты
        tests!.SetValues(testResults!); // опа!

        File.WriteAllText("report.json", JsonSerializer.Serialize(tests)); // сериализуем и сохраняем нахуй
    }
}