namespace NPerf.Fixture.IList
{
    using System;
    using System.Collections.Generic;

    using NPerf.Framework;

    /// <summary>
    /// Performance test for implementations of the .NET Base Class Library IList&lt;int&gt; interface.
    /// <see cref="http://msdn.microsoft.com/es-es/library/5y536ey6.aspx"/>
    /// </summary>
    [PerfTester(
        typeof(IList<int>),
        20,
        Description = "IList operations benchmark",
        FeatureDescription = "Collection size")]
    public class IListPerfs
    {
        private readonly Random random = new Random();

        /// <summary>
        /// The number of elements of the tested list for the current test execution.
        /// </summary>
        private int count;

        /// <summary>
        /// Calculates the number of elements of the tested list from the test index number.
        /// </summary>
        /// <param name="testIndex">
        /// The test index number.
        /// </param>
        /// <returns>
        /// The number of elements of the collection to be tested..
        /// </returns>
        public int CollectionCount(int testIndex)
        {
            return testIndex * 50000;
        }

        /// <summary>
        /// The value that describes each execution of a test.
        /// In this case, the size of the list.
        /// </summary>
        /// <param name="testIndex">
        /// The test index number.
        /// </param>
        /// <returns>
        /// A double value that describes an execution of the test.
        /// </returns>
        [PerfRunDescriptor]
        public double RunDescription(int testIndex)
        {
            return this.CollectionCount(testIndex);
        }

        /// <summary>
        /// Set up the list with the appropriate number of elements for a test execution.
        /// </summary>
        /// <param name="testIndex">
        /// The test index number.
        /// </param>
        /// <param name="list">
        /// The list to be tested.
        /// </param>
        [PerfSetUp]
        public void SetUp(int testIndex, IList<int> list)
        {
            this.count = this.CollectionCount(testIndex);

            for (var i = 0; i < this.count; i++)
            {
                list.Add(i);
            }
        }

        /// <summary>
        /// Performance test for adding elements to a list.
        /// </summary>
        /// <param name="list">
        /// The list to be tested.
        /// </param>
        [PerfTest]
        public void Add(IList<int> list)
        {
            list.Add(this.random.Next());
        }

        /// <summary>
        /// Performance test for inserting elements at the beginning of a list.
        /// </summary>
        /// <param name="list">
        /// The list to be tested.
        /// </param>
        [PerfTest]
        public void InsertAtTheBeginning(IList<int> list)
        {
            list.Insert(0, this.random.Next());
        }

        /// <summary>
        /// Performance test for inserting elements in the middle of a list.
        /// </summary>
        /// <param name="list">
        /// The list to be tested.
        /// </param>
        [PerfTest]
        public void InsertInTheMiddle(IList<int> list)
        {
            list.Insert(this.count / 2, this.random.Next());
        }

        /// <summary>
        /// Performance test for inserting elements at random positions of a list.
        /// </summary>
        /// <param name="list">
        /// The list to be tested.
        /// </param>
        [PerfTest]
        public void InsertAtRandomPositions(IList<int> list)
        {
            list.Insert(this.random.Next(0, this.count), this.random.Next());
        }

        /// <summary>
        /// Performance test for inserting elements at the end of a list.
        /// </summary>
        /// <param name="list">
        /// The list to be tested.
        /// </param>
        [PerfTest]
        public void InsertAtTheEnd(IList<int> list)
        {
            list.Insert(this.count - 1, this.random.Next());
        }

        /// <summary>
        /// Performance test for checking if a list contains an element.
        /// </summary>
        /// <param name="list">
        /// The list to be tested.
        /// </param>
        [PerfTest]
        public void Contains(IList<int> list)
        {
            list.Contains(this.random.Next());
        }

        /// <summary>
        /// Performance test for emptying a list.
        /// </summary>
        /// <param name="list">
        /// The list to be tested.
        /// </param>
        [PerfTest]
        public void Clear(IList<int> list)
        {
            list.Clear();
        }

        /// <summary>
        /// Performance test for counting the elements of a list.
        /// </summary>
        /// <param name="list">
        /// The list to be tested.
        /// </param>
        [PerfTest]
        public void Count(IList<int> list)
        {
            var foo = list.Count;
        }

        /// <summary>
        /// Performance test for removing elements from the beginning of a list.
        /// </summary>
        /// <param name="list">
        /// The list to be tested.
        /// </param>
        [PerfTest]
        public void RemoveAtTheBeginning(IList<int> list)
        {
            list.RemoveAt(0);
        }

        /// <summary>
        /// Performance test for removing elements from the middle of a list.
        /// </summary>
        /// <param name="list">
        /// The list to be tested.
        /// </param>
        [PerfTest]
        public void RemoveInTheMiddle(IList<int> list)
        {
            list.RemoveAt(this.count / 2);
        }

        /// <summary>
        /// Performance test for removing elements from random positions of a list.
        /// </summary>
        /// <param name="list">
        /// The list to be tested.
        /// </param>
        [PerfTest]
        public void RemoveAtRandomPositions(IList<int> list)
        {
            list.RemoveAt(this.random.Next(0, this.count));
        }

        /// <summary>
        /// Performance test for removing elements from the end of a list.
        /// </summary>
        /// <param name="list">
        /// The list to be tested.
        /// </param>
        [PerfTest]
        public void RemoveAtTheEnd(IList<int> list)
        {
            list.RemoveAt(this.count - 1);
        }

        /// <summary>
        /// Clears the tested list after the execution of a test.
        /// </summary>
        /// <param name="list">
        /// The list to be tested.
        /// </param>
        [PerfTearDown]
        public void TearDown(IList<int> list)
        {
            list.Clear();
        }
    } // End of IListPerfs class
}
