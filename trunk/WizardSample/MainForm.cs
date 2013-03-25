using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Solid.State;
using WizardSample.Pages;
using WizardSample.States;

namespace WizardSample
{
    public partial class MainForm : Form
    {
        // Private variables

        private SolidMachine<WizardTrigger> _sm;
        private WizardContext _wizardContext;

        // Private methods

        private void ConfigureWizard()
        {
            _wizardContext = new WizardContext();

            // Create the state machine with the correct context
            _sm = new SolidMachine<WizardTrigger>(_wizardContext);
            _sm.Transitioned += StateMachineOnTransitioned;

            // Assign the state machine to the context so it's accessible
            _wizardContext.StateMachine = _sm;

            _sm.State<WelcomePage>()
                .On(WizardTrigger.Cancel).GoesTo<ShutdownApplicationState>()
                .On(WizardTrigger.Next).GoesTo<InfoSelectionPage>();

            _sm.State<InfoSelectionPage>()
                .On(WizardTrigger.Cancel).GoesTo<ShutdownApplicationState>()
                .On(WizardTrigger.Next, () => _wizardContext.InfoSelection == InfoSelectionMode.Family).GoesTo<FamilyInfoPage1>()
                .On(WizardTrigger.Next, () => _wizardContext.InfoSelection == InfoSelectionMode.Home).GoesTo<HomeInfoPage1>()
                .On(WizardTrigger.Next, () => _wizardContext.InfoSelection == InfoSelectionMode.Work).GoesTo<WorkInfoPage>();

            _sm.State<FamilyInfoPage1>()
                .On(WizardTrigger.Next).GoesTo<FamilyInfoPage2>();
            _sm.State<FamilyInfoPage2>()
                .On(WizardTrigger.Next).GoesTo<FinishPage>();

            _sm.State<HomeInfoPage1>()
                .On(WizardTrigger.Next).GoesTo<HomeInfoPage2>();
            _sm.State<HomeInfoPage2>()
                .On(WizardTrigger.Next).GoesTo<FinishPage>();

            _sm.State<WorkInfoPage>()
                .On(WizardTrigger.Next).GoesTo<FinishPage>();

            _sm.State<FinishPage>()
                .On(WizardTrigger.Finish).GoesTo<ShutdownApplicationState>();

            _sm.Start();

        }

        /// <summary>
        /// This method is called when the wizard has changed state.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="eventArgs"></param>
        private void StateMachineOnTransitioned(object sender, TransitionedEventArgs eventArgs)
        {
            if (!InvokeRequired)
            {
                // Back button should only be available if there are states to go back to
                btnBack.Enabled = _sm.StateHistory.Length > 0;
                // The Next button is only available if its valid in the state machine configuration
                btnNext.Enabled = _sm.ValidTriggers.Contains(WizardTrigger.Next);
                // Cancel/Finish button also changes according to which triggers are valid
                if (_sm.ValidTriggers.Contains(WizardTrigger.Cancel))
                {
                    btnCancel.Text = "Cancel";
                    btnCancel.Enabled = true;
                }
                else if (_sm.ValidTriggers.Contains(WizardTrigger.Finish))
                {
                    btnCancel.Text = "Finish";
                    btnCancel.Enabled = true;
                }
                else
                    btnCancel.Enabled = false;
            }
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            // Put all the magic here to make sure that the form has been initialized correctly

            ConfigureWizard();
        }

        /// <summary>
        /// The Back button has been clicked.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnBack_Click(object sender, EventArgs e)
        {
            _sm.GoBack();
        }

        /// <summary>
        /// The Next button has been clicked.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnNext_Click(object sender, EventArgs e)
        {
            _sm.Trigger(WizardTrigger.Next);
        }

        /// <summary>
        /// The Cancel/Finish button has been clicked.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnCancel_Click(object sender, EventArgs e)
        {
            // Determine what trigger is the correct one
            if (_sm.ValidTriggers.Contains(WizardTrigger.Cancel))
            {
                // Confirm the we should indeed cancel
                if (MessageBox.Show(Program.MainForm, "Are you sure you want to cancel?", "Confirm", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                    _sm.Trigger(WizardTrigger.Cancel);
            }
            else
                _sm.Trigger(WizardTrigger.Finish);
        }

        // Constructor

        public MainForm()
        {
            InitializeComponent();
        }

        // Properties

        /// <summary>
        /// Makes the panel that holds the wizard pages internally visible.
        /// </summary>
        internal Control PageControl
        {
            get { return pnlMain; }
        }
    }

    /// <summary>
    /// One trigger for each button in the wizard.
    /// </summary>
    public enum WizardTrigger
    {
        Back,
        Next,
        Cancel,
        CancelConfirmed,
        Finish
    }
}
