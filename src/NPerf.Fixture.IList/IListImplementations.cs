namespace NPerf.Fixture.IList
{
    using SCG = System.Collections.Generic;

    public class ListBCL : SCG.List<int>
    {
    }

    public class ArrayListC5 : C5.ArrayList<int>
    {
    }

    public class HashedArrayListC5 : C5.HashedArrayList<int>
    {
    }

    public class LinkedListC5 : C5.LinkedList<int>
    {
    }

    public class HashedLinkedListC5 : C5.HashedLinkedList<int>
    {
    }
}
