using Puzzle;
using System.Windows;

namespace PuzzleGame
{
    public sealed partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            DataContext = this;

            StartGame();
        }

        void StartGame()
        {
            InitializePuzzleOne();
            law.Text = puzzleOne.LawGame;
            _puzzleOne.Visibility = Visibility.Visible;
            next.Click += oneNext_Click;
        }

        void InitializePuzzleOne()
        {
            int _is = 0, _in = 0;
            Number[] num = new Number[16];
            Selector[] sel = new Selector[5];
            var child = _puzzleOne.Children.GetEnumerator();

            while (child.MoveNext())
            {
                var numItem = child.Current as Number;

                if (numItem != null)
                {
                    num[_in++] = numItem;
                }
                else
                {
                    var selectItem = child.Current as Selector;

                    if (selectItem != null)
                        sel[_is++] = selectItem;
                }
            }

            puzzleOne.InitializeNumber(ref num);
            puzzleOne.InitializeSelector(ref sel);
        }

        void InitiaizePuzzeTwo()
        {
            Core[] c = new Core[64];
            var child = _puzzleTwo.Children.GetEnumerator();

            for (int i = 0; child.MoveNext();)
            {
                var item = child.Current as Core;

                if (item != null)
                    c[i++] = item;
            }

            puzzleTwo.Initialize(ref c);
        }

        void InitializePuzzleThree()
        {
            Location[] loc = new Location[7];
            var child = _puzzleThree.Children.GetEnumerator();

            for (int i = 0; child.MoveNext();)
            {
                var item = child.Current as Location;

                if (item != null)
                    loc[i++] = item;
            }

            puzzleThree.Initialize(ref loc);
        }

        private void puzzleOne_WinPuzzle()
        {
            win.Visibility = Visibility.Visible;
            next.Visibility = Visibility.Visible;
        }

        private void puzzleTwo_WinPuzzle()
        {
            win.Visibility = Visibility.Visible;
            //next.Visibility = Visibility.Visible;
        }

        private void puzzleThree_WinPuzzle()
        {
            win.Visibility = Visibility.Visible;
            next.Visibility = Visibility.Visible;
        }

        private void oneNext_Click(object sender, RoutedEventArgs e)
        {
            win.Visibility = Visibility.Hidden;
            next.Visibility = Visibility.Hidden;
            _puzzleOne.Visibility = Visibility.Collapsed;

            InitializePuzzleThree();
            law.Text = puzzleThree.LawGame;
            _puzzleThree.Visibility = Visibility.Visible;

            next.Click -= oneNext_Click;
            next.Click += threeNext_Click;
        }

        private void threeNext_Click(object sender, RoutedEventArgs e)
        {
            win.Visibility = Visibility.Hidden;
            next.Visibility = Visibility.Hidden;
            _puzzleThree.Visibility = Visibility.Collapsed;

            InitiaizePuzzeTwo();
            law.Text = puzzleTwo.LawGame;
            _puzzleTwo.Visibility = Visibility.Visible;

            next.Click -= threeNext_Click;
        }

        private void reset_Click(object sender, RoutedEventArgs e)
        {
            puzzleThree.Reset();
        }

        private void DragMove_Window(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            DragMove();
        }

        private void exit_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
