
namespace Puzzle
{
    sealed class PuzzlOne : System.Windows.UIElement, IPuzzle
    {
        Sum[] sum;
        Selector selected;

        public string LawGame { get; } = "Розріжте дану площину на 5 прямокутних частин так, щоб сума чисел у кожній частині була однаковою.";

        public event WinPuzzleDelageta WinPuzzle;

        public void InitializeNumber(ref Number[] num)
        {
            for (byte i = 0; i < num.Length; i += 2)
            {
                num[i].MouseUp += Number_Click;
                num[i + 1].MouseUp += Number_Click;
            }
        }

        public void InitializeSelector(ref Selector[] selector)
        {
            sum = selector;

            selector[0].MouseUp += Selector_Click;
            selector[1].MouseUp += Selector_Click;
            selector[2].MouseUp += Selector_Click;
            selector[3].MouseUp += Selector_Click;
            selector[4].MouseUp += Selector_Click;
        }

        void Calculate(Number n)
        {
            if (n.Sum != null)
            {
                if (selected == n.Sum)
                {
                    selected.Decrement(n);
                    n.Sum = null;
                }
                else
                {
                    selected.Increment(n);
                    n.Sum.Decrement(n);
                    n.Sum = selected;
                }
            }
            else
            {
                selected.Increment(n);
                n.Sum = selected;
            }
        }

        void Number_Click(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            if (selected != null)
            {
                var num = sender as Number;

                num.Background = selected != num.Sum ?
                    selected.Color : System.Windows.Media.Brushes.White;

                Calculate(num);

                if (sum[0].Collected && sum[1].Collected && sum[2].Collected &&
                    sum[3].Collected && sum[4].Collected)
                {
                    WinPuzzle?.Invoke();
                }
            }
        }

        void Selector_Click(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            selected?.Reset();
            selected = sender as Selector;
        }
    }

    sealed class Number : System.Windows.Controls.Grid, System.ComponentModel.INotifyPropertyChanged
    {
        byte _value;

        public Sum Sum { get; set; }
        public byte Identify { get; set; }

        public byte Value
        {
            get { return _value; }
            set
            {
                _value = value;
                OnPropertyChanged();
            }
        }

        public event System.ComponentModel.PropertyChangedEventHandler PropertyChanged;

        void OnPropertyChanged([System.Runtime.CompilerServices.CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new System.ComponentModel.PropertyChangedEventArgs(propertyName));
        }
    }

    abstract class Sum : System.Windows.Controls.Grid, System.ComponentModel.INotifyPropertyChanged
    {
        byte sum, count, identify;

        public bool Collected { get; private set; } = false;

        public byte CalcSum
        {
            get { return sum; }
            private set
            {
                sum = value;
                OnPropertyChanged();
            }
        }

        public void Increment(Number n)
        {
            ++count;
            CalcSum += n.Value;
            identify += n.Identify;

            Scaning();
        }

        public void Decrement(Number n)
        {
            --count;
            CalcSum -= n.Value;
            identify -= n.Identify;

            Scaning();
        }

        void Scaning()
        {
            if (CalcSum == 7 && identify / count == count)
                Collected = true;
            else
                Collected = false;
        }

        public event System.ComponentModel.PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged([System.Runtime.CompilerServices.CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new System.ComponentModel.PropertyChangedEventArgs(propertyName));
        }
    }

    sealed class Selector : Sum
    {
        double select;
        bool isSelected;

        public System.Windows.Media.Brush Color { set; get; }

        public double SelectStroke
        {
            get { return select; }
            set
            {
                select = value;
                OnPropertyChanged();
            }
        }

        public void Reset()
        {
            SelectStroke = 0.0;
            isSelected = false;
        }

        protected sealed override void OnMouseUp(System.Windows.Input.MouseButtonEventArgs e)
        {
            if (!isSelected)
            {
                SelectStroke = 7.0;
                isSelected = true;

                base.OnMouseUp(e);
            }
            else e.Handled = true;
        }
    }
}