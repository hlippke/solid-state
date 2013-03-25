namespace WizardSample.Pages
{
    public partial class FamilyInfoPage2 : BasePage
    {
        // Protected methods (BasePage)

        protected override void DoEntering(WizardContext context)
        {
            if (string.IsNullOrEmpty(context.FamilyPet))
                comboPets.SelectedIndex = 0;
            else
                comboPets.SelectedIndex = comboPets.Items.IndexOf(context.FamilyPet);
        }

        protected override void DoExiting(WizardContext context)
        {
            if (comboPets.SelectedIndex == 0)
                context.FamilyPet = "";
            else
                context.FamilyPet = comboPets.Text;
        }
        // Constructor

        public FamilyInfoPage2()
        {
            InitializeComponent();
        }
    }
}