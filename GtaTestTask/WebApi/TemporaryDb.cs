using GtaTestTask.WebApi.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GtaTestTask.WebApi
{
    internal class TemporaryDb
    {
        public List<PlayerDbData> PlayerDbDatas = new List<PlayerDbData>();

        public TemporaryDb() 
        {
            PlayerDbDatas.Add(new PlayerDbData("player1", "password1", 500));
            PlayerDbDatas.Add(new PlayerDbData("player2", "password2", 500));
            PlayerDbDatas.Add(new PlayerDbData("player3", "password3", 500));
        }
    }
}
