using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Solid.State;
using TelephoneSample.Telephone;

namespace TelephoneSample
{
    public partial class MainForm : Form
    {
        // Public constants

        public const int PICKING_UP_PHONE = 0;
        public const int INCOMING_CALL = 1;
        public const int NO_ANSWER = 2;
        public const int FINISHED_DIALING = 3;
        public const int HANGING_UP = 4;
        public const int ANSWERED = 5;
        public const int LINE_BUSY = 6;

        // Private variables

        private SolidMachine<int> _sm;

        // Private methods

        private void ConfigureTelephone()
        {
            _sm = new TelephoneMachine();
            
            _sm.State<IdleState>()
                .IsInitialState()
                .On(PICKING_UP_PHONE).GoesTo<DiallingState>()
                .On(INCOMING_CALL).GoesTo<RingingState>();

            _sm.State<RingingState>()
                .On(PICKING_UP_PHONE).GoesTo<ConversationState>()
                .On(NO_ANSWER).GoesTo<IdleState>();

            _sm.State<DiallingState>()
                .On(FINISHED_DIALING, () => !IsLineBusy).GoesTo<WaitForAnswerState>()
                .On(FINISHED_DIALING, () => IsLineBusy).GoesTo<LineBusyState>()
                .On(HANGING_UP).GoesTo<IdleState>();

            _sm.State<WaitForAnswerState>()
                .On(ANSWERED).GoesTo<ConversationState>()
                .On(LINE_BUSY).GoesTo<IdleState>()
                .On(HANGING_UP).GoesTo<IdleState>();

            _sm.State<ConversationState>()
                .On(HANGING_UP).GoesTo<IdleState>();

            _sm.Start();
        }

        // Constructor

        public MainForm()
        {
            InitializeComponent();

            // Create the state machine and set this form as the context (exposing the ITextboxLog interface
            // to the states)
            ConfigureTelephone();

        }

        // Properties

        public bool IsLineBusy
        {
            get { return checkIsLineBusy.Checked; }
            set { checkIsLineBusy.Checked = value; }
        }
    }
}
