namespace WizardSample
{
    /// <summary>
    /// Holds information about the choices that are made in the wizard
    /// and the data that is entered.
    /// </summary>
    public class WizardContext
    {
        // Properties

        public InfoSelectionMode InfoSelection { get; set; }
    }

    public enum InfoSelectionMode
    {
        Family = 0,
        Home = 1,
        Work = 2
    }
}