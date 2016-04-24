using System;
using System.Collections.Generic;


namespace AdKiller
{
    /// <summary>
    /// 随机数
    /// </summary>
    public class MyRandom : Random
    {
        static MyRandom myRnd = null;
        /// <summary>
        /// 全局随机对象
        /// </summary>
        public static MyRandom Rnd
        {
            get
            {
                if (myRnd == null)
                {
                    myRnd = new MyRandom();
                }
                return myRnd;
            }
        }
        List<Random> rList = new List<Random>(10);
        public MyRandom()
        {
            for (int i = 0; i < 10; i++)
            {
                rList.Add(new Random(Guid.NewGuid().GetHashCode()));
            }
        }
        public override int Next(int maxValue)
        {
            return rList[base.Next(rList.Count)].Next(maxValue);
        }
        public override int Next(int minValue, int maxValue)
        {
            return rList[base.Next(rList.Count)].Next(minValue, maxValue);
        }
        public override double NextDouble()
        {
            return rList[base.Next(rList.Count)].NextDouble();
        }
    }
}
