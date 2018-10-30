using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace day1
{
    class ReadBlockDate
    {
        private Dictionary<Vector3,int> blockList = new Dictionary<Vector3, int>();
        private List<Vector2> ChankList = new List<Vector2>();
        public ReadBlockDate()                  //バイナリファイル
        {                                       //ここをテキストファイルの絶対パスにしてね
            FileStream datafs = new FileStream(@"F:\ProjectITSUKI\3D\ITSUKI\day1\TextFile3.txt", FileMode.Open);
            StreamReader dataSr = new StreamReader(datafs);
            int X = 0, Z = 0;
            int x=0,y=0, z=0;
            while (!dataSr.EndOfStream)
            {
                ///一番最初の数値で座標を手に入れる
                ///後半の数値で1チャンク分のブロックデータを入れる
                ///それを何個も入れる
                string line = dataSr.ReadLine();
                string[] items = line.Split(new char[] { ',', ' ' }, StringSplitOptions.RemoveEmptyEntries);
                if (items.Length == 2)
                {
                    X = (int.Parse(items[0])) * 16;
                    Z = (int.Parse(items[1])) * 16;
                    ChankList.Add(new Vector2(X, Z));
                    x = 0;y = 0;z = 0;
                    continue;
                }
                if (items.Length != 16)  continue;   //16=文字の数 
                                                     
                z = 0;
                foreach (var item in items)
                {
                    blockList.Add(new Vector3(X + x, y, Z + z), int.Parse(item));
                    z++;
                }
                if (x < 15) x++; else { x = 0; y++; }//15 = 0から数えた16
            }
            dataSr.Close();
        }
        public Dictionary<Vector3, int> GetBlockDate()
        {
            return blockList;
        }
        public List<Vector2> GetChankDate()
        {
            return ChankList;
        }
    }
}
