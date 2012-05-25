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
    // Cette implémentation engendre des fuites de mémoires.
    // Pour y pallier il faudrait utiliser des "WeakEvents".
    public class CommandOnEnter : DependencyObject
    {
        #region Command
        /// <summary>
        /// La propriété attachée de type ICommand.
        /// </summary>
        public static readonly DependencyProperty CommandProperty =
            DependencyProperty.RegisterAttached("Command",
            typeof(ICommand), typeof(CommandOnEnter),
            new PropertyMetadata((ICommand)null, OnCommandChanged));

        /// <summary>
        /// Donne la commande executée lors de l'appuie sur la touche entrée.
        /// </summary>
        public static ICommand GetCommand(DependencyObject d)
        {
            return (ICommand)d.GetValue(CommandProperty);
        }

        /// <summary>
        /// Fixe la commande executée lors de l'appuie sur la touche entrée.
        /// </summary>
        public static void SetCommand(DependencyObject d, ICommand value)
        {
            d.SetValue(CommandProperty, value);
        }

        /// <summary>
        /// Gestion du changement de valeur de la propriété attachée.
        /// </summary>
        private static void OnCommandChanged(DependencyObject d,
            DependencyPropertyChangedEventArgs e)
        {
            var textBox = d as TextBox;
            if (textBox == null)
                throw new ArgumentOutOfRangeException(
                    @"Le behavior CommandOnEnter ne peut être
                        utilisé que sur un TextBox.");

            /*RGE textBox.PreviewKeyDown += textBox_PreviewKeyDown;*/
            /*RGE textBox.KeyDown += new KeyEventHandler(textBox_KeyDown);*/
            textBox.KeyDown += textBox_KeyDown;
            //var receiver = new WeakReference(new ReceiverTextBoxKeyDown(textBox, GetCommand(textBox)));

        }

        static void textBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e == null) return;
            if (e.Key != Key.Enter
                /*RGE || e.KeyboardDevice.Modifiers != ModifierKeys.None*/) return;

            //On obtient la textbox
            var senderDependencyObject = sender as TextBox;
            if (senderDependencyObject == null) return;

            var cmd = GetCommand(senderDependencyObject) as ICommand;
            if (cmd == null) return;

            //On execute la commande sur le dispatcher
            senderDependencyObject.Dispatcher.BeginInvoke(
                new Action(() =>
                {
                    if (cmd.CanExecute(null)) cmd.Execute(null);
                })/*RGE ,
                DispatcherPriority.Render, null*/);
        }
        #endregion
    }
}
