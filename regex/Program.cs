using System.Text.RegularExpressions;
using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Jobs;
using BenchmarkDotNet.Running;

BenchmarkRunner.Run<RegexBenchmarks>();

[SimpleJob(RuntimeMoniker.Net60)]
[SimpleJob(RuntimeMoniker.Net70)]
[MemoryDiagnoser(false)]
public partial class RegexBenchmarks
{
    private const string CurrencyRegex = @"\p{Sc}+\s*\d+";
    private readonly Regex _currencyRegex = new(CurrencyRegex);
    private readonly Regex _compiledCurrencyRegex = new(CurrencyRegex, RegexOptions.Compiled);

#if NET7_0
    [GeneratedRegex(CurrencyRegex)]
    private static partial Regex NewGeneratedRegex();

    private readonly Regex _generatedCurrencyRegex = NewGeneratedRegex();
#endif


    [Params("$ 5", "5 €")]
    public string PotentialCurrency { get; set; } = default!;

    [Benchmark]
    public void New_Instance()
    {
        new Regex(CurrencyRegex).IsMatch(PotentialCurrency);
    }

    [Benchmark]
    public void Cached_Instance()
    {
        _currencyRegex.IsMatch(PotentialCurrency);
    }

    [Benchmark]
    public void Static_Method()
    {
        Regex.IsMatch(PotentialCurrency, CurrencyRegex);
    }

    [Benchmark]
    public void New_Compiled_Instance()
    {
        new Regex(CurrencyRegex, RegexOptions.Compiled).IsMatch(PotentialCurrency);
    }

    [Benchmark]
    public void Cached_Compiled_Instance()
    {
        _compiledCurrencyRegex.IsMatch(PotentialCurrency);
    }

    [Benchmark]
    public void New_Generated_Instance()
    {
        #if NET7_0
        NewGeneratedRegex().IsMatch(PotentialCurrency);
        #endif
    }

    [Benchmark]
    public void New_Cached_Generated_Instance()
    {
        #if NET7_0
        _generatedCurrencyRegex.IsMatch(PotentialCurrency);
        #endif
    }
}
