using System;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;

namespace Librometer.Views.Behaviors
{
    public class ReceiverTextBoxKeyDown
    {
        LivetWeakEventListener<KeyEventHandler, KeyEventArgs> _listener;//strong reference of listener
        private ICommand _command;

        public ReceiverTextBoxKeyDown(TextBox t, ICommand command)
        {
            _listener = new LivetWeakEventListener<KeyEventHandler, KeyEventArgs>(
                        h => new KeyEventHandler(h),
                        h => t.KeyDown += h,
                        h => t.KeyDown -= h,
                        OnKeyDownEvent);
            this._command = command;
        }

        void OnKeyDownEvent(object sender, KeyEventArgs e)
        {
            if (e == null) return;
            if (e.Key != Key.Enter
                /*RGE || e.KeyboardDevice.Modifiers != ModifierKeys.None*/) return;

            //On obtient la textbox
            var senderDependencyObject = sender as TextBox;
            if (senderDependencyObject == null) return;

            /*RGE var cmd = GetCommand(senderDependencyObject) as ICommand;*/
            var cmd = this._command;
            if (cmd == null) return;

            //On execute la commande sur le dispatcher
            senderDependencyObject.Dispatcher.BeginInvoke(
                new Action(() =>
                {
                    if (cmd.CanExecute(null)) cmd.Execute(null);
                })/*RGE ,
                DispatcherPriority.Render, null*/);
        }


    }
}
