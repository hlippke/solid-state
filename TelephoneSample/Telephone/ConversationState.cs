using Solid.State;
using TelephoneSample.TelephoneParts;

namespace TelephoneSample.Telephone
{
    /// <summary>
    /// During the conversation, the microphone receives your voice
    /// and the speaker plays what the other side is saying. We have
    /// two dependencies here!
    /// </summary>
    public class ConversationState : TelephoneState
    {
        // Private variables

        private Speaker _speaker;
        private Microphone _microphone;

        // Constructor

        public ConversationState(Speaker speaker, Microphone microphone)
        {
            _speaker = speaker;
            _microphone = microphone;
        }

        protected override void DoEntering(SolidMachine<TelephoneTrigger> machine)
        {
            _microphone.StartReceivingChatter();
            _speaker.StartPlayingChatter();
        }

        protected override void DoExiting(SolidMachine<TelephoneTrigger> machine)
        {
            _microphone.StopReceivingChatter();
            _speaker.StopPlayingChatter();
        }
    }
}