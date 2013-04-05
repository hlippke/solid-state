namespace WizardSample.Pages
{
    public partial class WorkInfoPage : BasePage
    {
        // Protected methods

        protected override void DoEntering(WizardContext context)
        {
            // Select the correct item in the combobox
            var index = comboKindOfWork.Items.IndexOf(context.KindOfWork ?? "");
            if (index < 0)
                index = 0;

            comboKindOfWork.SelectedIndex = index;
        }

        protected override void DoExiting(WizardContext context)
        {
            // Save the selected value
            context.KindOfWork = comboKindOfWork.Text;
        }

        // Constructor

        public WorkInfoPage()
        {
            InitializeComponent();
        }
    }
}