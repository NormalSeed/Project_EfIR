using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Project_EfIR
{

    class Program
    {
        struct Position
        {
            public int x;
            public int y;
        }
        static void Main(string[] args)
        {
            bool gameOver = false;
            Position playerPos;
            char[,] map;
            int turnCount = 0;
            Start(out playerPos, out map);
            while (gameOver == false)
            {
                Render(playerPos, map);
                ConsoleKey key = Input();
                Update(key, ref playerPos, map, ref gameOver, ref turnCount);
            }
            End(turnCount);
        }

        static void Start(out Position playerPos, out char[,] map)
        {
            // 커서 안보이게
            Console.CursorVisible = false;
            // 플레이어 위치 초기설정
            playerPos.x = 8;
            playerPos.y = 10;

            //맵 작성
            map = new char[12, 11]
            {
            { '▦', '▦', '▦', '▦', '▦', '◎', '▦', '▦', '▦', '▦', '▦' },
            { '▦', '△', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', '▦' },
            { '▦', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', '▦' },
            { '▦', ' ', ' ', ' ', ' ', ' ', '△', ' ', ' ', ' ', '▦' },
            { '▦', '△', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', '▦' },
            { '▦', ' ', ' ', ' ', ' ', ' ', '△', ' ', '△', ' ', '▦' },
            { '▦', ' ', ' ', ' ', '△', ' ', ' ', '△', ' ', ' ', '▦' },
            { '▦', ' ', '△', ' ', ' ', ' ', ' ', ' ', ' ', ' ', '▦' },
            { '▦', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', '▦' },
            { '▦', '▦', '▦', '▦', ' ', ' ', ' ', ' ', ' ', ' ', '▦' },
            { ' ', ' ', ' ', '▦', ' ', ' ', ' ', ' ', ' ', ' ', '▦' },
            { ' ', ' ', ' ', '▦', '▦', '▦', '▦', '▦', '▦', '▦', '▦' }
            };
            ShowTitle();
        }
        static void ShowTitle()
        {
            // 타이틀 문구 출력
            Console.WriteLine("---------------\nEscape from Icy Road\n---------------\n미끄러운 빙판길을 탈출해 봅시다!\n기둥은 부딪히면 부서집니다\n\n아무키나 눌러서 시작하세요");
            // 아무 키나 입력 받아서 게임 시작
            Console.ReadKey(true);
            Console.Clear();
        }

        static ConsoleKey Input()
        {
            return Console.ReadKey(true).Key;
        }

        static void Render(Position playerPos, char[,] map)
        {
            // 덧그리기
            Console.SetCursorPosition(0, 0);
            // 맵 출력 함수
            PrintMap(map);
            // 플레이어 출력 함수
            PrintPlayer(playerPos);
        }
        // 맵 출력
        static void PrintMap(char[,] map)
        {
            Console.ForegroundColor = ConsoleColor.Blue;
            for (int y = 0; y < map.GetLength(0); y++) // 맵의 y 길이만큼 반복
            {
                for (int x = 0; x < map.GetLength(1); x++) // 맵의 x 길이만큼 반복
                {
                    Console.Write(map[y, x]); // 해당 위치 글자 출력
                }
                Console.WriteLine(); // 한줄 다음으로
            }
            Console.ResetColor();
        }
        // 플레이어 출력
        static void PrintPlayer(Position playerPos)
        {
            // 플레이어 위치 받아서
            Console.SetCursorPosition(playerPos.x, playerPos.y);
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.Write("▲");
            Console.ResetColor();

        }

        static void Update(ConsoleKey key, ref Position playerPos, char[,] map, ref bool gameOver, ref int turnCount)
        {
            Move(key, ref playerPos, map);
            Console.SetCursorPosition(12, 0);
            MoveCount(key, ref turnCount);
            Reset(key, ref playerPos, map, ref turnCount);
            if (IsClear(playerPos, map) == true)
            {
                gameOver = true;
            }
        }

        // 이동
        static void Move(ConsoleKey key, ref Position playerPos, char[,] map)
        {
            // 이동하면 이동방향으로 벽이나 기둥, 골에 부딪힐 때까지 이동
            Position hiddenPos;
            hiddenPos.x = 6;
            hiddenPos.y = 1;
            switch (key)
            {
                case ConsoleKey.A:
                case ConsoleKey.LeftArrow:
                    // 이동 방향에 빈칸이 있으면
                    if (map[playerPos.y, playerPos.x - 1] == ' ')
                    {
                        while (map[playerPos.y, playerPos.x - 1] == ' ')
                        {
                            // 이동방향 한칸 앞이 빈칸이면 계속 이동 벽, 골, 기둥이면 멈춤
                            playerPos.x--;
                            // 한칸씩 이동하는걸 보여주며 출력
                            Console.SetCursorPosition(0, 0);
                            PrintMap(map);
                            PrintPlayer(playerPos);
                            Thread.Sleep(50);
                        }
                        // 만약 이동 완료 후 이동방향 한칸 앞에 골이 있으면 한칸 더 이동하고 완료 표시로 바꾸기
                        if (map[playerPos.y, playerPos.x - 1] == '◎')
                        {
                            playerPos.x--;
                            map[playerPos.y, playerPos.x] = '★';
                        }
                    }
                    // 이동 방향에 골이 있으면
                    else if (map[playerPos.y, playerPos.x - 1] == '◎')
                    {
                        // 이동하고 완료 표시로 바꾸기
                        playerPos.x--;
                        map[playerPos.y, playerPos.x] = '★';
                    }
                    // 장애물(기둥)이면
                    else if (map[playerPos.y, playerPos.x - 1] == '△')
                    {
                        map[playerPos.y, playerPos.x - 1] = ' ';
                    }
                    // 벽이면
                    else
                    {

                    }
                    break;
                case ConsoleKey.D:
                case ConsoleKey.RightArrow:
                    if (map[playerPos.y, playerPos.x + 1] == ' ')
                    {
                        while (map[playerPos.y, playerPos.x + 1] == ' ')
                        {
                            playerPos.x++;
                            Console.SetCursorPosition(0, 0);
                            PrintMap(map);
                            PrintPlayer(playerPos);
                            Thread.Sleep(50);
                            // 오른쪽에서 부딪혔을 때만 생성되는 숨겨진 기둥
                            if (playerPos.x + 1 == hiddenPos.x && playerPos.y == hiddenPos.y)
                            {
                                map[hiddenPos.y, hiddenPos.x] = '▲';
                            }
                        }
                        if (map[playerPos.y, playerPos.x + 1] == '◎')
                        {
                            playerPos.x++;
                            map[playerPos.y, playerPos.x] = '★';
                        }
                    }
                    else if (map[playerPos.y, playerPos.x + 1] == '◎')
                    {
                        playerPos.x++;
                        map[playerPos.y, playerPos.x] = '★';
                    }
                    else if (map[playerPos.y, playerPos.x + 1] == '△')
                    {
                        map[playerPos.y, playerPos.x + 1] = ' ';
                    }
                    else
                    {

                    }
                    break;
                case ConsoleKey.W:
                case ConsoleKey.UpArrow:
                    if (map[playerPos.y - 1, playerPos.x] == ' ')
                    {
                        while (map[playerPos.y - 1, playerPos.x] == ' ')
                        {
                            playerPos.y--;
                            Console.SetCursorPosition(0, 0);
                            PrintMap(map);
                            PrintPlayer(playerPos);
                            Thread.Sleep(50);
                        }
                        if (map[playerPos.y - 1, playerPos.x] == '◎')
                        {
                            playerPos.y--;
                            map[playerPos.y, playerPos.x] = '★';
                        }
                    }
                    else if (map[playerPos.y - 1, playerPos.x] == '◎')
                    {
                        playerPos.y--;
                        map[playerPos.y, playerPos.x] = '★';
                    }
                    else if (map[playerPos.y - 1, playerPos.x] == '△')
                    {
                        map[playerPos.y - 1, playerPos.x] = ' ';
                    }
                    else
                    {

                    }

                    break;
                case ConsoleKey.S:
                case ConsoleKey.DownArrow:
                    if (map[playerPos.y + 1, playerPos.x] == ' ')
                    {
                        while (map[playerPos.y + 1, playerPos.x] == ' ')
                        {
                            playerPos.y++;
                            Console.SetCursorPosition(0, 0);
                            PrintMap(map);
                            PrintPlayer(playerPos);
                            Thread.Sleep(50);
                        }
                    }
                    else if (map[playerPos.y + 1, playerPos.x] == '◎')
                    {
                        playerPos.y++;
                        map[playerPos.y, playerPos.x] = '★';
                    }
                    else if (map[playerPos.y + 1, playerPos.x] == '△')
                    {
                        map[playerPos.y + 1, playerPos.x] = ' ';
                    }
                    else
                    {

                    }

                    break;

            }
        }
        // 리셋
        static void Reset(ConsoleKey key, ref Position playerPos, char[,] map, ref int turnCount)
        {
            char[,] mapOrigin = new char[12, 11]
            {
            { '▦', '▦', '▦', '▦', '▦', '◎', '▦', '▦', '▦', '▦', '▦' },
            { '▦', '△', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', '▦' },
            { '▦', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', '▦' },
            { '▦', ' ', ' ', ' ', ' ', ' ', '△', ' ', ' ', ' ', '▦' },
            { '▦', '△', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', '▦' },
            { '▦', ' ', ' ', ' ', ' ', ' ', '△', ' ', '△', ' ', '▦' },
            { '▦', ' ', ' ', ' ', '△', ' ', ' ', '△', ' ', ' ', '▦' },
            { '▦', ' ', '△', ' ', ' ', ' ', ' ', ' ', ' ', ' ', '▦' },
            { '▦', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', '▦' },
            { '▦', '▦', '▦', '▦', ' ', ' ', ' ', ' ', ' ', ' ', '▦' },
            { ' ', ' ', ' ', '▦', ' ', ' ', ' ', ' ', ' ', ' ', '▦' },
            { ' ', ' ', ' ', '▦', '▦', '▦', '▦', '▦', '▦', '▦', '▦' }
            };
            switch (key)
            {
                case ConsoleKey.R:
                    playerPos.x = 8;
                    playerPos.y = 10;
                    turnCount = 0;
                    Console.SetCursorPosition(0, 0);
                    MapToOrigin(ref map, mapOrigin);
                    PrintPlayer(playerPos);
                    MoveCount(key, ref turnCount);

                    break;
            }
        }
        static void MapToOrigin(ref char[,] map, char[,] mapOrigin)
        {
            for (int y = 0; y < map.GetLength(0); y++) // 맵의 y 길이만큼 반복
            {
                for (int x = 0; x < map.GetLength(1); x++) // 맵의 x 길이만큼 반복
                {
                    map[y, x] = mapOrigin[y, x]; // 해당 위치 글자 원본 맵으로 변경
                }
                Console.WriteLine(); // 한줄 다음으로
            }
            Console.ResetColor();
        }
        //이동 횟수
        static void MoveCount(ConsoleKey key, ref int turnCount)
        {
            switch (key)
            {
                case ConsoleKey.A:
                case ConsoleKey.RightArrow:
                case ConsoleKey.D:
                case ConsoleKey.LeftArrow:
                case ConsoleKey.W:
                case ConsoleKey.UpArrow:
                case ConsoleKey.S:
                case ConsoleKey.DownArrow:
                    turnCount++;
                    break;
            }
            Console.SetCursorPosition(12, 0);
            Console.WriteLine("                           ");// 턴 수가 10회가 넘으면 뒤쪽에 턴이 한번 더 쓰여서 입력줄 초기화
            Console.SetCursorPosition(12, 0);
            Console.WriteLine($"턴 수 : {turnCount} 턴");
        }

        // 클리어 조건
        static bool IsClear(Position playerPos, char[,] map)
        {
            // 플레이어 위치에 완료 표시(★)가 있으면 클리어
            if (map[playerPos.y, playerPos.x] == '★')
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        static void End(int turnCount)
        {
            Console.Clear();
            Console.WriteLine("빙판길 탈출 성공!");
            Console.WriteLine($"이동 횟수는 {turnCount}회 입니다");
        }
    }
}
