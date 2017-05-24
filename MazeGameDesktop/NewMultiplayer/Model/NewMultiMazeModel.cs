using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MazeGameDesktop.NewMultiplayer.Model
{
    class NewMultiMazeModel : INewMultiModel
    {
        public int Rows { set; get; }
        public int Columns { set; get; }

        public event PropertyChangedEventHandler PropertyChanged;

        private Client client;

        public NewMultiMazeModel()
        {
            client = new Client();
            client.PropertyChanged += 
        }

        private void ClientUpdated(object sender, PropertyChangedEventArgs e)
        {
            PropertyChanged?.Invoke(this, e);
        }

        public void GenerateMaze(string name, int rows, int cols)
        {
            throw new NotImplementedException();
        }

        public void JoinGame(string name)
        {
            throw new NotImplementedException();
        }

        public void Stop()
        {
            throw new NotImplementedException();
        }

        public void UpdateDefaultValues()
        {
            throw new NotImplementedException();
        }
    }
}
