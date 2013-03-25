using System.Windows.Forms;
using Solid.State;

namespace WizardSample.States
{
    /// <summary>
    /// State that shuts down the application so that event can be included in
    /// the state machine configuration.
    /// </summary>
    public class ShutdownApplicationState : ISolidState
    {
        public void Entering(object context)
        {
            // Exit the application without further ado
            Application.Exit();
        }

        public void Exiting(object context)
        {
            // No code here
        }
    }
}