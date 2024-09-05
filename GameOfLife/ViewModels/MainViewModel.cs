using GameOfLife.Common;
using GameOfLife.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using System.Windows;
using System.Windows.Input;

namespace GameOfLife.ViewModels
{
    public class MainViewModel:DependencyObject
    {


        /// <summary>
        /// 刷新间隔
        /// </summary>
        public int Interval
        {
            get { return (int)GetValue(IntervalProperty); }
            set { SetValue(IntervalProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Interval.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty IntervalProperty =
            DependencyProperty.Register("Interval", typeof(int), typeof(MainViewModel), new PropertyMetadata(100));


        /// <summary>
        /// 细胞集合
        /// </summary>
        public ObservableCollection<CellModel> Cells
        {
            get { return (ObservableCollection<CellModel>)GetValue(CellsProperty); }
            set { SetValue(CellsProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Cells.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty CellsProperty =
            DependencyProperty.Register("Cells", typeof(ObservableCollection<CellModel>), typeof(MainViewModel), new PropertyMetadata(new ObservableCollection<CellModel>()));



        /// <summary>
        /// 地图宽度
        /// </summary>
        public int MapWidth
        {
            get { return (int)GetValue(MapWidthProperty); }
            set { SetValue(MapWidthProperty, value); }
        }

        // Using a DependencyProperty as the backing store for MapWidth.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty MapWidthProperty =
            DependencyProperty.Register("MapWidth", typeof(int), typeof(MainViewModel), new PropertyMetadata(10));



        /// <summary>
        /// 地图高度
        /// </summary>
        public int MapHeight
        {
            get { return (int)GetValue(MapHeightProperty); }
            set { SetValue(MapHeightProperty, value); }
        }

        // Using a DependencyProperty as the backing store for MapHeight.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty MapHeightProperty =
            DependencyProperty.Register("MapHeight", typeof(int), typeof(MainViewModel), new PropertyMetadata(10));



        /// <summary>
        /// 画布宽度
        /// </summary>
        public double CanvasWidth
        {
            get { return (double)GetValue(CanvasWidthProperty); }
            set { SetValue(CanvasWidthProperty, value); }
        }

        // Using a DependencyProperty as the backing store for CanvasWidth.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty CanvasWidthProperty =
            DependencyProperty.Register("CanvasWidth", typeof(double), typeof(MainViewModel), new PropertyMetadata(1002d));



        /// <summary>
        /// 画布高度
        /// </summary>
        public double CanvasHeight
        {
            get { return (double)GetValue(CanvasHeightProperty); }
            set { SetValue(CanvasHeightProperty, value); }
        }

        // Using a DependencyProperty as the backing store for CanvasHeight.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty CanvasHeightProperty =
            DependencyProperty.Register("CanvasHeight", typeof(double), typeof(MainViewModel), new PropertyMetadata(1002d));


        /// <summary>
        /// 最大播种数
        /// </summary>
        public int MaxSeedingCount
        {
            get { return (int)GetValue(MaxSeedingCountProperty); }
            set { SetValue(MaxSeedingCountProperty, value); }
        }

        // Using a DependencyProperty as the backing store for MaxSeedingCount.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty MaxSeedingCountProperty =
            DependencyProperty.Register("MaxSeedingCount", typeof(int), typeof(MainViewModel), new PropertyMetadata(100));



        /// <summary>
        /// 播种数量
        /// </summary>
        public int SeedingCount
        {
            get { return (int)GetValue(SeedingCountProperty); }
            set { SetValue(SeedingCountProperty, value); }
        }

        // Using a DependencyProperty as the backing store for SeedingCount.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty SeedingCountProperty =
            DependencyProperty.Register("SeedingCount", typeof(int), typeof(MainViewModel), new PropertyMetadata(10));


        /// <summary>
        /// 运行状态，用于设置UI
        /// </summary>
        public bool IsRunning
        {
            get { return (bool)GetValue(IsRunningProperty); }
            set { SetValue(IsRunningProperty, value); }
        }

        // Using a DependencyProperty as the backing store for IsRunning.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty IsRunningProperty =
            DependencyProperty.Register("IsRunning", typeof(bool), typeof(MainViewModel), new PropertyMetadata(false));



        public MainViewModel()
        {
            _simulatorTicker.Elapsed += SimulatorTick;
            SetMapCommand.Execute(null);
        }

        private readonly Timer _simulatorTicker = new Timer();

        /// <summary>
        /// 模拟刻执行细胞状态更新
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// <exception cref="NotImplementedException"></exception>
        private void SimulatorTick(object sender, ElapsedEventArgs e)
        {
            App.Current.Dispatcher.Invoke(() =>
            {
                foreach (var cell in Cells)
                {
                    cell.Next();
                }
                foreach (var cell in Cells)
                {
                    cell.IsAlive = cell.NextAlive;
                }
            });
        }

        /// <summary>
        /// 设置地图
        /// </summary>
        public ICommand SetMapCommand => new RelayCommand(() =>
        {
            MaxSeedingCount = MapWidth * MapHeight;
            CanvasWidth = MapWidth * 100 + 2;
            CanvasHeight = MapHeight * 100 + 2;
            Cells.Clear();
            // 生成地图
            for (int i = 0; i < MaxSeedingCount; i++)
            {
                Cells.Add(new CellModel());
            }
            // 建立邻居关系
            for (int currentRow = 0; currentRow < MapHeight; currentRow++)
            {
                for (int currentColumn = 0; currentColumn < MapWidth; currentColumn++)
                {
                    int currentIndex = currentRow * MapWidth + currentColumn;
                    // 读取周围3*3区域内的成员
                    for (int row = currentRow - 1; row <= currentRow + 1; row++)
                    {
                        for(int column = currentColumn - 1; column <= currentColumn + 1; column++)
                        {
                            int index = row * MapWidth + column;
                            try
                            {
                                if (!Cells[currentIndex].Equals(Cells[index]))
                                {
                                    Cells[currentIndex].Neighbors.Add(Cells[index]);
                                }
                            }
                            catch(ArgumentOutOfRangeException)
                            {
                                // 忽略下标越界异常
                            }
                        }
                    }
                }
            }
        });

        /// <summary>
        /// 启动模拟
        /// </summary>
        public ICommand StartSimulationCommand => new RelayCommand(() =>
        {
            _simulatorTicker.Interval = Interval;
            IsRunning = true;
            _simulatorTicker.Start();
        });

        /// <summary>
        /// 暂停模拟
        /// </summary>
        public ICommand PauseSimulationCommand => new RelayCommand(() =>
        {
            _simulatorTicker.Stop();
            IsRunning = false;
        });

        /// <summary>
        /// 随机播种
        /// </summary>
        public ICommand SeedingCommand => new RelayCommand(() =>
        {
            // 重置棋盘
            ResetCommand.Execute(null);
            Random rd = new Random();
            for (int i = 0; i < SeedingCount; i++)
            {
                int randVal = rd.Next(0, MaxSeedingCount);
                // 校验是否重复播种
                while (Cells[randVal].IsAlive) 
                {
                    randVal = rd.Next(0, MaxSeedingCount);
                }
                // 更改细胞状态
                Cells[randVal].IsAlive = true;
            }
        });

        /// <summary>
        /// 重置棋盘
        /// </summary>
        public ICommand ResetCommand => new RelayCommand(() =>
        {
            foreach (var cell in Cells)
            {
                cell.IsAlive = false;
                cell.NextAlive = false;
            }
        });

    }
}
