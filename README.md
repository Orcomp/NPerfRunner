NPerfRunner: Run your NPerf performance tests
=============================================

NPerf Runner is a easy-to-use **performance and memory benchmark runner for [NPerf Framework][NPerfProject]**.

NPerf Runner lets you **easely run execution time and memory performance tests for your algorithms** and methods and **visually
compare** several implementations of an interface by plotting a **2D multiline chart**.

Using this framework, you can perform **time and memory algorithm complexity experimental analysis** or test theoretical
complexity results given by [Big O Notation][BigONotation].

You just have to code [NUnit][]-like test classes targeting a given interface, then run NPerf Runner, and, finally,
import the assemblies that contain the implementations of that interface.

![NPerfRunner](docs/img/NPerfRunner.png)

You may take a look at the [original documentation at CodeProject][NPerfArticleCodeProject].
> Please note NPerf has been completely re-written, however it keeps fully backward compatibility with the original NPerf.



Main Features
=============
+ Simple and intuitive graphical user interface using [Windows Presentation Foundation][WPF].
+ No need to modify existing classes or methods, just import the assemblies where they are.
+ Simultaneously comparison between different methods and implementations.
+ Loading interface implementations from multiple assemblies.
+ 2D interactive multiline graphs using [Oxyplot Library][Oxyplot].
+ Linear an logarithmic axis scaling.
+ Sequential and parallel test execution.
+ Directly configurable test execution parameters.



Introduction
=============

NPerf and NPerfRunner allow you to run performance tests against existing assemblies **without having to change a line of code on the target assemblies**.

If the methods you want to performance test implements an interface, you can write a performance fixture for that interface.
Once you have a compiled fixture, you can load it into NPerfRunner as well as the assemblies you want to performance test. NPerfRunner accepts multiple target assemblies.

NPerfRunner will find all the classes in all the target assemblies that implement the desired interface and run the tests against them.
The performance results (time and memory) will be dynamically displayed on the chart.

The collection size as well as the chart axis scale (logarithmic or linear) can easily be changed.

For examples on how to write performance fixtures please have a look at [Orcomp repository][OrcompSamples].



Setup in 5 steps
================

#### Step 0: Requirements.
+ Visual Studio 2012 with NuGet package manager.

#### Step 1: Download NPerf and NPerfRunner solutions from GitHub.
You will need these repositories:
+ NPerf Framework: https://github.com/Orcomp/NPerf
+ NPerf Runner: https://github.com/Orcomp/NPerfRunner (this one)
So download both zipped repositories or Git clone them to the same local folder.

#### Step 2: Place both solutions in the same folder.
Let's suppose your base folder is "PerformanceTest”, then the structure must be like this:
+ […]/PerformanceTest/NPerf/
+ […]/PerformanceTest/NPerfRunner/docs/
+ […]/PerformanceTest/NPerfRunner/src/
> It’s important to name “NPerf” the folder because NPerfRunner solution imports libraries from it.
> Otherwise, when you try to build NPerfRunner, it is going to fail because it can’t find required libraries.

#### Step 3: Open NPerf solution with Visual Studio and build it.
+ Open [...]/PerformanceTest/NPerf/src/Nperf.sln.
+ Build the solution.
> There may be some errors because Visual Studio can’t load missing packages.
> If so, go to TOOLS > Options... > Package Manager > General and check “Allow NuGet to download missing packages during build”. Then rebuild the solution.

#### Step 4: Open NPerfRunner solution with Visual Studio and build it.
+ Open [...]/PerformanceTest/NPerfRunner/src/NPerfRunner.sln
+ Set “NPerfRunner.Wpf” as StartUp project.
+ Build the solution and run the WPF application.

#### Step 5: Load the assemblies (dll’s) with the NPerf tests and start the tests.
+ On the NPerfRunner WPF application window, click “Load assembly...”.
+ Then browse for the assembly with the test classes.
+ Then load all the assemblies you want which contain implementations of the targeted interfaces.
+ Select the test fixtures you want to run by checking them on the tests tree panel.
+ If everything loaded correctly, click on “Start sequential” and you’ll get the graph.



How it works
============

NPerf.Fixture.ISorter example
-----------------------------
These test fixture compares different sorting algorithms for lists.
> You can find them at https://github.com/Orcomp/Orcomp/tree/master/Orc/Algorithms/Sort.


1. Unblock the file [...]/PerformanceTest/NPerfRunner/docs/sample/NPerf.Fixture.ISorter.zipb
    by right-­clicking onto it, then open “Properties”, and then click on “Unblock” at the bottom of the “General” tab.
2. Unzip [...]/PerformanceTest/NPerfRunner/docs/sample/NPerf.Fixture.ISorter.zip in the same folder.
3. Launch NPerfRunner WPF application.
4. On the main window click “Load assembly...” and browse for the assembly with the test fixture: [...]/NPerfRunner/docs/sample/NPerf.Fixture.ISorter/NPerf.Fixture.ISorter.dll.
5. Then, load the assembly [...]/NPerfRunner/docs/sample/NPerf.Fixture.ISorter/Orc.dll.

You should get a picture like this:

![NPerfRunner](docs/img/NPerfRunner.png)

> The chart uses the [Oxyplot library][Oxyplot], so you can zoom, pan, click on a line to see tooltips, etc...
> Please refere to the oxyplot homepage for shortcut keys.

NPerf.Fixture.IList example
---------------------------

This sample test fixture compares the performance of several System.Collections.Generic.IList<T> implementations from two libraries:
+ **.NET Base Class Library (BCL)**: Located in the assembly mscorlib.dll.
    http://msdn.microsoft.com/en-us/library/gg145045.aspx
+ **C5 Library**: There's a copy into NPerfRunner/src/libs folder for testing.
    Homepage: http://www.itu.dk/research/c5/
    GitHub repository: https://github.com/sestoft/C5/

Into NPerfRunner solution there is a project called NPerf.Fixture.IList which contains these performance tests.
Into this project there is a class called IListPerfs that has the test fixtures.

*PerfTester* attribute tells NPerf framework this class is a test fixture, in this case, for the target type
IList<int>. 20 is the number of times a test method is going to be executed.
```C#
[PerfTester(
    typeof(IList<int>),
    20,
    Description = "IList operations benchmark",
    FeatureDescription = "Collection size")]
public class IListPerfs
{
    ...
}
```

*PerfSetUp* attribute marks a method that prepares the IList<int> implementation instance before each running of a
test method in a fixture.
In this case, it adds as many elements to the list as the size calculated for the given test iteration (*testIndex*).
```C#
[PerfSetUp]
public void SetUp(int testIndex, IList<int> list)
{
    this.count = this.CollectionCount(testIndex);

    for (var i = 0; i < this.count; i++)
    {
        list.Add(i);
    }
}
```

*PerfTest* attribute indicates this is a test method. In this case, it simply adds a random integer to the list.
```C#
[PerfTest]
public void Add(IList<int> list)
{
    list.Add(this.random.Next());
}
```

BCL versus C5 IList&lt;T&gt; implementations
--------------------------------------
Compared implementations:
+ BCL: List&lt;T&gt;
+ C5: ArrayList&lt;T&gt;
+ C5: HashedArrayList&lt;T&gt;
+ C5: LinkedList&lt;T&gt;
+ C5: HashedLinkedList&lt;T&gt;

#### Comparison for Insert method.
Chart for insertion test. In this case, inserting an element at the beginning of the list.

> The chart shows how C5 HashedArrayList has linear time complexity for inserting at the beginning,
> while the rest have constant time complexity.

![NPerfRunner](/docs/img/NPerf.Fixture.IList_InsertAtTheBeginning_BCLvsC5_Time.png)

*You can see theoretical complexity for C5 IList implementations methods at [C5 Library Technical Report][C5TR] (Chapter 12, page 233)*.

#### Comparison for Contains method.
Chart for *contains* test. This method tests whether an element is in the list or not.

![NPerfRunner](/docs/img/NPerf.Fixture.IList_Contains_BCLvsC5_Time.png)



Troubleshooting
===============

#### Visual Studio cannot find package Microsoft.Bcl.Build.1.0.8.
Visual Studio cannot load NPerfRunner nor NPerfRunner.Wpf projects because it can't find the package Microsoft.Bcl.Build.1.0.8.
Try this:
+ Enable NuGet package restore and reload the solution. 
+ If it still can't load it, run `nuget install [...]/NPerfRunner/src/NPerfRunner/packages.config` in a console.

#### Exception raised when loading C5 library.
C5 library targets .NET Portable Class library (PCL), that's why NPerfRunner can't load referenced assemblies from it.
The exception may tell you it couldn't load *System.Core version=2.0.5* (part of the .NET PCL).
This will happen if you install C5 nuget package or you build the solution from the official C5 repositories.
So try this:
+ Use C5 assembly at [...]/NPerfRunner/src/libs/C5/C5.dll (targeted to .NET Framework 4).


<!-- LINKED REFERENCES -->
[NPerfProject]: https://github.com/Orcomp/NPerf
[C5TR]: http://www.itu.dk/research/c5/latest/ITU-TR-2006-76.pdf "Chapter 12, Performance details (page 233)"
[NPerfArticleCodeProject]: http://www.codeproject.com/Articles/5945/NPerf-A-Performance-Benchmark-Framework-for-NET
[NUnit]: http://www.nunit.org/ "NUnit, Unit testing framework for .NET"
[BigONotation]: http://en.wikipedia.org/wiki/Big_O_notation
[Oxyplot]: http://oxyplot.codeplex.com/ "Oxyplot chart library for .NET"
[WPF]: http://msdn.microsoft.com/es-es/library/ms754130.aspx "Windows Presentation Foundation"
[OrcompSamples]: https://github.com/Orcomp/Orcomp
[NPerfRepo]: https://github.com/Orcomp/NPerf
[NPerfRunnerRepo]: https://github.com/Orcomp/NPerfRunner
