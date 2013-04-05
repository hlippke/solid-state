namespace WizardSample.Pages
{
    public partial class InfoSelectionPage : BasePage
    {
        // Constructor

        public InfoSelectionPage()
        {
            InitializeComponent();
        }

        // Methods (BasePage)

        protected override void DoEntering(WizardContext context)
        {
            // Read from context which radio button should be selected
            radioFamily.Checked = context.InfoSelection == InfoSelectionMode.Family;
            radioWork.Checked = context.InfoSelection == InfoSelectionMode.Work;
        }

        protected override void DoExiting(WizardContext context)
        {
            // Save the selected choice to context since it will be used in the state machine guard
            // clauses when selecting the next page
            if (radioFamily.Checked)
                context.InfoSelection = InfoSelectionMode.Family;
            else
                context.InfoSelection = InfoSelectionMode.Work;
        }
    }
}