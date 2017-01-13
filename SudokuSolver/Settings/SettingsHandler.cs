using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Media.Imaging;

namespace SudokuSolver.Utility
{
    class SettingsHandler
    {
        public Image imgControl;
        public BitmapImage image;
        public string msg;
        public SettingsHandler(Image imgControl, BitmapImage img, string msg)
        {
            this.imgControl = imgControl;
            this.image = img;
            this.msg = msg;
        }
    }
}