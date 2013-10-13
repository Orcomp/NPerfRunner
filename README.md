NPerfRunner
===========
The tool to run your NPerf performance tests
--------------------------------------------

NPerf Runner is a easy-to-use **performance** and **memory** benchmark runner for [NPerf Framework][NPerfProject].

NPerf Runner lets you **easily run execution time and memory performance tests for your algorithms** and methods and **visually compare** several implementations of an interface by plotting a **2D multiline chart**.

Using this framework, you can perform **time and memory algorithm complexity experimental analysis** or test theoretical complexity results given by [Big O Notation][BigONotation].

You just have to code [NUnit][]-like test classes targeting a given interface. Start NPerfRunner, and simply import the assemblies that contain the implementations of that interface.

![NPerfRunner](docs/img/NPerfRunner.png)

The original documentation for NPerf can be found [here][NPerfArticleCodeProject].

> Please note NPerf has been completely re-written, however it keeps full backward compatibility with the original project.


Main Features
=============
+ Simple and intuitive graphical user interface using [Windows Presentation Foundation][WPF].
+ **No need to modify existing classes** or methods, just import the assemblies where they are.
+ Simultaneously compare different methods and implementations.
+ Can load interface implementations from **multiple** assemblies.
+ 2D interactive multiline graphs using [Oxyplot Library][Oxyplot].
+ Linear and logarithmic axis scaling.
+ Sequential and parallel test execution.


Introduction
=============

NPerf and NPerfRunner allow you to run performance tests against existing assemblies **without having to change a line of code on the target assemblies**.

If the methods you want to performance test implement an existing interface, you can **write a performance fixture for that interface**.
Once you have a compiled fixture, you can load it into NPerfRunner as well as the assemblies you want to performance test. NPerfRunner accepts multiple target assemblies.

**NPerfRunner will find all the classes in all the target assemblies that implement the desired interface**. You can then select the ones you want performance test.

The performance results (time and memory) will be dynamically displayed on the chart.

The collection size as well as the chart axis scale (logarithmic or linear) can easily be changed from the user interface.

Examples on how to write performance fixtures [can be found at the bottom of this document in the "How it works" section](#how-it-works) or have a look at the [Orcomp repository][OrcompSamples].


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
> See also [Troubleshooting section](#troubleshooting) down in this document.

#### Step 4: Open NPerfRunner solution with Visual Studio and build it.
+ Open [...]/PerformanceTest/NPerfRunner/src/NPerfRunner.sln
+ Set “NPerfRunner.Wpf” as StartUp project.
+ Build the solution and run the WPF application.

#### Step 5: Load the assemblies (dll’s) with the NPerf tests and start the tests.
+ On the NPerfRunner WPF application window, click “Load assembly...”.
+ Then load all the assemblies you want which contain implementations of the targeted interfaces.
+ Then load the assemblies with the test classes.
+ Select the test fixtures you want to run by checking them on the tests tree panel.
+ If everything loaded correctly, click on “Start sequential” and you’ll get the graph.



How it works
============

NPerf.Fixture.ISorter example
-----------------------------
These test fixture compares different sorting algorithms for lists.
> You can find them at [https://github.com/Orcomp/Orcomp/tree/master/Orc/Algorithms/Sort](https://github.com/Orcomp/Orcomp/tree/master/Orc/Algorithms/Sort).


1. Unblock the file [...]/NPerfRunner/docs/sample/NPerf.Fixture.ISorter.zip by right-­clicking it, then open “Properties”, and then click on “Unblock” at the bottom of the “General” tab.
2. Unzip [...]/NPerfRunner/docs/sample/NPerf.Fixture.ISorter.zip in the same folder.
3. Launch NPerfRunner WPF application.
4. On the main window click “Load assembly...” and browse for the assembly with the test fixture: [...]/NPerfRunner/docs/sample/NPerf.Fixture.ISorter/NPerf.Fixture.ISorter.dll.
5. Then, load the assembly [...]/NPerfRunner/docs/sample/NPerf.Fixture.ISorter/Orc.dll.

You should get a picture like this:

![NPerfRunner](docs/img/NPerfRunner.png)

On chart above you can see a set of **sorting algorithms with linear time complexity (e.g. QuickSort)**
and a set of algorithms with **logarithmic time complexity (e.g. ShellSort)**.
*Note the logarithmic scale on time axis*.

> The chart uses the [Oxyplot library][Oxyplot], so you can zoom, pan, click on a line to see tooltips, etc...
> Please refere to the oxyplot homepage for shortcut keys.

NPerf.Fixture.IList example
---------------------------

In the **NPerfRunner solution** there is a **project called NPerf.Fixture.IList**.
Into this project there is a **class called IListPerfs** that has an NPerf test fixture.

**This fixture allows you to compare several operations of an IList&lt;T&gt; implementation** such as:

+ Adding an element to the list (Add method).
+ Inserting an element to the list (Insert method) in different positions (start, middle, end and random positions).
+ Counting the elements of the list (Count method).
+ Remove all the elements of the list (Clear method).
+ Check if the list has a given element (Contains method)
+ Remove elements from the list (Remove method).
+ Remove elements at certain positions (RemoveAt method).

An *NPerf test fixture is a class with certain attributes* and methods provided by the NPerf framework.

**PerfTester attribute** tells the NPerf framework that this class is a test fixture, in this example the target type is IList<int>.

- *20* is the number of times a test method is going to be executed.
- *Description* is the text that will be placed on top of the results charts.
- *FeatureDescription* is a label for the X axis that describes the variable for this experiment.

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

**PerfSetUp attribute** marks a method that prepares the IList<int> implementation instance before each run of a test method in the fixture.
In this case, it adds as many elements to the list as the size calculated for the given test iteration (*testIndex*).
This method has to have this signature (provided the target interface is IList<int>).

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

**PerfTest attribute** indicates this is a test method.
In this case, it simply adds a random integer to the list.
Again, test methods have to have this signature (return *void* and accept one parameter of the target type).

```C#
[PerfTest]
public void Add(IList<int> list)
{
    list.Add(this.random.Next());
}
```

The [rest of the test methods included in this fixture](IListNPerfs.cs) are similar to this one.

Finally there is a method that executes after each test.

**PerfTearDown attribute** tells NPerf this is that method.
In this case we simply use it to clear the test list after the execution of the test, although it's not
required in this case because a new instance of the target type is created for each test.
This method is useful if you use IO resources or you want to assert conditions after a test.

```C#
[PerfTearDown]
public void TearDown(IList<int> list)
{
    list.Clear();
}
```

### BCL versus C5 IList&lt;T&gt; implementations

Using NPerf.Fixture.IList we compared the performance of several System.Collections.Generic.IList<T> implementations from these two libraries:

+ **.NET Base Class Library (BCL)**: Located in the assembly mscorlib.dll.
    + http://msdn.microsoft.com/en-us/library/gg145045.aspx
+ **C5 Library**: There's a copy into NPerfRunner/src/libs folder for testing.
    + Homepage: http://www.itu.dk/research/c5/
    + GitHub repository: https://github.com/sestoft/C5/
    + Nuget package: http://www.nuget.org/packages/C5

Compared IList&lt;T&gt; implementations:

+ BCL List&lt;T&gt;
+ C5 ArrayList&lt;T&gt;
+ C5 HashedArrayList&lt;T&gt;
+ C5 LinkedList&lt;T&gt;
+ C5 HashedLinkedList&lt;T&gt;

#### Comparison for Contains method.

Chart for *contains* test. This method tests whether an element is in the list or not.

It shows that **checking whether an element is in a list or not in hashed lists have constant time complexity** while the same operation in **non-hashed list have linear time complexity**.

![NPerfRunner](/docs/img/NPerf.Fixture.IList_Contains_BCLvsC5_Time.png)

#### Comparison for Insert method.

Chart for insertion test. In this case, inserting an element at the beginning of the list.

The following chart shows how C5 HashedArrayList, C5 ArrayList and BCL List have linear time complexity for
inserting at the beginning, while the LinkedLists from C5 has constant time complexity.

> Note the time axis has logarithmic scale.

![NPerfRunner](/docs/img/NPerf.Fixture.IList_InsertAtTheBeginning_BCLvsC5_Log-Time.png)

Both array-based C5 IList implementations and BCL List have O(n) time complexity for inserting at the beginning, C5 HashedArrayList has a greater constant according to the chart.

You can better see that the C5 ArrayList and the BCL List have linear time complexity for inserting at the beginning of a list by looking at the following chart which only shows these two implementations.

![NPerfRunner](/docs/img/NPerf.Fixture.IList_InsertAtTheBeginning_C5-ArrayList_BCL-List_Time.png)

> *You can see theoretical complexity for C5 IList implementations methods at [C5 Library Technical Report][C5TR] (Chapter 12, page 233)*.

There are more screenshots results comparing the BCL with C5 classes in this repository [at /docs/img/][https://github.com/Orcomp/NPerfRunner/docs/img/]


Links
======

Eric Lippert has written some nice articles on performance profiling. [Part 1](http://tech.pro/blog/1293/c-performance-benchmark-mistakes-part-one), [Part 2](http://tech.pro/tutorial/1295/c-performance-benchmark-mistakes-part-two), [Part 3](http://tech.pro/tutorial/1317/c-performance-benchmark-mistakes-part-three), [part 4](http://tech.pro/tutorial/1433/performance-benchmark-mistakes-part-four)

Troubleshooting
===============

#### Visual Studio cannot find package Microsoft.Bcl.Build.1.0.8.

Visual Studio cannot load NPerfRunner nor NPerfRunner.Wpf projects because it can't find the package Microsoft.Bcl.Build.1.0.8.

Try this:

+ Enable NuGet package restore and reload the solution. 
+ If it still can't load it, run `nuget install [...]/NPerfRunner/src/NPerfRunner/packages.config` in a console.

#### Exception raised when loading C5 library.

The C5 library targets ".NET Portable Class library" (PCL), that's why NPerfRunner can't load referenced assemblies from it.
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
