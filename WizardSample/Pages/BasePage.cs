using System.Windows.Forms;
using Solid.State;

namespace WizardSample.Pages
{
    /// <summary>
    /// Base class for wizard pages that implements the ISolidState
    /// interface and casts the state machine context to the WizardContext
    /// type passing it on to the descendant classes.
    /// </summary>
    public partial class BasePage : UserControl, ISolidState
    {
        // Protected methods

        protected virtual void DoEntering(WizardContext context)
        {
            // No code here
        }

        protected virtual void DoExiting(WizardContext context)
        {
            // No code here
        }

        // Constructor

        public BasePage()
        {
            InitializeComponent();
        }

        // Methods (ISolidState)

        public void Entering(object context)
        {
            // Let the wizard page perform custom intialization before being shown
            DoEntering(context as WizardContext);

            // Make sure the wizard page is shown
            if (Parent == null)
            {
                Parent = Program.MainForm.PageControl;
                Dock = DockStyle.Fill;
            }

            Visible = true;
        }

        public void Exiting(object context)
        {
            // Let the wizard page react to this
            DoExiting(context as WizardContext);

            Visible = false;
        }
    }
}