using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

using SudokuSolver.Utility;

namespace SudokuSolver.Settings
{
    /// <summary>
    /// Interaction logic for InterfaceSettingsWindow.xaml
    /// </summary>
    public partial class InterfaceSettingsWindow : Window
    {
        public InterfaceSettingsWindow()
        {
            InitializeComponent();
        }

        /// <summary>
        /// This is the event that fires when the ISW is loaded.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void gridWindow_Loaded(object sender, RoutedEventArgs e)
        {
            int counter = 0;
            foreach (Language lang in Enum.GetValues(typeof(Language)))
            {
                comboLanguage.Items.Add(lang);
                if (lang == InterfaceSettings.selectedLanguage)
                {
                    comboLanguage.SelectedIndex = counter;
                }
                counter++;
            }

            SetText();
        }

        /// <summary>
        /// Sets the text of all the controls, based on the selected language.
        /// </summary>
        private void SetText()
        {
            interfaceSettingsWindow.Title = LanguageReader.GetText("interfaceSettingsWindow", InterfaceSettings.selectedLanguage);
            btnApply.Content = LanguageReader.GetText("btnApply", InterfaceSettings.selectedLanguage);
        }

        /// <summary>
        /// Function that triggers when "btnApply" is clicked
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnApply_Click(object sender, RoutedEventArgs e)
        {
            InterfaceSettings.selectedLanguage = (Language)comboLanguage.SelectedItem;
            SetText();

            MainWindow main = (MainWindow)this.Owner;
            main.SetText();

            Properties.Settings.Default.language = InterfaceSettings.selectedLanguage.ToString();
            Properties.Settings.Default.Save();
        }
    }
}