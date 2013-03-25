using System.Text;

namespace WizardSample.Pages
{
    public partial class FinishPage : BasePage
    {
        // Protected methods (BasePage)

        protected override void DoEntering(WizardContext context)
        {
            // Let's write a little summary of what the user has chosen
            var sb = new StringBuilder();

            sb.AppendLine("The wizard is finished! Here's the info that was entered:");
            sb.AppendLine();
            if (context.InfoSelection == InfoSelectionMode.Work)
                sb.AppendLine(string.Format("You described your work: {0}", context.KindOfWork));
            else if (context.InfoSelection == InfoSelectionMode.Family)
            {
                sb.AppendLine(string.Format("Your family consists of {0} adults and {1} children", context.AdultCount,
                                            context.ChildCount));
                if (!string.IsNullOrEmpty(context.FamilyPet))
                    sb.AppendLine(string.Format("You also have {0}", context.FamilyPet));
            }
            else if (context.InfoSelection == InfoSelectionMode.Home)
            {
                sb.AppendLine(string.Format("You live in {0} and your zip code is {1}", context.HomeType, context.Zipcode));
            }

            tbSummary.Text = sb.ToString();
            tbSummary.SelectionStart = 0;
            tbSummary.SelectionLength = 0;
        }

        // Constructor

        public FinishPage()
        {
            InitializeComponent();
        }
    }
}