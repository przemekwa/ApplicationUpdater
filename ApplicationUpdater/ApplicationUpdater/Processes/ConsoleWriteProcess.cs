namespace ApplicationUpdater.Processes
{
    public class ConsoleWriteProcess
    {
        public string Msg { get; set; }
        public bool NewLine { get; set; }
        public bool OneLineMode { get; set; }
        public bool ShowProgress { get; internal set; }
        public bool SetNewProgress { get; internal set; }
        public double StepNumberProgress { get; internal set; }
        public double MaxProgress { get; internal set; }
    }
}
