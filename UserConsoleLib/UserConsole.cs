using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;

using System.Windows.Forms;

namespace UserConsoleLib
{
    /// <summary>
    /// Represents a preprogrammed console window with both input and output capabilities
    /// </summary>
    public partial class UserConsole : Form, IConsoleOutput
    {
        /// <summary>
        /// If set to false, this console window cannot be closed by standard messures. In that case: use ForceClose() insted
        /// </summary>
        public virtual bool CanClose { get; set; } = true;

        bool ForceClosing { get; set; }

        /// <summary>
        /// Gets or sets the IConsoleOutput object to output data to. If null or default, output will go to this window
        /// </summary>
        public virtual IConsoleOutput TargetOutputDevice { get; set; }

        int _lastHistoryIndex { get; set; }
        List<string> _commandHistory { get; } = new List<string>();

        /// <summary>
        /// Creates a new UserConsole window
        /// </summary>
        public UserConsole()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Clears this window's history
        /// </summary>
        public virtual void ClearBuffer()
        {
            ListOuput.Nodes.Clear();
        }

        /// <summary>
        /// Writes a line of red text to this window's history
        /// </summary>
        /// <param name="message"></param>
        public virtual void WriteError(string message)
        {
            ListOuput.Nodes.Add(new TreeNode(message) { ForeColor = Color.Red });
            ListOuput.Nodes[ListOuput.Nodes.Count - 1].EnsureVisible();
        }

        /// <summary>
        /// Writes a line of text to this window's history
        /// </summary>
        /// <param name="message"></param>
        public virtual void WriteLine(string message)
        {
            ListOuput.Nodes.Add(new TreeNode(message));
            ListOuput.Nodes[ListOuput.Nodes.Count - 1].EnsureVisible();
        }

        /// <summary>
        /// Writes a line of yellow text ot his window's history
        /// </summary>
        /// <param name="message"></param>
        public virtual void WriteWarning(string message)
        {
            ListOuput.Nodes.Add(new TreeNode(message) { ForeColor = Color.Yellow });
            ListOuput.Nodes[ListOuput.Nodes.Count - 1].EnsureVisible();
        }

        /// <summary>
        /// Code that is executed when the user presses the submit button
        /// </summary>
        /// <param name="sender">Sender of event</param>
        /// <param name="e">Event arguments</param>
        protected virtual void OnClickSubmit(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(TextboxInput.Text))
            {
                return;
            }

            ListOuput.Nodes.Add(new TreeNode("> " + TextboxInput.Text) { ForeColor = Color.Gray });
            ListOuput.Nodes[ListOuput.Nodes.Count - 1].EnsureVisible();

            Command.ParseLine(TextboxInput.Text, TargetOutputDevice ?? this);

            _commandHistory.Add(TextboxInput.Text);
            _lastHistoryIndex = 0;

            TextboxInput.Text = null;
        }

        /// <summary>
        /// Closes the console, even if CanClose has been set to true
        /// </summary>
        public void ForceClose()
        {
            ForceClosing = true;
            Close();
            ForceClosing = false;
        }

        /// <summary>
        /// Fires events and cancels the window's closing rutine if CanClose has been set to true
        /// </summary>
        /// <param name="e">Event arguments</param>
        protected override void OnClosing(CancelEventArgs e)
        {
            base.OnClosing(e);

            if (!CanClose && !ForceClosing)
            {
                e.Cancel = true;
            }
            
        }

        /// <summary>
        /// Handles history recall
        /// </summary>
        /// <param name="sender">Sender of event</param>
        /// <param name="e">Event arguments</param>
        protected void HandleKeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Up)
            {
                if (_lastHistoryIndex != _commandHistory.Count)
                {
                    _lastHistoryIndex++;
                }

                if (_lastHistoryIndex == 0)
                {
                    TextboxInput.Text = "";
                }
                else
                {

                    TextboxInput.Text = _commandHistory[_commandHistory.Count - _lastHistoryIndex];
                }
                e.Handled = true;

            }
            else if (e.KeyCode == Keys.Down)
            {
                if (_lastHistoryIndex != 0)
                {
                    _lastHistoryIndex--;
                }

                if (_lastHistoryIndex == 0)
                {

                    TextboxInput.Text = "";
                }
                else
                {
                    TextboxInput.Text = _commandHistory[_commandHistory.Count - _lastHistoryIndex];
                }
                e.Handled = true;

            }


        }
    }
}
