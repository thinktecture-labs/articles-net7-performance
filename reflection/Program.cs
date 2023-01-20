using System.Reflection;
using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Jobs;
using BenchmarkDotNet.Running;

BenchmarkRunner.Run<ReflectionBenchmarks>();

[SimpleJob(RuntimeMoniker.Net60)]
[SimpleJob(RuntimeMoniker.Net70)]
[MemoryDiagnoser(false)]
public class ReflectionBenchmarks
{
    private MethodInfo _method;
    private static void MyMethod() { }

    [GlobalSetup]
    public void Setup() => _method = typeof(ReflectionBenchmarks).GetMethod(nameof(MyMethod), BindingFlags.NonPublic | BindingFlags.Static);

    [Benchmark]
    public void MethodInfoInvoke() => _method.Invoke(null, null);

    [Benchmark]
    public Type GetUnderlyingType() => Enum.GetUnderlyingType(typeof(DayOfWeek));
}
