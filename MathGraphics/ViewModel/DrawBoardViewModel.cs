using GalaSoft.MvvmLight;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace MathGraphics
{
   public class DrawBoardViewModel: ViewModelBase
    {
        private ObservableCollection<MyFontFamily> fonts;
        private ObservableCollection<int> fontSizeList;
        private MyFontFamily selectedFontFamily;
        private ObservableCollection<string> foregrounds;
        private int selectedFontSize;
        private string selectedColor;
        private ObservableCollection<string> lineBrushes;
        private string selectedLineColor = string.Empty;     

        public DrawBoardViewModel()
        {

            CanvasSize canvasSize1 = new CanvasSize("100%", 1.0);
            CanvasSize canvasSize2 = new CanvasSize("125%", 1.25);
            CanvasSize canvasSize3 = new CanvasSize("150%", 1.5);
            CnavasSizeList.Add(canvasSize1);
            CnavasSizeList.Add(canvasSize2);
            CnavasSizeList.Add(canvasSize3);


            List<MyFontFamily> myFontList = new List<MyFontFamily>();

            myFontList = GetAllFonts();

            fonts = new ObservableCollection<MyFontFamily>(myFontList);
            fontSizeList = new ObservableCollection<int>() { 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17, 18, 19, 20, 21, 22, 23, 24, 26, 28, 32, 36, 42, 46, 50 };
            SelectedFontSize = fontSizeList.ToList().Find(p => p == 16);
            foregrounds = new ObservableCollection<string>() { "White", "Black", "Gray", "Green", "Blue", "Yellow", "Orange" };
            selectedColor = foregrounds.First();
            selectedFontFamily = myFontList.Find(p => p.FontName == "微软雅黑");


            var allColors = GetAllColors();
            lineBrushes = new ObservableCollection<string>(allColors);

            selectedLineColor = allColors.Find(p => p == "Red");
        }

        private List<string> GetAllColors()
        {

            List<string> colorNames = new List<string>();
            Type colorsType = typeof(Colors);
            var properties = colorsType.GetProperties();
            foreach (var item in properties)
            {
                colorNames.Add(item.Name);
            }

            return colorNames;
        }



        public string SelectedLineBrush
        {
            get { return selectedLineColor; }
            set { Set(ref selectedLineColor, value); }
        }


        public ObservableCollection<string> LineBrushes
        {
            get { return lineBrushes; }
            set { lineBrushes = value; }
        }


        private ObservableCollection<CanvasSize> canvasSizeList = new ObservableCollection<CanvasSize>();

        public ObservableCollection<CanvasSize> CnavasSizeList
        {
            get { return canvasSizeList; }
            set { Set(ref canvasSizeList, value); }
        }

        private CanvasSize selectedCanvasSize;

        public CanvasSize SelectedCanvasSize
        {
            get { return selectedCanvasSize; }
            set { Set(ref selectedCanvasSize, value); }
        }

        public string SelectedColor
        {
            get { return selectedColor; }
            set { Set(ref selectedColor, value); }
        }

        public ObservableCollection<string> Foregrounds
        {
            get { return foregrounds; }
            set { Set(ref foregrounds, value); }
        }

        public int SelectedFontSize
        {
            get { return selectedFontSize; }
            set { Set(ref selectedFontSize, value); }
        }

        public ObservableCollection<int> FontSizeList
        {
            get { return fontSizeList; }
            set { Set(ref fontSizeList, value); }
        }

        public MyFontFamily SelectedFontFamily
        {
            get { return selectedFontFamily; }
            set { Set(ref selectedFontFamily, value); }
        }

        public ObservableCollection<MyFontFamily> AllFonts
        {
            get { return fonts; }
            set { Set(ref fonts, value); }
        }

        private List<MyFontFamily> GetAllFonts()
        {
            List<MyFontFamily> myFontList = new List<MyFontFamily>();
            var fontCollection = Fonts.GetFontFamilies(@"c:\Windows\Fonts");

            if (fontCollection != null)
            {
                foreach (var item in fontCollection)
                {
                    var name = string.Empty;
                    var languageItem = System.Windows.Markup.XmlLanguage.GetLanguage("zh-cn");
                    if (item.FamilyNames.Keys.Contains(languageItem))
                    {
                        name = item.FamilyNames[languageItem];
                    }
                    else
                    {
                        var enLanguageItem = System.Windows.Markup.XmlLanguage.GetLanguage("en-us");
                        name = item.FamilyNames[enLanguageItem];
                    }
                    var myFontItem = new MyFontFamily() { FontFamilyEntity = item, FontName = name };

                    myFontList.Add(myFontItem);
                }
            }

            return myFontList;
        }

    }
}
