using PackageUpdateUtility.Modifications;

namespace PackageUpdateUtility;

public class Modificator
{
    public List<IModification> Modifications { get; private set; }

    public Modificator()
    {
        Modifications = new List<IModification>();
    }

    public void RunAllModifications()
    {

        foreach (IModification modification in Modifications)
        {
            modification.RunModification();
        }

    }

}