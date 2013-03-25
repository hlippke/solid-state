using System;

namespace WizardSample.Pages
{
    public partial class FamilyInfoPage1 : BasePage
    {
        // Protected methods (BasePage)

        protected override void DoEntering(WizardContext context)
        {
            comboAdults.SelectedIndex = comboAdults.Items.IndexOf(context.AdultCount.ToString());
            comboChildren.SelectedIndex = comboChildren.Items.IndexOf(context.ChildCount.ToString());
        }

        protected override void DoExiting(WizardContext context)
        {
            context.AdultCount = Convert.ToInt32(comboAdults.Text);
            context.ChildCount = Convert.ToInt32(comboChildren.Text);
        }

        // Constructor

        public FamilyInfoPage1()
        {
            InitializeComponent();
        }
    }
}