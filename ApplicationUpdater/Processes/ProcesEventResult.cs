namespace ApplicationUpdater
{
    public class ProcesEventResult
    {
        public string Result { get; set; }

        public static ProcesEventResult OK { get
            {
                return new ProcesEventResult
                {
                    Result = "[ OK ]"
                };
            }
        }

        public static ProcesEventResult STOP
        {
            get
            {
                return new ProcesEventResult
                {
                    Result = "[ STOP ]"
                };
           }
        }

        public static ProcesEventResult ERROR
        {
            get
            {
                return new ProcesEventResult
                {
                    Result = "[ ERROR ]"
                };
            }
        }
    }
}