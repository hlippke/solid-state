using Solid.State;

namespace WizardSample
{
    /// <summary>
    /// Holds information about the choices that are made in the wizard
    /// and the data that is entered.
    /// </summary>
    public class WizardContext
    {
        // Constructor

        public WizardContext()
        {
            // Some basic initialization
            AdultCount = 1;
        }

        // Properties

        /// <summary>
        /// A reference to the state machine so the states have access to it.
        /// </summary>
        public SolidMachine<WizardTrigger> StateMachine { get; set; }

        public InfoSelectionMode InfoSelection { get; set; }

        public string KindOfWork { get; set; }

        public int AdultCount { get; set; }

        public int ChildCount { get; set; }

        public string FamilyPet { get; set; }

        public string HomeType { get; set; }

        public string Zipcode { get; set; }
    }

    public enum InfoSelectionMode
    {
        Family = 0,
        Work = 1
    }
}