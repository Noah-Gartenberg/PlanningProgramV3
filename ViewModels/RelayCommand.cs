using System.Windows.Input;

namespace PlanningProgramV3.ViewModels
{
    public class RelayCommand : ICommand
    {

        /// <summary>
        /// NOTE TO SELF, SOMETHING I CAN DO IF I NEED MORE PARAMETERS IS TO ADD 2 PARAMETERS TO CAN EXECUTE: A REFERENCE TO A STACK, AND AN AMOUNT OF PARAMETERS TO POP FROM IT.
        /// THIS WOULD REQUIRE SOME WORK TO MAKE IT FUNCTION, BUT IT WOULD ALLOW ME TO HAVE MORE PARAMETERS
        /// </summary>

        private Action<object> execute;

        private Func<object?, bool> canExecute;
        private RelayCommand? getTasksForTimePeriod;
        private object value;

        public RelayCommand(Action<object> executeAction, Func<object?,bool>? canExecuteFunc)
        {
            execute = executeAction;
            canExecute = canExecuteFunc;
        }

        public event EventHandler? CanExecuteChanged;

        public bool CanExecute(object? parameter)
        {
            //assume that if canExecute is null, then generally, it can be called
            if (canExecute == null)
                return true;
            return canExecute(parameter);
        }

        public void Execute(object? parameter)
        {
            execute(parameter);
        }
    }
}
