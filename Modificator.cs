using PackageUpdateUtility.Modifications;

namespace PackageUpdateUtility;

public class Modificator
{
    private List<IModification> _modifications;

    public Modificator()
    {
        _modifications = new List<IModification>();
    }

    public void RunAllModifications()
    {

        lock (_modifications)
        {
            foreach (IModification modification in _modifications)
            {
                modification.RunModification();
            }
        }

    }

    public void AddModification(IModification modification)
    {
        lock (_modifications)
        {
            _modifications.Add(modification);
        }

    }
}