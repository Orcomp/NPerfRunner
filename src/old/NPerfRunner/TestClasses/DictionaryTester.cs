using System;
using System.Collections.Generic;
using System.Linq;
using NPerf.Framework;
using NPerfRunner.Types;

namespace NPerfRunner.TestClasses
{
    /*[PerfTester(typeof(ISorter), 10, Description = "Sort Algorithm benchmark for Random data", FeatureDescription = "Collection size")]
    public class DictionaryTester
    {
        protected int[] list;
        public int CollectionCount(int testIndex)
        {
            int n = 0;
            if (testIndex < 0)
                n = 10;
            else
                n = (testIndex + 1) * 500;
            return n;
        }

        [PerfRunDescriptor]
        public double RunDescription(int testIndex)
        {
            return (double)CollectionCount(testIndex);
        }

        [PerfSetUp]
        public void SetUp(int testIndex, ISorter sorter)
        {
            var rnd = new Random();
            this.list = new int[CollectionCount(testIndex)];
            for (int i = 0; i < this.list.Length; ++i)
                this.list[i] = rnd.Next();
        }

        [PerfTest]
        public void SortRandomData(ISorter sorter)
        {
            sorter.Sort(this.list);
        }
    }*/

    #region DateTimesSort Classes

    public interface IDateSorter
    {
        void Sort(List<DateRange> dates);
    }

    public class Aus : IDateSorter
    {
        public void Sort(List<DateRange> dates)
        {
            GetSortedDateTimes.Aus1(dates);
        }
    }

    public class Bawr : IDateSorter
    {
        public void Sort(List<DateRange> dates)
        {
            GetSortedDateTimes.Bawr(dates);
        }
    }

    public class C6c : IDateSorter
    {
        public void Sort(List<DateRange> dates)
        {
            GetSortedDateTimes.C6c(dates);
        }
    }

    public class ErwinReid : IDateSorter
    {
        public void Sort(List<DateRange> dates)
        {
            GetSortedDateTimes.ErwinReid(dates);
        }
    }

    public class Mihai : IDateSorter
    {
        public void Sort(List<DateRange> dates)
        {
            GetSortedDateTimes.Mihai(dates);
        }
    }

    public class MoustafaS : IDateSorter
    {
        public void Sort(List<DateRange> dates)
        {
            GetSortedDateTimes.MoustafaS(dates);
        }
    }
    public class Renze : IDateSorter
    {
        public void Sort(List<DateRange> dates)
        {
            GetSortedDateTimes.Renze(dates);
        }
    }

    public class RenzeExtended : IDateSorter
    {
        public void Sort(List<DateRange> dates)
        {
            GetSortedDateTimes.RenzeExtended(dates);
        }
    }

    public class SoftwareBender : IDateSorter
    {
        public void Sort(List<DateRange> dates)
        {
            GetSortedDateTimes.SoftwareBender(dates);
        }
    }

    public class V_Tom_R : IDateSorter
    {
        public void Sort(List<DateRange> dates)
        {
            GetSortedDateTimes.V_Tom_R(dates);
        }
    }

    public class Zaher : IDateSorter
    {
        public void Sort(List<DateRange> dates)
        {
            GetSortedDateTimes.Zaher(dates);
        }
    }

    #endregion

    //Main tests
    [PerfTester(typeof(IDateSorter), 2, Description = "Sort Algorithm benchmark for StartTimes and EndTimes", FeatureDescription = "Collection size")]
    public class DateTimesSortTester
    {
        int numberOfDateRanges = 4000; // Must be square rootable

        //Setup sorts
        public IEnumerable<DateRange> GetDateRangesRandomStartAndEndTimes(int count, int seed)
        {
            var date = DateTime.Now;
            var r = new Random(seed);
            var dateRanges = new List<DateRange>(count);
            for (int i = 0; i < count; i++)
            {
                DateTime startTime = date.AddSeconds(r.Next(-count, count));
                DateTime endTime = startTime.AddSeconds(r.Next(0, count));
                dateRanges.Add(new DateRange(startTime, endTime));
            }
            dateRanges.Sort(); return dateRanges;
        }

        public static IEnumerable<DateRange> GetDateRangesEndTimesSorted(int count)
        {
            var date = DateTime.Now;

            return Enumerable.Range(1, count).Select(x => new DateRange(date.AddMinutes(x), date.AddMinutes(x + 100)));
        }

        public static IEnumerable<DateRange> GetDateRangesRandomEndTimes(int count, int seed)
        {
            var date = DateTime.Now;

            var r = new Random(seed);

            return Enumerable.Range(1, count).Select(x => new DateRange(date.AddMinutes(x), date.AddMinutes(x + r.Next(0, 100)))).OrderBy(x => x);
        }

        public static IEnumerable<DateRange> GetDateRangesMultiGroupDescendingEndTimes(int count)
        {
            var n = Math.Sqrt(count);

            var date = DateTime.Now;

            var output = new List<DateRange>();

            for (int i = 0; i < n; i++)
            {
                for (int j = 0; j < n; j++)
                {
                    output.Add(new DateRange(date.AddMinutes(2 * n * i).AddMinutes(j), date.AddMinutes(2 * n * i).AddMinutes(2 * n - j)));
                }
            }

            // +-------------------+ +-------------------+
            //    +-------------+       +-------------+
            //        +-----+               +-----+
            //          +-+                   +-+

            return output.OrderBy(x => x);
        }

        protected List<DateRange> list1;
        protected List<DateRange> list2;
        protected List<DateRange> list3;
        protected List<DateRange> list4;

        public int CollectionCount(int testIndex)
        {
            return numberOfDateRanges*(testIndex);
        }

        [PerfRunDescriptor]
        public double RunDescription(int testIndex)
        {
            return (double)CollectionCount(testIndex);
        }

        [PerfSetUp]
        public void SetUp(int testIndex, IDateSorter sorter)
        {
            list1 = new List<DateRange>(GetDateRangesRandomStartAndEndTimes(CollectionCount(testIndex+1), 1));
            list2 = new List<DateRange>(GetDateRangesEndTimesSorted(CollectionCount(testIndex + 1)));
            list3 = new List<DateRange>(GetDateRangesRandomEndTimes(CollectionCount(testIndex + 1), 1));
            list4 = new List<DateRange>(GetDateRangesMultiGroupDescendingEndTimes(CollectionCount(testIndex + 1)));
        }

        [PerfTest]
        public void RandomStartAndEndTimes(IDateSorter sorter)
        {
            if(list1.Count!=0)
            sorter.Sort(this.list1);
        }

        [PerfTest]
        public void EndTimesSorted(IDateSorter sorter)
        {
            if (list2.Count != 0)
                sorter.Sort(this.list2);
        }

        [PerfTest]
        public void RandomEndTimes(IDateSorter sorter)
        {
            if (list3.Count != 0)
                sorter.Sort(this.list3);
        }

        [PerfTest]
        public void MultiGroupDescendingEndTimes(IDateSorter sorter)
        {
            if (list4.Count != 0)
                sorter.Sort(this.list4);
        }
    }
}
