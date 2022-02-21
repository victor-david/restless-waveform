using System;
using System.Diagnostics;
using System.Windows.Input;

namespace Restless.App.Wave
{
    /// <summary>
    /// Represents a command
    /// </summary>
    public class RelayCommand : ICommand 
    { 
        #region Private 
        private readonly Action<object> execute; 
        private readonly Predicate<object> canExecute;
        #endregion

        /************************************************************************/

        #region Public properties
        /// <summary>
        /// Gets or sets a parameter that is associated with this command.
        /// </summary>
        public object Parameter
        {
            get;
            set;
        }
        #endregion

        /************************************************************************/

        #region Constructor (private)
        /// <summary>
        /// Initializes a new instance of the <see cref="RelayCommand"/> class.
        /// </summary>
        /// <param name="execute">The method that executes the command.</param>
        /// <param name="canExecute">The method that checks if this command can execute. If null, no check is performed.</param>
        /// <param name="parameter">The parameter, or null if none needed.</param>
        private RelayCommand(Action<object> execute, Predicate<object> canExecute, object parameter) 
        {
            this.execute = execute ?? throw new ArgumentNullException(nameof(execute)); 
            this.canExecute = canExecute;
            Parameter = parameter;
        }
        #endregion

        /************************************************************************/

        #region Public methods
        /// <summary>
        /// Creates and returns an instance of <see cref="RelayCommand"/>.
        /// </summary>
        /// <param name="execute">The method that executes the command</param>
        /// <param name="canExecute">The method that checks if this command can execute. If null, no check is performed.</param>
        /// <param name="parameter">The parameter, or null if none needed.</param>
        /// <returns>A <see cref="RelayCommand"/> object.</returns>
        public static RelayCommand Create(Action<object> execute, Predicate<object> canExecute, object parameter)
        {
            return new RelayCommand(execute, canExecute, parameter);
        }

        /// <summary>
        /// Creates and returns an instance of <see cref="RelayCommand"/>.
        /// </summary>
        /// <param name="execute">The method that executes the command</param>
        /// <param name="canExecute">The method that checks if this command can execute. If null, no check is performed.</param>
        /// <returns>A <see cref="RelayCommand"/> object.</returns>
        public static RelayCommand Create(Action<object> execute, Predicate<object> canExecute)
        {
            return new RelayCommand(execute, canExecute, null);
        }


        /// <summary>
        /// Creates and returns an instance of <see cref="RelayCommand"/>.
        /// </summary>
        /// <param name="execute">The method that executes the command.</param>
        /// <returns>A <see cref="RelayCommand"/> object.</returns>
        /// <remarks>This overload creates a command that has no corresponding command predicate.</remarks>
        public static RelayCommand Create(Action<object> execute)
        {
            return new RelayCommand(execute, null, null);
        }
        #endregion

        /************************************************************************/

        #region ICommand Members
        /// <summary>
        /// Checks to see if this command can execute.
        /// </summary>
        /// <param name="parameter">An object to pass to the command evaulator method.</param>
        /// <returns>true if the command can excecute; otherwise, false.</returns>
        [DebuggerStepThrough] 
        public bool CanExecute(object parameter) 
        {
            parameter = Parameter ?? parameter;
            return canExecute == null || canExecute(parameter);
        }

        /// <summary>
        /// Occurs when the conditions that affect whether a command may excute change.
        /// </summary>
        public event EventHandler CanExecuteChanged
        {
            add => CommandManager.RequerySuggested += value; 
            remove => CommandManager.RequerySuggested -= value;
        }

        /// <summary>
        /// Executes the command
        /// </summary>
        /// <param name="parameter">An object to pass to the command method.</param>
        public void Execute(object parameter) 
        {
            parameter = Parameter ?? parameter;
            execute(parameter);
        }
        #endregion
    }
}