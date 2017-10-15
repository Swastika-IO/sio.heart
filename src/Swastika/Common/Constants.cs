using System;
using System.Collections.Generic;
using System.Text;

namespace Swastika.Common
{
    public class Constants
    {        
        public enum SearchType
        {
            All,
            Article,
            Module,
            Page
        }

        public enum TemplateFolder
        {
            Layouts,
            Pages,
            Modules,
            Articles,
            Widgets,

        }
        public enum ViewModelType
        {
            FrontEnd = 0,
            BackEnd = 1
        }

        public enum CateType
        {
            Blank = 0,
            Article = 1,
            List = 2,
            Home = 3,
            StaticUrl = 4,
            Modules = 5
        }

        public enum CatePosition
        {
            Top = 1,
            Left = 2,
            Footer = 3
        }

        public enum DataType
        {
            String = 0,
            Int = 1,
            Image = 2,
            Icon = 3,
            CodeEditor = 4,
            Html = 5,
            TextArea = 6,
            Boolean = 7
        }
    }
}
