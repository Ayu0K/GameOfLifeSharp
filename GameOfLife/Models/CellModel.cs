using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace GameOfLife.Models
{
    public class CellModel : DependencyObject
    {


        /// <summary>
        /// 指示该细胞是否存活
        /// </summary>
        public bool IsAlive
        {
            get { return (bool)GetValue(IsAliveProperty); }
            set { SetValue(IsAliveProperty, value); }
        }

        // Using a DependencyProperty as the backing store for IsAlive.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty IsAliveProperty =
            DependencyProperty.Register("IsAlive", typeof(bool), typeof(CellModel), new PropertyMetadata(false));

        /// <summary>
        /// 下一次是否存活
        /// </summary>
        public bool NextAlive { get; set; }

        /// <summary>
        /// 与该细胞相邻的细胞
        /// </summary>
        public List<CellModel> Neighbors { get; set; } = new List<CellModel>();

        /// <summary>
        /// 计算下一时刻该细胞的存活状态
        /// </summary>
        public void Next()
        {
            int aliveNeighbors = 0;
            // 统计邻居存活个数
            foreach (var neighbor in Neighbors)
            {
                if (neighbor.IsAlive)
                {
                    aliveNeighbors++;
                }
            }
            // 当前细胞为湮灭状态时，当周围有３个存活细胞时，则迭代后该细胞变成存活状态(模拟繁殖)。
            if (!IsAlive && aliveNeighbors == 3)
            {
                NextAlive = true;
            }
            // 当前细胞为存活状态时，当周围的邻居细胞少于２个存活时，该细胞变成湮灭状态(数量稀少)。
            else if (IsAlive && aliveNeighbors < 2)
            {
                NextAlive = false;
            }
            // 当前细胞为存活状态时，当周围有３个以上的存活细胞时，该细胞变成湮灭状态(数量过多)。
            else if (IsAlive && aliveNeighbors > 3)
            {
                NextAlive = false;
            }
            // 当前细胞为存活状态时，当周围有２个或３个存活细胞时，该细胞保持原样。
            else if (IsAlive && (aliveNeighbors == 2 || aliveNeighbors == 3))
            {
                NextAlive = true;
            }
            else
            {
                NextAlive = false;
            }
        }

    }
}
