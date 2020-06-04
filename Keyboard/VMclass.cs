using Keyboard.Helper_Class;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Input;
using System.Diagnostics;

namespace Keyboard
{
    //ViewModel class
    class VMclass:INotifyPropertyChanged
    {
        #region INotifyPropertyChanged class realization
        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged([CallerMemberName]string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        #endregion

        #region Private members
        private bool isCaps;
        private bool _isEnabledKeys;
        int position;
        char symbol;
        private string _currentstring;
        private string _randomstr;
        private KeyEventArgs _currentKeyArg;
        Stopwatch sWatch; 
        ModelClass model;
        #endregion

        public VMclass()
        {
            model = new ModelClass();
            InitGame();
            isCaps = false;
            _isEnabledKeys = false;
            sWatch = new Stopwatch();
        }

        #region Public ViewModel Properties
        public bool CapsLocked { get { return isCaps; } set { isCaps = value; OnPropertyChanged(nameof(isCaps)); } }
        public bool isEnabledAllKeys { get { return _isEnabledKeys; } set { _isEnabledKeys = value; OnPropertyChanged(nameof(isEnabledAllKeys)); } }
        public int GetSpeed { get { return model.Speed; } set { model.Speed = value; OnPropertyChanged(nameof(GetSpeed)); } }
        public int GetFails { get { return model.Fails; } set { model.Fails = value; OnPropertyChanged(nameof(GetFails)); } }
        public int GetDifficulty { get { return model.Difficulty; } set { model.Difficulty = value; OnPropertyChanged(nameof(GetDifficulty)); } }
        public bool GetisUpperCase { get { return model.isUpperCase; } set { model.isUpperCase = value; OnPropertyChanged(nameof(GetisUpperCase)); } }
        public string GetRandomString { get { return model.RandomString; } set { model.RandomString = value; OnPropertyChanged(nameof(GetRandomString)); } }
        public string GetCurrentString { get { return _currentstring; } set { _currentstring = value; OnPropertyChanged(nameof(GetCurrentString)); } }
        public KeyEventArgs CurrentKeyArg { get { return _currentKeyArg; } set { _currentKeyArg = value; OnPropertyChanged(nameof(CurrentKeyArg)); OnkeyPressed(); } }
        #endregion

        private void InitGame()
        {
            model.Speed = 0;
            model.Fails = 0;
            model.RandomString = string.Empty;
            GetCurrentString = string.Empty;
            position = 0;
            symbol = ' ';
            OnPropertyChanged(nameof(GetSpeed));
            OnPropertyChanged(nameof(GetFails));
        }

        #region Some Behaviour Logics and Commands of ViewModel class

        private void OnkeyPressed()
        {
            if (position >= GetRandomString.Length)
                return;
            symbol = Keyboards.GetKey(_currentKeyArg, isCaps);
            if (GetRandomString[position] == symbol)
            {
                GetCurrentString += symbol;
                position++;
            }
            else
                GetFails++;

            CalcSpeed();
        }

        private void CalcSpeed()
        {
            if (position > 5)
               GetSpeed = (int)(60.0 / (((double)sWatch.ElapsedMilliseconds / 1000) / position));         
        }

        private ICommand _CommandStart;
        public ICommand ButtonStart//command start game
        {
            get
            {
                if (_CommandStart == null)
                {
                    _CommandStart = new RelayCommand(param => MakeStartGame(), param => _isEnabledKeys ? false:true);
                }
                return _CommandStart;
            }
        }

        private ICommand _CommandStop;
        public ICommand ButtonStop//command stop game
        {
            get
            {
                if (_CommandStop == null)
                {
                    _CommandStop = new RelayCommand(param => MakeStopGame(), param => _isEnabledKeys ? true:false);
                }
                return _CommandStop;
            }
        }

        private void MakeStartGame()
        {
            InitGame();
            model.GenerateRandomString();
            isEnabledAllKeys = true;
            _randomstr = GetRandomString;
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(GetRandomString)));
            sWatch.Start();
        }

        private void MakeStopGame()
        {
            isEnabledAllKeys = false;
            sWatch.Stop();
            sWatch.Reset();
            model.RandomString = string.Empty;
            GetCurrentString = string.Empty;
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(GetRandomString)));
        }
        #endregion
    }


    static class Keyboards
    {
        ///This static class has just one method to coverse KeyEventArg to charcode
        ///Two switches usud for Lower and Upper char cenversion respectively
        ///
        #region Static GetKey Method return charcode of pressed non system key
        public static char GetKey(KeyEventArgs keyarg, bool isCapslock)
        {
            string symbol = " ";
            if ((keyarg.KeyboardDevice.Modifiers != ModifierKeys.Shift && isCapslock != true) ||
                (keyarg.KeyboardDevice.Modifiers == ModifierKeys.Shift && isCapslock == true))
            {
                switch (keyarg.Key.ToString())
                {
                    case "A":
                    case "B":
                    case "C":
                    case "D":
                    case "E":
                    case "F":
                    case "G":
                    case "H":
                    case "I":
                    case "J":
                    case "K":
                    case "L":
                    case "M":
                    case "N":
                    case "O":
                    case "P":
                    case "Q":
                    case "R":
                    case "S":
                    case "T":
                    case "U":
                    case "V":
                    case "W":
                    case "X":
                    case "Y":
                    case "Z":
                        symbol = keyarg.Key.ToString();
                        symbol = symbol.ToLower();
                        break;
                    case "Oem3":
                        symbol = "`";
                        break;
                    case "OemMinus":
                        symbol = "-";
                        break;
                    case "OemPlus":
                        symbol = "=";
                        break;
                    case "OemOpenBrackets":
                        symbol = "[";
                        break;
                    case "Oem6":
                        symbol = "]";
                        break;
                    case "Oem5":
                        symbol = "\\";
                        break;
                    case "Oem1":
                        symbol = ";";
                        break;
                    case "OemQuotes":
                        symbol = "'";
                        break;
                    case "OemComma":
                        symbol = ",";
                        break;
                    case "OemPeriod":
                        symbol = ".";
                        break;
                    case "OemQuestion":
                        symbol = "/";
                        break;
                    case "D1":
                        symbol = "1";
                        break;
                    case "D2":
                        symbol = "2";
                        break;
                    case "D3":
                        symbol = "3";
                        break;
                    case "D4":
                        symbol = "4";
                        break;
                    case "D5":
                        symbol = "5";
                        break;
                    case "D6":
                        symbol = "6";
                        break;
                    case "D7":
                        symbol = "7";
                        break;
                    case "D8":
                        symbol = "8";
                        break;
                    case "D9":
                        symbol = "9";
                        break;
                    case "D0":
                        symbol = "0";
                        break;
                }
            }
            else
            {
                switch (keyarg.Key.ToString())
                {
                    case "A":
                    case "B":
                    case "C":
                    case "D":
                    case "E":
                    case "F":
                    case "G":
                    case "H":
                    case "I":
                    case "J":
                    case "K":
                    case "L":
                    case "M":
                    case "N":
                    case "O":
                    case "P":
                    case "Q":
                    case "R":
                    case "S":
                    case "T":
                    case "U":
                    case "V":
                    case "W":
                    case "X":
                    case "Y":
                    case "Z":
                        symbol = keyarg.Key.ToString();
                        break;
                    case "Oem3":
                        symbol = "~";
                        break;
                    case "OemMinus":
                        symbol = "_";
                        break;
                    case "OemPlus":
                        symbol = "+";
                        break;
                    case "OemOpenBrackets":
                        symbol = "{";
                        break;
                    case "Oem6":
                        symbol = "}";
                        break;
                    case "Oem5":
                        symbol = "|";
                        break;
                    case "Oem1":
                        symbol = ":";
                        break;
                    case "OemQuotes":
                        symbol = "\"";
                        break;
                    case "OemComma":
                        symbol = "<";
                        break;
                    case "OemPeriod":
                        symbol = ">";
                        break;
                    case "OemQuestion":
                        symbol = "?";
                        break;
                    case "D1":
                        symbol = "!";
                        break;
                    case "D2":
                        symbol = "@";
                        break;
                    case "D3":
                        symbol = "#";
                        break;
                    case "D4":
                        symbol = "$";
                        break;
                    case "D5":
                        symbol = "%";
                        break;
                    case "D6":
                        symbol = "^";
                        break;
                    case "D7":
                        symbol = "&";
                        break;
                    case "D8":
                        symbol = "*";
                        break;
                    case "D9":
                        symbol = "(";
                        break;
                    case "D0":
                        symbol = ")";
                        break;
                }
            }       

            return symbol[0];
        }
        #endregion
    }

}
