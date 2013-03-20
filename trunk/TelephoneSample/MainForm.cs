using System;
using System.Linq;
using System.Reflection;
using System.Windows.Forms;
using Autofac;
using Solid.State;
using TelephoneSample.Telephone;
using TelephoneSample.TelephoneParts;

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

    public partial class MainForm : Form, IStateResolver, ISimpleLog
    {
        // Private variables

        private SolidMachine<TelephoneTrigger> _sm;
        private IContainer _container;

        // Private methods

        /// <summary>
        /// Register all needed classes in the Autofac IoC container so states
        /// etc. can have its dependencies injected in the constructor.
        /// </summary>
        private void ConfigureAutofac()
        {
            var builder = new ContainerBuilder();

            // Register all states (implementing the ISolidState interface)
            builder.RegisterAssemblyTypes(Assembly.GetExecutingAssembly()).Where(
                x => typeof (ISolidState).IsAssignableFrom(x)).AsSelf();

            // Register telephone parts (the different states will request these in the constructor)
            builder.RegisterType<Bell>().SingleInstance();
            builder.RegisterType<Microphone>().SingleInstance();
            builder.RegisterType<Speaker>().SingleInstance();
            builder.RegisterType<Keypad>().SingleInstance();

            // Register this form as a simple log, which will output any received log messages in
            // the textbox in the UI.
            builder.RegisterInstance(this).As<ISimpleLog>().SingleInstance();

            _container = builder.Build();
        }

        /// <summary>
        /// Configures and starts the telephone state machine.
        /// </summary>
        private void ConfigureTelephone()
        {
            _sm = new SolidMachine<TelephoneTrigger>(null, this);

            _sm.Transitioned += StateMachineOnTransitioned;

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

            _sm.State<LineBusyState>()
                .On(TelephoneTrigger.MeHangingUp).GoesTo<IdleState>();

            _sm.State<WaitForAnswerState>()
                .On(TelephoneTrigger.AnswerInOtherEnd).GoesTo<ConversationState>()
                .On(TelephoneTrigger.MeHangingUp).GoesTo<IdleState>();

            _sm.State<ConversationState>()
                .On(TelephoneTrigger.MeHangingUp).GoesTo<IdleState>()
                .On(TelephoneTrigger.OtherEndHangingUp).GoesTo<LineDisconnectedState>();

            _sm.State<LineDisconnectedState>()
                .On(TelephoneTrigger.MeHangingUp).GoesTo<IdleState>();

            _sm.Start();
        }

        /// <summary>
        /// Queries the state machine for the triggers that are currently valid and enables
        /// the buttons that correspond to them.
        /// </summary>
        private void UpdateAvailableTriggers()
        {
            if (!InvokeRequired)
            {
                // Get the available triggers and convert them to a list of strings.
                var triggers = _sm.ValidTriggers.Select(x => x.ToString()).ToList();

                // All buttons are available as children in the gboxTriggers control, and each
                // buttons Tag property is set to the Trigger it represents
                foreach (Control ctrl in gboxTriggers.Controls)
                {
                    if (ctrl is Button)
                        (ctrl as Button).Enabled = triggers.Contains(ctrl.Tag.ToString());
                }
            }
            else
                Invoke(new Action(UpdateAvailableTriggers));
        }

        private void UpdateStateLabel(Type stateType)
        {
            if (!InvokeRequired)
                lblState.Text = stateType.Name;
            else
                Invoke(new Action<Type>(UpdateStateLabel), stateType);
        }

        /// <summary>
        /// This method is called when the state machine has transitioned between states.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="eventArgs"></param>
        private void StateMachineOnTransitioned(object sender, TransitionedEventArgs eventArgs)
        {
            UpdateAvailableTriggers();

            UpdateStateLabel(eventArgs.TargetState);
        }

        /// <summary>
        /// This method is called when any of the trigger buttons is pushed.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void TriggerButton_Click(object sender, EventArgs e)
        {
            // Get the Tag property of the button as a string
            var tag = (sender as Button).Tag.ToString();

            // Convert it to a TelephoneTrigger
            var trigger = (TelephoneTrigger) Enum.Parse(typeof (TelephoneTrigger), tag);

            _sm.Trigger(trigger);
        }

        // Constructor

        public MainForm()
        {
            InitializeComponent();

            ConfigureAutofac();

            ConfigureTelephone();
        }

        // Methods

        /// <summary>
        /// This method implements the IStateResolver interface. Its purpose is to feed the state
        /// machine with state instances as needed. It does this through Autofac, which makes it
        /// possible for the states to get its dependencies as constructor parameters.
        /// </summary>
        /// <param name="stateType"></param>
        /// <returns></returns>
        public ISolidState ResolveState(Type stateType)
        {
            return (ISolidState) _container.Resolve(stateType);
        }

        /// <summary>
        /// This method implements the ISimpleLog interface, which makes it possible for states to request
        /// a ISimpleLog instance and then write log messages that will be displayed in the textbox in the
        /// main form.
        /// </summary>
        /// <param name="message"></param>
        public void Write(string message)
        {
            // Cannot count on this coming on the UI thread, may need to Invoke
            if (!InvokeRequired)
            {
                // Add to log and scroll to end
                tbLog.Text += message;
                tbLog.SelectionStart = tbLog.Text.Length;
                tbLog.ScrollToCaret();
            }
            else
                Invoke(new Action<string>(Write), message);
        }

        // Properties

        /// <summary>
        /// This property is used by the a guard clause in the telephone and is connected to the
        /// checkbox in the UI.
        /// </summary>
        public bool IsLineBusy
        {
            get { return checkIsLineBusy.Checked; }
            set { checkIsLineBusy.Checked = value; }
        }
    }
}