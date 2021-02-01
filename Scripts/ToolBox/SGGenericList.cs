using System; //This allows the IComparable Interface

public class SGGenericList : IComparable<SGGenericList>
{
    public string name;
    public int value;

    public SGGenericList(string newName, int newValue)
    {
        name = newName;
        value = newValue;
    }

    //This method is required by the IComparable
    //interface. 
    public int CompareTo(SGGenericList other)
    {
        if (other == null)
        {
            return 1;
        }

        return value - other.value;
    }

}
