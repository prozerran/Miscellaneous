using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Runtime.CompilerServices;
using System.Windows;

// C# WPF Multi-Language support
// Please setup language files and follow these steps....
//
// 1. Create all language files in folder Resource/Language, such as...
//      Resource/Language/MutiLangEng.xaml
//      Resource/Language/MutiLangSChi.xaml
//      Resource/Language/MutiLangTChi.xaml
//
// 2. App constructor must call LanguageSetting.Initialize();
//
// 3. All language files must set to Content/[Copy Always]
//
// 4. Using the language should be DynamicResource, such as....
//      <TextBlock Text="{DynamicResource CONNECT_CONNECTIONSTATUS}" />
//
// 5. On Language Change, make sure to call LanguageSetting.SetLanguage((LanguageEnum) theLang);

namespace WpfBrowserApp1.Common
{
    public abstract class SPBindableBase : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged([CallerMemberName] string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        protected void RaisePropertyChanged([CallerMemberName] string propertyName = null)
        {
            OnPropertyChanged(propertyName);
        }

        protected virtual bool SetProperty<T>(ref T storage, T value, [CallerMemberName] string propertyName = null)
        {
            if (!EqualityComparer<T>.Default.Equals(storage, value))
            {
                storage = value;
                OnPropertyChanged(propertyName);
                return true;
            }
            return false;
        }
    }

    #region LanguageModel
    public class LanguageModel : SPBindableBase
    {
        private string _languageCode;
        private string _languageName;
        private string _languageDisplayName;
        private string _resourcefile;
        private bool _languageenabled;

        public string LanguageCode
        {
            get { return _languageCode; }
            set { SetProperty(ref _languageCode, value); }
        }

        public string LanguageName
        {
            get { return _languageName; }
            set { SetProperty(ref _languageName, value); }
        }

        public string LanguageDisplayName
        {
            get { return _languageDisplayName; }
            set { SetProperty(ref _languageDisplayName, value); }
        }

        public string Resourcefile
        {
            get { return _resourcefile; }
            set { SetProperty(ref _resourcefile, value); }
        }

        public bool LanguageEnabled
        {
            get { return _languageenabled; }
            set { SetProperty(ref _languageenabled, value); }
        }
    }
    #endregion

    #region Language
    public class Language : LanguageModel
    {
        private ResourceDictionary _resource;
        public ResourceDictionary Resource
        {
            get { return _resource; }
            set { SetProperty(ref _resource, value); }
        }
    }

    public enum LanguageEnum
    {
        ENG = 0,
        TCHI_UNICODE = 1,
        SCHI_UNICODE = 2
    }

    public class LanguageSetting
    {
        private static string _currentLanguage = "en";  //zh-cn
        public static string CurrentLanguage
        {
            get { return _currentLanguage; }
            set
            {
                if (UpdateCurrentLanguage(value))
                    _currentLanguage = value;
            }
        }

        private static List<Language> _languageIndex;
        public static List<Language> LanguageIndex
        {
            get { return _languageIndex; }
        }

        public static void Initialize()
        {
            _languageIndex = new List<Language>();
            string dirstring = AppDomain.CurrentDomain.BaseDirectory + "Resource\\Language\\";
            DirectoryInfo directory = new DirectoryInfo(dirstring);
            FileInfo[] files = directory.GetFiles();
            foreach (var item in files)
            {
                Language language = new Language();
                ResourceDictionary rd = new ResourceDictionary();
                rd.Source = new Uri(item.FullName);
                language.LanguageCode = rd["LanguageCode"] == null ? "Unknown" : rd["LanguageCode"].ToString();
                language.LanguageName = rd["LanguageName"] == null ? "Unknown" : rd["LanguageName"].ToString();
                language.LanguageDisplayName = rd["LanguageDisplayName"] == null ? "Unknown" : rd["LanguageDisplayName"].ToString();
                language.LanguageEnabled = rd["LanguageEnabled"] == null ? false : bool.Parse(rd["LanguageEnabled"].ToString());
                language.Resourcefile = item.FullName;
                language.Resource = rd;
                if (language.LanguageEnabled)
                    _languageIndex.Add(language);
            }
        }

        private static bool UpdateCurrentLanguage(string LanguageCode)
        {
            if (LanguageIndex.Exists(P => P.LanguageCode == LanguageCode && P.LanguageEnabled == true))
            {
                Language language = LanguageIndex.Find(P => P.LanguageCode == LanguageCode && P.LanguageEnabled == true);
                if (language != null)
                {
                    foreach (var item in LanguageIndex)
                    {
                        Application.Current.Resources.MergedDictionaries.Remove(item.Resource);
                    }
                    Application.Current.Resources.MergedDictionaries.Add(language.Resource);
                    return true;
                }
            }
            return false;
        }

        public static string GetLanguageValue(string key)
        {
            ResourceDictionary rd = Application.Current.Resources;
            if (rd == null)
                return string.Empty;
            object obj = rd[key];
            return obj == null ? string.Empty : obj.ToString();
        }

        public static void SetLanguage(LanguageEnum lang)
        {
            switch (lang)
            {
                case LanguageEnum.ENG: LanguageSetting.CurrentLanguage = "en"; break;
                case LanguageEnum.TCHI_UNICODE: LanguageSetting.CurrentLanguage = "zht"; break;
                case LanguageEnum.SCHI_UNICODE: LanguageSetting.CurrentLanguage = "zh"; break;
                default: LanguageSetting.CurrentLanguage = "en"; break;
            }
        }

        public static LanguageEnum GetLanguage()
        {
            string lang = LanguageSetting.CurrentLanguage;

            if (lang.Equals("zht"))
                return LanguageEnum.TCHI_UNICODE;
            else if (lang.Equals("zh"))
                return LanguageEnum.SCHI_UNICODE;
            else
                return LanguageEnum.ENG;
        }
    }
    #endregion
}
