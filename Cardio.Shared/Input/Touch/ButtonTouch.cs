namespace Cardio.UI.Input.Touch
{
    public class ButtonTouch
    {
        //This key supposed to be a peaks counter to prevent the situation when more than one touch was performed by the player during single heart peak
        public int TouchNumber { get; set; }

        public ButtonType Button { get; set; }

        public TouchType TouchType { get; set; }

        public float PeakCoefficient { get; set; }
    }
}
