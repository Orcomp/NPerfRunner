NPerfRunner
===========

A performance and memory benchmark runner for NPerf.

Orignal documentation: http://www.codeproject.com/Articles/5945/NPerf-A-Performance-Benchmark-Framework-for-NET

(Please note NPerf has been completly re-written, however it should be fully backwards compatible with the original Nperf.)

Introduction
=============

NPerf and NPerfRunner allow you to run performance tests against existing assemblies. (Without having to change a line of code on the target assemblies.)

If the methods you want to performance test implements an interface, you can write a performance fixture for that interface.
Once you have a compiled fixture you can load it into NPerfRunner as well as the assemblies you want to performance test. (NPerfRunner will accpet multiple target assemblies.)

NPerfRunner will find all the methods in all the target assemblies that implemtent the desired interface and run the tests against them.
The performance (and memory) results will be dynamically displayed on the chart.

The collection size as well as the chart axis (logarithmic or linear) can easily be changed.

For examples on how to write performance fixtures please have a look here: https://github.com/Orcomp/Orcomp

Setup
=====

You will need these repos:
- https://github.com/Orcomp/NPerf
- https://github.com/Orcomp/NPerfRunner

First open the Nperf project and build it.
Then open the NPerfRunner and set the start up project to "NPerfRunner.Wpf"

How it works
============

Load the performance fixture assembly as well as the assemblies you want to run the perfomance tests against.
(There are some sample assemblies in the "doc/sample" folder. "unblock" the zip file before extracting it.)

The sample zip file contains:
A test fixture - NPerf.Fixture.ISorter.dll
A target assembly - orc.dll

Run the NPerfRunner.wpf project.

Click on the "Load Assemblies" button. Select the two assemblies mentioned above. Then click on one of the "Start" buttons.

You should get a picture like this:

![NPerfRunner](Orcomp.github.com/repository/docs/img/NPerfRunner.png)

The chart uses the Oxyplot library (http://oxyplot.codeplex.com/) so you can zoom, pan, click on a line to see tooltips etc... (Please refere to the oxyplot homepage for shortcut keys.)