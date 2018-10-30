using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;
using System.Linq;

namespace day1
{
    public class Game1 : Game
    {
        GraphicsDeviceManager graphics;
        BasicEffect effect;
        ReadBlockDate blockdate = new ReadBlockDate();
        private Dictionary<Vector3, int> blockList = new Dictionary<Vector3, int>();
        private List<Vector2> ChankList = new List<Vector2>();

        readonly int ChunkSize = 17;//チャンクは１６だが点で表すので１７
        readonly int _h0, _h1, _h2, _h3, _h4, _h5, _h6, _h7;
        int h0, h1, h2, h3, h4, h5, h6, h7;

        float Px = 0, Py= 0, Pz = -10;

        private float angleX = 0f;
        private float angleY = 0f;

        VertexPositionColor[] vertices;// = new VertexPositionColor[75140];//18785];//７５１４０
        //List<int> indices = new List<int>();
        private Dictionary<Vector4, int> indices = new Dictionary<Vector4, int>();
        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            blockList = blockdate.GetBlockDate();
            ChankList = blockdate.GetChankDate();
            vertices = new VertexPositionColor[18785 * ChankList.Count];
            _h0 = 0;
            _h1 = 1;
            _h2 = ChunkSize;
            _h3 = _h2 + 1;

            _h4 = ChunkSize * ChunkSize;
            _h5 = _h4 + 1;
            _h6 = _h4 + ChunkSize;
            _h7 = _h6 + 1;
        }

        protected override void Initialize()
        {
            bool a = true;
            int i = 0;
            //一チャンク分の点
            for (int j = 0; j < ChankList.Count; j++)
            {
                for (int y = 0; y < 65; y++)
                {
                    for (int x = 0; x < ChunkSize; x++)
                    {
                        for (int z = 0; z < ChunkSize; z++)//17こめ
                        {
                            if (a) vertices[i] = new VertexPositionColor(new Vector3(x + ChankList[j].X, y, z + ChankList[j].Y), Color.Green);
                            if (!a) vertices[i] = new VertexPositionColor(new Vector3(x + ChankList[j].X, y, z + ChankList[j].Y), Color.Red);
                            i++;
                        }
                    }
                    a = !a;
                }
            }
            //foreach (var item in blockList)
            //{

            //    if (item.Value != 0) Hairetu2(i, item.Key.X , item.Key.Y, item.Key.Z);
            //    i++;
            //}
            for (int j = 0; j < ChankList.Count; j++)
            {
                for (int y = 0; y < 1/*64*/; y++)
                {
                    for (int x = 0; x < ChunkSize - 1; x++)
                    {
                        for (int z = 0; z < ChunkSize - 1; z++)
                        {
                            if (blockList[new Vector3(x + ChankList[j].X, y, z + ChankList[j].Y)] != 0)
                            {
                                int shifting =_h4 * y + ChunkSize * x + z + 18785 * j;
                                if (blockList.ContainsKey(new Vector3(x + ChankList[j].X + 1, y, z + ChankList[j].Y)) && blockList[new Vector3(x + 1 + ChankList[j].X, y, z + ChankList[j].Y)] != 0) { } else BlockBack(new Vector3(x + ChankList[j].X, y, z + ChankList[j].Y), shifting); 
                                if (blockList.ContainsKey(new Vector3(x + ChankList[j].X - 1, y, z + ChankList[j].Y)) && blockList[new Vector3(x - 1 + ChankList[j].X, y, z + ChankList[j].Y)] != 0) { } else BlockFront(new Vector3(x + ChankList[j].X, y, z + ChankList[j].Y), shifting);
                                if (blockList.ContainsKey(new Vector3(x + ChankList[j].X, y, z + ChankList[j].Y + 1)) && blockList[new Vector3(x + ChankList[j].X, y, z + 1 + ChankList[j].Y)] != 0) { } else BlockRight(new Vector3(x + ChankList[j].X, y, z + ChankList[j].Y), shifting); 
                                if (blockList.ContainsKey(new Vector3(x + ChankList[j].X, y, z + ChankList[j].Y - 1)) && blockList[new Vector3(x + ChankList[j].X, y, z - 1 + ChankList[j].Y)] != 0) { } else BlockLeft(new Vector3(x + ChankList[j].X, y, z + ChankList[j].Y), shifting); 
                                if (blockList.ContainsKey(new Vector3(x + ChankList[j].X, y + 1, z + ChankList[j].Y)) && blockList[new Vector3(x + ChankList[j].X, y + 1, z + ChankList[j].Y)] != 0) { } else BlockUP(new Vector3(x + ChankList[j].X, y, z + ChankList[j].Y), shifting);
                                if (blockList.ContainsKey(new Vector3(x + ChankList[j].X, y - 1, z + ChankList[j].Y)) && blockList[new Vector3(x + ChankList[j].X, y - 1, z + ChankList[j].Y)] != 0) { } else BlockDown(new Vector3(x + ChankList[j].X, y, z + ChankList[j].Y), shifting);
                            }
                        }
                    }
                }
            }
            ///理想は焦点が重なったブロックを０(参照)にすると描画されてた部分が消されて周りのブロックだけを描画するプログラム
            ///x,y,zをブロックデータに直接入れてブロックを変える
            ///その下でブロックが０だったときの処理と普通のブロックのときの処理を書く
            ///そこから新たに点を結ぶデータを作る

            //Hairetu2(i, 0,1,0);
            //Hairetu2(1, 8,1, 0);
            //Hairetu2(2, 15, 1, 0);
            //indices.ToArray();
            base.Initialize();
        }
        public void BlockFront(Vector3 a,int Shifting)
        {
            h0 = _h0 + Shifting;
            h1 = _h1 + Shifting;
            h4 = _h4 + Shifting;
            h5 = _h5 + Shifting;
            indices.Add(new Vector4(a,1), h0);
            indices.Add(new Vector4(a,2), h5);
            indices.Add(new Vector4(a,3), h1);
            indices.Add(new Vector4(a,4), h0);
            indices.Add(new Vector4(a,5), h4);
            indices.Add(new Vector4(a,6), h5);
        }
        public void BlockRight(Vector3 a, int Shifting)
        {
            h1 = _h1 + Shifting;
            h3 = _h3 + Shifting;
            h5 = _h5 + Shifting;
            h7 = _h7 + Shifting;
            indices.Add(new Vector4(a, 7), h1);
            indices.Add(new Vector4(a, 8), h7);
            indices.Add(new Vector4(a, 9), h3);
            indices.Add(new Vector4(a,10), h1);
            indices.Add(new Vector4(a,11), h5);
            indices.Add(new Vector4(a,12), h7);
        }
        public void BlockBack(Vector3 a, int Shifting)
        {
            h2 = _h2 + Shifting;
            h3 = _h3 + Shifting;
            h6 = _h6 + Shifting;
            h7 = _h7 + Shifting;
            indices.Add(new Vector4(a,13), h3);
            indices.Add(new Vector4(a,14), h6);
            indices.Add(new Vector4(a,15), h2);
            indices.Add(new Vector4(a,16), h3);
            indices.Add(new Vector4(a,17), h7);
            indices.Add(new Vector4(a,18), h6);
        }
        public void BlockLeft(Vector3 a, int Shifting)
        {
            h0 = _h0 + Shifting;
            h2 = _h2 + Shifting;
            h4 = _h4 + Shifting;
            h6 = _h6 + Shifting;
            indices.Add(new Vector4(a,19), h2);
            indices.Add(new Vector4(a,20), h4);
            indices.Add(new Vector4(a,21), h0);
            indices.Add(new Vector4(a,22), h2);
            indices.Add(new Vector4(a,23), h6);
            indices.Add(new Vector4(a,24), h4);
        }
        public void BlockUP(Vector3 a, int Shifting)
        {
            h4 = _h4 + Shifting;
            h5 = _h5 + Shifting;
            h6 = _h6 + Shifting;
            h7 = _h7 + Shifting;
            indices.Add(new Vector4(a,25), h4);
            indices.Add(new Vector4(a,26), h7);
            indices.Add(new Vector4(a,27), h5);
            indices.Add(new Vector4(a,28), h4);
            indices.Add(new Vector4(a,29), h6);
            indices.Add(new Vector4(a,30), h7);
        }
        public void BlockDown(Vector3 a, int Shifting)
        {
            h0 = _h0 + Shifting;
            h1 = _h1 + Shifting;
            h2 = _h2 + Shifting;
            h3 = _h3 + Shifting;
            indices.Add(new Vector4(a,31), h0);
            indices.Add(new Vector4(a,32), h1);
            indices.Add(new Vector4(a,33), h3);
            indices.Add(new Vector4(a,34), h0);
            indices.Add(new Vector4(a,35), h3);
            indices.Add(new Vector4(a,36), h2);
        }
        //public void BlockClearFront(int Shifting)
        //{
        //    h0 = _h0 + Shifting;
        //    h1 = _h1 + Shifting;
        //    h4 = _h4 + Shifting;
        //    h5 = _h5 + Shifting;
        //    indices.RemoveAt(h0);
        //    indices.RemoveAt(h5);
        //    indices.RemoveAt(h1);
        //    indices.RemoveAt(h0);
        //    indices.RemoveAt(h4);
        //    indices.RemoveAt(h5);
        //}
        //public void BlockClearRight(int Shifting)
        //{
        //    h1 = _h1 + Shifting;
        //    h3 = _h3 + Shifting;
        //    h5 = _h5 + Shifting;
        //    h7 = _h7 + Shifting;
        //    indices.RemoveAt(h1);
        //    indices.RemoveAt(h7);
        //    indices.RemoveAt(h3);
        //    indices.RemoveAt(h1);
        //    indices.RemoveAt(h5);
        //    indices.RemoveAt(h7);
        //}
        //public void BlockClearBack(int Shifting)
        //{
        //    h2 = _h2 + Shifting;
        //    h3 = _h3 + Shifting;
        //    h6 = _h6 + Shifting;
        //    h7 = _h7 + Shifting;
        //    indices.RemoveAt(h3);
        //    indices.RemoveAt(h6);
        //    indices.RemoveAt(h2);
        //    indices.RemoveAt(h3);
        //    indices.RemoveAt(h7);
        //    indices.RemoveAt(h6);
        //}
        //public void BlockClearLeft(int Shifting)
        //{
        //    h0 = _h0 + Shifting;
        //    h2 = _h2 + Shifting;
        //    h4 = _h4 + Shifting;
        //    h6 = _h6 + Shifting;
        //    indices.RemoveAt(h2);
        //    indices.RemoveAt(h4);
        //    indices.RemoveAt(h0);
        //    indices.RemoveAt(h2);
        //    indices.RemoveAt(h6);
        //    indices.RemoveAt(h4);
        //}
        //public void BlockClearUP(int Shifting)
        //{
        //    h4 = _h4 + Shifting;
        //    h5 = _h5 + Shifting;
        //    h6 = _h6 + Shifting;
        //    h7 = _h7 + Shifting;
        //    indices.RemoveAt(h4);
        //    indices.RemoveAt(h7);
        //    indices.RemoveAt(h5);
        //    indices.RemoveAt(h4);
        //    indices.RemoveAt(h6);
        //    indices.RemoveAt(h7);
        //}
        //public void BlockClearDown(int Shifting)
        //{
        //    h0 = _h0 + Shifting;
        //    h1 = _h1 + Shifting;
        //    h2 = _h2 + Shifting;
        //    h3 = _h3 + Shifting;
        //    indices.RemoveAt(h0);
        //    indices.RemoveAt(h1);
        //    indices.RemoveAt(h3);
        //    indices.RemoveAt(h0);
        //    indices.RemoveAt(h3);
        //    indices.RemoveAt(h2);
        //}

        //public void Hairetu2(int i, float x, float y, float z)
        //{
        //    i *= 36;
        //    int a = (int)((_h4 * y) + (ChunkSize * x) + z);
        //    h0 = _h0 + a;
        //    h1 = _h1 + a;
        //    h2 = _h2 + a;
        //    h3 = _h3 + a;
        //    h4 = _h4 + a;
        //    h5 = _h5 + a;
        //    h6 = _h6 + a;
        //    h7 = _h7 + a;
        //    ///これらの一片分を部品化して
        //    ///上下左右のブロックによって
        //    ///表示するしないを決める

        //    //0,0,0からｘ軸のほうを見た時に
        //    //正面
        //    indices[0 + i] = h0;
        //    indices[1 + i] = h5;
        //    indices[2 + i] = h1;
        //    indices[3 + i] = h0;
        //    indices[4 + i] = h4;
        //    indices[5 + i] = h5;
        //    //右の面
        //    indices[6 + i] = h1;
        //    indices[7 + i] = h7;
        //    indices[8 + i] = h3;
        //    indices[9 + i] = h1;
        //    indices[10 + i] = h5;
        //    indices[11 + i] = h7;
        //    //背面
        //    indices[12 + i] = h3;
        //    indices[13 + i] = h6;
        //    indices[14 + i] = h2;
        //    indices[15 + i] = h3;
        //    indices[16 + i] = h7;
        //    indices[17 + i] = h6;
        //    //左の面
        //    indices[18 + i] = h2;
        //    indices[19 + i] = h4;
        //    indices[20 + i] = h0;
        //    indices[21 + i] = h2;
        //    indices[22 + i] = h6;
        //    indices[23 + i] = h4;
        //    //上面
        //    indices[24 + i] = h4;
        //    indices[25 + i] = h7;
        //    indices[26 + i] = h5;
        //    indices[27 + i] = h4;
        //    indices[28 + i] = h6;
        //    indices[29 + i] = h7;
        //    //下面
        //    indices[30 + i] = h0;
        //    indices[31 + i] = h1;
        //    indices[32 + i] = h3;
        //    indices[33 + i] = h0;
        //    indices[34 + i] = h3;
        //    indices[35 + i] = h2;
        //}
        protected override void LoadContent()
        {
            effect = new BasicEffect(GraphicsDevice)
            {
                VertexColorEnabled = true,
                //TextureEnabled = true,
                View = Matrix.CreateLookAt(
                new Vector3(Px, Py, Pz),   //カメラの位置
                new Vector3(0, 0, 0),  //カメラの見る点
                new Vector3(0, 0, 0)    //カメラの上向きベクトル。(0, -1, 0)にすると画面が上下逆になる
                ),
                Projection = Matrix.CreatePerspectiveFieldOfView
                (
                MathHelper.ToRadians(45),   //視野の角度。
                GraphicsDevice.Viewport.AspectRatio,//画面のアスペクト比(=横/縦)
                1,      //カメラからこれより近い物体は画面に映らない
                1000     //カメラからこれより遠い物体は画面に映らない
                )
            };
        }

        protected override void UnloadContent()
        {
            effect.Dispose();
        }
        int kx=0, kz=0;

        bool a = true;
        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();
            KeyboardState keyboardState = Keyboard.GetState();
            if (keyboardState.IsKeyDown(Keys.A)) Px -= 0.1f;
            if (keyboardState.IsKeyDown(Keys.D)) Px += 0.1f;
            if (keyboardState.IsKeyDown(Keys.W)) Pz -= 0.1f;
            if (keyboardState.IsKeyDown(Keys.S)) Pz += 0.1f;
            if (keyboardState.IsKeyDown(Keys.LeftShift)) Py -= 0.5f;
            if (keyboardState.IsKeyDown(Keys.Space)) Py += 0.5f;




            if (keyboardState.IsKeyDown(Keys.Down))
            {
                if (1.4 > angleX) angleX += 0.1f;
            }
            if (keyboardState.IsKeyDown(Keys.Up))
            {
                if (-1.4 < angleX) angleX -= 0.1f;
            }
            if (keyboardState.IsKeyDown(Keys.Right))
            {
                angleY -= 0.1f;
            }
            if (keyboardState.IsKeyDown(Keys.Left))
            {
                angleY += 0.1f;
            }
            var rotX = Matrix.CreateRotationX(angleX);
            var rotY = Matrix.CreateRotationY(angleY);

            var rot = rotY * rotX;//行列

            //var backward = Vector3.Backward;//new Vector3(0,0,1)

            var result = Vector3.Transform(Vector3.Backward, rot);//プレイヤーからの座標だから
            Vector3 CameraPosition = new Vector3(Px, Py, Pz) + (result * 3);                     //プレイヤー＋リザルト*カメラとプレイヤーの距離


            effect.View = Matrix.CreateLookAt(
            new Vector3(Px, Py, Pz),   //カメラの位置
            CameraPosition,   //カメラの見る点
            new Vector3(0, 1, 0)    //カメラの上向きベクトル。(0, -1, 0)にすると画面が上下逆になる
            );
            ///まず1,1の座標にあるブロックを消して
            ///周りのブロックを表示する
            ///なのでまず消す
            ///ブロックデータを点データとあわせて使う
            ///
            ///理想は焦点が重なったブロックを０(参照)にすると描画されてた部分が消されて周りのブロックだけを描画するプログラム
            ///x,y,zをブロックデータに直接入れてブロックを変える
            ///その下でブロックが０だったときの処理と普通のブロックのときの処理を書く
            ///そこから新たに点を結ぶデータを作る

            if (keyboardState.IsKeyDown(Keys.I))
            {
                if (15 > kz) kz++;
            }

            if (keyboardState.IsKeyDown(Keys.J))
            {
                if (15 > kx) kx++;
            }

            if (keyboardState.IsKeyDown(Keys.K))
            {
                if (kz > 1) kz--;
            }

            if (keyboardState.IsKeyDown(Keys.L))
            {
                if (kx > 1) kx--;
            }
            if (keyboardState.IsKeyDown(Keys.Enter) && a)
            {
                a = false;
                ///長押しかつzを-にすると描画が反転する
                ///
                ///ディクショナリの並べ替え機能により下2ブロックの並びがおかしい(正確には反転する(１２３４５→５４３２１))
                blockList[new Vector3(kx, 0, kz)] = 0;
                for (int i = 0; i < 37; i++)
                {
                    if (indices.ContainsKey(new Vector4(kx, 0, kz, i)))
                    {
                        indices.Remove(new Vector4(kx, 0, kz, i));
                    }
                }
                if (blockList.ContainsKey(new Vector3(kx - 1, 0, kz)) && blockList[new Vector3(kx - 1, 0, kz)] != 0)
                {
                    //後ろ
                    h2 = _h2 + (ChunkSize * (kx - 1) + _h4 * 0 + kz);
                    h3 = _h3 + (ChunkSize * (kx - 1) + _h4 * 0 + kz);
                    h6 = _h6 + (ChunkSize * (kx - 1) + _h4 * 0 + kz);
                    h7 = _h7 + (ChunkSize * (kx - 1) + _h4 * 0 + kz);
                    if (indices.ContainsKey(new Vector4(kx - 1, 0, kz, 13))) indices[new Vector4(kx - 1, 0, kz, 13)] = h3; else indices.Add(new Vector4(kx - 1, 0, kz, 13), h3);
                    if (indices.ContainsKey(new Vector4(kx - 1, 0, kz, 14))) indices[new Vector4(kx - 1, 0, kz, 14)] = h2; else indices.Add(new Vector4(kx - 1, 0, kz, 14), h2);
                    if (indices.ContainsKey(new Vector4(kx - 1, 0, kz, 15))) indices[new Vector4(kx - 1, 0, kz, 15)] = h6; else indices.Add(new Vector4(kx - 1, 0, kz, 15), h6);
                    if (indices.ContainsKey(new Vector4(kx - 1, 0, kz, 16))) indices[new Vector4(kx - 1, 0, kz, 16)] = h3; else indices.Add(new Vector4(kx - 1, 0, kz, 16), h3);
                    if (indices.ContainsKey(new Vector4(kx - 1, 0, kz, 17))) indices[new Vector4(kx - 1, 0, kz, 17)] = h6; else indices.Add(new Vector4(kx - 1, 0, kz, 17), h6);
                    if (indices.ContainsKey(new Vector4(kx - 1, 0, kz, 18))) indices[new Vector4(kx - 1, 0, kz, 18)] = h7; else indices.Add(new Vector4(kx - 1, 0, kz, 18), h7);
                }
                if (blockList.ContainsKey(new Vector3(kx, 0, kz + 1)) && blockList[new Vector3(kx, 0, kz + 1)] != 0)
                {
                    //左
                    h0 = _h0 + (ChunkSize * kx + _h4 * 0 + kz + 1);
                    h2 = _h2 + (ChunkSize * kx + _h4 * 0 + kz + 1);
                    h4 = _h4 + (ChunkSize * kx + _h4 * 0 + kz + 1);
                    h6 = _h6 + (ChunkSize * kx + _h4 * 0 + kz + 1);
                    if (indices.ContainsKey(new Vector4(kx, 0, kz + 1, 19))) indices[new Vector4(kx, 0, kz + 1, 19)] = h2; else indices.Add(new Vector4(kx, 0, kz + 1, 19), h2);
                    if (indices.ContainsKey(new Vector4(kx, 0, kz + 1, 20))) indices[new Vector4(kx, 0, kz + 1, 20)] = h0; else indices.Add(new Vector4(kx, 0, kz + 1, 20), h0);
                    if (indices.ContainsKey(new Vector4(kx, 0, kz + 1, 21))) indices[new Vector4(kx, 0, kz + 1, 21)] = h4; else indices.Add(new Vector4(kx, 0, kz + 1, 21), h4);
                    if (indices.ContainsKey(new Vector4(kx, 0, kz + 1, 22))) indices[new Vector4(kx, 0, kz + 1, 22)] = h2; else indices.Add(new Vector4(kx, 0, kz + 1, 22), h2);
                    if (indices.ContainsKey(new Vector4(kx, 0, kz + 1, 23))) indices[new Vector4(kx, 0, kz + 1, 23)] = h4; else indices.Add(new Vector4(kx, 0, kz + 1, 23), h4);
                    if (indices.ContainsKey(new Vector4(kx, 0, kz + 1, 24))) indices[new Vector4(kx, 0, kz + 1, 24)] = h6; else indices.Add(new Vector4(kx, 0, kz + 1, 24), h6);
                }

                if (blockList.ContainsKey(new Vector3(kx + 1, 0, kz)) && blockList[new Vector3(kx + 1, 0, kz)] != 0)
                {
                    //前
                    int b = (ChunkSize * (kx + 1) + _h4 * 0 + kz);
                    h0 = _h0 + b;
                    h1 = _h1 + b;
                    h4 = _h4 + b;
                    h5 = _h5 + b;
                    if (indices.ContainsKey(new Vector4(kx + 1, 0, kz, 1))) indices[new Vector4(kx + 1, 0, kz, 1)] = h0; else indices.Add(new Vector4(kx + 1, 0, kz, 1), h0);
                    if (indices.ContainsKey(new Vector4(kx + 1, 0, kz, 2))) indices[new Vector4(kx + 1, 0, kz, 2)] = h1; else indices.Add(new Vector4(kx + 1, 0, kz, 2), h1);
                    if (indices.ContainsKey(new Vector4(kx + 1, 0, kz, 3))) indices[new Vector4(kx + 1, 0, kz, 3)] = h5; else indices.Add(new Vector4(kx + 1, 0, kz, 3), h5);
                    if (indices.ContainsKey(new Vector4(kx + 1, 0, kz, 4))) indices[new Vector4(kx + 1, 0, kz, 4)] = h0; else indices.Add(new Vector4(kx + 1, 0, kz, 4), h0);
                    if (indices.ContainsKey(new Vector4(kx + 1, 0, kz, 5))) indices[new Vector4(kx + 1, 0, kz, 5)] = h5; else indices.Add(new Vector4(kx + 1, 0, kz, 5), h5);
                    if (indices.ContainsKey(new Vector4(kx + 1, 0, kz, 6))) indices[new Vector4(kx + 1, 0, kz, 6)] = h4; else indices.Add(new Vector4(kx + 1, 0, kz, 6), h4);
                }
                //if (blockList.ContainsKey(new Vector3(kx, 0, kz - 1)) && blockList[new Vector3(kx, 0, kz - 1)] != 0)
                //{
                //    //右
                //    h1 = _h1 + (ChunkSize * kx + _h4 * 0 + kz - 1);
                //    h3 = _h3 + (ChunkSize * kx + _h4 * 0 + kz - 1);
                //    h5 = _h5 + (ChunkSize * kx + _h4 * 0 + kz - 1);
                //    h7 = _h7 + (ChunkSize * kx + _h4 * 0 + kz - 1);
                //    if (indices.ContainsKey(new Vector4(kx, 0, kz - 1, 12))) indices[new Vector4(kx, 0, kz - 1, 12)] = h7; else indices.Add(new Vector4(kx, 0, kz - 1, 12), h7);
                //    if (indices.ContainsKey(new Vector4(kx, 0, kz - 1, 11))) indices[new Vector4(kx, 0, kz - 1, 11)] = h5; else indices.Add(new Vector4(kx, 0, kz - 1, 11), h5);
                //    if (indices.ContainsKey(new Vector4(kx, 0, kz - 1, 10))) indices[new Vector4(kx, 0, kz - 1, 10)] = h1; else indices.Add(new Vector4(kx, 0, kz - 1, 10), h1);
                //    if (indices.ContainsKey(new Vector4(kx, 0, kz - 1,  9))) indices[new Vector4(kx, 0, kz - 1,  9)] = h3; else indices.Add(new Vector4(kx, 0, kz - 1,  9), h3);
                //    if (indices.ContainsKey(new Vector4(kx, 0, kz - 1,  8))) indices[new Vector4(kx, 0, kz - 1,  8)] = h7; else indices.Add(new Vector4(kx, 0, kz - 1,  8), h7);
                //    if (indices.ContainsKey(new Vector4(kx, 0, kz - 1,  7))) indices[new Vector4(kx, 0, kz - 1,  7)] = h1; else indices.Add(new Vector4(kx, 0, kz - 1,  7), h1);


                //}



                ////BlockRight(new Vector3(1,0,0), ChunkSize * 1 + _h4 * 0 + 0);
                ////////BlockLeft(new Vector3(1,0,2), ChunkSize * 1 + _h4 * 0 + 2);
                //////BlockBack(new Vector3(0,0,1), ChunkSize * 0 + _h4 * 0 + 1);
                ////BlockFront(new Vector3(2, 0, 1), ChunkSize * 2 + _h4 * 0 + 1);
                //まず隣にブロックがあるか **　あったら点を結ぶ　なかったらなにもしない
            }
                if (keyboardState.IsKeyUp(Keys.Enter))
                {
                    a = true;
                }
            
            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);
            foreach (var pass in effect.CurrentTechnique.Passes)
            {
                pass.Apply();
                GraphicsDevice.DrawUserIndexedPrimitives(
                    PrimitiveType.TriangleList,   //三角形のリスト(変えればわかる)
                    vertices,                      //頂点
                    0,                             //頂点のオフセット
                    vertices.Length,               //頂点リストの長さ
                    indices.Values.ToArray(),             //頂点をつなげたリスト
                    0,                             //頂点をつなげたリストのオフセット
                    indices.Values.ToArray().Length / 3   //三角の頂点は３つだから３でわると三角の数が出てくる
                    );
            }
            base.Draw(gameTime);
        }
    }
}
