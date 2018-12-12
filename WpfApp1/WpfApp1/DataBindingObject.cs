using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace WpfApp1
{
    public enum Market
    {
        Cash,
        Derivative,
        FX,
        Commodity,
        Security
    }

    public class DataBindingObject : INotifyPropertyChanged
    {
        public DateTime StartDate { get; set; }

        private string _name { get; set; }
        public string Name
        {
            get { return _name; }
            set
            {
                if (value != _name)
                {
                    _name = value;
                    OnPropertyChanged();
                }
            }
        }

        private string _stockcode { get; set; }
        public string StockCode
        {
            get { return _stockcode; }
            set
            {
                if (value != _stockcode)
                {
                    _stockcode = value;
                    OnPropertyChanged();
                }
            }
        }

        private string _assetclass { get; set; }
        public string AssetClass
        {
            get { return _assetclass; }
            set
            {
                if (value != _assetclass)
                {
                    _assetclass = value;
                    OnPropertyChanged();
                }
            }
        }

        private bool _isequity { get; set; }
        public bool IsEquity
        {
            get { return _isequity; }
            set
            {
                if (value != _isequity)
                {
                    _isequity = value;
                    OnPropertyChanged();
                }
            }
        }

        private Market _broker { get; set; }
        public Market Broker
        {
            get { return _broker; }
            set
            {
                if (value != _broker)
                {
                    _broker = value;
                    OnPropertyChanged();
                }
            }
        }

        private int _num { get; set; }
        public int Number
        {
            get { return _num; }
            set
            {
                if (value != _num)
                {
                    _num = value;
                    OnPropertyChanged();
                }
            }
        }

        public static DataBindingObject GetDataBindingObject()
        {
            var obj = new DataBindingObject()
            {
                Name = "FIRST",
            };
            return obj;
        }

        public event PropertyChangedEventHandler PropertyChanged;
        
        // for 2-way binding over INPC
        private void OnPropertyChanged(
            [CallerMemberName] string caller = "")
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(caller));
            }
        }

        // for binding list
        public static ObservableCollection<DataBindingObject> GetDataBindingObjects()
        {
            var obj = new ObservableCollection<DataBindingObject>();
            obj.Add(new DataBindingObject() { Name = "BTC" });
            obj.Add(new DataBindingObject() { Name = "ETH" });
            return obj;
        }

        public static void StartWork(ObservableCollection<DataBindingObject> obj)
        {
            var r = new Random();

            while (true)
            {
                for (int i = 0; i < 4; i++) 
                    obj[i].Number = r.Next(1, 100);

                Thread.Sleep(200);
            }
        }

        // for binding list
        public static ObservableCollection<DataBindingObject> GetDataGridObjects()
        {
            var obj = new ObservableCollection<DataBindingObject>();
            obj.Add(new DataBindingObject() { StockCode = "BTC", AssetClass = "Token", IsEquity = false, Broker = Market.Cash, Number = 5 });
            obj.Add(new DataBindingObject() { StockCode = "ETH", AssetClass = "Utility", IsEquity = false, Broker = Market.Derivative, Number = 11 });
            obj.Add(new DataBindingObject() { StockCode = "LTC", AssetClass = "Utility", IsEquity = true, Broker = Market.Commodity, Number = 13 });
            obj.Add(new DataBindingObject() { StockCode = "BCH", AssetClass = "Security", IsEquity = true, Broker = Market.FX, Number= 15 });

            // infinite loop to change data
            Task.Run(() => StartWork(obj));

            return obj;
        }
    }
}
