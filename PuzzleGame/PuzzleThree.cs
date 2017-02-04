
namespace Puzzle
{
    enum Team { Left, Right }

    sealed class PuzzleThree : System.Windows.UIElement, IPuzzle
    {
        Location[] location;
        string leftToad, rightToad;
        sbyte step = 15;

        public event WinPuzzleDelageta WinPuzzle;

        public string LawGame { get; } = "Помінняти місцями фігуру, коричневих - наліво, сірих - направо. Фігури рухаються лише в перед - на наступну клітину або через одну клітку в залежності від наявності вільних.";

        public void Initialize(ref Location[] loca)
        {
            location = loca;

            leftToad = loca[0].Toad;
            rightToad = loca[6].Toad;

            loca[1].LeftLocation = loca[0];
            loca[2].LeftLocation = loca[1];
            loca[3].LeftLocation = loca[2];
            loca[4].LeftLocation = loca[3];
            loca[5].LeftLocation = loca[4];
            loca[6].LeftLocation = loca[5];

            loca[0].RightLocation = loca[1];
            loca[1].RightLocation = loca[2];
            loca[2].RightLocation = loca[3];
            loca[3].RightLocation = loca[4];
            loca[4].RightLocation = loca[5];
            loca[5].RightLocation = loca[6];

            foreach (var v in loca)
            {
                v.MouseUp += Location_Click;
            }
        }

        public void Reset()
        {
            step = 15;

            location[0].IsToad = true;
            location[0].Toad = leftToad;
            location[0].IsTeam = Team.Left;

            location[1].IsToad = true;
            location[1].Toad = leftToad;
            location[1].IsTeam = Team.Left;

            location[2].IsToad = true;
            location[2].Toad = leftToad;
            location[2].IsTeam = Team.Left;

            location[3].Toad = "";
            location[3].IsToad = false;

            location[4].IsToad = true;
            location[4].Toad = rightToad;
            location[4].IsTeam = Team.Right;

            location[5].IsToad = true;
            location[5].Toad = rightToad;
            location[5].IsTeam = Team.Right;

            location[6].IsToad = true;
            location[6].Toad = rightToad;
            location[6].IsTeam = Team.Right;
        }

        void Jump(Location l)
        {
            if (!l.IsToad) return;

            if (l.IsTeam == Team.Right)
            {
                if (l.LeftLocation == null)
                    return;

                if (!l.LeftLocation.IsToad)
                {
                    SetJump(l.LeftLocation, l);
                }
                else if (l.LeftLocation.IsTeam != l.IsTeam && l.LeftLocation.LeftLocation != null &&
                    !l.LeftLocation.LeftLocation.IsToad)
                {
                    SetJump(l.LeftLocation.LeftLocation, l);
                }
            }
            else
            {
                if (l.RightLocation == null)
                    return;

                if (!l.RightLocation.IsToad)
                {
                    SetJump(l.RightLocation, l);
                }
                else if (l.RightLocation.IsTeam != l.IsTeam && l.RightLocation.RightLocation != null &&
                    !l.RightLocation.RightLocation.IsToad)
                {
                    SetJump(l.RightLocation.RightLocation, l);
                }
            }
        }

        void SetJump(Location end, Location start)
        {
            --step;

            end.IsToad = true;
            end.Toad = start.Toad;
            end.IsTeam = start.IsTeam;

            start.Toad = "";
            start.IsToad = false;
        }

        void Location_Click(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            Jump(sender as Location);

            if (step == 0)
                WinPuzzle?.Invoke();
        }
    }

    sealed class Location : System.Windows.Controls.Grid, System.ComponentModel.INotifyPropertyChanged
    {
        string toad = "";

        public Team IsTeam { get; set; }
        public bool IsToad { get; set; }
        public Location LeftLocation { get; set; }
        public Location RightLocation { get; set; }

        public string Toad
        {
            get { return toad; }
            set
            {
                toad = value;
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
