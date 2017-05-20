using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared
{
    public static class Helper
    {
        public static string GetColorByPriority(string priority)
        {
            string color = String.Empty;
            switch (priority)
            {
                case PriorityConstants.LOW:
                    color = "#336633";
                    break;
                case PriorityConstants.VERY_LOW:
                    color = "#99CC99";
                    break;
                case PriorityConstants.NORMAL:
                    color = "#9999FF";
                    break;
                case PriorityConstants.HIGH:
                    color = "#FF6633";
                    break;
                case PriorityConstants.VERY_HIGH:
                    color = "#FF3333";
                    break;
                default:
                    color = "grey";
                    break;
            }
            return color;
        }
    }
}
