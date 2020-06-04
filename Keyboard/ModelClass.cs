using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using System.Runtime.CompilerServices;


namespace Keyboard
{
    class ModelClass: INotifyPropertyChanged
    {
        #region INotifyPropertyChanged Members

        /// <summary>
        /// Raises the PropertyChange event for the property specified
        /// </summary>
        /// <param name="propertyName">Property name to update. Is case-sensitive.</param>
        public virtual void RaisePropertyChanged(string propertyName)
        {
            OnPropertyChanged(propertyName);
        }

        /// <summary>
        /// Raised when a property on this object has a new value.
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// Raises this object's PropertyChanged event.
        /// </summary>
        /// <param name="propertyName">The property that has a new value.</param>
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = "")
        {

            PropertyChangedEventHandler handler = this.PropertyChanged;
            if (handler != null)
            {
                var e = new PropertyChangedEventArgs(propertyName);
                handler(this, e);
            }
        }

        #endregion // INotifyPropertyChanged Members

        #region Private Model Members
        const int size = 100;
        private int _speed;
        private int _fails;
        private int _difficulty;
        private bool _isUpperCase;
        private string _randomstring;
        Random rnd;
        #endregion

        public ModelClass()
        {
            rnd = new Random();
            _speed = 0;
            _fails = 0;
            _difficulty = 40;
            _isUpperCase = false;
            _randomstring = string.Empty;
        }

        #region Public Properties
        public int Speed { get { return _speed; } set { _speed = value; OnPropertyChanged(nameof(Speed)); } }
        public int Fails { get { return _fails; } set { _fails = value; OnPropertyChanged(nameof(Fails)); } }
        public int Difficulty { get { return _difficulty; } set { _difficulty = value; OnPropertyChanged(nameof(Difficulty)); } }
        public bool isUpperCase { get { return _isUpperCase; } set { _isUpperCase = value; OnPropertyChanged(nameof(isUpperCase)); } }
        public string RandomString { get { return _randomstring; } set { _randomstring = value; OnPropertyChanged(nameof(RandomString)); } }
        #endregion

        #region Just One Method Generating random string depending of difficulty and case sensitivity
        public void GenerateRandomString()
        {
            if (_difficulty > KeyboardKeys.lstring.Length || _difficulty > KeyboardKeys.ustring.Length)
                return;
            StringBuilder randtext = new StringBuilder();
            int counter = 0;
            for(int i = 0; i < size; i++)
            {
                randtext.Append(KeyboardKeys.lstring[rnd.Next(0, _difficulty)]);
                if(_isUpperCase)
                {
                    randtext.Append(KeyboardKeys.ustring[rnd.Next(0, _difficulty)]);
                    i++;
                    counter++;
                }

                if (i % rnd.Next(4, 7) == 0 && counter >= 4)
                { randtext.Append(' '); counter = 0; }
                counter++;
            }
            RandomString = randtext.ToString();
        }
        #endregion
    }

    static class KeyboardKeys
    {
        public const string lstring = @"abcdefghijklmnopqrstuvwxyz 1234567890[];'\,./`-=";
        public const string ustring = @"ABCDEFGHIJKLMNOPQRSTUVWXYZ !@#$%^&*(){}|:<>?~_+""";      
    }
}
