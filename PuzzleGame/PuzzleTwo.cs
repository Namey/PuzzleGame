
namespace Puzzle
{
    sealed class PuzzleTwo : System.Windows.UIElement, IPuzzle, System.ComponentModel.INotifyPropertyChanged
    {
        sbyte count;

        public event WinPuzzleDelageta WinPuzzle;

        public string LawGame { get; } = "Мета гри - розмістити на шаховій площині 8 фігур так, щоб вони не потрапляли на лінії вогню один одного (вертикально, горизонтально або діагонально).";
        public string ImageQueen { get; set; } = "";

        public sbyte Count
        {
            get { return count; }
            private set
            {
                count = value;
                OnPropertyChanged();
            }
        }

        public void Initialize(ref Core[] c)
        {
            Core[,] core = new Core[8, 8];

            foreach (var v in c)
            {
                core[v.X, v.Y] = v;
                v.MouseUp += Core_Click;
            }

            for (int x = 0; x < 8; x++)
            {
                for (int y = 0; y < 8; y++)
                {
                    int _x = x + 1, _y = y + 1,
                        x_ = x - 1, y_ = y - 1;

                    if (y_ > -1)
                    {
                        core[x, y_].CoreLink[0] = core[x, y];
                        core[x, y].CoreLink[1] = core[x, y_];
                    }

                    if (x_ > -1)
                    {
                        core[x_, y].CoreLink[2] = core[x, y];
                        core[x, y].CoreLink[3] = core[x_, y];
                    }

                    if (y_ > -1 && x_ > -1)
                    {
                        core[x_, y_].CoreLink[4] = core[x, y];
                        core[x, y].CoreLink[5] = core[x_, y_];
                    }

                    if (_x < 8 && y_ > -1)
                    {
                        core[_x, y_].CoreLink[6] = core[x, y];
                        core[x, y].CoreLink[7] = core[_x, y_];
                    }

                    if (_y < 8 && x_ > -1)
                    {
                        core[x_, _y].CoreLink[7] = core[x, y];
                        core[x, y].CoreLink[6] = core[x_, _y];
                    }
                }
            }
        }

        void FindQueen(Core c)
        {
            Core temp;

            for (int i = 0; i < 8; i++)
            {
                temp = c.CoreLink[i];

                while (temp != null)
                {
                    if (temp.IsQueen)
                    {
                        DisableQueen(temp);
                        break;
                    }

                    temp = temp.CoreLink[i];
                }
            }
        }

        void EnableQueen(Core c)
        {
            ++Count;
            c.IsQueen = true;
            c.Queen = ImageQueen;
        }

        void DisableQueen(Core c)
        {
            --Count;
            c.Queen = "";
            c.IsQueen = false;
        }

        private void Core_Click(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            var c = sender as Core;

            if (c.IsQueen)
            {
                DisableQueen(c);
            }
            else
            {
                FindQueen(c);
                EnableQueen(c);

                if (count == 8)
                    WinPuzzle?.Invoke();
            }
        }

        public event System.ComponentModel.PropertyChangedEventHandler PropertyChanged;

        void OnPropertyChanged([System.Runtime.CompilerServices.CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new System.ComponentModel.PropertyChangedEventArgs(propertyName));
        }
    }

    sealed class Core : System.Windows.Controls.Grid, System.ComponentModel.INotifyPropertyChanged
    {
        string queen = "";

        public byte X { get; set; }
        public byte Y { get; set; }
        public bool IsQueen { get; set; } = false;
        public Core[] CoreLink { get; private set; } = new Core[8];

        public string Queen
        {
            get { return queen; }
            set
            {
                queen = value;
                OnPropertyChanged();
            }
        }

        public event System.ComponentModel.PropertyChangedEventHandler PropertyChanged;

        void OnPropertyChanged([System.Runtime.CompilerServices.CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new System.ComponentModel.PropertyChangedEventArgs(propertyName));
        }
    }
}
