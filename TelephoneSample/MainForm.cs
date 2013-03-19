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
    public enum TelephoneTrigger
    {
        PickingUpPhone,
        IncomingCall,
        IgnoreIncomingCall,
        FinishedDialling,
        AnswerInOtherEnd,
        MeHangingUp,
        OtherEndHangingUp
    }

    public partial class MainForm : Form
    {
        // Private variables

        private SolidMachine<TelephoneTrigger> _sm;

        // Private methods

        private void ConfigureTelephone()
        {
            _sm = new SolidMachine<TelephoneTrigger>(this);
            
            _sm.State<IdleState>()
                .IsInitialState()
                .On(TelephoneTrigger.PickingUpPhone).GoesTo<DiallingState>()
                .On(TelephoneTrigger.IncomingCall).GoesTo<RingingState>();

            _sm.State<RingingState>()
                .On(TelephoneTrigger.PickingUpPhone).GoesTo<ConversationState>()
                .On(TelephoneTrigger.IgnoreIncomingCall).GoesTo<IdleState>();

            _sm.State<DiallingState>()
                .On(TelephoneTrigger.FinishedDialling, () => !IsLineBusy).GoesTo<WaitForAnswerState>()
                .On(TelephoneTrigger.FinishedDialling, () => IsLineBusy).GoesTo<LineBusyState>()
                .On(TelephoneTrigger.MeHangingUp).GoesTo<IdleState>();

            _sm.State<WaitForAnswerState>()
                .On(TelephoneTrigger.AnswerInOtherEnd).GoesTo<ConversationState>()
                .On(TelephoneTrigger.MeHangingUp).GoesTo<IdleState>();

            _sm.State<ConversationState>()
                .On(TelephoneTrigger.MeHangingUp).GoesTo<IdleState>()
                .On(TelephoneTrigger.OtherEndHangingUp).GoesTo<LineDisconnectedState>();

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
