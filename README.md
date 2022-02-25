# dotnet-config-performance-issue
Repo to show performance impact of large config files in .NET Core

```
// * Summary *

BenchmarkDotNet=v0.13.1, OS=Windows 10.0.19044.1526 (21H2)
AMD Ryzen 9 5900X, 1 CPU, 24 logical and 12 physical cores
.NET SDK=6.0.102
  [Host]     : .NET 6.0.2 (6.0.222.6406), X64 RyuJIT
  DefaultJob : .NET 6.0.2 (6.0.222.6406), X64 RyuJIT


|                     Method |        ConfigFile | UseCache |      Mean |     Error |    StdDev |
|--------------------------- |------------------ |--------- |----------:|----------:|----------:|
| GetObjectFromConfiguration | /largeconfig.json |    False | 90.313 us | 0.8263 us | 0.7325 us |
| GetObjectFromConfiguration | /largeconfig.json |     True |  1.996 us | 0.0083 us | 0.0074 us |
| GetObjectFromConfiguration | /smallconfig.json |    False |  1.959 us | 0.0072 us | 0.0067 us |
| GetObjectFromConfiguration | /smallconfig.json |     True |  2.052 us | 0.0171 us | 0.0160 us |

// * Hints *
Outliers
  ConfigurationBench.GetObjectFromConfiguration: Default -> 1 outlier  was  removed (92.77 us)
  ConfigurationBench.GetObjectFromConfiguration: Default -> 1 outlier  was  removed (2.02 us)

// * Legends *
  ConfigFile : Value of the 'ConfigFile' parameter
  UseCache   : Value of the 'UseCache' parameter
  Mean       : Arithmetic mean of all measurements
  Error      : Half of 99.9% confidence interval
  StdDev     : Standard deviation of all measurements
  1 us       : 1 Microsecond (0.000001 sec)
```
