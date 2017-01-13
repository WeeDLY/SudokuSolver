using System;
using System.IO;
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
    /// Interaction logic for GameSettingsWindow.xaml
    /// </summary>
    public partial class GameSettingsWindow : Window
    {
        private ImageHelper imgHelper = new ImageHelper();

        private bool changesMade; // Keeps track if the user made any changes to the GameSettingsWindow

        public GameSettingsWindow()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Event that triggers when the gameSettingsWindow is loaded.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void gameSettingsWindow_Loaded(object sender, RoutedEventArgs e)
        {
            textCol.Text = GameSettings.colSize.ToString();
            textRow.Text = GameSettings.rowSize.ToString();
            textGrid.Text = GameSettings.gridSize.ToString();
            textMin.Text = GameSettings.minValue.ToString();
            textMax.Text = GameSettings.maxValue.ToString();

            changesMade = false;

            SetAllCorrect();
            SetText();
        }

        /// <summary>
        /// Sets the text of all the controls, based on the selected language.
        /// </summary>
        private void SetText()
        {
            gameSettingsTitle.Title = LanguageReader.GetText("gameSettingsTitle", InterfaceSettings.selectedLanguage);
            gswHeader.Content = LanguageReader.GetText("gswHeader", InterfaceSettings.selectedLanguage);

            lblColumns.Content = LanguageReader.GetText("lblColumns", InterfaceSettings.selectedLanguage);
            lblRows.Content = LanguageReader.GetText("lblRows", InterfaceSettings.selectedLanguage);
            lblGrid.Content = LanguageReader.GetText("lblGrid", InterfaceSettings.selectedLanguage);
            lblMin.Content = LanguageReader.GetText("lblMin", InterfaceSettings.selectedLanguage);
            lblMax.Content = LanguageReader.GetText("lblMax", InterfaceSettings.selectedLanguage);

            btnApply.Content = LanguageReader.GetText("btnApply", InterfaceSettings.selectedLanguage);
            btnCancel.Content = LanguageReader.GetText("btnCancel", InterfaceSettings.selectedLanguage);
        }

        /// <summary>
        /// Sets all the Image controls to the correct image.
        /// </summary>
        private void SetAllCorrect()
        {
            colImg.Source = imgHelper.BitmapToImageSource(Properties.Resources.correct);
            rowImg.Source = imgHelper.BitmapToImageSource(Properties.Resources.correct);
            gridImg.Source = imgHelper.BitmapToImageSource(Properties.Resources.correct);
            minImg.Source = imgHelper.BitmapToImageSource(Properties.Resources.correct);
            maxImg.Source = imgHelper.BitmapToImageSource(Properties.Resources.correct);
        }

        /// <summary>
        /// Function that triggers when btnApply is clicked.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnApply_Click(object sender, RoutedEventArgs e)
        {
            if (!changesMade)
                return;

            SetAllCorrect();

            List<SettingsHandler> invalidSettings = new List<SettingsHandler>();

            int colNum = stringToNum(textCol.Text);
            if (HigherThan(GameSettings.maxColSize, colNum))
            {
                string errorMsg = LanguageReader.GetText("errorCol", InterfaceSettings.selectedLanguage);
                errorMsg += GameSettings.maxColSize + ".";
                invalidSettings.Add(new SettingsHandler(colImg,
                                    imgHelper.BitmapToImageSource(Properties.Resources.incorrect),
                                    errorMsg));
            }

            int rowNum = stringToNum(textRow.Text);
            if (HigherThan(GameSettings.maxRowSize, rowNum))
            {
                string errorMsg = LanguageReader.GetText("errorRow", InterfaceSettings.selectedLanguage);
                errorMsg += GameSettings.maxRowSize + ".";
                invalidSettings.Add(new SettingsHandler(rowImg,
                                    imgHelper.BitmapToImageSource(Properties.Resources.incorrect),
                                    errorMsg));
            }

            int gridNum = stringToNum(textGrid.Text);
            if (HigherThan(GameSettings.maxGridSize, gridNum))
            {
                string errorMsg = LanguageReader.GetText("errorGrid", InterfaceSettings.selectedLanguage);
                errorMsg += GameSettings.maxGridSize + ".";
                invalidSettings.Add(new SettingsHandler(gridImg,
                    imgHelper.BitmapToImageSource(Properties.Resources.incorrect),
                    errorMsg));
            }

            int minNum = stringToNum(textMin.Text);
            int maxNum = stringToNum(textMax.Text);

            int lowestDiff = lowestDifference(colNum, rowNum, gridNum);
            bool diffValid = ValidDifference(lowestDiff, minNum, maxNum);
            if(!diffValid)
            {
                string errorMsg = LanguageReader.GetText("errorDifference", InterfaceSettings.selectedLanguage);
                errorMsg += lowestDiff + ".";
                invalidSettings.Add(new SettingsHandler(minImg,
                                    imgHelper.BitmapToImageSource(Properties.Resources.incorrect),
                                    errorMsg));

                invalidSettings.Add(new SettingsHandler(maxImg,
                                    imgHelper.BitmapToImageSource(Properties.Resources.incorrect),
                                    String.Empty));
            }

            if (invalidSettings.Count <= 0)
            {
                GameSettings.colSize = colNum;
                GameSettings.rowSize = rowNum;
                GameSettings.gridSize = gridNum;
                GameSettings.minValue = minNum;
                GameSettings.maxValue = maxNum;

                Properties.Settings.Default.colSize = GameSettings.colSize;
                Properties.Settings.Default.rowSize = GameSettings.rowSize;
                Properties.Settings.Default.gridSize = GameSettings.gridSize;
                Properties.Settings.Default.minValue = GameSettings.minValue;
                Properties.Settings.Default.maxValue = GameSettings.maxValue;
                Properties.Settings.Default.Save();

                string header = LanguageReader.GetText("gameSettingsAppliedHeader", InterfaceSettings.selectedLanguage);
                string body = LanguageReader.GetText("gameSettingsApplied", InterfaceSettings.selectedLanguage);
                MessageBox.Show(body, header, MessageBoxButton.OK, MessageBoxImage.Information);
            }
            else
            {
                StringBuilder sb = new StringBuilder();
                foreach(SettingsHandler sh in invalidSettings)
                {
                    sh.imgControl.Source = sh.image;
                    sb.Append(sh.msg + "\n");
                }
                MessageBox.Show(sb.ToString());
            }
        }

        /// <summary>
        /// Function that triggers when btnCancel is clicked
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        /// <summary>
        /// Event that is triggered when the GameSettingsWindow is closed.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Window_Closed(object sender, EventArgs e)
        {
            if(changesMade)
            {
                MainWindow main = (MainWindow)this.Owner;
                main.CreateSudoku();
            }
        }

        #region TextBox Changed event
        private void textCol_TextChanged(object sender, TextChangedEventArgs e)
        {
            colImg.Source = imgHelper.BitmapToImageSource(Properties.Resources.qMark);
            changesMade = true;
        }
        private void textRow_TextChanged(object sender, TextChangedEventArgs e)
        {
            rowImg.Source = imgHelper.BitmapToImageSource(Properties.Resources.qMark);
            changesMade = true;
        }

        private void textGrid_TextChanged(object sender, TextChangedEventArgs e)
        {
            gridImg.Source = imgHelper.BitmapToImageSource(Properties.Resources.qMark);
            changesMade = true;
        }

        private void textMin_TextChanged(object sender, TextChangedEventArgs e)
        {
            minImg.Source = imgHelper.BitmapToImageSource(Properties.Resources.qMark);
            changesMade = true;
        }

        private void textMax_TextChanged(object sender, TextChangedEventArgs e)
        {
            maxImg.Source = imgHelper.BitmapToImageSource(Properties.Resources.qMark);
            changesMade = true;
        }
        #endregion

        private bool HigherThan(int max, int num)
        {
            return max < num ? true : false;
        }

        /// <summary>
        /// Returns true if the current difference between min and max is larger than the lowest difference
        /// Returns false otherwise
        /// </summary>
        /// <param name="lowestDiff"></param>
        /// <param name="min"></param>
        /// <param name="max"></param>
        /// <returns></returns>
        private bool ValidDifference(int lowestDiff, int min, int max)
        {
            int diff = max - min + 1;
            return diff >= lowestDiff;
        }

        /// <summary>
        /// Returns the lowest difference, based on the column, row and grid
        /// </summary>
        /// <param name="col"></param>
        /// <param name="row"></param>
        /// <param name="grid"></param>
        /// <returns></returns>
        private int lowestDifference(int col, int row, int grid)
        {
            int highest = col > row ? col : row;
            grid *= grid;
            if (grid > highest)
                return grid;
            return highest;
        }

        /// <summary>
        /// Function that converts string to an integer.
        /// Returns 0, if it's unable to convert to an integer.
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        private int stringToNum(string text)
        {
            int num;
            int.TryParse(text, out num);
            return num;
        }
    }
}