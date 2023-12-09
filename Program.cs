using System;

namespace CShape实践__飞行棋
{
    class Program
    {
        public static void Main(string[] arge)
        {
            #region 控制台初始化_主逻辑
            int width = 46;
            int height = 30;
            ConsoleInit(width, height);
            #endregion

            #region 场景选择_主逻辑
            //枚举的调用
            E_SceneType sceneType = new E_SceneType();
            //记录游戏是否结束 的 开关
            bool isGameOver = false;

            while (!isGameOver)
            {
                switch (sceneType)
                {
                    case E_SceneType.Begin:
                        #region 1.开始界面
                        BeginScene(width, height, ref sceneType, ref isGameOver);
                        Console.Clear();
                        #endregion
                        break;

                    case E_SceneType.Game:
                        #region 2.游玩界面
                        //调用 游戏场景函数 打印不变元素
                        GameScene(width, height, ref sceneType, ref isGameOver);
                        Console.Clear();
                        #endregion
                        break;

                    case E_SceneType.End:
                        #region 3.结束界面
                        EndScene(width, height, ref sceneType, ref isGameOver);
                        Console.Clear();
                        #endregion
                        break;
                }
            }
            #endregion
        }
        #region 0.控制台初始化_函数
        static void ConsoleInit(int width, int height)
        {
            //隐藏光标
            Console.CursorVisible = false;
            //设置窗口大小
            Console.SetWindowSize(width, height);
            Console.SetBufferSize(width, height);
        }
        #endregion

        #region 1.开始场景_函数
        static void BeginScene(int width, int height, ref E_SceneType sceneType, ref bool isGameOver)
        {
            //记录 判断操作后选择的颜色
            bool isEnterGame = true;
            //结束 开始场景循环 的按钮
            bool isBeginOver = false;

            Console.SetCursorPosition(width / 2 - 4, height / 2 - 8);
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine("飞 行 棋");

            while (!isBeginOver)
            {
                Console.SetCursorPosition(width / 2 - 4, height / 2 - 5);
                Console.ForegroundColor = isEnterGame ? ConsoleColor.Red : ConsoleColor.White;
                Console.WriteLine("开始游戏");


                Console.SetCursorPosition(width / 2 - 4, height / 2 - 2);
                Console.ForegroundColor = !isEnterGame ? ConsoleColor.Red : ConsoleColor.White;
                Console.WriteLine("退出游戏");

                switch (Console.ReadKey(true).Key)
                {
                    case ConsoleKey.W:
                        isEnterGame = true;
                        break;
                    case ConsoleKey.S:
                        isEnterGame = false;
                        break;
                    case ConsoleKey.E:
                        if (isEnterGame)
                        {
                            sceneType = E_SceneType.Game;
                            //结束当前场景循环 进入主体循环
                            isBeginOver = true;
                        }
                        else
                        {
                            //结束当前场景循环 进入主体循环
                            isBeginOver = true;
                            //结束 主体循环
                            isGameOver = true;
                        }
                        break;
                }
            }
        }
        #endregion

        #region 2.游玩场景_函数
        static void GameScene(int width, int height, ref E_SceneType sceneType, ref bool isGameOver)
        {
            PintWallAndInfor(width, height);
            //打印地图
            Map map = new Map(12, 3, 88);
            map.Draw();
            //打印玩家和电脑
            Character player = new Character(E_Character_Type.Player);
            Character computer = new Character(E_Character_Type.Computer);
            PrintCharacter(player, computer, map);

            //游戏场景循环
            while (true)
            {
                //检测输入,按e键继续
                Console.ReadKey(true);
                //处理骰子逻辑
                //是否到达终点
                if (PlayerMove(height, ref player, ref computer, map))
                {
                    //改变场景 值
                    sceneType = E_SceneType.End;
                    break;
                }
                else
                    isGameOver = false;
                //打印地图
                map.Draw();
                //打印玩家
                PrintCharacter(player, computer, map);

                //检测输入,按e键继续
                Console.ReadKey(true);
                //处理骰子逻辑
                if (PlayerMove(height, ref computer, ref player, map))
                {
                    //结束游戏循环
                    isGameOver = true;
                    sceneType = E_SceneType.End;
                    break;
                }
                else
                    isGameOver = false;
                //打印地图
                map.Draw();
                //打印玩家
                PrintCharacter(computer, player, map);
            }
        }
        #endregion

        #region 3.结束场景_函数
        static void EndScene(int width, int height, ref E_SceneType sceneType, ref bool isGameOver)
        {
            //记录 判断操作后选择的颜色
            bool isEnterGame = true;
            //结束 开始场景循环 的按钮
            bool isEndOver = false;

            Console.SetCursorPosition(width / 2 - 4, height / 2 - 8);
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine("游戏 结束");

            while (!isEndOver)
            {
                Console.SetCursorPosition(width / 2 - 4, height / 2 - 5);
                Console.ForegroundColor = isEnterGame ? ConsoleColor.Red : ConsoleColor.White;
                Console.WriteLine("重新开始");

                Console.SetCursorPosition(width / 2 - 4, height / 2 - 2);
                Console.ForegroundColor = !isEnterGame ? ConsoleColor.Red : ConsoleColor.White;
                Console.WriteLine("退出游戏");

                switch (Console.ReadKey(true).Key)
                {
                    case ConsoleKey.W:
                        isEnterGame = true;
                        break;
                    case ConsoleKey.S:
                        isEnterGame = false;
                        break;
                    case ConsoleKey.E:
                        if (isEnterGame)
                        {
                            sceneType = E_SceneType.Game;
                            //结束当前场景循环 进入主体循环
                            isEndOver = true;
                        }
                        else
                        {
                            //结束当前场景循环 进入主体循环
                            isEndOver = true;
                            //结束 主体循环
                            isGameOver = true;
                        }
                        break;
                }
            }
        }
        #endregion

        #region 2.5打印不变信息_函数
        static void PintWallAndInfor(int width, int height)
        {
            //不变的红墙 打印
            Console.ForegroundColor = ConsoleColor.Red;
            //横向的墙
            for (int i = 0; i < width; i += 2)
            {
                //上 排的墙
                Console.SetCursorPosition(i, 0);
                Console.Write("■");
                //中1 排的墙
                Console.SetCursorPosition(i, height - 11);
                Console.Write("■");
                //中2 排的墙
                Console.SetCursorPosition(i, height - 6);
                Console.Write("■");
                //下 排的墙
                Console.SetCursorPosition(i, height - 1);
                Console.Write("■");
            }
            //竖向的墙
            for (int i = 0; i < height; i++)
            {
                //左 排的墙
                Console.SetCursorPosition(0, i);
                Console.Write("■");
                //右 排的墙
                Console.SetCursorPosition(width - 2, i);
                Console.Write("■");
            }

            //不变的信息 打印
            Console.ForegroundColor = ConsoleColor.White;
            Console.SetCursorPosition(2, height - 10);
            Console.Write("□:普通格子");

            Console.ForegroundColor = ConsoleColor.Blue;
            Console.SetCursorPosition(2, height - 9);
            Console.Write("||:暂停，一回合不动    ");
            Console.ForegroundColor = ConsoleColor.Red;
            Console.Write("〇：炸弹，倒退5格");

            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.SetCursorPosition(2, height - 8);
            Console.Write("{}：时空隧道，随机倒退，暂停，换位置");

            Console.SetCursorPosition(2, height - 7);
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.Write("★：玩家  ");
            Console.ForegroundColor = ConsoleColor.Magenta;
            Console.Write("▲：电脑  ");
            Console.ForegroundColor = ConsoleColor.Green;
            Console.Write("@：玩家电脑重合  ");
        }
        #endregion

        #region 7. 角色打印_函数
        /// <summary>
        /// 为了实现玩家和电脑重合，打印特殊图形，而建立的一个方法
        /// </summary>
        /// <param name="player">玩家的形象</param>
        /// <param name="computer">电脑的形象</param>
        /// <param name="map">地图的形象</param>
        static void PrintCharacter(Character player, Character computer, Map map)
        {
            //位置 重合时
            if (player.posIndex == computer.posIndex)
            {
                //打印 "@ "
                //先有获取了地图，才能获取玩家打印的位置
                //获得地图中单元格的位置 存入grid变量中
                Grid grid = map.grids[player.posIndex];
                Console.SetCursorPosition(grid.pos.x, grid.pos.y);
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.Write("@ ");
            }
            else//不重合时
            {
                player.Draw(map);
                computer.Draw(map);
            }
        }
        #endregion

        #region  8. 骰子逻辑_的 方法
        //位置参数 是 更新！注意，当前处理逻辑，角色没有打印，故仍可改变变量
        //因为需要判断是否到达终点位置，故为bool类型
        //传入地图形象 获取地图位置 用于判断是否到达终点，和获取对应格子形象
        //传入player/otherPlayer，可以交换位置
        /// <summary>
        /// 玩家投骰子的行为
        /// </summary>
        /// <param name="player">玩家形象</param>
        /// <param name="computer">电脑形象</param>
        /// <param name="map">地图形象</param>
        /// <returns>到达终结:true，未到终点:false</returns>
        static bool PlayerMove(int h, ref Character player, ref Character otherPlayer, Map map)
        {
            //获得骰子 、随机倒退 、时空隧道 的 三个随机情况
            Random r = new Random();
            //记录每回合走的步数
            int stepNum = 0;

            //没有到达终点
            //判断是否 打开了 暂停下回合的开关
            if (player.isPlayerPuase)
            {
                player.isPlayerPuase = false;
                ClearInfor(h);
                Console.SetCursorPosition(2, h - 5);
                Console.Write("{0}的暂停本回合", (player.type == E_Character_Type.Player ? "玩家" : "电脑"));
            }
            else
            {
                //投骰子后，player位置移动
                stepNum = r.Next(1, 7);
                player.posIndex += stepNum;
                //是否 到达终点
                if (player.posIndex >= map.grids.Length - 1)
                {
                    Console.ReadKey(true);
                    Console.SetCursorPosition(2, h - 4);
                    Console.Write("{0}到达了终点,取得了胜利", (player.type == E_Character_Type.Player ? "玩家" : "电脑"));
                    player.posIndex = map.grids.Length - 1;
                    return true;
                }
                //调用清除文字的方法
                ClearInfor(h);
                Console.SetCursorPosition(2, h - 5);
                Console.Write("{0}的回合，移动{1}格", (player.type == E_Character_Type.Player ? "玩家" : "电脑"), stepNum);
                //到达不同方格后处理的对应逻辑
                //获得当前格子的类型
                switch (map.grids[player.posIndex].type)
                {
                    case E_Grid_Type.Normal:
                        break;
                    case E_Grid_Type.Boom:
                        player.posIndex -= 5;
                        //不能小于第一个方格
                        if (player.posIndex <= 0)
                            player.posIndex = 0;
                        Console.SetCursorPosition(2, h - 4);
                        Console.Write("{0}踩到了炸弹，后退了5格", (player.type == E_Character_Type.Player ? "玩家" : "电脑"));
                        break;
                    case E_Grid_Type.Puase:
                        player.isPlayerPuase = true;
                        Console.SetCursorPosition(2, h - 4);
                        Console.Write("{0}踩到了暂停，下回合将暂停", (player.type == E_Character_Type.Player ? "玩家" : "电脑"));
                        break;
                    case E_Grid_Type.TimeTravel:
                        //时空隧道 的 三个随机情况
                        switch (r.Next(1, 4))
                        {
                            case 1:
                                //随机倒退
                                stepNum = r.Next(1, 7);
                                player.posIndex -= stepNum;
                                //不能小于第一个方格
                                if (player.posIndex <= 0)
                                    player.posIndex = 0;
                                Console.SetCursorPosition(2, h - 4);
                                Console.Write("{0}踩到了时空隧道，随机后退{1}格", (player.type == E_Character_Type.Player ? "玩家" : "电脑"), stepNum);
                                break;
                            case 2:
                                player.isPlayerPuase = true;
                                Console.SetCursorPosition(2, h - 4);
                                Console.Write("{0}踩到了时空隧道，下回合将暂停", (player.type == E_Character_Type.Player ? "玩家" : "电脑"));
                                break;
                            case 3:
                                //换位置
                                int temp = player.posIndex;
                                player.posIndex = otherPlayer.posIndex;
                                otherPlayer.posIndex = temp;
                                Console.SetCursorPosition(2, h - 4);
                                Console.Write("{0}踩到了时空隧道，与{1}交换位置", (player.type == E_Character_Type.Player ? "玩家" : "电脑"), (otherPlayer.type == E_Character_Type.Player ? "玩家" : "电脑"));
                                break;
                        }
                        break;
                }
            }
            return false;
        }

        #endregion

        #region 8.5 清除信息的 方法
        static void ClearInfor(int h)
        {
            Console.SetCursorPosition(2, h - 5);
            Console.Write("                                    ");
            Console.SetCursorPosition(2, h - 4);
            Console.Write("                                    ");
            Console.SetCursorPosition(2, h - 3);
            Console.Write("                                    ");
            Console.SetCursorPosition(2, h - 2);
            Console.Write("                                    ");
        }
        #endregion
    }
    #region 4.场景选择相关_枚举
    /// <summary>
    /// 场景切换 的枚举
    /// </summary>
    enum E_SceneType
    {
        /// <summary>
        /// 开始游戏
        /// </summary>
        Begin,
        /// <summary>
        /// 游戏场景
        /// </summary>
        Game,
        /// <summary>
        /// 结束场景
        /// </summary>
        End
    }
    #endregion


    #region 5.格子_枚举_结构体
    /// <summary>
    /// 格子_枚举
    /// </summary>
    enum E_Grid_Type
    {
        /// <summary>
        /// 普通格子
        /// </summary>
        Normal,
        /// <summary>
        /// 炸弹
        /// </summary>
        Boom,
        /// <summary>
        /// 暂停
        /// </summary>
        Puase,
        /// <summary>
        /// 时刻隧道，随机倒退、暂停、交换位置
        /// </summary>
        TimeTravel,
    }

    /// <summary>
    /// 2维向量坐标
    /// </summary>
    struct Vector2
    {
        public int x;
        public int y;

        public Vector2(int x, int y)
        {
            this.x = x;
            this.y = y;
        }
    }

    /// <summary>
    /// 格子_结构体
    /// </summary>
    struct Grid
    {
        //格子类型
        public E_Grid_Type type;
        //格子位置
        public Vector2 pos;

        /// <summary>
        /// 初始化构造函数
        /// </summary>
        /// <param name="gridType">格子类型</param>
        /// <param name="x">格子x轴</param>
        /// <param name="y">格子y轴</param>
        public Grid(E_Grid_Type type, int x, int y)
        {
            this.type = type;
            pos.x = x;
            pos.y = y;
        }

        public void Draw()
        {
            Console.SetCursorPosition(pos.x, pos.y);
            switch (type)
            {
                case E_Grid_Type.Normal:
                    Console.ForegroundColor = ConsoleColor.White;
                    Console.Write("□");
                    break;
                case E_Grid_Type.Boom:
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.Write("〇");
                    break;
                case E_Grid_Type.Puase:
                    Console.ForegroundColor = ConsoleColor.Blue;
                    Console.Write("||");
                    break;
                case E_Grid_Type.TimeTravel:
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    Console.Write("{}");
                    break;
            }
        }
    }
    #endregion


    #region 6.地图_结构体
    /// <summary>
    /// 地图结构体
    /// </summary>
    struct Map
    {
        //数组存储 所有的格子的位置
        public Grid[] grids;

        //地图初始化构造函数
        public Map(int x, int y, int num)
        {
            grids = new Grid[num];

            //地图内包含 格子的类型 随机个
            Random r = new Random();
            //抽象概率
            int numRandom;

            //绘制地图的逻辑
            //记录x方向的方格
            sbyte indexX = 0;
            //记录y方向的方格
            sbyte indexY = 0;
            //实现“反向 步长”
            int stepNum = 2;

            //遍历 分配地图的格子
            for (int i = 0; i < num; i++)
            {
                //每次遍历 都是一个新的numRandom
                numRandom = r.Next(0, 101);
                //地图的85%的概率为 普通方格（开头、结尾必是普通方格）  //num-1是最大下标
                if (numRandom <= 75 || i == 0 || i == num - 1)
                {
                    grids[i].type = E_Grid_Type.Normal;
                }
                //地图的5%的概率为 炸弹方格
                else if (numRandom > 75 && numRandom <= 82)
                {
                    grids[i].type = E_Grid_Type.Boom;
                }
                //地图的5%的概率为 暂停方格
                else if (numRandom > 82 && numRandom <= 90)
                {
                    grids[i].type = E_Grid_Type.Puase;
                }
                //地图的5%的概率为 时刻隧道方格 随机倒退，暂停，换位置
                else
                {
                    grids[i].type = E_Grid_Type.TimeTravel;
                }

                //记录 每个格子的位置
                grids[i].pos = new Vector2(x, y);
                //横向走到第10个 停下
                if (indexX == 10)
                {
                    //再向竖向走
                    ++y;
                    ++indexY;
                    //竖向走到第2个停下
                    if (indexY == 2)
                    {
                        //归0 重数
                        indexX = 0;
                        indexY = 0;
                        //反向 步长
                        stepNum = -stepNum;
                    }
                }
                else
                {
                    //先向横向走
                    x += stepNum;
                    ++indexX;
                }
            }
        }

        // 画地图的方法
        public void Draw()
        {
            for (int i = 0; i < grids.Length; i++)
            {
                grids[i].Draw();
            }
        }
    }
    #endregion


    #region 7.角色_枚举_结构体
    enum E_Character_Type
    {
        /// <summary>
        /// 玩家
        /// </summary>
        Player,
        /// <summary>
        /// 电脑
        /// </summary>
        Computer
    }

    struct Character
    {
        //用于检测是否 暂停一回合 的开关
        public bool isPlayerPuase;

        public E_Character_Type type;
        public int posIndex;

        public Character(E_Character_Type type, int index = 0, bool isPuase = false)
        {
            isPlayerPuase = isPuase;
            //记录角色的位置(用于当作 grids[]数组的下标)
            posIndex = index;
            //初始化角色类型
            this.type = type;
        }

        public void Draw(Map mapInfor)
        {
            //必须先得到地图，才能够 得到在地图上的哪一个格子
            //从传入的地图中 得到格子信息
            Grid grid = mapInfor.grids[posIndex];
            //设置玩家的位置
            Console.SetCursorPosition(grid.pos.x, grid.pos.y);
            switch (type)
            {
                case E_Character_Type.Player:
                    Console.ForegroundColor = ConsoleColor.Cyan;
                    Console.Write("★");
                    break;
                case E_Character_Type.Computer:
                    Console.ForegroundColor = ConsoleColor.Magenta;
                    Console.Write("▲");
                    break;
            }
        }
    }
    #endregion
}