using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading;
using Unity.VisualScripting;
using UnityEngine;

public class Manager : MonoBehaviour
{
    // �������0~8������
    int GetRandomNumber()
    {
        return UnityEngine.Random.Range(0, 8);
    }

    public List<Cell> cellRow0;
    public List<Cell> cellRow1;
    public List<Cell> cellRow2;
    public List<Cell> cellRow3;
    public List<Cell> cellRow4;
    public List<Cell> cellRow5;
    public List<Cell> cellRow6;
    public List<Cell> cellRow7;

    public Knight knight;
    // ��ʼ������
    public List<List<Cell>> cells = new List<List<Cell>>(8);
    List<Tuple<int, int>> moveKnight = new List<Tuple<int, int>>();
    void Start()
    {
        cells.Add(cellRow0);
        cells.Add(cellRow1);
        cells.Add(cellRow2);
        cells.Add(cellRow3);
        cells.Add(cellRow4);
        cells.Add(cellRow5);
        cells.Add(cellRow6);
        cells.Add(cellRow7);

        int first_x = GetRandomNumber();
        int first_y = GetRandomNumber();
        knight.transform.position = cells[first_x][first_y].transform.position; // ��һ�ε�λ�� 
        knight.transform.position = cells[first_x][first_y].transform.position;

        // ���ú��������Ҫ�ߵ�λ��
        dfsSolution(first_x, first_y);
    }

    // ��̤���̲���
    void dfsSolution(int x, int y)
    {
        //����ƶ�����
        int[] dx = { -2, -1, 1, 2, 2, 1, -1, -2 };
        int[] dy = { 1, 2, 2, 1, -1, -2, -2, -1 };
        bool[,] isVisited = new bool[8, 8]; // ��¼�Ƿ������
        List<Tuple<int, int>> nowRoad = new(); // Ԫ���¼·��
        bool isGotAnswer = false; // �Ƿ�õ���
        void dfs(int x, int y)
        {
            if (isGotAnswer)
            {
                return;
            }
            if (nowRoad.Count == 64)
            {
                moveKnight = new(nowRoad);
                isGotAnswer = true;
                return;
            }
            List<Tuple<int, int>> nextMoves = new();

            for (int i = 0; i < 8; i++)
            {
                int x1 = x + dx[i];
                int y1 = y + dy[i];
                if (x1 >= 0 && x1 < 8 && y1 >= 0 && y1 < 8 && !isVisited[x1, y1])
                {
                    nextMoves.Add(Tuple.Create(x1, y1));
                }
            }

            int CountNextMoves(int x, int y)
            {
                int count = 0;
                for (int i = 0; i < 8; i++)
                {
                    int x1 = x + dx[i];
                    int y1 = y + dy[i];
                    if (x1 >= 0 && x1 < 8 && y1 >= 0 && y1 < 8 && !isVisited[x1, y1])
                    {
                        count++;
                    }
                }
                return count;
            }

            // ����Warnsdorff�������� (��֦�Ż�)
            nextMoves.Sort((a, b) => CountNextMoves(a.Item1, a.Item2).CompareTo(CountNextMoves(b.Item1, b.Item2)));

            foreach (var move in nextMoves)
            {
                int x1 = move.Item1;
                int y1 = move.Item2;
                isVisited[x1, y1] = true;
                nowRoad.Add(Tuple.Create(x1, y1));
                dfs(x1, y1);
                nowRoad.RemoveAt(nowRoad.Count - 1);
                isVisited[x1, y1] = false;
            }
        }

        nowRoad.Add(new Tuple<int, int>(x, y));
        isVisited[x, y] = true;
        dfs(x, y);
    }

    int i = 0;
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) && i < moveKnight.Count)
        {
            knight.Move(cells[moveKnight[i].Item1][moveKnight[i].Item2].GetPosition());
            cells[moveKnight[i].Item1][moveKnight[i].Item2].SetStep(i + 1);
            print(cells[moveKnight[i].Item1][moveKnight[i].Item2]);
            ++i;
        }
    }
}
